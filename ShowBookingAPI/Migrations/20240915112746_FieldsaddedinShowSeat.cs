using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowBooking.Migrations
{
    /// <inheritdoc />
    public partial class FieldsaddedinShowSeat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeatColumn",
                table: "ShowSeats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SeatNumber",
                table: "ShowSeats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeatRow",
                table: "ShowSeats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TicketPrice",
                table: "ShowSeats",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TicketTypeName",
                table: "ShowSeats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatColumn",
                table: "ShowSeats");

            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "ShowSeats");

            migrationBuilder.DropColumn(
                name: "SeatRow",
                table: "ShowSeats");

            migrationBuilder.DropColumn(
                name: "TicketPrice",
                table: "ShowSeats");

            migrationBuilder.DropColumn(
                name: "TicketTypeName",
                table: "ShowSeats");
        }
    }
}
