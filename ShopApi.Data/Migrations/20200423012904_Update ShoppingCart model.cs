using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopApi.Data.Migrations
{
    public partial class UpdateShoppingCartmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ShoppingCarts");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreatedUtc",
                table: "ShoppingCarts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreatedUtc",
                table: "ShoppingCarts");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ShoppingCarts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
