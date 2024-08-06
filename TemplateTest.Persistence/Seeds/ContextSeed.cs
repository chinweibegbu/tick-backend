using TemplateTest.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TemplateTest.Persistence.Seeds
{
    public static class ContextSeed
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            CreateRoles(modelBuilder);

            CreateJwtUsers(modelBuilder);

            CreateBasicUsers(modelBuilder);

            MapUserRole(modelBuilder);
        }

        private static void CreateRoles(ModelBuilder modelBuilder)
        {
            List<IdentityRole> roles = DefaultRoles.IdentityRoleList();
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }

        private static void CreateJwtUsers(ModelBuilder modelBuilder)
        {
            List<User> users = DefaultUsers.UserList();
            modelBuilder.Entity<User>().HasData(users);
        }

        private static void CreateBasicUsers(ModelBuilder modelBuilder)
        {
            List<BasicUser> basicUsers = DefaultBasicUsers.BasicUserList();
            modelBuilder.Entity<BasicUser>().HasData(basicUsers);
        }

        private static void MapUserRole(ModelBuilder modelBuilder)
        {
            var identityUserRoles = MappingUserRole.IdentityUserRoleList();
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(identityUserRoles);
        }
    }
}
