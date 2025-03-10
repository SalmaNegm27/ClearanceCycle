using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearanceCycle.DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class init20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<DateTime>(
                name: "AccoountsClosedAt",
                table: "ClearanceRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddColumn<bool>(
                name: "IsResigneeAccountsColosed",
                table: "ClearanceRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccoountsClosedAt",
                table: "ClearanceRequests");

            migrationBuilder.DropColumn(
                name: "IsResigneeAccountsColosed",
                table: "ClearanceRequests");

            
        }
    }
}
