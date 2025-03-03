using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearanceCycle.DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class init10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextStepId",
                table: "StepActions");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovalGroupIds",
                table: "Steps",
                type: "int",
                maxLength: 300,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsParallel",
                table: "Steps",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "StepTransitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StepActionId = table.Column<int>(type: "int", nullable: false),
                    NextActionStepId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepTransitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StepTransitions_StepActions_StepActionId",
                        column: x => x.StepActionId,
                        principalTable: "StepActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_StepTransitions_Steps_NextActionStepId",
                        column: x => x.NextActionStepId,
                        principalTable: "Steps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StepTransitions");

            migrationBuilder.DropColumn(
                name: "IsParallel",
                table: "Steps");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovalGroupIds",
                table: "Steps",
                type: "int",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 300);

            migrationBuilder.AddColumn<int>(
                name: "NextStepId",
                table: "StepActions",
                type: "int",
                nullable: true);
        }
    }
}
