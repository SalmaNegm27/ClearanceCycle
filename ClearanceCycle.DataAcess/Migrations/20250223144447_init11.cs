using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearanceCycle.DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class init11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StepTransitions");

            migrationBuilder.DropColumn(
                name: "ApprovalGroupId",
                table: "StepApprovalGroups");

            migrationBuilder.AlterColumn<string>(
                name: "ApprovalGroupIds",
                table: "Steps",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 300);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalGroupIds",
                table: "StepApprovalGroups",
                type: "nvarchar(300)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NextStepId",
                table: "StepActions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalGroupIds",
                table: "StepApprovalGroups");

            migrationBuilder.DropColumn(
                name: "NextStepId",
                table: "StepActions");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovalGroupIds",
                table: "Steps",
                type: "int",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AddColumn<int>(
                name: "ApprovalGroupId",
                table: "StepApprovalGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StepTransitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NextActionStepId = table.Column<int>(type: "int", nullable: false),
                    StepActionId = table.Column<int>(type: "int", nullable: false),
                    NextStepId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepTransitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StepTransitions_StepActions_StepActionId",
                        column: x => x.StepActionId,
                        principalTable: "StepActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StepTransitions_Steps_NextActionStepId",
                        column: x => x.NextActionStepId,
                        principalTable: "Steps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StepTransitions_NextActionStepId",
                table: "StepTransitions",
                column: "NextActionStepId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTransitions_StepActionId",
                table: "StepTransitions",
                column: "StepActionId");
        }
    }
}
