using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gudena.Data.Migrations
{
    /// <inheritdoc />
    public partial class LinkShippingtoUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Shippings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Shippings_ApplicationUserId",
                table: "Shippings",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shippings_AspNetUsers_ApplicationUserId",
                table: "Shippings",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shippings_AspNetUsers_ApplicationUserId",
                table: "Shippings");

            migrationBuilder.DropIndex(
                name: "IX_Shippings_ApplicationUserId",
                table: "Shippings");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Shippings");
        }
    }
}
