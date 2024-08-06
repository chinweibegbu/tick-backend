using TemplateTest.Domain.Constant;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TemplateTest.Persistence.Seeds
{
    public static class MappingUserRole
    {
        public static List<IdentityUserRole<string>> IdentityUserRoleList()
        {
            return new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>
                {
                    RoleId = RoleConstants.BankStaff,
                    UserId = RoleConstants.BankStaffUser
                },
                new IdentityUserRole<string>
                {
                    RoleId = RoleConstants.Administrator,
                    UserId = RoleConstants.AdministratorUser
                },
                new IdentityUserRole<string>
                {
                    RoleId = RoleConstants.Settlement,
                    UserId = RoleConstants.SettlementUser
                }
            };
        }
    }
}
