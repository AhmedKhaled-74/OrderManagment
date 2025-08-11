using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "OrderNumberSequence");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    itemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    itemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    itemPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    itemTotalQuantity = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.itemId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    orderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    orderNumber = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValueSql: "CONCAT('Order_', YEAR(GETDATE()), '_', NEXT VALUE FOR dbo.OrderNumberSequence)"),
                    orderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    customerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.orderId);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    orderItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    orderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    itemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    itemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    quantity = table.Column<long>(type: "bigint", nullable: false),
                    unitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.orderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Items_itemId",
                        column: x => x.itemId,
                        principalTable: "Items",
                        principalColumn: "itemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_orderId",
                        column: x => x.orderId,
                        principalTable: "Orders",
                        principalColumn: "orderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "itemId", "itemName", "itemPrice", "itemTotalQuantity" },
                values: new object[,]
                {
                    { new Guid("65811328-1c98-4f2d-ba52-17cc34285225"), "Air Pods", 149.99m, 200L },
                    { new Guid("71721afe-3d15-4e24-bbf0-d19d90130b6d"), "Laptop Omine", 999.99m, 50L },
                    { new Guid("8b241257-bfc6-4fdb-ba3c-d5cbce698fd9"), "Mouse Copra M11", 29.99m, 300L },
                    { new Guid("d60609e0-7915-4dc1-808b-c9f31b2d8b50"), "Keyboard Logitech 250x4b4", 79.99m, 150L },
                    { new Guid("d74d4ef8-4efa-4865-9f15-52243c3320f7"), "IPhone15", 699.99m, 100L }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "orderId", "customerName", "orderDate" },
                values: new object[,]
                {
                    { new Guid("27786645-a44a-494b-89ca-c30588ddc137"), "Jane Smith", new DateTime(2023, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("4af1776a-a87f-4f76-9418-18d84002e439"), "John Doe", new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "orderItemId", "itemId", "itemName", "orderId", "quantity", "unitPrice" },
                values: new object[,]
                {
                    { new Guid("061d2367-e16b-4d23-9ece-e1a287f7f615"), new Guid("d74d4ef8-4efa-4865-9f15-52243c3320f7"), "IPhone15", new Guid("4af1776a-a87f-4f76-9418-18d84002e439"), 1L, 699.99m },
                    { new Guid("12eb0d68-3d4f-4378-abb4-767720838a6e"), new Guid("71721afe-3d15-4e24-bbf0-d19d90130b6d"), "Laptop Omine", new Guid("4af1776a-a87f-4f76-9418-18d84002e439"), 2L, 999.99m },
                    { new Guid("86075d69-54e4-4a96-b474-7de4b6215c5b"), new Guid("d74d4ef8-4efa-4865-9f15-52243c3320f7"), "IPhone15", new Guid("27786645-a44a-494b-89ca-c30588ddc137"), 1L, 699.99m },
                    { new Guid("b38cbdb5-fb2e-4e77-acc2-6f23e09936d3"), new Guid("d60609e0-7915-4dc1-808b-c9f31b2d8b50"), "Keyboard Logitech 250x4b4", new Guid("4af1776a-a87f-4f76-9418-18d84002e439"), 2L, 79.99m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_itemId",
                table: "OrderItems",
                column: "itemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_orderId",
                table: "OrderItems",
                column: "orderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropSequence(
                name: "OrderNumberSequence");
        }
    }
}
