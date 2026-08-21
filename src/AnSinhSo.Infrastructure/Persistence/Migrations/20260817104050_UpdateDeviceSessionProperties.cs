using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnSinhSo.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeviceSessionProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastSeen",
                table: "DeviceSessions",
                newName: "LastActivityAt");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "DeviceSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RevokedAt",
                table: "DeviceSessions",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "DeviceSessions");

            migrationBuilder.DropColumn(
                name: "RevokedAt",
                table: "DeviceSessions");

            migrationBuilder.RenameColumn(
                name: "LastActivityAt",
                table: "DeviceSessions",
                newName: "LastSeen");
        }
    }
}
