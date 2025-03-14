using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearanceCycle.DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class init26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SingleApprovalGroupId",
                table: "StepApprovalGroups");

            migrationBuilder.DropColumn(
                name: "MajorAreaId",
                table: "ApprovalGroupEmployees");

            migrationBuilder.AddColumn<int>(
                name: "MajourAreaId",
                table: "ApprovalGroupEmployees",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalGroupEmployees_MajourAreas_MajourAreaId",
                table: "ApprovalGroupEmployees",
                column: "MajourAreaId",
                principalTable: "MajourAreas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalGroupEmployees_MajourAreas_MajourAreaId",
                table: "ApprovalGroupEmployees");

            migrationBuilder.AddColumn<int>(
                name: "SingleApprovalGroupId",
                table: "StepApprovalGroups",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MajourAreaId",
                table: "ApprovalGroupEmployees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MajorAreaId",
                table: "ApprovalGroupEmployees",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalGroupEmployees_MajourAreas_MajourAreaId",
                table: "ApprovalGroupEmployees",
                column: "MajourAreaId",
                principalTable: "MajourAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
