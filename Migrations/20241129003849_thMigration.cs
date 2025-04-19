using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bigtoria.Migrations
{
    /// <inheritdoc />
    public partial class thMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Category",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldMaxLength: 9);

            migrationBuilder.AddColumn<bool>(
                name: "ShowFilter",
                table: "Category",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowFilter",
                table: "Category");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Category",
                type: "bit",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(9)",
                oldMaxLength: 9);
        }
    }
}
