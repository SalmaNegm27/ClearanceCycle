using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearanceCycle.DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class init5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "ClearanceRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");    

            migrationBuilder.CreateTable(
                name: "ClearanceSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsParallel = table.Column<bool>(type: "bit", nullable: false),
                    StepOrder = table.Column<int>(type: "int", nullable: false),
                    ApprovalGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClearanceSteps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClearanceStepStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    StepId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalGroupId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClearanceStepStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClearanceStepStatuses_ClearanceRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "ClearanceRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClearanceStepStatuses_ClearanceSteps_StepId",
                        column: x => x.StepId,
                        principalTable: "ClearanceSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClearanceStepStatuses_approvalGroups_ApprovalGroupId",
                        column: x => x.ApprovalGroupId,
                        principalTable: "approvalGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropTable(
                name: "ClearanceStepStatuses");

            migrationBuilder.DropTable(
                name: "ClearanceSteps");

           

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "ClearanceRequests");

        }
    }
}
