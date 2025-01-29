using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingPlatform.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addForiegnKeyinGateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Gates_VehicleTypeId",
                table: "Gates",
                column: "VehicleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gates_VehicleTypes_VehicleTypeId",
                table: "Gates",
                column: "VehicleTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gates_VehicleTypes_VehicleTypeId",
                table: "Gates");

            migrationBuilder.DropIndex(
                name: "IX_Gates_VehicleTypeId",
                table: "Gates");
        }
    }
}
