using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using ShowBooking.Models;

namespace ShowBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MovieController : ControllerBase
    {
        private readonly ShowContext _context;

        public MovieController(ShowContext context)
        {
            _context = context;
        }

        // GET: api/Movie
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            try
            {
                var movies = await _context.Movies.ToListAsync();

                var movieDtos = movies.Select(movie => new Movie
                {
                    MovieID = movie.MovieID,
                    MovieName = movie.MovieName,
                    Genre = movie.Genre,
                    Language = movie.Language,
                    DurationMinutes = movie.DurationMinutes,
                    ReleaseDate = movie.ReleaseDate,
                    Description = movie.Description,
                    Image = movie.Image
                }).ToList();

                return Ok(movieDtos);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("location/{location}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesWithFutureShows(string location)
        {
            try
            {
                var currentDate = DateTime.Today;
                var currentTime = DateTime.Now.TimeOfDay;

                var moviesWithFutureShows = await _context.Movies
                    .Where(m => _context.TheatreShows
                        .Any(ts => ts.MovieID == m.MovieID &&
                                   ts.ShowDate >= currentDate &&
                                   ts.TheatreScreen.Theatre.City.ToLower() == location))
                    .ToListAsync();

                if (!moviesWithFutureShows.Any())
                {
                    return NotFound($"No movies found with future shows in {location}.");
                }

                return Ok(moviesWithFutureShows);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("movie/{movieId}")]
        public async Task<ActionResult<object>> GetMovieDetailsWithScreenTypes(int movieId)
        {
            try
            {
                var currentDate = DateTime.Now;

                // Query to get movie details along with the unique screen types it's showing on
                var movieDetails = await _context.Movies
                    .Where(m => m.MovieID == movieId)
                    .Select(m => new
                    {
                        MovieId = m.MovieID,
                        MovieName = m.MovieName,
                        Genre = m.Genre,
                        DurationMinutes = m.DurationMinutes,
                        ReleaseDate = m.ReleaseDate,
                        Description = m.Description,
                        Image = m.Image,
                        ScreenTypes = _context.TheatreShows
                            .Where(ts => ts.MovieID == m.MovieID && ts.ShowDate >= currentDate) // Filter for future shows
                            .Select(ts => ts.TheatreScreen.ScreenType)
                            .Distinct() // Only return unique screen types
                            .ToList()
                    })
                    .FirstOrDefaultAsync();

                if (movieDetails == null)
                {
                    return NotFound($"Movie with ID {movieId} not found or no upcoming shows.");
                }

                return Ok(movieDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("theatre/{id}")]
        public async Task<ActionResult<IEnumerable<object>>> GetMoviesByTheatre(int id)
        {
            try
            {
                var movies = await _context.MoviesInTheatre
                    .Where(m => m.TheatreID == id && m.IsActive)
                    .Select(m => new
                    {
                        MovieID = m.MovieID,
                        MovieName = m.Movie!.MovieName,
                        IsActive = m.IsActive
                    })
                    .ToListAsync();

                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles ="1")]
        [HttpPost("mapmovie/{theatreId}/{movieId}")]
        public async Task<ActionResult> CreatingMapping(int theatreId, int movieId)
        {
            try
            {
                // Create a new mapping for the movie and theatre
                var movieInTheatre = new MovieInTheatre
                {
                    MovieID = movieId,
                    TheatreID = theatreId,
                    IsActive = true
                };

                // Add the mapping to the context
                _context.MoviesInTheatre.Add(movieInTheatre);
                await _context.SaveChangesAsync();

                // Return a success response
                return CreatedAtAction("GetMovie", new { id = movieId }, movieInTheatre);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error occurred while creating the mapping: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Movie/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            try
            {
                var movie = await _context.Movies.FindAsync(id);
                if (movie == null)
                {
                    return NotFound();
                }

                var movieDtos = new Movie
                {
                    MovieID = movie.MovieID,
                    MovieName = movie.MovieName,
                    Genre = movie.Genre,
                    Language = movie.Language,
                    DurationMinutes = movie.DurationMinutes,
                    ReleaseDate = movie.ReleaseDate,
                    Description = movie.Description,
                    Image = $"{Request.Scheme}://{Request.Host}/images/{movie.Image}"
                };

                return Ok(movieDtos);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [Authorize(Roles = "2")]
        [HttpPut("update/{movieId}/{theatreId}")]
        public async Task<IActionResult> UpdateMovieStatus(int movieId, int theatreId)
        {
            try
            {
                MovieInTheatre movieInTheatre = await _context.MoviesInTheatre.FirstOrDefaultAsync(m => m.MovieID == movieId && m.TheatreID == theatreId);

                if(movieInTheatre != null)
                {
                    movieInTheatre.IsActive = !movieInTheatre.IsActive;
                    _context.MoviesInTheatre.Update(movieInTheatre);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Concurrency error occurred while updating the movie.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return StatusCode(200, "Movie Status updated successfully");
        }

        // POST: api/Movie
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            try
            {
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMovie", new { id = movie.MovieID }, movie);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error occurred while creating the movie: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "1")]
        [HttpPost("addMovie")]
        public async Task<IActionResult> AddMovie([FromForm] MovieDTO movieDto)
        {
            string ImageString = null;
            if (movieDto.ImageUrl != null && movieDto.ImageUrl.Length > 0)
            {
                var filePath = Path.Combine(@"D:\AngularTraining\TicketBooking\src\assets\Images", movieDto.ImageUrl.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await movieDto.ImageUrl.CopyToAsync(stream);
                }

                ImageString = movieDto.ImageUrl.FileName; 
            }

            var movie = new Movie
            {
                MovieName = movieDto.MovieName,
                Genre = movieDto.Genre,
                Language = movieDto.Language,
                Description = movieDto.Description,
                ReleaseDate = movieDto.ReleaseDate,
                DurationMinutes = movieDto.DurationMinutes,
                Image = ImageString
            };

            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Movie added successfully", movie = movieDto });
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieID == id);
        }


    }
}
