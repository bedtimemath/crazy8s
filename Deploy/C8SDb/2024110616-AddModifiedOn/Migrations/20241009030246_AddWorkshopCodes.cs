using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Database.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkshopCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ChosenTimeSlot",
                table: "Unfinisheds",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkshopCodes",
                columns: table => new
                {
                    WorkshopCodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartsOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EndsOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopCodes", x => x.WorkshopCodeId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkshopCodes");

            migrationBuilder.DropColumn(
                name: "ChosenTimeSlot",
                table: "Unfinisheds");
        }
    }
}
