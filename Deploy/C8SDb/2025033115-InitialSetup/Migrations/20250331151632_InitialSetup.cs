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
                name: "KitPages",
                columns: table => new
                {
                    KitPageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitPages", x => x.KitPageId);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    OfferId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FulcoId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Year = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Season = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 8192, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.OfferId);
                });

            migrationBuilder.CreateTable(
                name: "OldNews",
                columns: table => new
                {
                    OldNewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldTableName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    OldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewTableName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OldNews", x => x.OldNewId);
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    PlaceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "Requests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkshopCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AppointmentId = table.Column<long>(type: "bigint", nullable: true),
                    AppointmentStartsOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReferenceSource = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceSourceOther = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ClubsRequested = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", maxLength: 8192, nullable: true),
                    SubmittedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.RequestId);
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
                    ClubsString = table.Column<string>(type: "nvarchar(max)", maxLength: 8192, nullable: true),
                    WorkshopCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChosenTimeSlot = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReferenceSource = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceSourceOther = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", maxLength: 8192, nullable: true),
                    EndPart01On = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EndPart02On = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EndPart03On = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EndPart04On = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SubmittedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unfinisheds", x => x.UnfinishedId);
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
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Comments = table.Column<string>(type: "nvarchar(max)", maxLength: 8192, nullable: true),
                    OrderedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ArriveBy = table.Column<DateOnly>(type: "date", nullable: true),
                    ShippedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EmailedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    InvoiceId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId");
                });

            migrationBuilder.CreateTable(
                name: "Kits",
                columns: table => new
                {
                    KitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false, computedColumnSql: "CASE WHEN [Version] IS NOT NULL THEN 'C8.S' + CAST([Season] AS VARCHAR) + '.' + [Year] + '.' + [Version] + '.' + RIGHT([AgeLevel],2) ELSE 'C8.S' + CAST([Season] AS VARCHAR) + '.' + [Year] + '.' + RIGHT([AgeLevel],2) END"),
                    Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Year = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Season = table.Column<int>(type: "int", nullable: false),
                    AgeLevel = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Version = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", maxLength: 8192, nullable: true),
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    KitPageId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kits", x => x.KitId);
                    table.ForeignKey(
                        name: "FK_Kits_KitPages_KitPageId",
                        column: x => x.KitPageId,
                        principalTable: "KitPages",
                        principalColumn: "KitPageId");
                    table.ForeignKey(
                        name: "FK_Kits_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "OfferId");
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TimeZone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    JobTitleOther = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WordPressId = table.Column<int>(type: "int", nullable: true),
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
                name: "Tickets",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_Tickets_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId");
                    table.ForeignKey(
                        name: "FK_Tickets_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "PlaceId");
                    table.ForeignKey(
                        name: "FK_Tickets_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderOffers",
                columns: table => new
                {
                    OrderOfferId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ordinal = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OfferId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderOffers", x => x.OrderOfferId);
                    table.ForeignKey(
                        name: "FK_OrderOffers_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "OfferId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderOffers_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
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

            migrationBuilder.CreateTable(
                name: "InvoicePersons",
                columns: table => new
                {
                    InvoicePersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ordinal = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
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
                    ExpiresOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    KitPageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                    table.ForeignKey(
                        name: "FK_Permissions_KitPages_KitPageId",
                        column: x => x.KitPageId,
                        principalTable: "KitPages",
                        principalColumn: "KitPageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permissions_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    ClubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    StartsOn = table.Column<DateOnly>(type: "date", nullable: true),
                    KitId = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    TicketId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.ClubId);
                    table.ForeignKey(
                        name: "FK_Clubs_Kits_KitId",
                        column: x => x.KitId,
                        principalTable: "Kits",
                        principalColumn: "KitId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clubs_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "PlaceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clubs_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId");
                });

            migrationBuilder.CreateTable(
                name: "TicketPersons",
                columns: table => new
                {
                    TicketPersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ordinal = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    TicketId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPersons", x => x.TicketPersonId);
                    table.ForeignKey(
                        name: "FK_TicketPersons_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketPersons_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClubPersons",
                columns: table => new
                {
                    ClubPersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ordinal = table.Column<int>(type: "int", nullable: false),
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
                name: "Notes",
                columns: table => new
                {
                    NoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reference = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 8192, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: true),
                    InvoiceId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    PlaceId = table.Column<int>(type: "int", nullable: true),
                    TicketId = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_Notes_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId");
                });

            migrationBuilder.CreateTable(
                name: "OrderClubs",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderClubs", x => new { x.OrderId, x.ClubId });
                    table.ForeignKey(
                        name: "FK_OrderClubs_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderClubs_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_Clubs_KitId",
                table: "Clubs",
                column: "KitId");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_PlaceId",
                table: "Clubs",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_TicketId",
                table: "Clubs",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePersons_InvoiceId",
                table: "InvoicePersons",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePersons_PersonId",
                table: "InvoicePersons",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_KitPages_Url",
                table: "KitPages",
                column: "Url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kits_KitPageId",
                table: "Kits",
                column: "KitPageId");

            migrationBuilder.CreateIndex(
                name: "IX_Kits_OfferId",
                table: "Kits",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Kits_Year_Season_AgeLevel_Version",
                table: "Kits",
                columns: new[] { "Year", "Season", "AgeLevel", "Version" },
                unique: true,
                filter: "[Version] IS NOT NULL");

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
                name: "IX_Notes_TicketId",
                table: "Notes",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_FulcoId",
                table: "Offers",
                column: "FulcoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_Year_Season_Version",
                table: "Offers",
                columns: new[] { "Year", "Season", "Version" },
                unique: true,
                filter: "[Version] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OldNews_NewTableName_NewId",
                table: "OldNews",
                columns: new[] { "NewTableName", "NewId" });

            migrationBuilder.CreateIndex(
                name: "IX_OldNews_OldTableName_OldId",
                table: "OldNews",
                columns: new[] { "OldTableName", "OldId" });

            migrationBuilder.CreateIndex(
                name: "IX_OldNews_OldTableName_OldId_NewTableName_NewId",
                table: "OldNews",
                columns: new[] { "OldTableName", "OldId", "NewTableName", "NewId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderClubs_ClubId",
                table: "OrderClubs",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderOffers_OfferId",
                table: "OrderOffers",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderOffers_OrderId",
                table: "OrderOffers",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_InvoiceId",
                table: "Orders",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_KitPageId",
                table: "Permissions",
                column: "KitPageId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_PersonId",
                table: "Permissions",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_Email",
                table: "Persons",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_PlaceId",
                table: "Persons",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_ParentId",
                table: "Places",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_OrderId",
                table: "Shipments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPersons_PersonId",
                table: "TicketPersons",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPersons_TicketId",
                table: "TicketPersons",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_InvoiceId",
                table: "Tickets",
                column: "InvoiceId",
                unique: true,
                filter: "[InvoiceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PlaceId",
                table: "Tickets",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_RequestId",
                table: "Tickets",
                column: "RequestId",
                unique: true);

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
                name: "ClubPersons");

            migrationBuilder.DropTable(
                name: "InvoicePersons");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "OldNews");

            migrationBuilder.DropTable(
                name: "OrderClubs");

            migrationBuilder.DropTable(
                name: "OrderOffers");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "TicketPersons");

            migrationBuilder.DropTable(
                name: "Unfinisheds");

            migrationBuilder.DropTable(
                name: "WorkshopCodes");

            migrationBuilder.DropTable(
                name: "Clubs");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Kits");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "KitPages");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Requests");
        }
    }
}
