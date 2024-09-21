using Microsoft.EntityFrameworkCore;
using ShowBooking.Interface;
using ShowBooking.Models;

public class ShowRepository : IShow
{
    private readonly ShowContext _context;

    public ShowRepository(ShowContext context)
    {
        _context = context;
    }

    public async Task CreateShow(TheatreShow show)
    {
        try
        {

            var seats = await _context.Seats.Include(s => s.TicketType).Where(s => s.ScreenID == show.ScreenID).ToListAsync();

            show.AvailableSeats = seats.Count();
            show.Status = "active";
            _context.TheatreShows.Add(show);
            await _context.SaveChangesAsync();

            var showSeats = new List<ShowSeat>();

            foreach (var seat in seats)
            {
                showSeats.Add(new ShowSeat
                {
                    ShowID = show.ShowID,
                    SeatID = seat.SeatID,
                    SeatNumber = seat.SeatNumber,
                    SeatRow = seat.SeatRow,
                    SeatColumn = seat.SeatColumn,
                    TicketTypeName = seat.TicketType.TicketTypeName,
                    TicketPrice = seat.TicketType.Price,
                    IsBooked = false,
                });
            }

            _context.ShowSeats.AddRange(showSeats);

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating the show and its seats.", ex);
        }
    }

    public async Task<IEnumerable<TheatreShow>> GetAllShows()
    {
        try
        {
            return await _context.TheatreShows.Include(s => s.Movie).Include(s => s.TheatreScreen).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving shows.", ex);
        }
    }

    public async Task<ShowSeatDetailsDTO> GetShowSeats(int id)
    {
        try
        {
            var show = await _context.TheatreShows
            .Include(s => s.TheatreScreen)
            .ThenInclude(s => s.Theatre)
            .Include(s => s.Movie)
            .Include(s => s.ShowSeats)
            .FirstOrDefaultAsync(s => s.ShowID == id);

            if (show == null)
            {
                throw new Exception($"Show with ID {id} not found.");
            }

            var groupedSeats = show.ShowSeats
            .GroupBy(ss => new { ss.TicketTypeName, ss.TicketPrice }) 
            .Select(group => new GroupedShowSeatsDTO
            {
                TicketTypeName = group.Key.TicketTypeName,
                TicketTypePrice = group.Key.TicketPrice,
                Seats = group.Select(seat => new ShowSeatDTO
                {
                    SeatId = seat.ShowSeatID,
                    SeatNumber = seat.SeatNumber,
                    SeatRow = seat.SeatRow,
                    SeatColumn = seat.SeatColumn,
                    IsBooked = seat.IsBooked,
                    Price = seat.TicketPrice,
                    TicketTypeName = seat.TicketTypeName
                }).ToList()
            })
            .ToList();

            return new ShowSeatDetailsDTO
            {
                MovieName = show.Movie!.MovieName,
                TheatreName = show.TheatreScreen!.Theatre!.TheatreName,
                ShowDate = show.ShowDate,
                ShowTime = show.ShowTime,
                GroupedShowSeats = groupedSeats
            };
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the show details.", ex);
        }
    }



    public async Task<ShowDetailsDTO> GetShow(int id)
    {
        try
        {

            var show = await _context.TheatreShows
            .Include(s => s.Movie)
            .Include(s => s.TheatreScreen)
            .Include(s => s.ShowSeats)
            .FirstOrDefaultAsync(s => s.ShowID == id);

            if (show == null)
            {
                throw new Exception("Show not found.");
            }

            var ticketTypeSeats = show.ShowSeats
                .GroupBy(seat => new { seat.TicketTypeName, seat.TicketPrice })
                .Select(group => new TicketTypeSeatsDTO
                {
                    TicketTypeName = group.Key.TicketTypeName,
                    Price = group.Key.TicketPrice,
                    TotalSeats = group.Count(),
                    BookedSeats = group.Count(seat => seat.IsBooked),
                    AvailableSeats = group.Count(seat => !seat.IsBooked)
                }).ToList();

            var showDetails = new ShowDetailsDTO
            {
                ShowID = show.ShowID,
                ShowName = show.ShowName,
                ShowDate = show.ShowDate,
                ShowTime = show.ShowTime,
                DurationMinutes = show.DurationMinutes,
                Description = show.Description,
                MovieName = show.Movie.MovieName,
                ScreenName = show.TheatreScreen.ScreenName,
                Status = show.Status,
                TicketTypeSeats = ticketTypeSeats
            };

            return showDetails;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the show.", ex);
        }
    }


    public async Task UpdateShowStatus(int showId, string status)
    {
        try
        {
            var show = await _context.TheatreShows.FindAsync(showId);
            if (show == null)
            {
                throw new Exception($"Show with ID {showId} not found.");
            }

            show.Status = status;

            _context.Entry(show).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the show status.", ex);
        }
    }


    public async Task DeleteShow(int id)
    {
        try
        {
            var show = await _context.TheatreShows.FindAsync(id);
            if (show != null)
            {
                _context.TheatreShows.Remove(show);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the show.", ex);
        }
    }

    public async Task<IEnumerable<TheatreShow>> GetShowsByMovie(int movieId)
    {
        try
        {
            return await _context.TheatreShows
                .Where(s => s.MovieID == movieId)
                .Include(s => s.Movie)
                .Include(s => s.TheatreScreen)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving shows by movie.", ex);
        }
    }

    public async Task<IEnumerable<TheatreShow>> GetShowsByScreen(int screenId)
    {
        try
        {
            return await _context.TheatreShows
                .Where(s => s.ScreenID == screenId)
                .Include(s => s.Movie)
                .Include(s => s.TheatreScreen)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving shows by screen.", ex);
        }
    }

    public async Task<IEnumerable<ShowGroupedByScreenDTO>> GetShowsByTheatre(int theatreId, DateTime date)
    {
        try
        {
            var theatre = _context.Theatres.FirstOrDefault(t => t.TheatreID == theatreId);
            var groupedShows = await _context.TheatreShows
                .Where(s => s.TheatreScreen!.TheatreID == theatreId && s.ShowDate.Date == date.Date)
                .Include(s => s.Movie)
                .Include(s => s.TheatreScreen)
                .GroupBy(s => new
                {
                    s.ScreenID,
                    s.TheatreScreen!.ScreenName
                })
                .Select(g => new ShowGroupedByScreenDTO
                {
                    ScreenID = g.Key.ScreenID,
                    ScreenName = g.Key.ScreenName,
                    Shows = g.Select(sh => new ShowDTO
                    {
                        ShowID = sh.ShowID,
                        ShowTime = sh.ShowTime,
                        MovieName = sh.Movie!.MovieName
                    }).ToList()
                })
                .ToListAsync();

            return groupedShows;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving shows by theatre.", ex);
        }
    }


    public async Task<IEnumerable<GroupedTheatreShows>> GetFilteredShows(ShowFilter filter)
    {
        var currentDate = DateTime.Today;
        var currentTime = DateTime.Now.TimeOfDay;

        var query = _context.TheatreShows
                            .Include(s => s.TheatreScreen)
                            .ThenInclude(screen => screen.TicketTypes)
                            .Include(s => s.Movie)
                            .Include(s => s.TheatreScreen.Theatre)
                            .AsQueryable();

        // Filter by ShowDate
        if (filter.ShowDate.HasValue)
        {
            DateTime date = filter.ShowDate.Value.Date;
            if (date == currentDate)
            {
                // If the show is today, filter shows after the current time
                query = query.Where(show => show.ShowDate == date && show.ShowTime >= currentTime);
            }
            else
            {
                // If the show is on a future date, show all shows
                query = query.Where(show => show.ShowDate == date);
            }
        }

        // Filter by MovieID
        if (filter.MovieID.HasValue)
        {
            query = query.Where(s => s.MovieID == filter.MovieID.Value);
        }

        // Filter by price range
        if (filter.MinPrice.HasValue || filter.MaxPrice.HasValue)
        {
            query = query.Where(s => s.TheatreScreen.TicketTypes
                                .Any(t => (!filter.MinPrice.HasValue || t.Price >= filter.MinPrice.Value) &&
                                          (!filter.MaxPrice.HasValue || t.Price <= filter.MaxPrice.Value)));
        }

        // Filter by MinShowTime and MaxShowTime if provided
        if (filter.MinShowTime.HasValue)
        {
            query = query.Where(s => s.ShowTime >= filter.MinShowTime.Value);
        }

        if (filter.MaxShowTime.HasValue)
        {
            query = query.Where(s => s.ShowTime <= filter.MaxShowTime.Value);
        }

        // Filter by City
        if (!string.IsNullOrEmpty(filter.City))
        {
            query = query.Where(s => s.TheatreScreen.Theatre.City == filter.City);
        }

        // Group results by Theatre and include shows
        var result = await query
            .GroupBy(s => new
            {
                s.TheatreScreen.Theatre.TheatreID,
                s.TheatreScreen.Theatre.TheatreName,
                s.TheatreScreen.Theatre.City
            })
            .Select(g => new GroupedTheatreShows
            {
                TheatreID = g.Key.TheatreID,
                TheatreName = g.Key.TheatreName,
                City = g.Key.City,
                Shows = g.Select(show => new TheatreShowDTO
                {
                    ShowID = show.ShowID,
                    ShowName = show.ShowName,
                    ShowDate = show.ShowDate,
                    ShowTime = show.ShowTime,
                    MovieName = show.Movie.MovieName,
                    ScreenType = show.TheatreScreen.ScreenType,
                    AvailableSeats = show.AvailableSeats,
                    TicketPrices = show.TheatreScreen.TicketTypes.Select(t => t.Price).ToList()
                }).ToList()
            }).ToListAsync();

        return result;
    }


    public async Task<IEnumerable<object>> SearchShowsGroupedByTheatre(
    DateTime selectedDate,
    int theatreId,
    decimal? minPrice = null,
    decimal? maxPrice = null,
    TimeSpan? startTime = null,
    TimeSpan? endTime = null)
    {
        var currentDate = DateTime.Now.Date;
        var currentTime = DateTime.Now.TimeOfDay;

        
        var query = _context.TheatreShows
                        .Include(show => show.TheatreScreen)
                            .ThenInclude(screen => screen.TicketTypes)
                        .Include(show => show.Movie)
                        .Include(show => show.TheatreScreen.Theatre)
                        .AsQueryable();

        
        query = query.Where(show => show.TheatreScreen.Theatre.TheatreID == theatreId);

        if (selectedDate.Date == currentDate)
        {
            query = query.Where(show => show.ShowDate == currentDate && show.ShowTime > currentTime);
        }
        else if (selectedDate.Date > currentDate)
        {
            query = query.Where(show => show.ShowDate == selectedDate.Date);
        }

        if (minPrice.HasValue && maxPrice.HasValue)
        {
            query = query.Where(show => show.TheatreScreen.TicketTypes.Any(t => t.Price >= minPrice && t.Price <= maxPrice));
        }
        else if (minPrice.HasValue)
        {
            query = query.Where(show => show.TheatreScreen.TicketTypes.Any(t => t.Price >= minPrice));
        }
        else if (maxPrice.HasValue)
        {
            query = query.Where(show => show.TheatreScreen.TicketTypes.Any(t => t.Price <= maxPrice));
        }

        if (startTime.HasValue)
        {
            query = query.Where(show => show.ShowTime >= startTime.Value);
        }
        if (endTime.HasValue)
        {
            query = query.Where(show => show.ShowTime <= endTime.Value);
        }

        var result = await query
            .GroupBy(show => new
            {
                show.TheatreScreen.Theatre.TheatreID,
                show.TheatreScreen.Theatre.TheatreName,
                show.TheatreScreen.Theatre.City
            })
            .Select(theatreGroup => new
            {
                TheatreID = theatreGroup.Key.TheatreID,
                TheatreName = theatreGroup.Key.TheatreName,
                City = theatreGroup.Key.City,
                Movies = theatreGroup.GroupBy(show => new
                {
                    show.Movie.MovieID,
                    show.Movie.MovieName
                })
                .Select(movieGroup => new
                {
                    MovieID = movieGroup.Key.MovieID,
                    MovieName = movieGroup.Key.MovieName,
                    Shows = movieGroup.Select(show => new
                    {
                        ShowID = show.ShowID,
                        ShowName = show.ShowName,
                        ShowTime = show.ShowTime,
                        Duration = show.DurationMinutes,
                        Price = show.TheatreScreen.TicketTypes.Min(t => t.Price), // Minimum price of the ticket types
                        AvailableSeats = show.AvailableSeats,
                        Status = show.Status
                    }).ToList()
                }).ToList()
            }).ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Movie>> SearchMoviesWithShowsInLocation(
    string location,
    string? language = null,
    string? screenType = null,
    string? genre = null)
    {
        var query = _context.TheatreShows
                        .Include(show => show.Movie)
                        .Include(show => show.TheatreScreen.Theatre)
                        .AsQueryable();

        if (!string.IsNullOrWhiteSpace(location))
        {
            query = query.Where(show => show.TheatreScreen.Theatre.City.Contains(location));
        }

        if (!string.IsNullOrWhiteSpace(language))
        {
            query = query.Where(show => show.Movie.Language == language);
        }

        if (!string.IsNullOrWhiteSpace(screenType))
        {
            query = query.Where(show => show.TheatreScreen.ScreenType == screenType);
        }

        if (!string.IsNullOrWhiteSpace(genre))
        {
            query = query.Where(show => show.Movie.Genre == genre);
        }

        var movies = await query
            .Select(show => show.Movie)
            .Distinct()
            .ToListAsync();

        return movies;
    }





}
