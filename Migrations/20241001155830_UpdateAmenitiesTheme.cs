using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAmenitiesTheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomUnities");

            migrationBuilder.AddColumn<int>(
                name: "AmenitiesThemeId",
                table: "IconClasses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AmenitiesThemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmenitiesThemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    IconClassId = table.Column<int>(type: "int", nullable: true),
                    AmenitiesThemeId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Amenities_AmenitiesThemes_AmenitiesThemeId",
                        column: x => x.AmenitiesThemeId,
                        principalTable: "AmenitiesThemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Amenities_IconClasses_IconClassId",
                        column: x => x.IconClassId,
                        principalTable: "IconClasses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Amenities_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_IconClasses_AmenitiesThemeId",
                table: "IconClasses",
                column: "AmenitiesThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_AmenitiesThemeId",
                table: "Amenities",
                column: "AmenitiesThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_IconClassId",
                table: "Amenities",
                column: "IconClassId",
                unique: true,
                filter: "[IconClassId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_RoomId",
                table: "Amenities",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_IconClasses_AmenitiesThemes_AmenitiesThemeId",
                table: "IconClasses",
                column: "AmenitiesThemeId",
                principalTable: "AmenitiesThemes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IconClasses_AmenitiesThemes_AmenitiesThemeId",
                table: "IconClasses");

            migrationBuilder.DropTable(
                name: "Amenities");

            migrationBuilder.DropTable(
                name: "AmenitiesThemes");

            migrationBuilder.DropIndex(
                name: "IX_IconClasses_AmenitiesThemeId",
                table: "IconClasses");

            migrationBuilder.DropColumn(
                name: "AmenitiesThemeId",
                table: "IconClasses");

            migrationBuilder.CreateTable(
                name: "RoomUnities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IconClassId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomUnities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomUnities_IconClasses_IconClassId",
                        column: x => x.IconClassId,
                        principalTable: "IconClasses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoomUnities_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomUnities_IconClassId",
                table: "RoomUnities",
                column: "IconClassId",
                unique: true,
                filter: "[IconClassId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoomUnities_RoomId",
                table: "RoomUnities",
                column: "RoomId");
        }
    }
}
