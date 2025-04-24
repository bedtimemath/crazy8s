using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SC.Audit.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataChanges",
                columns: table => new
                {
                    DataChangeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    EntityName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EntityState = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PropertiesJson = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataChanges", x => x.DataChangeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataChanges_EntityId",
                table: "DataChanges",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_DataChanges_EntityName",
                table: "DataChanges",
                column: "EntityName");

            migrationBuilder.CreateIndex(
                name: "IX_DataChanges_EntityState",
                table: "DataChanges",
                column: "EntityState");

            migrationBuilder.CreateIndex(
                name: "IX_DataChanges_Identifier",
                table: "DataChanges",
                column: "Identifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataChanges");
        }
    }
}
