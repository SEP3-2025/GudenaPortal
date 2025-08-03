using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gudena.Data.Migrations
{
    /// <inheritdoc />
    public partial class LinkShippingtoBusinessUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessUserId",
                table: "Shippings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Shippings_BusinessUserId",
                table: "Shippings",
                column: "BusinessUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shippings_AspNetUsers_BusinessUserId",
                table: "Shippings",
                column: "BusinessUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shippings_AspNetUsers_BusinessUserId",
                table: "Shippings");

            migrationBuilder.DropIndex(
                name: "IX_Shippings_BusinessUserId",
                table: "Shippings");

            migrationBuilder.DropColumn(
                name: "BusinessUserId",
                table: "Shippings");
        }
    }
}
