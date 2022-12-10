using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTracker.Infrastructure.Data.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "366cddc3-8ffb-4b6c-9df7-aaf99d737444", "33f73add-bb37-4d27-bb48-5fe0e682cd04" });

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("b299462b-ec10-4573-bcc9-b308e088a550"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33f73add-bb37-4d27-bb48-5fe0e682cd04");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "366cddc3-8ffb-4b6c-9df7-aaf99d737444",
                column: "ConcurrencyStamp",
                value: "a87773a5-bb9d-4656-acf3-fc55d0c5bcdf");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c65a4ecb-89b0-4c14-8a90-4bafea94d642",
                column: "ConcurrencyStamp",
                value: "113530be-d34b-45f1-b21e-f5cf5c40f7cd");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DepartmentId", "Email", "EmailConfirmed", "FirstName", "LastName", "LeadedDepartmentId", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a14e20b2-f833-42c5-bb8a-abfac472a59c", 0, "db0e8012-c3f8-4c82-8d5a-b3b6ee9c5b7e", null, "admin@mail.com", false, "Admin", "User", null, false, null, null, null, "AQAAAAEAACcQAAAAECVxYekLsxt7IWKet3YlIL0O+IusmN8lf8t9hH8eWsWYA2O3cmHy7HzCOVK98zp9wQ==", null, false, "5232b6fc-62c5-416a-b278-339d8d780a12", false, "Administrator" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "IsActive", "LeadId", "Name" },
                values: new object[] { new Guid("187de7f4-8a45-415d-b9e3-7d7d36188c30"), true, "a14e20b2-f833-42c5-bb8a-abfac472a59c", "Initial Department" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "366cddc3-8ffb-4b6c-9df7-aaf99d737444", "a14e20b2-f833-42c5-bb8a-abfac472a59c" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "366cddc3-8ffb-4b6c-9df7-aaf99d737444", "a14e20b2-f833-42c5-bb8a-abfac472a59c" });

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("187de7f4-8a45-415d-b9e3-7d7d36188c30"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a14e20b2-f833-42c5-bb8a-abfac472a59c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "366cddc3-8ffb-4b6c-9df7-aaf99d737444",
                column: "ConcurrencyStamp",
                value: "a920fb3d-f6c6-47d7-bc99-dbd6cce8876e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c65a4ecb-89b0-4c14-8a90-4bafea94d642",
                column: "ConcurrencyStamp",
                value: "80dc19d2-9924-40c3-bca2-822fa7f727d7");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DepartmentId", "Email", "EmailConfirmed", "FirstName", "IsActive", "IsGuest", "LastName", "LeadedDepartmentId", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "33f73add-bb37-4d27-bb48-5fe0e682cd04", 0, "1d823ae5-e778-4d54-9293-c8ef8376c4aa", null, "admin@mail.com", false, "Admin", false, false, "User", null, false, null, null, null, "AQAAAAEAACcQAAAAEATWsC7vy8qDFSGII9DsMRhK/iRp+c75oBXMme1liVQOKNPnhnPDNVJZQYZyV7O2Ug==", null, false, "d301191c-7d6e-4cec-8793-f4cd53b52b89", false, "Administrator" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "IsActive", "LeadId", "Name" },
                values: new object[] { new Guid("b299462b-ec10-4573-bcc9-b308e088a550"), true, "33f73add-bb37-4d27-bb48-5fe0e682cd04", "Initial Department" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "366cddc3-8ffb-4b6c-9df7-aaf99d737444", "33f73add-bb37-4d27-bb48-5fe0e682cd04" });
        }
    }
}
