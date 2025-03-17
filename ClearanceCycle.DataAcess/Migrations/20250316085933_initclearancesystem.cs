using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearanceCycle.DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class initclearancesystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                });

 
            migrationBuilder.CreateTable(
                name: "ClearanceReasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClearanceReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cycles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cycles", x => x.Id);
                });
  
            migrationBuilder.CreateTable(
                name: "StepApprovalGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentStepId = table.Column<int>(type: "int", nullable: true),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepApprovalGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Steps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovalGroupIds = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CycleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Steps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Steps_Cycles_CycleId",
                        column: x => x.CycleId,
                        principalTable: "Cycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

           
            migrationBuilder.CreateTable(
                name: "StepApprovalAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StepApprovalGroupId = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovalGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepApprovalAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StepApprovalAssignments_StepApprovalGroups_StepApprovalGroupId",
                        column: x => x.StepApprovalGroupId,
                        principalTable: "StepApprovalGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StepActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionId = table.Column<int>(type: "int", nullable: false),
                    CurrentStepId = table.Column<int>(type: "int", nullable: false),
                    NextStepId = table.Column<int>(type: "int", nullable: true),
                    NeedComment = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StepActions_Actions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "Actions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StepActions_Steps_CurrentStepId",
                        column: x => x.CurrentStepId,
                        principalTable: "Steps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
  
          
            migrationBuilder.CreateTable(
                name: "ClearanceRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ResigneeHrId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ResigneeName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ClearanceReasonId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    ResignationFileName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastWorkingDayDate = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    IsCanceled = table.Column<bool>(type: "bit", nullable: false),
                    DirectManagerHrid = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    SecondManagerHrId = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    StepApprovalGroupId = table.Column<int>(type: "int", nullable: false),
                    IsResigneeAccountsClosed = table.Column<bool>(type: "bit", nullable: false),
                    AccountsClosedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        name: "FK_ClearanceRequests_Employees_EmployeeId",
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
                name: "EscalationPointEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovalGroupEmployeeId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(20)", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ClearanceHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClearanceRequestId = table.Column<int>(type: "int", nullable: false),
                    ActionBy = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    ActionAt = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    ApprovalGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
               name: "IX_ClearanceRequests_IsFinished",
               table: "ClearanceRequests",
               column: "IsFinished");

            migrationBuilder.CreateIndex(
                name: "IX_ClearanceRequests_StepApprovalGroupId",
                table: "ClearanceRequests",
                column: "StepApprovalGroupId");


            migrationBuilder.CreateIndex(
                name: "IX_EscalationPointEmployees_ApprovalGroupEmployeeId",
                table: "EscalationPointEmployees",
                column: "ApprovalGroupEmployeeId");

         
            migrationBuilder.CreateIndex(
                name: "IX_StepActions_ActionId",
                table: "StepActions",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_StepActions_CurrentStepId",
                table: "StepActions",
                column: "CurrentStepId");

            migrationBuilder.CreateIndex(
                name: "IX_StepApprovalAssignments_StepApprovalGroupId",
                table: "StepApprovalAssignments",
                column: "StepApprovalGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Steps_CycleId",
                table: "Steps",
                column: "CycleId");

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClearanceHistories");

            migrationBuilder.DropTable(
                name: "EscalationPointEmployees");

            migrationBuilder.DropTable(
                name: "StepActions");

            migrationBuilder.DropTable(
                name: "StepApprovalAssignments");

            migrationBuilder.DropTable(
                name: "ClearanceRequests");


            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropTable(
                name: "Steps");

            migrationBuilder.DropTable(
                name: "ClearanceReasons");

            migrationBuilder.DropTable(
                name: "StepApprovalGroups");

     
            migrationBuilder.DropTable(
                name: "Cycles");

           


        }
    }
}
