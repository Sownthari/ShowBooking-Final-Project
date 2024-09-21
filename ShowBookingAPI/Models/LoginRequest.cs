using Microsoft.AspNetCore.Mvc;

namespace ShowBooking.Models
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RowConfig
    {
        public string RowLetter { get; set; }
        public int ColumnCount { get; set; } 
        public int TicketTypeID { get; set; }
    }

    public class BulkSeats
    {
        public int ScreenID { get; set; }
        public List<RowConfig> RowConfigs { get; set; }
    }

    public class ShowFilter
    {
        public DateTime? ShowDate { get; set; }
        public int? MovieID { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public TimeSpan? MinShowTime { get; set; }
        public TimeSpan? MaxShowTime { get; set; }
        public string ScreenType { get; set; }
        public string City { get; set; }
    }

    public class GroupedTheatreShows
    {
        public int TheatreID { get; set; }
        public string TheatreName { get; set; }
        public string City { get; set; }
        public List<TheatreShowDTO> Shows { get; set; }
    }

    public class TheatreShowDTO
    {
        public int ShowID { get; set; }
        public string ShowName { get; set; }
        public DateTime ShowDate { get; set; }
        public TimeSpan ShowTime { get; set; }
        public string MovieName { get; set; }
        public string ScreenType { get; set; }
        public int AvailableSeats { get; set; }
        public List<decimal> TicketPrices { get; set; }

    }

    public class SearchShowsRequest
    {
        public DateTime SelectedDate { get; set; }
        public int TheatreId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }

    public class MovieSearchRequest
    {
        public string Location { get; set; }
        public string? Language { get; set; }
        public string? ScreenType { get; set; }
        public string? Genre { get; set; }
    }

    public class BookTicketsRequest
    {
        public List<int> SeatIds { get; set; }
        public int ShowID { get; set; }
        public int UserID { get; set; }
    }

    public class BookingHistoryDTO
    {
        public int BookingID { get; set; }
        public string MovieName { get; set; }
        public string TheatreName { get; set; }
        public DateTime ShowDate { get; set; }
        public TimeSpan ShowTime { get; set; }
        public List<SeatDetailsDTO> Seats { get; set; }
    }

    public class SeatDetailsDTO
    {
        public string SeatNumber { get; set; }
        public string SeatRow { get; set; }
        public int SeatColumn { get; set; }
        public int TicketTypeID { get; set; }
        public decimal Price { get; set; }
    }

    public class TheatreScreenDTO
    {
        public int ScreenID { get; set; }
        public string ScreenName { get; set; }
        public string ScreenType { get; set; }
        public int ScreenCapacity { get; set; }
        public List<TicketTypeDTO> TicketTypes { get; set; }
    }

    public class TicketTypeDTO
    {
        public int TicketTypeID { get; set; }
        public string TicketTypeName { get; set; }
        public decimal Price { get; set; }
        public int SeatCount { get; set; }
    }

    public class CreateScreenDTO
    {
        public int TheatreID { get; set; }
        public int ScreenID { get; set; }
        public string ScreenName { get; set; }
        public int SeatingCapacity { get; set; }
        public string ScreenType { get; set; }
        public List<TicketDTO> TicketTypes { get; set; }
        public List<SeatDTO> Seats { get; set; }
    }

    public class SeatDTO
    {
        public string SeatRow { get; set; }
        public int SeatColumn { get; set; }
        public string TicketType { get; set; }
    }

    public class TicketDTO
    {
        public int TicketTypeID { get; set; }
        public string TicketTypeName { get; set; }
        public decimal Price { get; set; }
    }

    public class ShowGroupedByScreenDTO
    {
        public int ScreenID { get; set; }
        public string ScreenName { get; set; }
        public List<ShowDTO> Shows { get; set; }
    }

    public class ShowDTO
    {
        public int ShowID { get; set; }
        public TimeSpan ShowTime { get; set; }
        public string MovieName { get; set; }
    }

    public class ShowDetailsDTO
    {
        public int ShowID { get; set; }
        public string ShowName { get; set; }
        public DateTime ShowDate { get; set; }
        public TimeSpan ShowTime { get; set; }
        public int DurationMinutes { get; set; }
        public string Description { get; set; }
        public string MovieName { get; set; }
        public string ScreenName { get; set; }
        public string Status { get; set; }
        public List<TicketTypeSeatsDTO> TicketTypeSeats { get; set; }
    }

    public class TicketTypeSeatsDTO
    {
        public string TicketTypeName { get; set; }
        public decimal Price { get; set; }
        public int TotalSeats { get; set; }
        public int BookedSeats { get; set; }
        public int AvailableSeats { get; set; }
    }

    public class ShowSeatDTO
    {
        public int SeatId { get; set; }
        public string SeatNumber { get; set; }
        public string SeatRow { get; set; }
        public int SeatColumn { get; set; }
        public bool IsBooked { get; set; }
        public decimal Price { get; set; }
        public string TicketTypeName { get; set; }
    }

    public class GroupedShowSeatsDTO
    {
        public string TicketTypeName { get; set; }
        public decimal TicketTypePrice { get; set; }
        public List<ShowSeatDTO> Seats { get; set; }
    }

    public class ShowSeatDetailsDTO
    {
        public string MovieName { get; set; }
        public string TheatreName { get; set; }
        public DateTime ShowDate { get; set; }
        public TimeSpan ShowTime { get; set; }
        public List<GroupedShowSeatsDTO> GroupedShowSeats { get; set; }
    }

    public class TheatreBookingRequestDTO
    {
        public int UserId { get; set; }
        public int ShowId { get; set; }
        public List<int> SeatIds { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class MovieDTO
    {
        public int MovieID { get; set; }
        public string MovieName { get; set; } 
        public string Genre { get; set; } 
        public string Language { get; set; } 
        public int DurationMinutes { get; set; } 
        public DateTime ReleaseDate { get; set; } 
        public string Description { get; set; }
        [FromForm(Name = "imageUrl")]
        public IFormFile ImageUrl { get; set; }
    }

    public class ChangePasswordDTO
    {
        public int UserID { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }


}