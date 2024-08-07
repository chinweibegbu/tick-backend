using Tick.Domain.Constant;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Tick.Persistence.Seeds
{
    public static class MappingUserRole
    {
        public static List<IdentityUserRole<string>> IdentityUserRoleList()
        {
            return new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>
                {
                    RoleId = RoleConstants.Ticker,
                    UserId = RoleConstants.TickerUser
                },
                new IdentityUserRole<string>
                {
                    RoleId = RoleConstants.Administrator,
                    UserId = RoleConstants.AdministratorUser
                }
            };
        }
    }
}
