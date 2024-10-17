using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class AddManyToManyRoomAndProperty3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomWithRoomProperty_RoomProperties_RoomPropertyId",
                table: "RoomWithRoomProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomWithRoomProperty_Rooms_RoomId",
                table: "RoomWithRoomProperty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomWithRoomProperty",
                table: "RoomWithRoomProperty");

            migrationBuilder.RenameTable(
                name: "RoomWithRoomProperty",
                newName: "RoomWithRoomProperties");

            migrationBuilder.RenameIndex(
                name: "IX_RoomWithRoomProperty_RoomPropertyId",
                table: "RoomWithRoomProperties",
                newName: "IX_RoomWithRoomProperties_RoomPropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomWithRoomProperty_RoomId",
                table: "RoomWithRoomProperties",
                newName: "IX_RoomWithRoomProperties_RoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomWithRoomProperties",
                table: "RoomWithRoomProperties",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomWithRoomProperties_RoomProperties_RoomPropertyId",
                table: "RoomWithRoomProperties",
                column: "RoomPropertyId",
                principalTable: "RoomProperties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomWithRoomProperties_Rooms_RoomId",
                table: "RoomWithRoomProperties",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomWithRoomProperties_RoomProperties_RoomPropertyId",
                table: "RoomWithRoomProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomWithRoomProperties_Rooms_RoomId",
                table: "RoomWithRoomProperties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomWithRoomProperties",
                table: "RoomWithRoomProperties");

            migrationBuilder.RenameTable(
                name: "RoomWithRoomProperties",
                newName: "RoomWithRoomProperty");

            migrationBuilder.RenameIndex(
                name: "IX_RoomWithRoomProperties_RoomPropertyId",
                table: "RoomWithRoomProperty",
                newName: "IX_RoomWithRoomProperty_RoomPropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomWithRoomProperties_RoomId",
                table: "RoomWithRoomProperty",
                newName: "IX_RoomWithRoomProperty_RoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomWithRoomProperty",
                table: "RoomWithRoomProperty",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomWithRoomProperty_RoomProperties_RoomPropertyId",
                table: "RoomWithRoomProperty",
                column: "RoomPropertyId",
                principalTable: "RoomProperties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomWithRoomProperty_Rooms_RoomId",
                table: "RoomWithRoomProperty",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
