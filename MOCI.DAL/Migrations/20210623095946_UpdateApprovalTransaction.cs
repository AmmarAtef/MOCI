using Microsoft.EntityFrameworkCore.Migrations;

namespace MOCI.DAL.Migrations
{
    public partial class UpdateApprovalTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ManagerId",
                table: "TimeSheetApprovals",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "TimeSheetApprovals",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeSheetApprovals_UserId",
                table: "TimeSheetApprovals",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSheetApprovals_User_UserId",
                table: "TimeSheetApprovals",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSheetApprovals_User_UserId",
                table: "TimeSheetApprovals");

            migrationBuilder.DropIndex(
                name: "IX_TimeSheetApprovals_UserId",
                table: "TimeSheetApprovals");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TimeSheetApprovals");

            migrationBuilder.AlterColumn<long>(
                name: "ManagerId",
                table: "TimeSheetApprovals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
