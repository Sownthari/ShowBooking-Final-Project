using Microsoft.AspNetCore.Mvc;
using ShowBooking.Interface;
using ShowBooking.Models;

namespace ShowBooking.Service
{
    public class TheatreService
    {
        private readonly ITheatre _theatreRepo;

        public TheatreService(ITheatre theatreRepo)
        {
            _theatreRepo = theatreRepo;
        }

        public async Task CreateTheatre(Theatre theatre)
        {
             await  _theatreRepo.CreateTheatre(theatre);
        }

        public async Task DeleteTheatre(int id)
        {
            await _theatreRepo.DeleteTheatre(id);
        }

        public async Task<IEnumerable<MovieInTheatre>> GetAllMovieInTheatre(int theatreID)
        {
            return await  _theatreRepo.GetAllMovieInTheatre(theatreID);
        }

        public async Task<IEnumerable<Theatre>> GetAllTheatre()
        {
            return await  _theatreRepo.GetAllTheatre();
        }

        public async Task<Theatre> GetTheatre(int id)
        {
            return await _theatreRepo.GetTheatre(id);
        }

        public async Task UpdateTheatre(Theatre theatre)
        {
            await _theatreRepo.UpdateTheatre(theatre);
        }

        [HttpGet("search")]
        public async Task<IEnumerable<Theatre>> SearchTheatresByLocation(string location)
        {
            return await _theatreRepo.SearchTheatresByLocation(location);
        }

    }
}
