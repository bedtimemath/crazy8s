using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Database.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddHostedBefore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasDoneBefore",
                table: "Unfinisheds",
                newName: "HasHostedBefore");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasHostedBefore",
                table: "Unfinisheds",
                newName: "HasDoneBefore");
        }
    }
}
