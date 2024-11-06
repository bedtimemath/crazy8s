using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Database.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddClubsString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClubsString",
                table: "Unfinisheds",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClubsString",
                table: "Unfinisheds");
        }
    }
}
