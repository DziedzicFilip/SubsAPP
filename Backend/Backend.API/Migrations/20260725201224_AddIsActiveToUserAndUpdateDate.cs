using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.API.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToUserAndUpdateDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b72ce8d1-bb76-4bf9-cd28-11ce0455f686");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "a18be9c0-aa65-4af8-bd17-00bd9344e575", 0, "86cae99f-9cc3-46f1-90f3-c75cf0487e5d", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@twojadomena.pl", true, "Jan", "Kowalski", false, null, "ADMIN@TWOJADOMENA.PL", "ADMIN@TWOJADOMENA.PL", "AQAAAAIAAYagAAAAEHPXIX2NZGWxeuxKh/c0ukuC5hLtSLPkmz2B/PRbSYMYkunee79MbpvTSmJepySsTA==", null, false, "96e7c772-a885-458a-ad25-6438eac9fb7b", false, "admin@twojadomena.pl" },
                    { "b72ce8d1-bb76-4bf9-cd28-11ce0455f686", 0, "283b0146-f1a8-48ba-9f51-6b256778ef9f", new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "user@twojadomena.pl", true, "Anna", "Nowak", false, null, "USER@TWOJADOMENA.PL", "USER@TWOJADOMENA.PL", "AQAAAAIAAYagAAAAEKztEMdrmO1MAPDNgB3mTmiZyzgMSr/DlXb4MQE8ThdUC7CE99HCbRv60oKkFeuKkw==", null, false, "71d20b73-93f8-43a5-a266-bb5448fb5acc", false, "user@twojadomena.pl" }
                });
        }
    }
}
