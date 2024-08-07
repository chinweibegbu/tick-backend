using Tick.Domain.Constant;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Tick.Persistence.Seeds
{
    public static class DefaultRoles
    {
        public static List<IdentityRole> IdentityRoleList()
        {
            return new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Id = RoleConstants.Administrator,
                    Name = Roles.Administrator,
                    NormalizedName = Roles.Administrator.ToUpper(),
                    ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147a23"
                },
                new IdentityRole
                {
                    Id = RoleConstants.Ticker,
                    Name = Roles.Ticker,
                    NormalizedName = Roles.Ticker.ToUpper(),
                    ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147a56"
                },
            };
        }
    }
}
