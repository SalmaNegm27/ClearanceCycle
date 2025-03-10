using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearanceCycle.DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class init16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StepApprovalGroupApproval");

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

            migrationBuilder.CreateIndex(
                name: "IX_StepApprovalAssignments_StepApprovalGroupId",
                table: "StepApprovalAssignments",
                column: "StepApprovalGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StepApprovalAssignments");

            migrationBuilder.CreateTable(
                name: "StepApprovalGroupApproval",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StepApprovalGroupId = table.Column<int>(type: "int", nullable: false),
                    ApprovalGroupId = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepApprovalGroupApproval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StepApprovalGroupApproval_StepApprovalGroups_StepApprovalGroupId",
                        column: x => x.StepApprovalGroupId,
                        principalTable: "StepApprovalGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StepApprovalGroupApproval_StepApprovalGroupId",
                table: "StepApprovalGroupApproval",
                column: "StepApprovalGroupId");
        }
    }
}
