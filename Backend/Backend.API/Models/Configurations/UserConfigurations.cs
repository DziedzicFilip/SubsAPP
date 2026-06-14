using Backend.API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Backend.API.Models.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
           
            var passwordHasher = new PasswordHasher<User>();

            string adminId = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            string userId = "b72ce8d1-bb76-4bf9-cd28-11ce0455f686";

            var admin = new User
            {
                Id = adminId,
                UserName = "admin@twojadomena.pl",
                NormalizedUserName = "ADMIN@TWOJADOMENA.PL",
                Email = "admin@twojadomena.pl",
                NormalizedEmail = "ADMIN@TWOJADOMENA.PL",
                EmailConfirmed = true,
                FirstName = "Jan",
                LastName = "Kowalski",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };
            admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin123!");

            var regularUser = new User
            {
                Id = userId,
                UserName = "user@twojadomena.pl",
                NormalizedUserName = "USER@TWOJADOMENA.PL",
                Email = "user@twojadomena.pl",
                NormalizedEmail = "USER@TWOJADOMENA.PL",
                EmailConfirmed = true,
                FirstName = "Anna",
                LastName = "Nowak",
                CreatedAt = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc)
            };
            regularUser.PasswordHash = passwordHasher.HashPassword(regularUser, "User123!");

            
            builder.HasData(admin, regularUser);
        }
    }
}