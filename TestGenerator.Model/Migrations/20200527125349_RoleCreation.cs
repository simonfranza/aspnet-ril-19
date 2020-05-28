using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestGenerator.Model.Migrations
{
    public partial class RoleCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[,]
                {
                    {
                        Guid.NewGuid().ToString(),
                        "Administrator",
                        "Admin",
                        Guid.NewGuid().ToString()
                    }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
