using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class LogsAndInventories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //    migrationBuilder.DropForeignKey(
            //        name: "FK_ProductInventories_ShopCategories_ShopProductId",
            //        table: "ProductInventories");

            //    migrationBuilder.RenameColumn(
            //        name: "ShopProductId",
            //        table: "ProductInventories",
            //        newName: "ShopCategoryId");

            //    migrationBuilder.RenameIndex(
            //        name: "IX_ProductInventories_ShopProductId",
            //        table: "ProductInventories",
            //        newName: "IX_ProductInventories_ShopCategoryId");

            //    migrationBuilder.AddForeignKey(
            //        name: "FK_ProductInventories_ShopCategories_ShopCategoryId",
            //        table: "ProductInventories",
            //        column: "ShopCategoryId",
            //        principalTable: "ShopCategories",
            //        principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //    migrationBuilder.DropForeignKey(
            //        name: "FK_ProductInventories_ShopCategories_ShopCategoryId",
            //        table: "ProductInventories");

            //    migrationBuilder.RenameColumn(
            //        name: "ShopCategoryId",
            //        table: "ProductInventories",
            //        newName: "ShopProductId");

            //    migrationBuilder.RenameIndex(
            //        name: "IX_ProductInventories_ShopCategoryId",
            //        table: "ProductInventories",
            //        newName: "IX_ProductInventories_ShopProductId");

            //    migrationBuilder.AddForeignKey(
            //        name: "FK_ProductInventories_ShopCategories_ShopProductId",
            //        table: "ProductInventories",
            //        column: "ShopProductId",
            //        principalTable: "ShopCategories",
            //        principalColumn: "Id");
        }
    }
}
