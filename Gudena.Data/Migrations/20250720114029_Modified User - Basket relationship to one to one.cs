using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gudena.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedUserBasketrelationshiptoonetoone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Baskets_ApplicationUserId",
                table: "Baskets");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_ApplicationUserId",
                table: "Baskets",
                column: "ApplicationUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Baskets_ApplicationUserId",
                table: "Baskets");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_ApplicationUserId",
                table: "Baskets",
                column: "ApplicationUserId");
        }
    }
}
