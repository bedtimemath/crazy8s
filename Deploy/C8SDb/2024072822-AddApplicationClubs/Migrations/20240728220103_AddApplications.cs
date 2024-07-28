using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Database.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldSystemApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ApplicantType = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ApplicantFirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ApplicantLastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ApplicantEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ApplicantPhone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ApplicantPhoneExt = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ApplicantTimeZone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrganizationName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    OrganizationType = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    OrganizationTypeOther = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrganizationTaxIdentifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkshopCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    SubmittedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    OldSystemNotes = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    LinkedCoachId = table.Column<int>(type: "int", nullable: true),
                    LinkedOrganizationId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK_Applications_Coaches_LinkedCoachId",
                        column: x => x.LinkedCoachId,
                        principalTable: "Coaches",
                        principalColumn: "CoachId");
                    table.ForeignKey(
                        name: "FK_Applications_Organizations_LinkedOrganizationId",
                        column: x => x.LinkedOrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_LinkedCoachId",
                table: "Applications",
                column: "LinkedCoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_LinkedOrganizationId",
                table: "Applications",
                column: "LinkedOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_OldSystemApplicationId",
                table: "Applications",
                column: "OldSystemApplicationId",
                unique: true,
                filter: "[OldSystemApplicationId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");
        }
    }
}
