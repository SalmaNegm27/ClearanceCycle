using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearanceCycle.DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class init15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DepartmentManagerHrid",
                table: "ClearanceRequests",
                newName: "SecondManagerHrId");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "SecondManagerHrId",
                table: "ClearanceRequests",
                newName: "DepartmentManagerHrid");
        }
    }
}
