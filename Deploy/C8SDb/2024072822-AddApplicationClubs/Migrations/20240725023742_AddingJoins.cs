using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Database.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddingJoins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Coaches",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_OrganizationId",
                table: "Coaches",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coaches_Organizations_OrganizationId",
                table: "Coaches",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coaches_Organizations_OrganizationId",
                table: "Coaches");

            migrationBuilder.DropIndex(
                name: "IX_Coaches_OrganizationId",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Coaches");
        }
    }
}
