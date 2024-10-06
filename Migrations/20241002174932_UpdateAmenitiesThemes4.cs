using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAmenitiesThemes4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AmenitiesThemes_Rooms_RoomId",
                table: "AmenitiesThemes");

            migrationBuilder.DropIndex(
                name: "IX_AmenitiesThemes_RoomId",
                table: "AmenitiesThemes");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "AmenitiesThemes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "AmenitiesThemes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AmenitiesThemes_RoomId",
                table: "AmenitiesThemes",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_AmenitiesThemes_Rooms_RoomId",
                table: "AmenitiesThemes",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
