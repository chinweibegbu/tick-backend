using Tick.Domain.Entities;
using Tick.Domain.Enum;
using System;
using System.Collections.Generic;

namespace Tick.Persistence.Seeds
{
    public static class DefaultBasicUsers
    {
        public static List<BasicUser> BasicUserList()
        {
            return new List<BasicUser>()
            {
                new BasicUser
                {
                    Id = "BSR_482242804225Y91361313",
                    Name = "Access Bank Key 1",
                    ApiKey = "SEC_EPDGYJMVUXEGELEZWHBZGDPZHNIKIZWXUTMJHBNMWWMPBYMFOY",
                    Status = BasicAuthStatus.Active,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2023-10-20"),
                    UpdatedAt = DateTime.Parse("2023-10-20")
                },
                new BasicUser
                {
                    Id = "BSR_482242804225Y91908738",
                    Name = "AFF Key 1",
                    ApiKey = "SEC_EPDGYJMVUXEGELEZWHBZGDPZHNIKIZWXUTMJHBNMWWKDHWIWW",
                    Status = BasicAuthStatus.Active,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2023-10-20"),
                    UpdatedAt = DateTime.Parse("2023-10-20")
                }
            };
        }
    }
}