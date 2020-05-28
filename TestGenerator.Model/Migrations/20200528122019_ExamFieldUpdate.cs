using Microsoft.EntityFrameworkCore.Migrations;

namespace TestGenerator.Model.Migrations
{
    public partial class ExamFieldUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Exams",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Exams");
        }
    }
}
