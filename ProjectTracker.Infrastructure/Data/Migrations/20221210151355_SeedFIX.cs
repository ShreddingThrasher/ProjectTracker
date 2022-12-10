using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTracker.Infrastructure.Data.Migrations
{
    public partial class SeedFIX : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("187de7f4-8a45-415d-b9e3-7d7d36188c30"));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "366cddc3-8ffb-4b6c-9df7-aaf99d737444",
                column: "ConcurrencyStamp",
                value: "4d1a2528-1991-4bd6-b740-1e7668edabd2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c65a4ecb-89b0-4c14-8a90-4bafea94d642",
                column: "ConcurrencyStamp",
                value: "4ee5b44e-12b2-4c96-a6db-0ea177577169");

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "IsActive", "LeadId", "Name" },
                values: new object[] { new Guid("76c836e9-f620-4b7e-90e5-b8f15f1564a8"), true, "a14e20b2-f833-42c5-bb8a-abfac472a59c", "Initial Department" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a14e20b2-f833-42c5-bb8a-abfac472a59c",
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "LastName", "LeadedDepartmentId", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "730032f7-f0c7-4ddb-b7fe-b5022bc3085d", "administrator@mail.com", "Administrator", "Administrator", new Guid("76c836e9-f620-4b7e-90e5-b8f15f1564a8"), "ADMINISTRATOR@MAIL.COM", "ADMINISTRATOR", "AQAAAAEAACcQAAAAEO11TMy/lEIYDYu8PW0OZLdj23xGrySKmCpFaQp8CZzUpTsehY8XVrfS99hkaLZABw==", "7e4ededb-5f88-49b2-a066-891f7cbcc28b" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("76c836e9-f620-4b7e-90e5-b8f15f1564a8"));

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

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a14e20b2-f833-42c5-bb8a-abfac472a59c",
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "LastName", "LeadedDepartmentId", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "db0e8012-c3f8-4c82-8d5a-b3b6ee9c5b7e", "admin@mail.com", "Admin", "User", null, null, null, "AQAAAAEAACcQAAAAECVxYekLsxt7IWKet3YlIL0O+IusmN8lf8t9hH8eWsWYA2O3cmHy7HzCOVK98zp9wQ==", "5232b6fc-62c5-416a-b278-339d8d780a12" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "IsActive", "LeadId", "Name" },
                values: new object[] { new Guid("187de7f4-8a45-415d-b9e3-7d7d36188c30"), true, "a14e20b2-f833-42c5-bb8a-abfac472a59c", "Initial Department" });
        }
    }
}
