using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelLinkWithIcon2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IconClasses_RoomTypes_RoomTypeId",
                table: "IconClasses");

            migrationBuilder.DropIndex(
                name: "IX_IconClasses_RoomTypeId",
                table: "IconClasses");

            migrationBuilder.DropColumn(
                name: "RoomTypeId",
                table: "IconClasses");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "RoomTypeId",
                table: "IconClasses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IconClasses_RoomTypeId",
                table: "IconClasses",
                column: "RoomTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_IconClasses_RoomTypes_RoomTypeId",
                table: "IconClasses",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
