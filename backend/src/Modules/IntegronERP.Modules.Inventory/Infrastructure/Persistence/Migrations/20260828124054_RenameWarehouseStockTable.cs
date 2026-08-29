using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegronERP.Modules.Inventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameWarehouseStockTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_warehouse_stocks_Products_ProductId",
                table: "warehouse_stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_warehouse_stocks_warehouses_WarehouseId",
                table: "warehouse_stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_warehouse_stocks",
                table: "warehouse_stocks");

            migrationBuilder.RenameTable(
                name: "warehouse_stocks",
                newName: "WarehouseStocks");

            migrationBuilder.RenameIndex(
                name: "IX_warehouse_stocks_WarehouseId",
                table: "WarehouseStocks",
                newName: "IX_WarehouseStocks_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_warehouse_stocks_ProductId",
                table: "WarehouseStocks",
                newName: "IX_WarehouseStocks_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_warehouse_stocks_CompanyId_ProductId_WarehouseId",
                table: "WarehouseStocks",
                newName: "IX_WarehouseStocks_CompanyId_ProductId_WarehouseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseStocks",
                table: "WarehouseStocks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseStocks_Products_ProductId",
                table: "WarehouseStocks",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseStocks_warehouses_WarehouseId",
                table: "WarehouseStocks",
                column: "WarehouseId",
                principalTable: "warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseStocks_Products_ProductId",
                table: "WarehouseStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseStocks_warehouses_WarehouseId",
                table: "WarehouseStocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseStocks",
                table: "WarehouseStocks");

            migrationBuilder.RenameTable(
                name: "WarehouseStocks",
                newName: "warehouse_stocks");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseStocks_WarehouseId",
                table: "warehouse_stocks",
                newName: "IX_warehouse_stocks_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseStocks_ProductId",
                table: "warehouse_stocks",
                newName: "IX_warehouse_stocks_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseStocks_CompanyId_ProductId_WarehouseId",
                table: "warehouse_stocks",
                newName: "IX_warehouse_stocks_CompanyId_ProductId_WarehouseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_warehouse_stocks",
                table: "warehouse_stocks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_warehouse_stocks_Products_ProductId",
                table: "warehouse_stocks",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_warehouse_stocks_warehouses_WarehouseId",
                table: "warehouse_stocks",
                column: "WarehouseId",
                principalTable: "warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
