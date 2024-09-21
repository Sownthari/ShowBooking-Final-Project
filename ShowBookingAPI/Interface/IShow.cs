using ShowBooking.Models;

namespace ShowBooking.Interface
{
    public interface IShow
    {
        Task CreateShow(TheatreShow show);
        Task<IEnumerable<TheatreShow>> GetAllShows();
        Task<ShowDetailsDTO> GetShow(int id);
        Task<ShowSeatDetailsDTO> GetShowSeats(int id);
        Task UpdateShowStatus(int showId, string status);

        Task DeleteShow(int id);
        Task<IEnumerable<TheatreShow>> GetShowsByMovie(int movieId);
        Task<IEnumerable<TheatreShow>> GetShowsByScreen(int screenId);
        Task<IEnumerable<ShowGroupedByScreenDTO>> GetShowsByTheatre(int theatreId, DateTime date);
        Task<IEnumerable<GroupedTheatreShows>> GetFilteredShows(ShowFilter filter);

        Task<IEnumerable<Movie>> SearchMoviesWithShowsInLocation(
        string location,
        string? language = null,
        string? screenType = null,
        string? genre = null);

        Task<IEnumerable<object>> SearchShowsGroupedByTheatre(
        DateTime selectedDate,
        int theatreId,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        TimeSpan? startTime = null,
        TimeSpan? endTime = null);


    }

}
