using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Domain.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEndPart05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndPart05On",
                table: "Unfinisheds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndPart05On",
                table: "Unfinisheds",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
