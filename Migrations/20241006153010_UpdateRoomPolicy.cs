using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomPolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PolicyId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_PolicyId",
                table: "Rooms",
                column: "PolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomPolicies_PolicyId",
                table: "Rooms",
                column: "PolicyId",
                principalTable: "RoomPolicies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomPolicies_PolicyId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_PolicyId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "PolicyId",
                table: "Rooms");
        }
    }
}
