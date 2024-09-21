using Microsoft.EntityFrameworkCore;
using ShowBooking.Models;
using ShowBooking.Interface;

namespace ShowBooking.Repository
{
    public class TheatreRepository : ITheatre
    {
        private readonly ShowContext _context;

        public TheatreRepository(ShowContext context)
        {
            _context = context;
        }

        public async Task CreateTheatre(Theatre theatre)
        {
            _context.Theatres.Add(theatre);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Theatre>> GetAllTheatre()
        {
            return await _context.Theatres.ToListAsync();
        }

        public async Task<Theatre> GetTheatre(int UserID)
        {
            return await _context.Theatres.FirstOrDefaultAsync(t => t.UserID == UserID);
        }

        public async Task UpdateTheatre(Theatre theatre)
        {
            _context.Entry(theatre).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTheatre(int id)
        {
            var theatre = await _context.Theatres.FindAsync(id);
            if (theatre != null)
            {
                _context.Theatres.Remove(theatre);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MovieInTheatre>> GetAllMovieInTheatre(int theatreId)
        {
            return await _context.MoviesInTheatre
                .Where(mit => mit.TheatreID == theatreId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Theatre>> SearchTheatresByLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return Enumerable.Empty<Theatre>();
            }

            var theatres = await _context.Theatres
                .Where(t => t.City.Contains(location) || t.TheatreAddress.Contains(location))
                .ToListAsync();

            return theatres;
        }


    }
}
