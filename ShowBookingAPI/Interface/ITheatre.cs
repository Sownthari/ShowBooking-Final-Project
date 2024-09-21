using ShowBooking.Models;

namespace ShowBooking.Interface
{
    public interface ITheatre
    {
        Task CreateTheatre(Theatre theatre);
        Task<IEnumerable<Theatre>> GetAllTheatre();
        Task<Theatre> GetTheatre(int userID);
        Task UpdateTheatre(Theatre theatre);
        Task DeleteTheatre(int id);
        Task<IEnumerable<MovieInTheatre>> GetAllMovieInTheatre(int theatreID);
        Task<IEnumerable<Theatre>> SearchTheatresByLocation(string location);
    }
}
