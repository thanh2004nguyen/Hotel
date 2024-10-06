using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAmenitiesThemes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Amenities_AmenitiesThemes_AmenitiesThemeId",
                table: "Amenities");

            migrationBuilder.DropTable(
                name: "AmenitiesRoom");

            migrationBuilder.DropTable(
                name: "AmenitiesThemeRoom");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "AmenitiesThemes",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AmenitiesThemeId",
                table: "Amenities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AmenitiesThemes_RoomId",
                table: "AmenitiesThemes",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_RoomId",
                table: "Amenities",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Amenities_AmenitiesThemes_AmenitiesThemeId",
                table: "Amenities",
                column: "AmenitiesThemeId",
                principalTable: "AmenitiesThemes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Amenities_Rooms_RoomId",
                table: "Amenities",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AmenitiesThemes_Rooms_RoomId",
                table: "AmenitiesThemes",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Amenities_AmenitiesThemes_AmenitiesThemeId",
                table: "Amenities");

            migrationBuilder.DropForeignKey(
                name: "FK_Amenities_Rooms_RoomId",
                table: "Amenities");

            migrationBuilder.DropForeignKey(
                name: "FK_AmenitiesThemes_Rooms_RoomId",
                table: "AmenitiesThemes");

            migrationBuilder.DropIndex(
                name: "IX_AmenitiesThemes_RoomId",
                table: "AmenitiesThemes");

            migrationBuilder.DropIndex(
                name: "IX_Amenities_RoomId",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "AmenitiesThemes");

            migrationBuilder.AlterColumn<int>(
                name: "AmenitiesThemeId",
                table: "Amenities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "AmenitiesRoom",
                columns: table => new
                {
                    AmenitiesId = table.Column<int>(type: "int", nullable: false),
                    RoomsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmenitiesRoom", x => new { x.AmenitiesId, x.RoomsId });
                    table.ForeignKey(
                        name: "FK_AmenitiesRoom_Amenities_AmenitiesId",
                        column: x => x.AmenitiesId,
                        principalTable: "Amenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AmenitiesRoom_Rooms_RoomsId",
                        column: x => x.RoomsId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AmenitiesThemeRoom",
                columns: table => new
                {
                    AmenitiesThemesId = table.Column<int>(type: "int", nullable: false),
                    RoomsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmenitiesThemeRoom", x => new { x.AmenitiesThemesId, x.RoomsId });
                    table.ForeignKey(
                        name: "FK_AmenitiesThemeRoom_AmenitiesThemes_AmenitiesThemesId",
                        column: x => x.AmenitiesThemesId,
                        principalTable: "AmenitiesThemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AmenitiesThemeRoom_Rooms_RoomsId",
                        column: x => x.RoomsId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AmenitiesRoom_RoomsId",
                table: "AmenitiesRoom",
                column: "RoomsId");

            migrationBuilder.CreateIndex(
                name: "IX_AmenitiesThemeRoom_RoomsId",
                table: "AmenitiesThemeRoom",
                column: "RoomsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Amenities_AmenitiesThemes_AmenitiesThemeId",
                table: "Amenities",
                column: "AmenitiesThemeId",
                principalTable: "AmenitiesThemes",
                principalColumn: "Id");
        }
    }
}
