using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTracker.Infrastructure.Data.Migrations
{
    public partial class SeedAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "366cddc3-8ffb-4b6c-9df7-aaf99d737444", "a920fb3d-f6c6-47d7-bc99-dbd6cce8876e", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c65a4ecb-89b0-4c14-8a90-4bafea94d642", "80dc19d2-9924-40c3-bca2-822fa7f727d7", "DepartmentLead", "DEPARTMENTLEAD" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DepartmentId", "Email", "EmailConfirmed", "FirstName", "LastName", "LeadedDepartmentId", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "33f73add-bb37-4d27-bb48-5fe0e682cd04", 0, "492671f7-96e7-4a03-be1c-9867d608467f", null, "admin@mail.com", false, "Admin", "User", null, false, null, null, null, null, null, false, "44d6a452-9cdf-449b-b163-92436081d392", false, "Administrator" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "366cddc3-8ffb-4b6c-9df7-aaf99d737444", "33f73add-bb37-4d27-bb48-5fe0e682cd04" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c65a4ecb-89b0-4c14-8a90-4bafea94d642");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "366cddc3-8ffb-4b6c-9df7-aaf99d737444", "33f73add-bb37-4d27-bb48-5fe0e682cd04" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "366cddc3-8ffb-4b6c-9df7-aaf99d737444");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33f73add-bb37-4d27-bb48-5fe0e682cd04");
        }
    }
}
