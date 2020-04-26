using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopApi.Data.Migrations
{
    public partial class UpdateCartItemmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "ShoppingCartItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ShoppingCartItems",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ShoppingCartItems",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "ShoppingCartItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "ShoppingCartItems");
        }
    }
}
