using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowBooking.Migrations
{
    /// <inheritdoc />
    public partial class Dbcreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    MovieID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.MovieID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Theatres",
                columns: table => new
                {
                    TheatreID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    TheatreName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TheatreAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theatres", x => x.TheatreID);
                    table.ForeignKey(
                        name: "FK_Theatres_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MoviesInTheatre",
                columns: table => new
                {
                    MovieID = table.Column<int>(type: "int", nullable: false),
                    TheatreID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesInTheatre", x => new { x.MovieID, x.TheatreID });
                    table.ForeignKey(
                        name: "FK_MoviesInTheatre_Movies_MovieID",
                        column: x => x.MovieID,
                        principalTable: "Movies",
                        principalColumn: "MovieID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoviesInTheatre_Theatres_TheatreID",
                        column: x => x.TheatreID,
                        principalTable: "Theatres",
                        principalColumn: "TheatreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TheatreScreens",
                columns: table => new
                {
                    ScreenID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TheatreID = table.Column<int>(type: "int", nullable: false),
                    ScreenName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SeatingCapacity = table.Column<int>(type: "int", nullable: false),
                    ScreenType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheatreScreens", x => x.ScreenID);
                    table.ForeignKey(
                        name: "FK_TheatreScreens_Theatres_TheatreID",
                        column: x => x.TheatreID,
                        principalTable: "Theatres",
                        principalColumn: "TheatreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TheatreShows",
                columns: table => new
                {
                    ShowID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieID = table.Column<int>(type: "int", nullable: false),
                    ScreenID = table.Column<int>(type: "int", nullable: false),
                    ShowName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ShowDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShowTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    AvailableSeats = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheatreShows", x => x.ShowID);
                    table.ForeignKey(
                        name: "FK_TheatreShows_Movies_MovieID",
                        column: x => x.MovieID,
                        principalTable: "Movies",
                        principalColumn: "MovieID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TheatreShows_TheatreScreens_ScreenID",
                        column: x => x.ScreenID,
                        principalTable: "TheatreScreens",
                        principalColumn: "ScreenID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketTypes",
                columns: table => new
                {
                    TicketTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ScreenID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTypes", x => x.TicketTypeID);
                    table.ForeignKey(
                        name: "FK_TicketTypes_TheatreScreens_ScreenID",
                        column: x => x.ScreenID,
                        principalTable: "TheatreScreens",
                        principalColumn: "ScreenID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TheatreBookings",
                columns: table => new
                {
                    BookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ShowID = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheatreBookings", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK_TheatreBookings_TheatreShows_ShowID",
                        column: x => x.ShowID,
                        principalTable: "TheatreShows",
                        principalColumn: "ShowID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TheatreBookings_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    SeatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScreenID = table.Column<int>(type: "int", nullable: false),
                    SeatNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SeatRow = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    SeatColumn = table.Column<int>(type: "int", nullable: false),
                    TicketTypeID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.SeatID);
                    table.ForeignKey(
                        name: "FK_Seats_TheatreScreens_ScreenID",
                        column: x => x.ScreenID,
                        principalTable: "TheatreScreens",
                        principalColumn: "ScreenID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seats_TicketTypes_TicketTypeID",
                        column: x => x.TicketTypeID,
                        principalTable: "TicketTypes",
                        principalColumn: "TicketTypeID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ShowSeats",
                columns: table => new
                {
                    ShowSeatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowID = table.Column<int>(type: "int", nullable: false),
                    SeatID = table.Column<int>(type: "int", nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowSeats", x => x.ShowSeatID);
                    table.ForeignKey(
                        name: "FK_ShowSeats_Seats_SeatID",
                        column: x => x.SeatID,
                        principalTable: "Seats",
                        principalColumn: "SeatID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShowSeats_TheatreShows_ShowID",
                        column: x => x.ShowID,
                        principalTable: "TheatreShows",
                        principalColumn: "ShowID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TheatreTickets",
                columns: table => new
                {
                    TicketID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingID = table.Column<int>(type: "int", nullable: false),
                    ShowSeatID = table.Column<int>(type: "int", nullable: false),
                    TicketTypeID = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheatreTickets", x => x.TicketID);
                    table.ForeignKey(
                        name: "FK_TheatreTickets_ShowSeats_ShowSeatID",
                        column: x => x.ShowSeatID,
                        principalTable: "ShowSeats",
                        principalColumn: "ShowSeatID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TheatreTickets_TheatreBookings_BookingID",
                        column: x => x.BookingID,
                        principalTable: "TheatreBookings",
                        principalColumn: "BookingID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TheatreTickets_TicketTypes_TicketTypeID",
                        column: x => x.TicketTypeID,
                        principalTable: "TicketTypes",
                        principalColumn: "TicketTypeID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoviesInTheatre_TheatreID",
                table: "MoviesInTheatre",
                column: "TheatreID");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_ScreenID",
                table: "Seats",
                column: "ScreenID");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_TicketTypeID",
                table: "Seats",
                column: "TicketTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ShowSeats_SeatID",
                table: "ShowSeats",
                column: "SeatID");

            migrationBuilder.CreateIndex(
                name: "IX_ShowSeats_ShowID",
                table: "ShowSeats",
                column: "ShowID");

            migrationBuilder.CreateIndex(
                name: "IX_TheatreBookings_ShowID",
                table: "TheatreBookings",
                column: "ShowID");

            migrationBuilder.CreateIndex(
                name: "IX_TheatreBookings_UserID",
                table: "TheatreBookings",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Theatres_UserID",
                table: "Theatres",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TheatreScreens_TheatreID",
                table: "TheatreScreens",
                column: "TheatreID");

            migrationBuilder.CreateIndex(
                name: "IX_TheatreShows_MovieID",
                table: "TheatreShows",
                column: "MovieID");

            migrationBuilder.CreateIndex(
                name: "IX_TheatreShows_ScreenID",
                table: "TheatreShows",
                column: "ScreenID");

            migrationBuilder.CreateIndex(
                name: "IX_TheatreTickets_BookingID",
                table: "TheatreTickets",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_TheatreTickets_ShowSeatID",
                table: "TheatreTickets",
                column: "ShowSeatID");

            migrationBuilder.CreateIndex(
                name: "IX_TheatreTickets_TicketTypeID",
                table: "TheatreTickets",
                column: "TicketTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketTypes_ScreenID",
                table: "TicketTypes",
                column: "ScreenID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoviesInTheatre");

            migrationBuilder.DropTable(
                name: "TheatreTickets");

            migrationBuilder.DropTable(
                name: "ShowSeats");

            migrationBuilder.DropTable(
                name: "TheatreBookings");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "TheatreShows");

            migrationBuilder.DropTable(
                name: "TicketTypes");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "TheatreScreens");

            migrationBuilder.DropTable(
                name: "Theatres");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

        }
    }
}
