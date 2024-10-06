using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelLinkWithIcon3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomTypes_IconClasses_IconClassId",
                table: "RoomTypes");

            migrationBuilder.DropIndex(
                name: "IX_RoomTypes_IconClassId",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "IconClassId",
                table: "RoomTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IconClassId",
                table: "RoomTypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypes_IconClassId",
                table: "RoomTypes",
                column: "IconClassId",
                unique: true,
                filter: "[IconClassId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTypes_IconClasses_IconClassId",
                table: "RoomTypes",
                column: "IconClassId",
                principalTable: "IconClasses",
                principalColumn: "Id");
        }
    }
}
