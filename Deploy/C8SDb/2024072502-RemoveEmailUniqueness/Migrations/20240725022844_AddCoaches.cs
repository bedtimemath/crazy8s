using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Database.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddCoaches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Organizations_OldSystemCompanyId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_OldSystemOrganizationId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "AuthId",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "TagLine",
                table: "Coaches");

            migrationBuilder.AlterColumn<Guid>(
                name: "OldSystemOrganizationId",
                table: "Organizations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Coaches",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Coaches",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Coaches",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "OldSystemCoachId",
                table: "Coaches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OldSystemCompanyId",
                table: "Coaches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldSystemNotes",
                table: "Coaches",
                type: "nvarchar(max)",
                maxLength: 4096,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OldSystemOrganizationId",
                table: "Coaches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OldSystemUserId",
                table: "Coaches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneExt",
                table: "Coaches",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "Coaches",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OldSystemOrganizationId",
                table: "Organizations",
                column: "OldSystemOrganizationId",
                unique: true,
                filter: "[OldSystemOrganizationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_OldSystemCoachId",
                table: "Coaches",
                column: "OldSystemCoachId",
                unique: true,
                filter: "[OldSystemCoachId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Organizations_OldSystemOrganizationId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Coaches_OldSystemCoachId",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "OldSystemCoachId",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "OldSystemCompanyId",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "OldSystemNotes",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "OldSystemOrganizationId",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "OldSystemUserId",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "PhoneExt",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "Coaches");

            migrationBuilder.AlterColumn<Guid>(
                name: "OldSystemOrganizationId",
                table: "Organizations",
                type: "uniqueidentifier",
                maxLength: 512,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Coaches",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthId",
                table: "Coaches",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Coaches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Coaches",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Coaches",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Coaches",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TagLine",
                table: "Coaches",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OldSystemCompanyId",
                table: "Organizations",
                column: "OldSystemCompanyId",
                unique: true,
                filter: "[OldSystemCompanyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OldSystemOrganizationId",
                table: "Organizations",
                column: "OldSystemOrganizationId",
                unique: true);
        }
    }
}
