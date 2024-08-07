using Tick.Core.Contract;
using Tick.Core.DTO.Request;
using Tick.Core.DTO.Response;
using Tick.Domain.Common;
using Tick.Domain.QueryParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tick.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService accountService)
        {
            _userService = accountService;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<Response<AuthenticationResponse>>> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _userService.AuthenticateAsync(request, HttpContext.RequestAborted));
        }
        [HttpPost("logout")]
        public async Task<ActionResult<Response<string>>> Logout()
        {
            return Ok(await _userService.LogoutAsync());
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet("getUsers")]
        public async Task<ActionResult<PagedResponse<List<UserResponse>>>> GetUsers([FromQuery] UserQueryParameters queryParameters)
        {
            return Ok(await _userService.GetUsersAsync(queryParameters, HttpContext.RequestAborted));
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet("getUser/{id}")]
        public async Task<ActionResult<Response<UserResponse>>> GetUserById(string id)
        {
            return Ok(await _userService.GetUserById(id, HttpContext.RequestAborted));
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost("addUser")]
        public async Task<ActionResult<Response<string>>> AddUser(AddUserRequest request)
        {
            return Ok(await _userService.AddUserAsync(request, HttpContext.RequestAborted));
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost("editUser")]
        public async Task<ActionResult<Response<string>>> EditUser([FromBody] EditUserRequest request)
        {
            return Ok(await _userService.EditUserAsync(request, HttpContext.RequestAborted));
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost("deleteUser")]
        public async Task<ActionResult<Response<string>>> DeleteUser([FromBody] DeleteUserRequest request)
        {
            return Ok(await _userService.DeleteUserAsync(request, HttpContext.RequestAborted));
        }
        [AllowAnonymous]
        [HttpPost("resetUser")]
        public async Task<ActionResult<Response<string>>> ResetUser([FromBody] ResetUserRequest request)
        {
            return Ok(await _userService.ResetUserAsync(request));
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost("resetUserLockout")]
        public async Task<ActionResult<Response<string>>> ResetUserLockout([FromBody] ResetUserRequest request)
        {
            return Ok(await _userService.ResetUserLockoutAsync(request));
        }
        [AllowAnonymous]
        [HttpPost("passwordReset")]
        public async Task<ActionResult<Response<string>>> PasswordReset([FromBody] PasswordResetRequest request)
        {
            return Ok(await _userService.PasswordReset(request));
        }
    }
}