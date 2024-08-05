using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace C8S.Database.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderSkuTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderSkus_Orders_OrdersOrderId",
                table: "OrderSkus");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderSkus_Skus_SkusSkuId",
                table: "OrderSkus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderSkus",
                table: "OrderSkus");

            migrationBuilder.RenameColumn(
                name: "SkusSkuId",
                table: "OrderSkus",
                newName: "SkuId");

            migrationBuilder.RenameColumn(
                name: "OrdersOrderId",
                table: "OrderSkus",
                newName: "Quantity");

            migrationBuilder.RenameIndex(
                name: "IX_OrderSkus_SkusSkuId",
                table: "OrderSkus",
                newName: "IX_OrderSkus_SkuId");

            migrationBuilder.AddColumn<int>(
                name: "OrderSkuId",
                table: "OrderSkus",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "OrderSkus",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "OldSystemOrderId",
                table: "OrderSkus",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OldSystemOrderSkuId",
                table: "OrderSkus",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OldSystemSkuId",
                table: "OrderSkus",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "OrderSkus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ordinal",
                table: "OrderSkus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderSkus",
                table: "OrderSkus",
                column: "OrderSkuId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_OrderSkus_Orders_OrderId",
                table: "OrderSkus",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderSkus_Skus_SkuId",
                table: "OrderSkus",
                column: "SkuId",
                principalTable: "Skus",
                principalColumn: "SkuId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderSkus_Orders_OrderId",
                table: "OrderSkus");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderSkus_Skus_SkuId",
                table: "OrderSkus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderSkus",
                table: "OrderSkus");

            migrationBuilder.DropIndex(
                name: "IX_OrderSkus_OldSystemOrderSkuId",
                table: "OrderSkus");

            migrationBuilder.DropIndex(
                name: "IX_OrderSkus_OrderId",
                table: "OrderSkus");

            migrationBuilder.DropColumn(
                name: "OrderSkuId",
                table: "OrderSkus");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "OrderSkus");

            migrationBuilder.DropColumn(
                name: "OldSystemOrderId",
                table: "OrderSkus");

            migrationBuilder.DropColumn(
                name: "OldSystemOrderSkuId",
                table: "OrderSkus");

            migrationBuilder.DropColumn(
                name: "OldSystemSkuId",
                table: "OrderSkus");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrderSkus");

            migrationBuilder.DropColumn(
                name: "Ordinal",
                table: "OrderSkus");

            migrationBuilder.RenameColumn(
                name: "SkuId",
                table: "OrderSkus",
                newName: "SkusSkuId");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderSkus",
                newName: "OrdersOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderSkus_SkuId",
                table: "OrderSkus",
                newName: "IX_OrderSkus_SkusSkuId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderSkus",
                table: "OrderSkus",
                columns: new[] { "OrdersOrderId", "SkusSkuId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderSkus_Orders_OrdersOrderId",
                table: "OrderSkus",
                column: "OrdersOrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderSkus_Skus_SkusSkuId",
                table: "OrderSkus",
                column: "SkusSkuId",
                principalTable: "Skus",
                principalColumn: "SkuId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
