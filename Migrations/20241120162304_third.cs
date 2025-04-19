using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bigtoria.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SaleDetail_ProductId",
                table: "SaleDetail");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetail_ProductId",
                table: "SaleDetail",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SaleDetail_ProductId",
                table: "SaleDetail");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetail_ProductId",
                table: "SaleDetail",
                column: "ProductId",
                unique: true);
        }
    }
}
