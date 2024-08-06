using TemplateTest.Domain.Enum;
using System;
using System.Collections.Generic;

namespace TemplateTest.Core.DTO.Response
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole DefaultRole { get; set; }
        public string DefaultRoleMeaning => Enum.GetName(typeof(UserRole), DefaultRole);
        public bool IsActive { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime LastLoginTime { get; set; }
        public string JWToken { get; set; }
        public double ExpiresIn { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
