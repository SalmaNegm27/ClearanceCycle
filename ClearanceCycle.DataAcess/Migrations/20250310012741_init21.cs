using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearanceCycle.DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class init21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StepApprovalGroups",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "EscalationPointEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovalGroupEmployeeId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscalationPointEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EscalationPointEmployees_ApprovalGroupEmployees_ApprovalGroupEmployeeId",
                        column: x => x.ApprovalGroupEmployeeId,
                        principalTable: "ApprovalGroupEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EscalationPointEmployees_ApprovalGroupEmployeeId",
                table: "EscalationPointEmployees",
                column: "ApprovalGroupEmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EscalationPointEmployees");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StepApprovalGroups");
        }
    }
}
