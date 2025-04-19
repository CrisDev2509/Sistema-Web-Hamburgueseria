using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bigtoria.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Client_ClientId",
                table: "Sales");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Sales",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Client_ClientId",
                table: "Sales",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Client_ClientId",
                table: "Sales");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Client_ClientId",
                table: "Sales",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
