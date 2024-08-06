using TemplateTest.Domain.Constant;
using TemplateTest.Domain.Entities;
using TemplateTest.Domain.Enum;
using System;
using System.Collections.Generic;

namespace TemplateTest.Persistence.Seeds
{
    public static class DefaultUsers
    {
        public static List<User> UserList()
        {
            return new List<User>()
            {
                new User
                {
                    Id = RoleConstants.AdministratorUser,
                    UserName = "shadao",
                    Email = "Oluwatosin.Shada@ACCESSBANKPLC.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    // Password@123
                    PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
                    NormalizedEmail= "OLUWATOSIN.SHADA@ACCESSBANKPLC.COM",
                    NormalizedUserName="SHADAO",
                    FirstName = "Oluwatosin",
                    LastName = "Shada",
                    DefaultRole = UserRole.Administrator,
                    IsActive = true,
                    IsLoggedIn = false,
                    ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147e45",
                    SecurityStamp = "71f781f7-e957-469b-96df-9f2035147e93",
                    AccessFailedCount = 0,
                    LockoutEnabled = false,
                    LastLoginTime = DateTime.Parse("2023-10-20"),
                    CreatedAt = DateTime.Parse("2023-10-20"),
                    UpdatedAt = DateTime.Parse("2023-10-20")
                },
                new User
                {
                    Id = RoleConstants.BankStaffUser,
                    UserName = "ohuet",
                    Email = "Thelma.Ohue@ACCESSBANKPLC.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    // Password@123
                    PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
                    NormalizedEmail= "THELMA.OHUE@ACCESSBANKPLC.COM",
                    NormalizedUserName="OHUET",
                    FirstName = "Thelma",
                    LastName = "Ohue",
                    DefaultRole = UserRole.BankStaff,
                    IsActive = true,
                    IsLoggedIn = false,
                    ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147e98",
                    SecurityStamp = "71f781f7-e957-469b-96df-9f2035147e37",
                    AccessFailedCount = 0,
                    LockoutEnabled = false,
                    LastLoginTime = DateTime.Parse("2023-10-20"),
                    CreatedAt = DateTime.Parse("2023-10-20"),
                    UpdatedAt = DateTime.Parse("2023-10-20")
                },
                new User
                {
                    Id = RoleConstants.SettlementUser,
                    UserName = "makinded",
                    Email = "Daniel.Makinde@ACCESSBANKPLC.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    // Password@123
                    PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
                    NormalizedEmail= "DANIEL.MAKINDE@ACCESSBANKPLC.COM",
                    NormalizedUserName="MAKINDED",
                    FirstName = "Daniel",
                    LastName = "Makinde",
                    DefaultRole = UserRole.Settlement,
                    IsActive = true,
                    IsLoggedIn = false,
                    ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147eb1",
                    SecurityStamp = "71f781f7-e957-469b-96df-9f2035147eb2",
                    AccessFailedCount = 0,
                    LockoutEnabled = false,
                    LastLoginTime = DateTime.Parse("2023-10-20"),
                    CreatedAt = DateTime.Parse("2023-10-20"),
                    UpdatedAt = DateTime.Parse("2023-10-20")
                }
            };
        }
    }
}