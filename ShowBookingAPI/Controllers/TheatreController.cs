using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowBooking.Interface;
using ShowBooking.Models;
using ShowBooking.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheatreController : ControllerBase
    {
        private readonly ITheatre _theatreService;

        public TheatreController(ITheatre theatreService)
        {
            _theatreService = theatreService;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Theatre>>> GetAllTheatres()
        {
            try
            {
                var theatres = await _theatreService.GetAllTheatre();
                return Ok(theatres);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "2")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Theatre>> GetTheatre(int id)
        {
            try
            {
                var theatre = await _theatreService.GetTheatre(id);
                if (theatre == null)
                {
                    return NotFound();
                }
                return Ok(theatre);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles ="1")]
        [HttpPost]
        public async Task<ActionResult<Theatre>> CreateTheatre([FromBody] Theatre theatre)
        {
            try
            {
                await _theatreService.CreateTheatre(theatre);
                return CreatedAtAction(nameof(GetTheatre), new { id = theatre.TheatreID }, theatre);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "2")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTheatre([FromBody] Theatre theatre)
        {

            try
            {
                await _theatreService.UpdateTheatre(theatre);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound($"The theatre with ID {theatre.TheatreID} was not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("{theatreId}/movies")]
        public async Task<ActionResult<IEnumerable<MovieInTheatre>>> GetMoviesInTheatre(int theatreId)
        {
            try
            {
                var movies = await _theatreService.GetAllMovieInTheatre(theatreId);
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Theatre>>> SearchTheatresByLocation(string location)
        {
            try
            {
                var theatres = await _theatreService.SearchTheatresByLocation(location);
                if (theatres == null || !theatres.Any())
                {
                    return NotFound("No theatres found for the specified location.");
                }

                return Ok(theatres);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
