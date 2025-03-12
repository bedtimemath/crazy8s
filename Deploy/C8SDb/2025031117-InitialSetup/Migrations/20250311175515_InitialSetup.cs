using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Domain.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    PlaceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldSystemCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemOrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemPostalAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemUsaPostalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    TypeOther = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxIdentifier = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Line1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Line2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    ZIPCode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    IsMilitary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.PlaceId);
                    table.ForeignKey(
                        name: "FK_Places_Places_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Places",
                        principalColumn: "PlaceId");
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
                    Year = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Season = table.Column<int>(type: "int", nullable: true),
                    AgeLevel = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ClubSize = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skus", x => x.SkuId);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopCodes",
                columns: table => new
                {
                    WorkshopCodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartsOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EndsOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopCodes", x => x.WorkshopCodeId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldSystemCoachId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemOrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TimeZone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    JobTitleOther = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WordPressUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PlaceId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                    table.ForeignKey(
                        name: "FK_Persons_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "PlaceId");
                });

            migrationBuilder.CreateTable(
                name: "InvoicePersons",
                columns: table => new
                {
                    InvoicePersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicePersons", x => x.InvoicePersonId);
                    table.ForeignKey(
                        name: "FK_InvoicePersons_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoicePersons_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    SkuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                    table.ForeignKey(
                        name: "FK_Permissions_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                    table.ForeignKey(
                        name: "FK_Permissions_Skus_SkuId",
                        column: x => x.SkuId,
                        principalTable: "Skus",
                        principalColumn: "SkuId");
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldSystemApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemLinkedCoachId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemLinkedOrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PersonType = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PersonFirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PersonLastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PersonEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PersonPhone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PersonTimeZone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PlaceName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    PlaceAddress1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PlaceAddress2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PlaceCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PlaceState = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    PlacePostalCode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PlaceType = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PlaceTypeOther = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PlaceTaxIdentifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkshopCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FullSlateAppointmentId = table.Column<long>(type: "bigint", nullable: true),
                    FullSlateAppointmentStartsOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReferenceSource = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceSourceOther = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    SubmittedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    PlaceId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_Requests_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                    table.ForeignKey(
                        name: "FK_Requests_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "PlaceId");
                });

            migrationBuilder.CreateTable(
                name: "RequestedClubs",
                columns: table => new
                {
                    RequestedClubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldSystemApplicationClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldSystemLinkedClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AgeLevel = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ClubSize = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Season = table.Column<int>(type: "int", nullable: false),
                    StartsOn = table.Column<DateOnly>(type: "date", nullable: false),
                    RequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestedClubs", x => x.RequestedClubId);
                    table.ForeignKey(
                        name: "FK_RequestedClubs_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "RequestId");
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    SaleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    RequestId = table.Column<int>(type: "int", nullable: true),
                    InvoiceId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.SaleId);
                    table.ForeignKey(
                        name: "FK_Sales_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId");
                    table.ForeignKey(
                        name: "FK_Sales_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "PlaceId");
                    table.ForeignKey(
                        name: "FK_Sales_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "RequestId");
                });

            migrationBuilder.CreateTable(
                name: "Unfinisheds",
                columns: table => new
                {
                    UnfinishedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    PersonType = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PersonFirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PersonLastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PersonEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PersonPhone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PersonTimeZone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HasHostedBefore = table.Column<bool>(type: "bit", nullable: true),
                    AddressHasChanged = table.Column<bool>(type: "bit", nullable: true),
                    PlaceName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    PlaceAddress1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PlaceAddress2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PlaceCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PlaceState = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    PlacePostalCode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PlaceType = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PlaceTypeOther = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PlaceTaxIdentifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ClubsString = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    WorkshopCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChosenTimeSlot = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReferenceSource = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceSourceOther = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    EndPart01On = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EndPart02On = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EndPart03On = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EndPart04On = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SubmittedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    PlaceId = table.Column<int>(type: "int", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unfinisheds", x => x.UnfinishedId);
                    table.ForeignKey(
                        name: "FK_Unfinisheds_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "RequestId");
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
                    Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Year = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Season = table.Column<int>(type: "int", nullable: true),
                    AgeLevel = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ClubSize = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    StartsOn = table.Column<DateOnly>(type: "date", nullable: true),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.ClubId);
                    table.ForeignKey(
                        name: "FK_Clubs_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "PlaceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clubs_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "SaleId");
                });

            migrationBuilder.CreateTable(
                name: "SalePersons",
                columns: table => new
                {
                    SalePersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalePersons", x => x.SalePersonId);
                    table.ForeignKey(
                        name: "FK_SalePersons_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalePersons_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "SaleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClubPersons",
                columns: table => new
                {
                    ClubPersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubPersons", x => x.ClubPersonId);
                    table.ForeignKey(
                        name: "FK_ClubPersons_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubPersons_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId");
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
                    ContactName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Recipient = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Line1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Line2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    ZIPCode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    IsMilitary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    OrderedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ArriveBy = table.Column<DateOnly>(type: "date", nullable: true),
                    ShippedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EmailedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ClubId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId");
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    NoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reference = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: true),
                    InvoiceId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    PlaceId = table.Column<int>(type: "int", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: true),
                    SaleId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_Notes_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId");
                    table.ForeignKey(
                        name: "FK_Notes_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId");
                    table.ForeignKey(
                        name: "FK_Notes_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK_Notes_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                    table.ForeignKey(
                        name: "FK_Notes_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "PlaceId");
                    table.ForeignKey(
                        name: "FK_Notes_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "RequestId");
                    table.ForeignKey(
                        name: "FK_Notes_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "SaleId");
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
                    SkuId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    ShipmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackingNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShipMethod = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ShipMethodOther = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.ShipmentId);
                    table.ForeignKey(
                        name: "FK_Shipments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubPersons_ClubId",
                table: "ClubPersons",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubPersons_PersonId",
                table: "ClubPersons",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_OldSystemClubId",
                table: "Clubs",
                column: "OldSystemClubId",
                unique: true,
                filter: "[OldSystemClubId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_PlaceId",
                table: "Clubs",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_SaleId",
                table: "Clubs",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePersons_InvoiceId",
                table: "InvoicePersons",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePersons_PersonId",
                table: "InvoicePersons",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ClubId",
                table: "Notes",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_InvoiceId",
                table: "Notes",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_OrderId",
                table: "Notes",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_PersonId",
                table: "Notes",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_PlaceId",
                table: "Notes",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_RequestId",
                table: "Notes",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_SaleId",
                table: "Notes",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClubId",
                table: "Orders",
                column: "ClubId",
                unique: true,
                filter: "[ClubId] IS NOT NULL");

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
                name: "IX_Permissions_PersonId",
                table: "Permissions",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_SkuId",
                table: "Permissions",
                column: "SkuId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_OldSystemCoachId",
                table: "Persons",
                column: "OldSystemCoachId",
                unique: true,
                filter: "[OldSystemCoachId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_PlaceId",
                table: "Persons",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_OldSystemOrganizationId",
                table: "Places",
                column: "OldSystemOrganizationId",
                unique: true,
                filter: "[OldSystemOrganizationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Places_ParentId",
                table: "Places",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedClubs_OldSystemApplicationClubId",
                table: "RequestedClubs",
                column: "OldSystemApplicationClubId",
                unique: true,
                filter: "[OldSystemApplicationClubId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedClubs_RequestId",
                table: "RequestedClubs",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_OldSystemApplicationId",
                table: "Requests",
                column: "OldSystemApplicationId",
                unique: true,
                filter: "[OldSystemApplicationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_PersonId",
                table: "Requests",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_PlaceId",
                table: "Requests",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_SalePersons_PersonId",
                table: "SalePersons",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_SalePersons_SaleId",
                table: "SalePersons",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_InvoiceId",
                table: "Sales",
                column: "InvoiceId",
                unique: true,
                filter: "[InvoiceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_PlaceId",
                table: "Sales",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_RequestId",
                table: "Sales",
                column: "RequestId",
                unique: true,
                filter: "[RequestId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_OrderId",
                table: "Shipments",
                column: "OrderId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Unfinisheds_RequestId",
                table: "Unfinisheds",
                column: "RequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubPersons");

            migrationBuilder.DropTable(
                name: "InvoicePersons");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "OrderSkus");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "RequestedClubs");

            migrationBuilder.DropTable(
                name: "SalePersons");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "Unfinisheds");

            migrationBuilder.DropTable(
                name: "WorkshopCodes");

            migrationBuilder.DropTable(
                name: "Skus");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Clubs");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Places");
        }
    }
}
