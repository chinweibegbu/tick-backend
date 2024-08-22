using Tick.Domain.Enum;
using System;
using System.Collections.Generic;

namespace Tick.Core.DTO.Response
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserRole DefaultRole { get; set; }
        public string DefaultRoleMeaning => Enum.GetName(typeof(UserRole), DefaultRole);
        public List<string> Roles { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
