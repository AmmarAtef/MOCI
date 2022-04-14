using Microsoft.EntityFrameworkCore.Migrations;

namespace MOCI.DAL.Migrations
{
    public partial class EmployeeGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "GradeId",
                table: "User",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_GradeId",
                table: "User",
                column: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Grades_GradeId",
                table: "User",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Grades_GradeId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_GradeId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "User");
        }
    }
}
