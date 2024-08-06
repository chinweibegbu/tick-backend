using TemplateTest.Domain.Constant;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TemplateTest.Persistence.Seeds
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
                    Id = RoleConstants.BankStaff,
                    Name = Roles.BankStaff,
                    NormalizedName = Roles.BankStaff.ToUpper(),
                    ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147a56"
                },
                new IdentityRole
                {
                    Id = RoleConstants.Settlement,
                    Name = Roles.Settlement,
                    NormalizedName = Roles.Settlement.ToUpper(),
                    ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147a87"
                }
            };
        }
    }
}
