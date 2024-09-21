using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowBooking.Models;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;


namespace ShowBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly ShowContext _context;

        public StatisticsController(ShowContext showContext)
        {
            _context = showContext;
        }

        [Authorize(Roles = "1")]
        [HttpGet("api/admin/top-performing-movies")]
        public async Task<IActionResult> GetTopPerformingMovies(
    [FromQuery] DateTime? startDate = null,
    [FromQuery] DateTime? endDate = null,
    [FromQuery] string genre = null)
        {
            var query = _context.Movies
                .Select(m => new
                {
                    m.MovieID,
                    m.MovieName,
                    m.Genre,
                    TotalTicketsSold = _context.TheatreTickets
                        .Include(t => t.ShowSeat).ThenInclude(t => t.TheatreShow).ThenInclude(m => m.Movie)
                        .Where(tt => tt.ShowSeat.TheatreShow.Movie.MovieID == m.MovieID)
                        .Count(),
                    TotalRevenue = _context.TheatreTickets
                        .Include(t => t.ShowSeat).ThenInclude(t => t.TheatreShow).ThenInclude(m => m.Movie)
                        .Where(tt => tt.ShowSeat.TheatreShow.Movie.MovieID == m.MovieID)
                        .Sum(tt => tt.Price)
                })
                .AsQueryable();

            // Apply Date Filter
            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(m => _context.TheatreTickets
                    .Include(t => t.ShowSeat).ThenInclude(t => t.TheatreShow)
                    .Where(tt => tt.ShowSeat.TheatreShow.ShowDate >= startDate.Value && tt.ShowSeat.TheatreShow.ShowDate <= endDate.Value)
                    .Any());
            }

            // Apply Genre Filter
            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(m => m.Genre == genre);
            }

            var topMovies = await query
                .OrderByDescending(m => m.TotalTicketsSold)
                .Take(5)
                .ToListAsync();

            return Ok(topMovies);
        }

        [Authorize(Roles = "1")]
        [HttpGet("api/admin/revenue-by-theatre")]
        public async Task<IActionResult> GetRevenueByTheatre(
    [FromQuery] DateTime? startDate = null,
    [FromQuery] DateTime? endDate = null,
    [FromQuery] string city = null)
        {
            var query = _context.Theatres.Select(t => new
            {
                t.TheatreID,
                t.TheatreName,
                t.City,
                TotalRevenue = _context.TheatreTickets
                    .Include(t => t.TicketType).ThenInclude(t => t.TheatreScreen).ThenInclude(t => t.Theatre)
                    .Where(tt => tt.TicketType.TheatreScreen.Theatre.TheatreID == t.TheatreID)
                    .Sum(tt => tt.Price)
            })
            .AsQueryable();

            // Apply Date Filter
            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(t => _context.TheatreTickets
                    .Include(t => t.TicketType).ThenInclude(t => t.TheatreScreen)
                    .Where(tt => tt.ShowSeat.TheatreShow.ShowDate >= startDate.Value && tt.ShowSeat.TheatreShow.ShowDate <= endDate.Value)
                    .Any());
            }

            // Apply City Filter
            if (!string.IsNullOrEmpty(city))
            {
                query = query.Where(t => t.City == city);
            }

            var revenueByTheatre = await query
                .OrderByDescending(t => t.TotalRevenue)
                .ToListAsync();

            return Ok(revenueByTheatre);
        }

        [HttpGet("api/theatre/occupancy-rate/{theatreId}")]
        public async Task<IActionResult> GetOccupancyRateByScreen(
    int theatreId,
    [FromQuery] DateTime? startDate = null,
    [FromQuery] DateTime? endDate = null,
    [FromQuery] string screenType = null)
        {
            var query = _context.TheatreScreens
                .Where(ts => ts.TheatreID == theatreId)
                .Select(ts => new
                {
                    ts.ScreenID,
                    ts.ScreenName,
                    ts.ScreenType,
                    TotalSeats = ts.Seats.Count(),
                    BookedSeats = _context.TheatreTickets
                        .Include(t => t.TicketType).ThenInclude(t => t.TheatreScreen)
                        .Where(tt => tt.TicketType.TheatreScreen.ScreenID == ts.ScreenID)
                        .Count(),
                    OccupancyRate = (_context.TheatreTickets
                        .Include(t => t.TicketType).ThenInclude(t => t.TheatreScreen)
                        .Where(tt => tt.TicketType.TheatreScreen.ScreenID == ts.ScreenID)
                        .Count() / (double)ts.Seats.Count()) * 100
                })
                .AsQueryable();

            // Apply Date Filter
            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(ts => _context.TheatreTickets
                    .Include(t => t.ShowSeat.TheatreShow)
                    .Where(tt => tt.ShowSeat.TheatreShow.ShowDate >= startDate.Value && tt.ShowSeat.TheatreShow.ShowDate <= endDate.Value)
                    .Any());
            }

            // Apply Screen Type Filter
            if (!string.IsNullOrEmpty(screenType))
            {
                query = query.Where(ts => ts.ScreenType == screenType);
            }

            var occupancyRate = await query.ToListAsync();
            return Ok(occupancyRate);
        }

        [Authorize(Roles = "2")]
        [HttpGet("api/theatre/ticket-sales-trend/{theatreId}")]
        public async Task<IActionResult> GetTicketSalesTrend(int theatreId, [FromQuery] string period = "daily")
        {
            var query = _context.TheatreTickets
                .Include(t => t.TicketType).ThenInclude(t => t.TheatreScreen).ThenInclude(t => t.Theatre)
                .Where(tt => tt.TicketType.TheatreScreen.Theatre.TheatreID == theatreId)
                .AsQueryable();

            if (period == "daily")
            {
                var dailySales = await query
                    .GroupBy(tt => tt.ShowSeat.TheatreShow.ShowDate.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        TicketsSold = g.Count(),
                        TotalRevenue = g.Sum(tt => tt.Price)
                    })
                    .ToListAsync();

                return Ok(dailySales);
            }
            else if (period == "weekly")
            {
                

                var tickets = await query
                .Where(tt => tt.ShowSeat != null
                             && tt.ShowSeat.TheatreShow != null
                             && tt.ShowSeat.TheatreShow.ShowDate.Date != null)
                .ToListAsync();

                var weeklySales = tickets
                    .GroupBy(tt => new
                    {
                        Year = tt.ShowSeat.TheatreShow.ShowDate.Year,
                        Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                            tt.ShowSeat.TheatreShow.ShowDate,
                            CalendarWeekRule.FirstDay,
                            DayOfWeek.Monday)
                    })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Week = g.Key.Week,
                        TicketsSold = g.Count(),
                        TotalRevenue = g.Sum(tt => tt.Price)
                    })
                    .ToList();

                return Ok(weeklySales);

            }
            else if (period == "monthly")
            {
                var monthlySales = await query
                    .GroupBy(tt => new { Year = tt.ShowSeat.TheatreShow.ShowDate.Year, Month = tt.ShowSeat.TheatreShow.ShowDate.Month })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        TicketsSold = g.Count(),
                        TotalRevenue = g.Sum(tt => tt.Price)
                    })
                    .ToListAsync();

                return Ok(monthlySales);
            }
            else
            {
                return BadRequest("Invalid period specified.");
            }
        }





    }
}
