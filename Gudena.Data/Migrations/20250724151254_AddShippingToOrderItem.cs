using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gudena.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddShippingToOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders_OrderId",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "OrderItemId",
                table: "Shippings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingStatus",
                table: "Shippings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Payments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Shippings_OrderItemId",
                table: "Shippings",
                column: "OrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders_OrderId",
                table: "Payments",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shippings_OrderItems_OrderItemId",
                table: "Shippings",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders_OrderId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Shippings_OrderItems_OrderItemId",
                table: "Shippings");

            migrationBuilder.DropIndex(
                name: "IX_Shippings_OrderItemId",
                table: "Shippings");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "Shippings");

            migrationBuilder.DropColumn(
                name: "ShippingStatus",
                table: "Shippings");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders_OrderId",
                table: "Payments",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
