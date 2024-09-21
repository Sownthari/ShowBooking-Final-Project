using Microsoft.EntityFrameworkCore;
using ShowBooking.Models;
using System.Net.Sockets;

namespace ShowBooking.Repository
{
    public class BookingRepository
    {
        private readonly ShowContext _context;

        public BookingRepository(ShowContext context)
        {
            _context = context;
        }

        public async Task<int> BookTickets(BookTicketsRequest request)
        {
            if (request == null || request.SeatIds == null || request.SeatIds.Count == 0)
            {
                throw new ArgumentException("Invalid request data.");
            }

            var seatsToBook = await _context.ShowSeats
            .Where(seat => request.SeatIds.Contains(seat.ShowSeatID) && !seat.IsBooked && seat.ShowID == request.ShowID)
            .Include(seat => seat.TheatreShow)
                .ThenInclude(show => show.TheatreScreen)
                    .ThenInclude(screen => screen.TicketTypes)
            .ToListAsync();

            if (seatsToBook.Count == 0)
            {
                throw new Exception("No available seats found for booking.");
            }

            decimal totalPrice = 0;

            foreach (var seat in seatsToBook)
            {
                var ticketPrices = seat.TheatreShow.TheatreScreen.TicketTypes;

                var price = ticketPrices.FirstOrDefault()?.Price ?? 0;
                totalPrice += price;
            }



            var booking = new TheatreBooking
            {
                ShowID = request.ShowID,
                UserID = request.UserID,
                BookingDate = DateTime.Now,
                TotalAmount = totalPrice,
            };

            foreach (var seat in seatsToBook)
            {
                // Fetch the TicketType for the current seat
                var ticketType = await _context.ShowSeats
                    .Where(s => s.ShowSeatID == seat.ShowSeatID)
                    .Include(s => s.TheatreShow)
                        .ThenInclude(show => show.TheatreScreen)
                            .ThenInclude(screen => screen.TicketTypes)
                    .SelectMany(s => s.TheatreShow.TheatreScreen.TicketTypes)
                    .FirstOrDefaultAsync();

                if (ticketType != null)
                {
                    var ticket = new TheatreTicket
                    {
                        BookingID = booking.BookingID,
                        ShowSeatID = seat.ShowSeatID,
                        Price = ticketType.Price,
                        TicketTypeID = ticketType.TicketTypeID,
                        Status = "Booked"
                    };

                    booking.TheatreTickets.Add(ticket);

                    seat.IsBooked = true;
                }
            }


            _context.TheatreBookings.Add(booking);

            await _context.SaveChangesAsync();
            return booking.BookingID;
        }

        public async Task<IEnumerable<BookingHistoryDTO>> GetBookingHistoryForUser(int userId)
        {
            var bookingHistory = await _context.TheatreBookings
                .Include(b => b.TheatreTickets)
                    .ThenInclude(tt => tt.ShowSeat)
                        .ThenInclude(ss => ss.TheatreShow)
                            .ThenInclude(ts => ts.Movie)
                .Include(b => b.TheatreTickets)
                    .ThenInclude(tt => tt.ShowSeat)
                        .ThenInclude(ss => ss.TheatreShow)
                            .ThenInclude(ss => ss.TheatreScreen)
                                .ThenInclude(ts => ts.Theatre)
                .Where(b => b.UserID == userId)
                .Select(b => new BookingHistoryDTO
                {
                    BookingID = b.BookingID,
                    MovieName = b.TheatreTickets.FirstOrDefault().ShowSeat.TheatreShow.Movie.MovieName,
                    TheatreName = b.TheatreTickets.FirstOrDefault().ShowSeat.TheatreShow.TheatreScreen.Theatre.TheatreName,
                    ShowDate = b.TheatreTickets.FirstOrDefault().ShowSeat.TheatreShow.ShowDate,
                    ShowTime = b.TheatreTickets.FirstOrDefault().ShowSeat.TheatreShow.ShowTime,
                    Seats = b.TheatreTickets.Select(tt => new SeatDetailsDTO
                    {
                        SeatNumber = tt.ShowSeat.Seat.SeatNumber, // Ensure this property exists
                        SeatRow = tt.ShowSeat.Seat.SeatRow,       // Ensure this property exists
                        TicketTypeID = tt.TicketTypeID, // Get TicketTypeID from TheatreTicket
                        Price = _context.ShowSeats
                            .Where(s => s.ShowSeatID == tt.ShowSeatID)
                            .Include(s => s.TheatreShow)
                                .ThenInclude(show => show.TheatreScreen)
                                    .ThenInclude(screen => screen.TicketTypes)
                            .SelectMany(s => s.TheatreShow.TheatreScreen.TicketTypes
                                .Where(t => t.TicketTypeID == tt.TicketTypeID)
                                .Select(t => t.Price)
                            ).FirstOrDefault() // Get Price based on TicketTypeID
                    }).ToList()
                })
                .ToListAsync();

            return bookingHistory;
        }


    }
}
