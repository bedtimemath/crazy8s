using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Database.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUnfinisheds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "SubmittedOn",
                table: "Unfinisheds",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndPart01On",
                table: "Unfinisheds",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndPart02On",
                table: "Unfinisheds",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndPart03On",
                table: "Unfinisheds",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndPart04On",
                table: "Unfinisheds",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndPart05On",
                table: "Unfinisheds",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndPart01On",
                table: "Unfinisheds");

            migrationBuilder.DropColumn(
                name: "EndPart02On",
                table: "Unfinisheds");

            migrationBuilder.DropColumn(
                name: "EndPart03On",
                table: "Unfinisheds");

            migrationBuilder.DropColumn(
                name: "EndPart04On",
                table: "Unfinisheds");

            migrationBuilder.DropColumn(
                name: "EndPart05On",
                table: "Unfinisheds");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "SubmittedOn",
                table: "Unfinisheds",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);
        }
    }
}
