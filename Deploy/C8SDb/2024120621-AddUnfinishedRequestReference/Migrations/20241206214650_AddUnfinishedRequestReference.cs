using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Domain.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddUnfinishedRequestReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "Unfinisheds",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unfinisheds_RequestId",
                table: "Unfinisheds",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Unfinisheds_Requests_RequestId",
                table: "Unfinisheds",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "ApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Unfinisheds_Requests_RequestId",
                table: "Unfinisheds");

            migrationBuilder.DropIndex(
                name: "IX_Unfinisheds_RequestId",
                table: "Unfinisheds");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "Unfinisheds");
        }
    }
}
