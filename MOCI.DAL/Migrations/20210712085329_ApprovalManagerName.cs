using Microsoft.EntityFrameworkCore.Migrations;

namespace MOCI.DAL.Migrations
{
    public partial class ApprovalManagerName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManagerName",
                table: "TimeSheetApprovals",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagerName",
                table: "TimeSheetApprovals");
        }
    }
}
