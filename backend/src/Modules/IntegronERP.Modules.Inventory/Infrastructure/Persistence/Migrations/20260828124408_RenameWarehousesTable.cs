using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegronERP.Modules.Inventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameWarehousesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseStocks_warehouses_WarehouseId",
                table: "WarehouseStocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_warehouses",
                table: "warehouses");

            migrationBuilder.RenameTable(
                name: "warehouses",
                newName: "Warehouses");

            migrationBuilder.RenameIndex(
                name: "IX_warehouses_CompanyId_Code",
                table: "Warehouses",
                newName: "IX_Warehouses_CompanyId_Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseStocks_Warehouses_WarehouseId",
                table: "WarehouseStocks",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseStocks_Warehouses_WarehouseId",
                table: "WarehouseStocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses");

            migrationBuilder.RenameTable(
                name: "Warehouses",
                newName: "warehouses");

            migrationBuilder.RenameIndex(
                name: "IX_Warehouses_CompanyId_Code",
                table: "warehouses",
                newName: "IX_warehouses_CompanyId_Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_warehouses",
                table: "warehouses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseStocks_warehouses_WarehouseId",
                table: "WarehouseStocks",
                column: "WarehouseId",
                principalTable: "warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
