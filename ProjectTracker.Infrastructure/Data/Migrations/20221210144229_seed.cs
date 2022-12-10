using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTracker.Infrastructure.Data.Migrations
{
    public partial class seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33f73add-bb37-4d27-bb48-5fe0e682cd04",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1d823ae5-e778-4d54-9293-c8ef8376c4aa", "AQAAAAEAACcQAAAAEATWsC7vy8qDFSGII9DsMRhK/iRp+c75oBXMme1liVQOKNPnhnPDNVJZQYZyV7O2Ug==", "d301191c-7d6e-4cec-8793-f4cd53b52b89" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "IsActive", "LeadId", "Name" },
                values: new object[] { new Guid("b299462b-ec10-4573-bcc9-b308e088a550"), true, "33f73add-bb37-4d27-bb48-5fe0e682cd04", "Initial Department" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("b299462b-ec10-4573-bcc9-b308e088a550"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33f73add-bb37-4d27-bb48-5fe0e682cd04",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "492671f7-96e7-4a03-be1c-9867d608467f", null, "44d6a452-9cdf-449b-b163-92436081d392" });
        }
    }
}
