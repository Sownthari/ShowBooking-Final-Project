using ShowBooking.Interface;
using ShowBooking.Models;

namespace ShowBooking.Service
{
    public class ShowService
    {
        private readonly IShow _showRepo;

        public ShowService(IShow showRepo)
        {
            _showRepo = showRepo;
        }

        public async Task CreateShow(TheatreShow show)
        {
            await  _showRepo.CreateShow(show);
        }

        public async Task DeleteShow(int id)
        {
            await  _showRepo.DeleteShow(id);
        }

        public async Task<IEnumerable<TheatreShow>> GetAllShows()
        {
            return await _showRepo.GetAllShows();
        }

        public async Task<IEnumerable<GroupedTheatreShows>> GetFilteredShows(ShowFilter filter)
        {
            return await _showRepo.GetFilteredShows(filter);
        }

        public async Task<ShowSeatDetailsDTO> GetShowSeats(int id)
        {
            return await _showRepo.GetShowSeats(id);
        }

        public async Task<ShowDetailsDTO> GetShow(int id)
        {
            return await _showRepo.GetShow(id);
        }

        public async Task<IEnumerable<TheatreShow>> GetShowsByMovie(int movieId)
        {
            return await _showRepo.GetShowsByMovie(movieId);
        }

        public async Task<IEnumerable<TheatreShow>> GetShowsByScreen(int screenId)
        {
            return await _showRepo.GetShowsByScreen(screenId);
        }

        public async Task<IEnumerable<ShowGroupedByScreenDTO>> GetShowsByTheatre(int theatreId, DateTime date)
        {
            return await _showRepo.GetShowsByTheatre(theatreId, date);
        }

        public async Task UpdateShowStatus(int showId, string status)
        {
            await _showRepo.UpdateShowStatus(showId, status);
        }

        public async Task<IEnumerable<object>> SearchShowsGroupedByTheatre(
        DateTime selectedDate,
        int theatreId,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        TimeSpan? startTime = null,
        TimeSpan? endTime = null)
        {
            return await _showRepo.SearchShowsGroupedByTheatre(selectedDate, theatreId, minPrice, maxPrice, startTime, endTime);  
        }

        public async Task<IEnumerable<Movie>> SearchMoviesWithShowsInLocation(
        string location,
        string? language = null,
        string? screenType = null,
        string? genre = null)
        {
            return await _showRepo.SearchMoviesWithShowsInLocation(location, language, screenType, genre);
        }

    }
}
