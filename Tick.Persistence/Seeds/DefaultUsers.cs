using Tick.Domain.Constant;
using Tick.Domain.Entities;
using Tick.Domain.Enum;
using System;
using System.Collections.Generic;

namespace Tick.Persistence.Seeds
{
    public static class DefaultUsers
    {
        public static List<Ticker> UserList()
        {
            return new List<Ticker>()
            {
                new Ticker
                {
                    Id = RoleConstants.AdministratorUser,
                    UserName = "ibegbuc",
                    Email = "chinwe.ibegbu@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    // Password@123
                    PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
                    NormalizedEmail= "CHINWE.IBEGBU@GMAIL.COM",
                    NormalizedUserName="IBEGBUC",
                    FirstName = "Chinwe",
                    LastName = "Ibegbu",
                    DefaultRole = UserRole.Administrator,
                    IsActive = true,
                    IsLoggedIn = false,
                    ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147e45",
                    SecurityStamp = "71f781f7-e957-469b-96df-9f2035147e93",
                    AccessFailedCount = 0,
                    LockoutEnabled = false,
                    LastLoginTime = DateTime.Parse("2024-08-06"),
                    CreatedAt = DateTime.Parse("2024-08-06"),
                    UpdatedAt = DateTime.Parse("2024-08-06")
                },
                new Ticker
                {
                    Id = RoleConstants.TickerUser,
                    UserName = "dabyibegbu",
                    Email = "dabyaibegbu@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    // Password@123
                    PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
                    NormalizedEmail= "DABYAIBEGBU@GMAIL.COM",
                    NormalizedUserName="DABYIBEGBU",
                    FirstName = "Daby",
                    LastName = "Ibegbu",
                    DefaultRole = UserRole.Ticker,
                    IsActive = true,
                    IsLoggedIn = false,
                    ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147e98",
                    SecurityStamp = "71f781f7-e957-469b-96df-9f2035147e37",
                    AccessFailedCount = 0,
                    LockoutEnabled = false,
                    LastLoginTime = DateTime.Parse("2024-08-06"),
                    CreatedAt = DateTime.Parse("2024-08-06"),
                    UpdatedAt = DateTime.Parse("2024-08-06")
                }
            };
        }
    }
}