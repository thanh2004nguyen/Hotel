using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AmenitiesThemes_IconClasses_IconClassId",
                table: "AmenitiesThemes");

            migrationBuilder.DropForeignKey(
                name: "FK_AmenitiesThemes_Rooms_RoomId",
                table: "AmenitiesThemes");

            migrationBuilder.DropIndex(
                name: "IX_AmenitiesThemes_IconClassId",
                table: "AmenitiesThemes");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "AmenitiesThemes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IconClassId",
                table: "AmenitiesThemes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AmenitiesThemes_Rooms_RoomId",
                table: "AmenitiesThemes",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AmenitiesThemes_IconClasses_IconClassId",
                table: "AmenitiesThemes");

            migrationBuilder.DropForeignKey(
                name: "FK_AmenitiesThemes_Rooms_RoomId",
                table: "AmenitiesThemes");

            migrationBuilder.DropIndex(
                name: "IX_AmenitiesThemes_IconClassId",
                table: "AmenitiesThemes");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "AmenitiesThemes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
                name: "IX_AmenitiesThemes_IconClassId",
                table: "AmenitiesThemes",
                column: "IconClassId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AmenitiesThemes_IconClasses_IconClassId",
                table: "AmenitiesThemes",
                column: "IconClassId",
                principalTable: "IconClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AmenitiesThemes_Rooms_RoomId",
                table: "AmenitiesThemes",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
