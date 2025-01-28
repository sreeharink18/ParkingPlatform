using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingPlatform.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addAllTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gates_VehicleTypes_VehicleTypeId",
                table: "Gates");

            migrationBuilder.DropIndex(
                name: "IX_Gates_VehicleTypeId",
                table: "Gates");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ArrivalWaitingTime",
                table: "WaitingParkingDetails",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DepartureTime",
                table: "UserParkingDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ArrivalTime",
                table: "UserParkingDetails",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "ArrivalWaitingTime",
                table: "WaitingParkingDetails",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "DepartureTime",
                table: "UserParkingDetails",
                type: "time",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "ArrivalTime",
                table: "UserParkingDetails",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

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
    }
}
