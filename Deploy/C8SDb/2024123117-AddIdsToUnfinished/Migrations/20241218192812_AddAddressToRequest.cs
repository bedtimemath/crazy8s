using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Domain.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressToRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlaceAddress1",
                table: "Requests",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlaceAddress2",
                table: "Requests",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlaceCity",
                table: "Requests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlacePostalCode",
                table: "Requests",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlaceState",
                table: "Requests",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlaceAddress1",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PlaceAddress2",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PlaceCity",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PlacePostalCode",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PlaceState",
                table: "Requests");
        }
    }
}
