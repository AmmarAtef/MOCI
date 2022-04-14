using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MOCI.DAL.Migrations
{
    public partial class BaseActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Activities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "BaseActivityId",
                table: "Activities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BaseActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseActivities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_BaseActivityId",
                table: "Activities",
                column: "BaseActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_BaseActivities_BaseActivityId",
                table: "Activities",
                column: "BaseActivityId",
                principalTable: "BaseActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_BaseActivities_BaseActivityId",
                table: "Activities");

            migrationBuilder.DropTable(
                name: "BaseActivities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_BaseActivityId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "BaseActivityId",
                table: "Activities");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Activities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
