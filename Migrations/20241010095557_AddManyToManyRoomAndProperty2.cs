using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class AddManyToManyRoomAndProperty2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomProperties_Rooms_RoomId",
                table: "RoomProperties");

            migrationBuilder.DropIndex(
                name: "IX_RoomProperties_RoomId",
                table: "RoomProperties");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "RoomProperties");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "RoomProperties",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomProperties_RoomId",
                table: "RoomProperties",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomProperties_Rooms_RoomId",
                table: "RoomProperties",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
