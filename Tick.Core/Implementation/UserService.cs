﻿using Tick.Core.Contract;
using Tick.Core.Contract.Repository;
using Tick.Core.DTO.Request;
using Tick.Core.DTO.Response;
using Tick.Core.Exceptions;
using Tick.Core.Extension;
using Tick.Domain.Common;
using Tick.Domain.Entities;
using Tick.Domain.Enum;
using Tick.Domain.QueryParameters;
using Tick.Domain.Settings;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using SendGrid;
using SendGrid.Helpers.Mail;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Tick.Core.Implementation
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IBasicUserRepository _basicUserRepository;
        private readonly JWTSettings _jwtSettings;
        private readonly SendGridSettings _sendGridSettings;
        private readonly CloudinarySettings _cloudinarySettings;
        private readonly UserManager<Ticker> _userManager;
        private readonly SignInManager<Ticker> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAppSessionService _appSession;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ExternalApiOptions _externalApiOptions;

        public UserService(IMapper mapper,
            ILogger<UserService> logger,
            IBasicUserRepository basicUserRepository,
            IOptions<JWTSettings> jwtSettings,
            IOptions<SendGridSettings> sendGridSettings,
            IOptions<CloudinarySettings> cloudinarySettings,
            UserManager<Ticker> userManager,
            SignInManager<Ticker> signInManager,
            RoleManager<IdentityRole> roleManager,
            IAppSessionService appSession,
            INotificationService notificationService,
            IHttpContextAccessor httpContextAccessor,
            IOptions<ExternalApiOptions> externalApiOptions)
        {
            _mapper = mapper;
            _logger = logger;
            _basicUserRepository = basicUserRepository;
            _jwtSettings = jwtSettings.Value;
            _sendGridSettings = sendGridSettings.Value;
            _cloudinarySettings = cloudinarySettings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _appSession = appSession;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _externalApiOptions = externalApiOptions.Value;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, CancellationToken cancellationToken)
        {
            // Check for the email
            Ticker user = await _userManager.Users
                .Where(x => x.NormalizedEmail == request.Email.ToUpper())
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new ApiException($"No account found with email - {request.Email}", httpStatusCode:HttpStatusCode.BadRequest);
            }

            if (!user.IsActive)
            {
                throw new ApiException($"Inactive account", httpStatusCode:HttpStatusCode.BadRequest);
            }

            // Verify the username and password
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    throw new ApiException($"This user has been locked. Kindly contact the administrator.", httpStatusCode:HttpStatusCode.BadRequest);
                }
                throw new ApiException($"Incorrect password for email - {request.Email}", httpStatusCode:HttpStatusCode.BadRequest);
            }
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user, cancellationToken);

            user.IsLoggedIn = true;
            AuthenticationResponse response = _mapper.Map<Ticker, AuthenticationResponse>(user);

            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.ExpiresIn = _jwtSettings.DurationInMinutes * 60;
            response.ExpiryDate = DateTime.UtcNow.AddSeconds(_jwtSettings.DurationInMinutes * 60);

            user.LastLoginTime = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return new Response<AuthenticationResponse>(response, $"Authenticated email - {user.Email}");
        }

        public async Task<BasicAuthResponse> BasicAuthenticateAsync(string apiKey, CancellationToken cancellationToken)
        {
            // Check for the username
            BasicUser basicUser = await _basicUserRepository.GetBasicUserByApiKey(apiKey, cancellationToken);

            if (basicUser == null)
            {
                throw new Exception($"Invalid API Key");
            }

            var response = _mapper.Map<BasicUser, BasicAuthResponse>(basicUser);

            return response;
        }

        private async Task<JwtSecurityToken> GenerateJWToken(Ticker user, CancellationToken cancellationToken)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            DateTime utcNow = DateTime.UtcNow;
            string ipAddress = IpHelper.GetIpAddress();
            string sessionKey = Guid.NewGuid().ToString();
            await _appSession.CreateSession(sessionKey, user.UserName);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                //new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, utcNow.Ticks.ToString()),
                new Claim("userId", user.Id),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
                new Claim("emailAddress", user.Email),
                new Claim("username", user.UserName),
                new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<Response<string>> LogoutAsync()
        {
            var username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;

            _appSession.DeleteSession(username);

            // [TODO] Handle situations where the JWT token is expired
            Ticker user = await _userManager.FindByNameAsync(username);
            user.IsLoggedIn = false;
            await _userManager.UpdateAsync(user);
            return new Response<string>($"Successfully logged out user with username - {username}", (int)HttpStatusCode.OK, true);
        }

        public async Task<PagedResponse<List<UserResponse>>> GetUsersAsync(UserQueryParameters queryParameters, CancellationToken cancellationToken)
        {
            IQueryable<Ticker> pagedData = _userManager.Users;

            string query = queryParameters.Query;
            UserRole? role = queryParameters.Role;
            UserStatus? status = queryParameters.Status;

            // Check if there is a query and apply it
            if (!string.IsNullOrEmpty(query))
            {
                pagedData = pagedData.Where(x => x.Id.ToLower().Contains(query.ToLower())
                   || x.UserName.ToLower().Contains(query.ToLower())
                   || x.Email.ToLower().Contains(query.ToLower())
                   || x.FirstName.ToLower().Contains(query.ToLower())
                   || x.LastName.ToLower().Contains(query.ToLower()));
            }

            if (role.HasValue)
            {
                pagedData = pagedData.Where(x => x.DefaultRole == role.Value);
            }

            // Check the status passed in the query parameters and if available use it to filter the result
            if (status.HasValue)
            {
                bool isActive = status.Value == UserStatus.Active;
                pagedData = pagedData.Where(x => x.IsActive == isActive);
            }

            List<Ticker> userList = await pagedData
                .OrderByDescending(x => x.CreatedAt)
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync(cancellationToken);

            List<UserResponse> response = _mapper.Map<List<Ticker>, List<UserResponse>>(userList);

            int totalRecords = await pagedData.CountAsync(cancellationToken);

            return new PagedResponse<List<UserResponse>>(response, queryParameters.PageNumber, queryParameters.PageSize, totalRecords, $"Successfully retrieved users");
        }

        public async Task<Response<UserResponse>> GetUserById(string id, CancellationToken cancellationToken)
        {
            Ticker userData = await _userManager.Users
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (userData == null)
            {
                throw new ApiException($"No user found with ID - {id}", httpStatusCode:HttpStatusCode.NotFound);
            }

            UserResponse response = _mapper.Map<Ticker, UserResponse>(userData);

            return new Response<UserResponse>(response, $"Successfully retrieved user details for user with ID - {id}");
        }

        public async Task<Response<string>> AddUserAsync(AddUserRequest request, CancellationToken cancellationToken)
        {
            // Check that the username is unique
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Username {request.UserName} is already registered", httpStatusCode:HttpStatusCode.Conflict);
            }

            // Check that the email is unique
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                throw new ApiException($"Email {request.Email} is already registered", httpStatusCode: HttpStatusCode.Conflict);
            }

            // Get user role: Admin or Ticker
            var roleName = Enum.GetName(typeof(UserRole), request.Role) ?? throw new ApiException($"Invalid role specified", httpStatusCode: HttpStatusCode.NotFound);

            // Check if the role exists
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                throw new ApiException($"Invalid role specified", httpStatusCode: HttpStatusCode.NotFound);
            }

            string username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            string email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "emailAddress")?.Value;

            Ticker newUser = _mapper.Map<Ticker>(request);
            newUser.CreatedAt = DateTime.UtcNow;
            newUser.UpdatedAt = DateTime.UtcNow;
            newUser.IsActive = true;

            // Configure Cloudinary
            Account account = new Account(_cloudinarySettings.CloudName, _cloudinarySettings.ApiKey, _cloudinarySettings.ApiSecret);
            Cloudinary cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;

            // Upload image to Cloudinary IF it exists
            if (request.ProfileImage != null)
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(name: request.ProfileImage.FileName, stream: request.ProfileImage.OpenReadStream()),
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = true
                };

                var uploadResult = cloudinary.Upload(uploadParams);

                // Add user's profile image Cloudinary URL to user
                newUser.ProfileImageUrl = uploadResult.Url.ToString();
            } 
            else {
                // Add default profile image Cloudinary URL to user
                newUser.ProfileImageUrl = _cloudinarySettings.DefaultImageUrl;
            }
            
            // Create user + Include password
            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                throw new ApiException($"{result.Errors.FirstOrDefault().Description}");
            }
            
            // Add user to role
            IdentityResult roleResult = await _userManager.AddToRoleAsync(newUser, roleName);

            if (!roleResult.Succeeded)
            {
                // Roll back user creation and throw an error
                await _userManager.DeleteAsync(newUser);
                throw new ApiException($"An error occured while adding the user to the role");
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(newUser);

            string userName = newUser.UserName;
            string firstName = newUser.FirstName;
            string userEmail = newUser.Email;
            string url = $"{_externalApiOptions.PasswordResetUrl}/{HttpUtility.UrlEncode(token)}";

            // NOT WORKING
            // WHY ? No email service configured
            string jobId = BackgroundJob.Enqueue(() => _notificationService.SendPasswordResetToken(userName, url, firstName, userEmail));
            _logger.LogInformation($"Successfully sent out the Password Reset job with Job ID {jobId}");

            return new Response<string>(newUser.Id, message: $"Successfully registered user with email - {request.Email}");
        }

        public async Task<Response<string>> EditUserAsync(EditUserRequest request, CancellationToken cancellationToken)
        {
            Ticker user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new ApiException($"Username {request.UserName} could not be found", httpStatusCode: HttpStatusCode.NotFound);
            }

            var roleName = Enum.GetName(typeof(UserRole), request.Role) ?? throw new ApiException($"Invalid role specified", httpStatusCode: HttpStatusCode.NotFound);

            // Check if the new role exists
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                throw new ApiException($"This role doesn't exists. Please check your roles and try again.", httpStatusCode: HttpStatusCode.NotFound);
            }

            string username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            string email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "emailAddress")?.Value;

            user.FirstName = string.IsNullOrEmpty(request.FirstName) ? user.FirstName : request.FirstName;
            user.LastName = string.IsNullOrEmpty(request.LastName) ? user.LastName : request.LastName;
            user.Email = string.IsNullOrEmpty(request.Email) ? user.Email : request.Email;
            user.UpdatedAt = DateTime.UtcNow;
            user.DefaultRole = request.Role ?? user.DefaultRole;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                throw new ApiException(updateResult.Errors.FirstOrDefault()?.Description ?? "An error occured while updating user");
            }

            IList<string> existingRoles = await _userManager.GetRolesAsync(user);

            if (!existingRoles.Contains(roleName))
            {
                // If there are existing roles then delete them
                if (existingRoles.Count > 0)
                {
                    IdentityResult roleResult = await _userManager.RemoveFromRolesAsync(user, existingRoles);

                    if (!roleResult.Succeeded)
                    {
                        throw new ApiException(roleResult.Errors.FirstOrDefault().Description);
                    }
                }

                IdentityResult addRoleResult = await _userManager.AddToRoleAsync(user, roleName);

                if (!addRoleResult.Succeeded)
                {
                    throw new ApiException(addRoleResult.Errors.FirstOrDefault().Description);
                }
            }

            return new Response<string>(user.Id, message: $"Successfully edited user with username - {request.UserName}");
        }

        public async Task<Response<string>> DeleteUserAsync(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            Ticker user = await _userManager.FindByNameAsync(request.UserName) ?? throw new ApiException($"No user found with username - {request.UserName}", httpStatusCode: HttpStatusCode.NotFound);

            // Work on the request logging
            string username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            string email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "emailAddress")?.Value;

            var existingRoles = await _userManager.GetRolesAsync(user);

            var roleResult = await _userManager.RemoveFromRolesAsync(user, existingRoles);
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new ApiException(result.Errors.FirstOrDefault().Description);
            }

            return new Response<string>(user.Id, message: $"Successfully deleted the with username - {request.UserName}");
        }

        public async Task<Response<string>> ResetUserAsync(ResetUserRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // This is a security measure to prevent a bad actor from knowing the list of users on the platform.
                return new Response<string>("", "Reset user successful ??");
            }

            string userName = user.UserName;
            string userFirstName = user.FirstName;
            string userEmail = user.Email;
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string encodedToken = HttpUtility.UrlEncode(token);

            /*
            var resetUserRequest = new
            {
                UserName = userName,
                Token = HttpUtility.UrlEncode(token)
            };
            IDictionary<string, string> param = resetUserRequest.ToDictionary();

            Uri url = new(QueryHelpers.AddQueryString(_externalApiOptions.PasswordResetUrl, param));

            string jobId = BackgroundJob.Enqueue(() => _notificationService.SendPasswordResetToken(userName, url.ToString(), userFirstName, userEmail));
            _logger.LogInformation($"Successfully sent out the Password Reset job with Job ID {jobId}");
            */

            // Send email
            var apiKey = _sendGridSettings.SendGridApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("chinwe.ibegbu@gmail.com", "TICK");
            var to = new EmailAddress(userEmail, userName);
            var dynamicEmailTemplateId = "d-087e3ae80b1b4f64b96eabe81194270c";
            var templateData = new DynamicEmailTemplateData(encodedToken, userEmail);
            
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, dynamicEmailTemplateId, templateData);
            var response = await client.SendEmailAsync(msg);

            return new Response<string>("", $"Email successfully sent to user with email - {request.Email}");
        }

        public async Task<Response<string>> ResetUserLockoutAsync(ResetUserRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException($"No user could be found with email - {request.Email}", httpStatusCode: HttpStatusCode.NotFound);
            }

            var resetResult = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now);

            if (!resetResult.Succeeded)
            {
                _logger.LogError("An error occured while resetting lockout");
                _logger.LogError(CoreHelpers.ClassToJsonData(resetResult.Errors));
                throw new ApiException($"An error occured while resetting lockout");
            }

            return new Response<string>(user.Id, message: $"Successfully reset user with email - {request.Email}");
        }

        public async Task<Response<string>> PasswordReset(PasswordResetRequest request)
        {
            Ticker user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException($"No user found with email - {request.Email}", httpStatusCode: HttpStatusCode.NotFound);
            }

            // TODO: Find out why the request token was already decoded
            // string token = HttpUtility.UrlDecode(request.Token);

            IdentityResult resetResponse = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!resetResponse.Succeeded)
            {
                throw new ApiException(resetResponse.Errors.FirstOrDefault().Description);
            }

            return new Response<string>(user.Id, message: $"Successfully reset password for user with email - {request.Email}");
        }
    }
}
