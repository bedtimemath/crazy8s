using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Domain.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class FixRequestNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestedClubs_Requests_ApplicationId",
                table: "RequestedClubs");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "RequestedClubs",
                newName: "RequestId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestedClubs_ApplicationId",
                table: "RequestedClubs",
                newName: "IX_RequestedClubs_RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestedClubs_Requests_RequestId",
                table: "RequestedClubs",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "RequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestedClubs_Requests_RequestId",
                table: "RequestedClubs");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                table: "RequestedClubs",
                newName: "ApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestedClubs_RequestId",
                table: "RequestedClubs",
                newName: "IX_RequestedClubs_ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestedClubs_Requests_ApplicationId",
                table: "RequestedClubs",
                column: "ApplicationId",
                principalTable: "Requests",
                principalColumn: "RequestId");
        }
    }
}
