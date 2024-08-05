using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Database.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderSkus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Skus",
                columns: table => new
                {
                    SkuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldSystemSkuId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Season = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
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
                name: "OrderSkus",
                columns: table => new
                {
                    OrdersOrderId = table.Column<int>(type: "int", nullable: false),
                    SkusSkuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSkus", x => new { x.OrdersOrderId, x.SkusSkuId });
                    table.ForeignKey(
                        name: "FK_OrderSkus_Orders_OrdersOrderId",
                        column: x => x.OrdersOrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderSkus_Skus_SkusSkuId",
                        column: x => x.SkusSkuId,
                        principalTable: "Skus",
                        principalColumn: "SkuId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_OrderSkus_SkusSkuId",
                table: "OrderSkus",
                column: "SkusSkuId");

            migrationBuilder.CreateIndex(
                name: "IX_Skus_OldSystemSkuId",
                table: "Skus",
                column: "OldSystemSkuId",
                unique: true,
                filter: "[OldSystemSkuId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderSkus");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Skus");
        }
    }
}
