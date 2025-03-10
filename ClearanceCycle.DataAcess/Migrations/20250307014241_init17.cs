using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearanceCycle.DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class init17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsParallel",
                table: "Steps");

            migrationBuilder.AddColumn<bool>(
                name: "NeedComment",
                table: "StepActions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedComment",
                table: "StepActions");

            migrationBuilder.AddColumn<bool>(
                name: "IsParallel",
                table: "Steps",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
