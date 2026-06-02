using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoleConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_role_permission_permission_id",
                table: "role_permission",
                column: "permission_id");

            migrationBuilder.AddForeignKey(
                name: "fk_role_permission_permissions_permission_id",
                table: "role_permission",
                column: "permission_id",
                principalTable: "permissions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permission_role_role_id",
                table: "role_permission",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_role_permission_permissions_permission_id",
                table: "role_permission");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permission_role_role_id",
                table: "role_permission");

            migrationBuilder.DropIndex(
                name: "ix_role_permission_permission_id",
                table: "role_permission");
        }
    }
}
