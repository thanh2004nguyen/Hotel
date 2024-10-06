using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAmenitiesTheme2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IconClasses_AmenitiesThemes_AmenitiesThemeId",
                table: "IconClasses");

            migrationBuilder.DropIndex(
                name: "IX_IconClasses_AmenitiesThemeId",
                table: "IconClasses");

            migrationBuilder.DropColumn(
                name: "AmenitiesThemeId",
                table: "IconClasses");

            migrationBuilder.AddColumn<int>(
                name: "IconClassId",
                table: "AmenitiesThemes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AmenitiesThemes_IconClassId",
                table: "AmenitiesThemes",
                column: "IconClassId",
                unique: true,
                filter: "[IconClassId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AmenitiesThemes_IconClasses_IconClassId",
                table: "AmenitiesThemes",
                column: "IconClassId",
                principalTable: "IconClasses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AmenitiesThemes_IconClasses_IconClassId",
                table: "AmenitiesThemes");

            migrationBuilder.DropIndex(
                name: "IX_AmenitiesThemes_IconClassId",
                table: "AmenitiesThemes");

            migrationBuilder.DropColumn(
                name: "IconClassId",
                table: "AmenitiesThemes");

            migrationBuilder.AddColumn<int>(
                name: "AmenitiesThemeId",
                table: "IconClasses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IconClasses_AmenitiesThemeId",
                table: "IconClasses",
                column: "AmenitiesThemeId");

            migrationBuilder.AddForeignKey(
                name: "FK_IconClasses_AmenitiesThemes_AmenitiesThemeId",
                table: "IconClasses",
                column: "AmenitiesThemeId",
                principalTable: "AmenitiesThemes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
