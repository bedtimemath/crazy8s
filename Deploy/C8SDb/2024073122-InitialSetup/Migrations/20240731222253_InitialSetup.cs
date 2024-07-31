using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Database.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldSystemUsaPostalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RecipientName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BusinessName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StreetAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsMilitary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldSystemCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemOrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemPostalAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Culture = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    TypeOther = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxIdentifier = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.OrganizationId);
                    table.ForeignKey(
                        name: "FK_Organizations_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
                });

            migrationBuilder.CreateTable(
                name: "Coaches",
                columns: table => new
                {
                    CoachId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldSystemCoachId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemOrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PhoneExt = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coaches", x => x.CoachId);
                    table.ForeignKey(
                        name: "FK_Coaches_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId");
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldSystemApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemLinkedCoachId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemLinkedOrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ApplicantType = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ApplicantFirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ApplicantLastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ApplicantEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ApplicantPhone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ApplicantPhoneExt = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ApplicantTimeZone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrganizationName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    OrganizationType = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    OrganizationTypeOther = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrganizationTaxIdentifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkshopCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    SubmittedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsCoachRemoved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsOrganizationRemoved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    LinkedCoachId = table.Column<int>(type: "int", nullable: true),
                    LinkedOrganizationId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK_Applications_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
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

            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    ClubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldSystemClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemOrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemCoachId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemMeetingAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AgeLevel = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ClubSize = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Season = table.Column<int>(type: "int", nullable: false),
                    StartsOn = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    CoachId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.ClubId);
                    table.ForeignKey(
                        name: "FK_Clubs_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
                    table.ForeignKey(
                        name: "FK_Clubs_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "CoachId");
                    table.ForeignKey(
                        name: "FK_Clubs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationClubs",
                columns: table => new
                {
                    ApplicationClubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldSystemApplicationClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemLinkedClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AgeLevel = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ClubSize = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Season = table.Column<int>(type: "int", nullable: false),
                    StartsOn = table.Column<DateOnly>(type: "date", nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationClubs", x => x.ApplicationClubId);
                    table.ForeignKey(
                        name: "FK_ApplicationClubs_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_OldSystemUsaPostalId",
                table: "Addresses",
                column: "OldSystemUsaPostalId",
                unique: true,
                filter: "[OldSystemUsaPostalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationClubs_ApplicationId",
                table: "ApplicationClubs",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationClubs_OldSystemApplicationClubId",
                table: "ApplicationClubs",
                column: "OldSystemApplicationClubId",
                unique: true,
                filter: "[OldSystemApplicationClubId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_AddressId",
                table: "Applications",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");

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

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_AddressId",
                table: "Clubs",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_CoachId",
                table: "Clubs",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_OldSystemClubId",
                table: "Clubs",
                column: "OldSystemClubId",
                unique: true,
                filter: "[OldSystemClubId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_OrganizationId",
                table: "Clubs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_OldSystemCoachId",
                table: "Coaches",
                column: "OldSystemCoachId",
                unique: true,
                filter: "[OldSystemCoachId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_OrganizationId",
                table: "Coaches",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_AddressId",
                table: "Organizations",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OldSystemOrganizationId",
                table: "Organizations",
                column: "OldSystemOrganizationId",
                unique: true,
                filter: "[OldSystemOrganizationId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationClubs");

            migrationBuilder.DropTable(
                name: "Clubs");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Coaches");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
