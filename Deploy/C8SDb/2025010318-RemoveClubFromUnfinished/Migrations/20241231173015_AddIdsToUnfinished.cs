using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Domain.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddIdsToUnfinished : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClubId",
                table: "Unfinisheds",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "Unfinisheds",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "Unfinisheds",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "Unfinisheds");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Unfinisheds");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Unfinisheds");
        }
    }
}
