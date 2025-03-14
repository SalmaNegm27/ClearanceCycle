using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearanceCycle.DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class init25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsResigneeAccountsColosed",
                table: "ClearanceRequests",
                newName: "IsResigneeAccountsClosed");

            migrationBuilder.RenameColumn(
                name: "AccoountsClosedAt",
                table: "ClearanceRequests",
                newName: "AccountsClosedAt");

            migrationBuilder.AddColumn<int>(
                name: "SingleApprovalGroupId",
                table: "StepApprovalGroups",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SingleApprovalGroupId",
                table: "StepApprovalGroups");

            migrationBuilder.RenameColumn(
                name: "IsResigneeAccountsClosed",
                table: "ClearanceRequests",
                newName: "IsResigneeAccountsColosed");

            migrationBuilder.RenameColumn(
                name: "AccountsClosedAt",
                table: "ClearanceRequests",
                newName: "AccoountsClosedAt");
        }
    }
}
