using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUtcSuffixToFields : Migration
    {
        /// <inheritdoc />
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.RenameColumn(
                name: "processed_on",
                table: "outbox_messages",
                newName: "processed_on_utc" );

            migrationBuilder.RenameColumn(
                name: "occured_on",
                table: "outbox_messages",
                newName: "occured_on_utc" );
        }

        /// <inheritdoc />
        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.RenameColumn(
                name: "processed_on_utc",
                table: "outbox_messages",
                newName: "processed_on" );

            migrationBuilder.RenameColumn(
                name: "occured_on_utc",
                table: "outbox_messages",
                newName: "occured_on" );
        }
    }
}
