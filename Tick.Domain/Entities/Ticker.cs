using Tick.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Tick.Domain.Entities
{
    public class Ticker : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public bool IsLoggedIn { get; set; }
        // Work around to easily manage roles. We need a better implementation of user management.
        public UserRole? DefaultRole { get; set; }
        public DateTime LastLoginTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string ProfileImageUrl { get; set; }
        public ICollection<Entities.Task> Tasks { get; set; }
    }
}