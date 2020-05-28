using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestGenerator.Model.Migrations
{
    public partial class AjoutTableModules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                table: "Exams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    ModuleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 255, nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.ModuleId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ModuleId",
                table: "Questions",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ModuleId",
                table: "Exams",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Modules_ModuleId",
                table: "Exams",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Modules_ModuleId",
                table: "Questions",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Modules_ModuleId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Modules_ModuleId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Questions_ModuleId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Exams_ModuleId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Exams");
        }
    }
}
