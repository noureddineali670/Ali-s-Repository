using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupplierManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddItemLedgerEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemLedgerEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderItemId = table.Column<int>(type: "int", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemLedgerEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemLedgerEntries_PurchaseOrderItems_PurchaseOrderItemId",
                        column: x => x.PurchaseOrderItemId,
                        principalTable: "PurchaseOrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemLedgerEntries_PurchaseOrderItemId",
                table: "ItemLedgerEntries",
                column: "PurchaseOrderItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemLedgerEntries");
        }
    }
}
