using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnSinhSo.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AuthorizationInfrastructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSessions_CurrentTokenHash",
                table: "UserSessions");

            migrationBuilder.DropIndex(
                name: "IX_UserSessions_FamilyId",
                table: "UserSessions");

            migrationBuilder.DropIndex(
                name: "IX_UserSessions_UserId_ExpiresAtUtc",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "CurrentTokenHash",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "ExpiresAtUtc",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "RevokedReason",
                table: "UserSessions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserSessions",
                newName: "RefreshTokenFamilyId");

            migrationBuilder.RenameColumn(
                name: "SecurityStampSnapshot",
                table: "UserSessions",
                newName: "RefreshTokenHash");

            migrationBuilder.RenameColumn(
                name: "RevokedAtUtc",
                table: "UserSessions",
                newName: "RevokedAt");

            migrationBuilder.RenameColumn(
                name: "LastActivityUtc",
                table: "UserSessions",
                newName: "ExpiresAt");

            migrationBuilder.RenameColumn(
                name: "FamilyId",
                table: "UserSessions",
                newName: "CitizenIdentityId");

            migrationBuilder.AddColumn<string>(
                name: "RevokeReason",
                table: "UserSessions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CitizenIdentities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CitizenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrimaryPhone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FailedAttemptCount = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizenIdentities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtpVerifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CitizenIdentityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodeHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TargetPhone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FailedAttemptCount = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpVerifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsSystemRole = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityLinkedProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderType = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LinkedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CitizenIdentityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityLinkedProviders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityLinkedProviders_CitizenIdentities_CitizenIdentityId",
                        column: x => x.CitizenIdentityId,
                        principalTable: "CitizenIdentities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityTrustedDevices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TrustedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUsedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CitizenIdentityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityTrustedDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityTrustedDevices_CitizenIdentities_CitizenIdentityId",
                        column: x => x.CitizenIdentityId,
                        principalTable: "CitizenIdentities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PermissionGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_PermissionGroups_PermissionGroupId",
                        column: x => x.PermissionGroupId,
                        principalTable: "PermissionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CitizenIdentityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_CitizenIdentities_CitizenIdentityId",
                        column: x => x.CitizenIdentityId,
                        principalTable: "CitizenIdentities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_CitizenIdentityId_ExpiresAt",
                table: "UserSessions",
                columns: new[] { "CitizenIdentityId", "ExpiresAt" },
                filter: "[IsRevoked] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_RefreshTokenFamilyId",
                table: "UserSessions",
                column: "RefreshTokenFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_RefreshTokenHash",
                table: "UserSessions",
                column: "RefreshTokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CitizenIdentities_CitizenId",
                table: "CitizenIdentities",
                column: "CitizenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityLinkedProviders_CitizenIdentityId",
                table: "IdentityLinkedProviders",
                column: "CitizenIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityTrustedDevices_CitizenIdentityId",
                table: "IdentityTrustedDevices",
                column: "CitizenIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_OtpVerifications_CitizenIdentityId_Status",
                table: "OtpVerifications",
                columns: new[] { "CitizenIdentityId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroups_Code",
                table: "PermissionGroups",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Code",
                table: "Permissions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_PermissionGroupId",
                table: "Permissions",
                column: "PermissionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_CitizenIdentityId_RoleId",
                table: "UserRoles",
                columns: new[] { "CitizenIdentityId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityLinkedProviders");

            migrationBuilder.DropTable(
                name: "IdentityTrustedDevices");

            migrationBuilder.DropTable(
                name: "OtpVerifications");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "CitizenIdentities");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "PermissionGroups");

            migrationBuilder.DropIndex(
                name: "IX_UserSessions_CitizenIdentityId_ExpiresAt",
                table: "UserSessions");

            migrationBuilder.DropIndex(
                name: "IX_UserSessions_RefreshTokenFamilyId",
                table: "UserSessions");

            migrationBuilder.DropIndex(
                name: "IX_UserSessions_RefreshTokenHash",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "RevokeReason",
                table: "UserSessions");

            migrationBuilder.RenameColumn(
                name: "RevokedAt",
                table: "UserSessions",
                newName: "RevokedAtUtc");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenHash",
                table: "UserSessions",
                newName: "SecurityStampSnapshot");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenFamilyId",
                table: "UserSessions",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ExpiresAt",
                table: "UserSessions",
                newName: "LastActivityUtc");

            migrationBuilder.RenameColumn(
                name: "CitizenIdentityId",
                table: "UserSessions",
                newName: "FamilyId");

            migrationBuilder.AddColumn<string>(
                name: "CurrentTokenHash",
                table: "UserSessions",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAtUtc",
                table: "UserSessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RevokedReason",
                table: "UserSessions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_CurrentTokenHash",
                table: "UserSessions",
                column: "CurrentTokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_FamilyId",
                table: "UserSessions",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserId_ExpiresAtUtc",
                table: "UserSessions",
                columns: new[] { "UserId", "ExpiresAtUtc" },
                filter: "[IsRevoked] = 0");
        }
    }
}
