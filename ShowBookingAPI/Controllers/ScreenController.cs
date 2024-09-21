using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShowBooking.Interface; 
using ShowBooking.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "1,2")]
    public class ScreenController : ControllerBase
    {
        private readonly IScreen _screenService;

        public ScreenController(IScreen screenService)
        {
            _screenService = screenService;
        }

        [HttpGet("theatre/{theatreID}")]
        public async Task<ActionResult<IEnumerable<TheatreScreenDTO>>> GetScreens(int theatreID)
        {
            try
            {
                var screens = await _screenService.GetScreens(theatreID);
                return Ok(screens);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{screenID}")]
        public async Task<ActionResult<CreateScreenDTO>> GetScreenById(int screenID)
        {
            try
            {
                var screen = await _screenService.GetScreenById(screenID);
                if (screen == null)
                {
                    return NotFound($"Screen with ID {screenID} not found.");
                }
                return Ok(screen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("screens/{theatreID}")]
        public async Task<ActionResult<IEnumerable<object>>> GetScreensName(int theatreID)
        {
            try
            {
                var screens = await _screenService.GetScreensName(theatreID);
                if (screens == null)
                {
                    return NotFound($"Screen with ID {theatreID} not found.");
                }
                return Ok(screens);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateScreen([FromBody] CreateScreenDTO screen)
        {
            try
            {
                await _screenService.CreateScreen(screen);
                return Ok(new { message = "Screen created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateScreen([FromBody] CreateScreenDTO screenDto)
        {

            try
            {
                await _screenService.UpdateScreen(screenDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Screen with ID {screenDto.ScreenID} not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("ScreenTicketTypes/{screenID}")]
        public async Task<ActionResult<IEnumerable<TicketType>>> GetTicketTypes(int screenID)
        {
            try
            {
                var ticketTypes = await _screenService.GetAllTicketTypes(screenID);
                return Ok(ticketTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("TicketType/{id}")]
        public async Task<ActionResult<TicketType>> GetTicketTypeById(int id)
        {
            try
            {
                var ticketType = await _screenService.GetTicketTypeById(id);
                if (ticketType == null)
                {
                    return NotFound($"Ticket type with ID {id} not found.");
                }
                return Ok(ticketType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("screen/{screenID}")]
        public async Task<ActionResult<IEnumerable<Seat>>> GetSeatsByScreen(int screenID)
        {
            try
            {
                var seats = await _screenService.GetSeatsByScreen(screenID);
                return Ok(seats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
