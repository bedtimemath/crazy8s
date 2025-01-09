using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Domain.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressChangedToUnfinished : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AddressHasChanged",
                table: "Unfinisheds",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressHasChanged",
                table: "Unfinisheds");
        }
    }
}
