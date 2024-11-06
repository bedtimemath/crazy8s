using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Database.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddModifiedOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "WorkshopCodes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Unfinisheds",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Skus",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Organizations",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "OrderSkus",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Orders",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Coaches",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Clubs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Applications",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "ApplicationClubs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Addresses",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "WorkshopCodes");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Unfinisheds");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Skus");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "OrderSkus");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "ApplicationClubs");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Addresses");
        }
    }
}
