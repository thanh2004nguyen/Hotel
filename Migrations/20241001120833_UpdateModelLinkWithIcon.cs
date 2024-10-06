using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelLinkWithIcon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomPropertyDetails");

            migrationBuilder.DropColumn(
                name: "RoomPolicyId",
                table: "IconClasses");

            migrationBuilder.DropColumn(
                name: "RoomPropertyId",
                table: "IconClasses");

            migrationBuilder.DropColumn(
                name: "RoomUnityId",
                table: "IconClasses");

            migrationBuilder.AddColumn<int>(
                name: "IconClassId",
                table: "RoomUnities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IconClassId",
                table: "RoomProperties",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IconClassId",
                table: "RoomPolicies",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomTypeId",
                table: "IconClasses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomUnities_IconClassId",
                table: "RoomUnities",
                column: "IconClassId",
                unique: true,
                filter: "[IconClassId] IS NOT NULL");

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

            migrationBuilder.AddForeignKey(
                name: "FK_RoomPolicies_IconClasses_IconClassId",
                table: "RoomPolicies",
                column: "IconClassId",
                principalTable: "IconClasses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomProperties_IconClasses_IconClassId",
                table: "RoomProperties",
                column: "IconClassId",
                principalTable: "IconClasses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomUnities_IconClasses_IconClassId",
                table: "RoomUnities",
                column: "IconClassId",
                principalTable: "IconClasses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IconClasses_RoomTypes_RoomTypeId",
                table: "IconClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomPolicies_IconClasses_IconClassId",
                table: "RoomPolicies");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomProperties_IconClasses_IconClassId",
                table: "RoomProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomUnities_IconClasses_IconClassId",
                table: "RoomUnities");

            migrationBuilder.DropIndex(
                name: "IX_RoomUnities_IconClassId",
                table: "RoomUnities");

            migrationBuilder.DropIndex(
                name: "IX_RoomProperties_IconClassId",
                table: "RoomProperties");

            migrationBuilder.DropIndex(
                name: "IX_RoomPolicies_IconClassId",
                table: "RoomPolicies");

            migrationBuilder.DropIndex(
                name: "IX_IconClasses_RoomTypeId",
                table: "IconClasses");

            migrationBuilder.DropColumn(
                name: "IconClassId",
                table: "RoomUnities");

            migrationBuilder.DropColumn(
                name: "IconClassId",
                table: "RoomProperties");

            migrationBuilder.DropColumn(
                name: "IconClassId",
                table: "RoomPolicies");

            migrationBuilder.AlterColumn<int>(
                name: "RoomTypeId",
                table: "IconClasses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RoomPolicyId",
                table: "IconClasses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomPropertyId",
                table: "IconClasses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomUnityId",
                table: "IconClasses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RoomPropertyDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    RoomPropertyId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomPropertyDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomPropertyDetails_RoomProperties_RoomPropertyId",
                        column: x => x.RoomPropertyId,
                        principalTable: "RoomProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomPropertyDetails_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomPropertyDetails_RoomId",
                table: "RoomPropertyDetails",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomPropertyDetails_RoomPropertyId",
                table: "RoomPropertyDetails",
                column: "RoomPropertyId");
        }
    }
}
