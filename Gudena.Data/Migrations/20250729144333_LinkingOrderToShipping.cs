using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gudena.Data.Migrations
{
    public partial class LinkingOrderToShipping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop old foreign key and index if any
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shippings_ShippingId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShippingId",
                table: "Orders");

            // Add nullable OrderId column first
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Shippings",
                type: "integer",
                nullable: true);

            // Populate OrderId from related OrderItem's OrderId using raw SQL
            migrationBuilder.Sql(
                @"UPDATE ""Shippings"" s
                  SET ""OrderId"" = oi.""OrderId""
                  FROM ""OrderItems"" oi
                  WHERE s.""OrderItemId"" = oi.""Id"";");

            // Check if there are any Shippings with null OrderId
            // If so, skip altering the column to non-nullable
            // Otherwise, make OrderId non-nullable
            migrationBuilder.Sql(
                @"DO $$
                BEGIN
                    IF EXISTS (SELECT 1 FROM ""Shippings"" WHERE ""OrderId"" IS NULL) THEN
                        RAISE NOTICE 'Skipping making OrderId NOT NULL because some values are NULL.';
                    ELSE
                        ALTER TABLE ""Shippings"" ALTER COLUMN ""OrderId"" SET NOT NULL;
                    END IF;
                END
                $$;");

            // Create index on OrderId
            migrationBuilder.CreateIndex(
                name: "IX_Shippings_OrderId",
                table: "Shippings",
                column: "OrderId");

            // Add foreign key constraint to Orders table with cascade delete
            migrationBuilder.AddForeignKey(
                name: "FK_Shippings_Orders_OrderId",
                table: "Shippings",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shippings_Orders_OrderId",
                table: "Shippings");

            migrationBuilder.DropIndex(
                name: "IX_Shippings_OrderId",
                table: "Shippings");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Shippings");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingId",
                table: "Orders",
                column: "ShippingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shippings_ShippingId",
                table: "Orders",
                column: "ShippingId",
                principalTable: "Shippings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
