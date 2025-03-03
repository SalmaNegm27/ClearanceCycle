using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearanceCycle.DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "ClearanceReasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClearanceReasons", x => x.Id);
                });



            migrationBuilder.CreateTable(
                name: "StepApprovalGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentStepId = table.Column<int>(type: "int", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalGroupId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepApprovalGroups", x => x.Id);
                });






            migrationBuilder.CreateTable(
                name: "ClearanceRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ResigneeHrId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResigneeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClearanceReasonId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    ResignationFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastWorkingDayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    IsCanceled = table.Column<bool>(type: "bit", nullable: false),
                    StepApprovalGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClearanceRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClearanceRequests_ClearanceReasons_ClearanceReasonId",
                        column: x => x.ClearanceReasonId,
                        principalTable: "ClearanceReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClearanceRequests_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClearanceRequests_StepApprovalGroups_StepApprovalGroupId",
                        column: x => x.StepApprovalGroupId,
                        principalTable: "StepApprovalGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClearanceHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClearanceRequestId = table.Column<int>(type: "int", nullable: false),
                    ActionBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClearanceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClearanceHistories_ClearanceRequests_ClearanceRequestId",
                        column: x => x.ClearanceRequestId,
                        principalTable: "ClearanceRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateIndex(
                name: "IX_ClearanceHistories_ClearanceRequestId",
                table: "ClearanceHistories",
                column: "ClearanceRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ClearanceRequests_ClearanceReasonId",
                table: "ClearanceRequests",
                column: "ClearanceReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_ClearanceRequests_EmployeeId",
                table: "ClearanceRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClearanceRequests_StepApprovalGroupId",
                table: "ClearanceRequests",
                column: "StepApprovalGroupId");




        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropTable(
                name: "ClearanceHistories");


            migrationBuilder.DropTable(
                name: "ClearanceRequests");

            migrationBuilder.DropTable(
                name: "ClearanceReasons");


            migrationBuilder.DropTable(
                name: "StepApprovalGroups");


        }
    }
}
