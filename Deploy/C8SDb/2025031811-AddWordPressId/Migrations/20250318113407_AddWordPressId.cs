using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Domain.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddWordPressId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WordPressUser",
                table: "Persons");

            migrationBuilder.AddColumn<int>(
                name: "WordPressId",
                table: "Persons",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WordPressId",
                table: "Persons");

            migrationBuilder.AddColumn<string>(
                name: "WordPressUser",
                table: "Persons",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
