using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIconClassRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AmenitiesThemes_IconClasses_IconClassId",
                table: "AmenitiesThemes");

            migrationBuilder.DropIndex(
                name: "IX_RoomProperties_IconClassId",
                table: "RoomProperties");

            migrationBuilder.DropIndex(
                name: "IX_RoomPolicies_IconClassId",
                table: "RoomPolicies");

            migrationBuilder.DropIndex(
                name: "IX_AmenitiesThemes_IconClassId",
                table: "AmenitiesThemes");

            migrationBuilder.DropIndex(
                name: "IX_Amenities_IconClassId",
                table: "Amenities");

            migrationBuilder.AlterColumn<int>(
                name: "IconClassId",
                table: "AmenitiesThemes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomProperties_IconClassId",
                table: "RoomProperties",
                column: "IconClassId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomPolicies_IconClassId",
                table: "RoomPolicies",
                column: "IconClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AmenitiesThemes_IconClassId",
                table: "AmenitiesThemes",
                column: "IconClassId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_IconClassId",
                table: "Amenities",
                column: "IconClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_AmenitiesThemes_IconClasses_IconClassId",
                table: "AmenitiesThemes",
                column: "IconClassId",
                principalTable: "IconClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AmenitiesThemes_IconClasses_IconClassId",
                table: "AmenitiesThemes");

            migrationBuilder.DropIndex(
                name: "IX_RoomProperties_IconClassId",
                table: "RoomProperties");

            migrationBuilder.DropIndex(
                name: "IX_RoomPolicies_IconClassId",
                table: "RoomPolicies");

            migrationBuilder.DropIndex(
                name: "IX_AmenitiesThemes_IconClassId",
                table: "AmenitiesThemes");

            migrationBuilder.DropIndex(
                name: "IX_Amenities_IconClassId",
                table: "Amenities");

            migrationBuilder.AlterColumn<int>(
                name: "IconClassId",
                table: "AmenitiesThemes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_RoomProperties_IconClassId",
                table: "RoomProperties",
                column: "IconClassId",
                unique: true,
                filter: "[IconClassId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoomPolicies_IconClassId",
                table: "RoomPolicies",
                column: "IconClassId",
                unique: true,
                filter: "[IconClassId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AmenitiesThemes_IconClassId",
                table: "AmenitiesThemes",
                column: "IconClassId",
                unique: true,
                filter: "[IconClassId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_IconClassId",
                table: "Amenities",
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
    }
}
