﻿using System;
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
                name: "Skus",
                columns: table => new
                {
                    SkuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldSystemSkuId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Season = table.Column<int>(type: "int", nullable: false),
                    AgeLevel = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ClubSize = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skus", x => x.SkuId);
                });

            migrationBuilder.CreateTable(
                name: "Unfinisheds",
                columns: table => new
                {
                    UnfinishedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ApplicantType = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ApplicantFirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ApplicantLastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ApplicantEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ApplicantPhone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ApplicantTimeZone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrganizationName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    OrganizationAddress1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OrganizationAddress2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OrganizationCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrganizationState = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    OrganizationPostalCode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    OrganizationType = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    OrganizationTypeOther = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrganizationTaxIdentifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkshopCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceSource = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceSourceOther = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    SubmittedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unfinisheds", x => x.UnfinishedId);
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
                    ReferenceSource = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceSourceOther = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
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

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldSystemOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemShippingAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ContactPhoneExt = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    OrderedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ArriveBy = table.Column<DateOnly>(type: "date", nullable: false),
                    ShippedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EmailedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    BatchIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
                    table.ForeignKey(
                        name: "FK_Orders_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId");
                });

            migrationBuilder.CreateTable(
                name: "OrderSkus",
                columns: table => new
                {
                    OrderSkuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldSystemOrderSkuId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemSkuId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Ordinal = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<short>(type: "smallint", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    SkuId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSkus", x => x.OrderSkuId);
                    table.ForeignKey(
                        name: "FK_OrderSkus_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderSkus_Skus_SkuId",
                        column: x => x.SkuId,
                        principalTable: "Skus",
                        principalColumn: "SkuId",
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
                name: "IX_Orders_AddressId",
                table: "Orders",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClubId",
                table: "Orders",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OldSystemOrderId",
                table: "Orders",
                column: "OldSystemOrderId",
                unique: true,
                filter: "[OldSystemOrderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSkus_OldSystemOrderSkuId",
                table: "OrderSkus",
                column: "OldSystemOrderSkuId",
                unique: true,
                filter: "[OldSystemOrderSkuId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSkus_OrderId",
                table: "OrderSkus",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSkus_SkuId",
                table: "OrderSkus",
                column: "SkuId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Skus_OldSystemSkuId",
                table: "Skus",
                column: "OldSystemSkuId",
                unique: true,
                filter: "[OldSystemSkuId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unfinisheds_Code",
                table: "Unfinisheds",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationClubs");

            migrationBuilder.DropTable(
                name: "OrderSkus");

            migrationBuilder.DropTable(
                name: "Unfinisheds");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Skus");

            migrationBuilder.DropTable(
                name: "Clubs");

            migrationBuilder.DropTable(
                name: "Coaches");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
