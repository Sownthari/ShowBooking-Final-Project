using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShowBooking.Models;
using ShowBooking.Service;

[Route("api/[controller]")]
[ApiController]
public class TheatreShowController : ControllerBase
{
    private readonly ShowService _showService;

    public TheatreShowController(ShowService showService)
    {
        _showService = showService;
    }
    [Authorize(Roles = "2")]
    [HttpPost]
    public async Task<IActionResult> CreateShow([FromBody] TheatreShow show)
    {
        try
        {
            await _showService.CreateShow(show);
            return CreatedAtAction(nameof(GetShow), new { id = show.ShowID }, show);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TheatreShow>>> GetAllShows()
    {
        try
        {
            var shows = await _showService.GetAllShows();
            return Ok(shows);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize(Roles = "2")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ShowDetailsDTO>> GetShow(int id)
    {
        try
        {
            var show = await _showService.GetShow(id);
            if (show == null)
            {
                return NotFound("Show not found.");
            }
            return Ok(show);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("show/{id}")]
    [Authorize]
    public async Task<ActionResult<ShowSeatDetailsDTO>> GetShowSeats(int id)
    {
        try
        {
            var show = await _showService.GetShowSeats(id);
            if (show == null)
            {
                return NotFound("Show not found.");
            }
            return Ok(show);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize(Roles ="2")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShowStatus(int id, [FromBody] string status)
    {
        try
        {
            await _showService.UpdateShowStatus(id,status);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [Authorize(Roles = "2")]
    [HttpGet("theatre/{theatreId}/{date}")]
    public async Task<ActionResult<IEnumerable<ShowGroupedByScreenDTO>>> GetShowsByTheatre(int theatreId, string date)
    {
        try
        {
            if (!DateTime.TryParse(date, out DateTime parsedDate))
            {
                return BadRequest("Invalid date format.");
            }

            var shows = await _showService.GetShowsByTheatre(theatreId, parsedDate);
            return Ok(shows);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize]
    [HttpPost("filtered")]
    public async Task<ActionResult<IEnumerable<GroupedTheatreShows>>> GetFilteredShows([FromBody] ShowFilter filter)
    {
        if (filter == null)
        {
            return BadRequest("Filter cannot be null.");
        }

        var shows = await _showService.GetFilteredShows(filter);
        return Ok(shows);
    }

    

}
