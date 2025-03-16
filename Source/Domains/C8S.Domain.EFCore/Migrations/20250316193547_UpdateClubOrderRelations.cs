using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Domain.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClubOrderRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_ClubId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClubId",
                table: "Orders",
                column: "ClubId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_ClubId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClubId",
                table: "Orders",
                column: "ClubId",
                unique: true,
                filter: "[ClubId] IS NOT NULL");
        }
    }
}
