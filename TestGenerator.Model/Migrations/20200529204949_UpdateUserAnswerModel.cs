using Microsoft.EntityFrameworkCore.Migrations;

namespace TestGenerator.Model.Migrations
{
    public partial class UpdateUserAnswerModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "ExamAttempts");

            migrationBuilder.RenameColumn(
                name: "ExamParticipationId",
                table: "UserAnswers",
                newName: "ExamAttemptId");

            migrationBuilder.AddColumn<int>(
                name: "AnswserId",
                table: "UserAnswers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ExamAttempts",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAttempts_ExamId",
                table: "ExamAttempts",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAttempts_UserId",
                table: "ExamAttempts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAttempts_Exams_ExamId",
                table: "ExamAttempts",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "ExamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAttempts_AspNetUsers_UserId",
                table: "ExamAttempts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamAttempts_Exams_ExamId",
                table: "ExamAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamAttempts_AspNetUsers_UserId",
                table: "ExamAttempts");

            migrationBuilder.DropIndex(
                name: "IX_ExamAttempts_ExamId",
                table: "ExamAttempts");

            migrationBuilder.DropIndex(
                name: "IX_ExamAttempts_UserId",
                table: "ExamAttempts");

            migrationBuilder.DropColumn(
                name: "AnswserId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ExamAttempts");

            migrationBuilder.RenameColumn(
                name: "ExamAttemptId",
                table: "UserAnswers",
                newName: "ExamParticipationId");

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "ExamAttempts",
                nullable: false,
                defaultValue: 0);
        }
    }
}
