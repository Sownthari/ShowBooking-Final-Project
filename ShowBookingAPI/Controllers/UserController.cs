using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowBooking.Models;
using ShowBooking.Repository;
using ShowBooking.Service;


namespace ShowBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ShowContext _context;

        public UserController(UserService userService, ShowContext context)
        {
            _userService = userService;
            _context = context;
        }

        [Authorize(Roles = "1")]
        [HttpGet("organisers")]
        public async Task<IActionResult> GetOrganizers()
        {
            try
            {
                var users = await _userService.GetOrganizers();

                if (users == null || !users.Any())
                {
                    return NotFound(new { Message = "No users found." });
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetails(int id)
        {
            try
            {
                var user = await _userService.GetDetails(id);

                if (user == null)
                {
                    return NotFound(new { Message = $"User with ID {id} not found." });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest(new { message = "User object is null." });
            }

            try
            {
                var existingUser = await _userService.GetUserByEmail(user.Email);
                if (existingUser != null)
                {
                    return Conflict(new { message = "Email already exists. Please use a different email." });
                }

                await _userService.Register(user);

                return CreatedAtAction(nameof(Register), new { id = user.UserID },
                    new { message = "User successfully registered", userId = user.UserID });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email and password are required.");
            }
            try
            {
                string token = await _userService.Login(request.Email, request.Password);

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Invalid email or password.");
                }

                return Ok(new { token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateDetails([FromBody] User user)
        {
            if (user == null || user.UserID <= 0)
            {
                return BadRequest(new { Message = "Invalid user data." });
            }

            try
            {
                var updatedUser = await _userService.UpdateDetails(user);

                if (updatedUser == null)
                {
                    return NotFound(new { Message = "User not found." });
                }

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                // Returning the exception message in JSON format
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("updatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] ChangePasswordDTO user)
        {
            if (user == null || user.UserID <= 0)
            {
                return BadRequest(new { Message = "Invalid user data." });
            }

            try
            {
                await _userService.ChangePassword(user.UserID, user.OldPassword, user.NewPassword);

                return Ok("Updated password successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [Authorize(Roles = "1")]
        [HttpPost("CreateOrganizer")]
        public async Task<IActionResult> CreateOrganizer([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest(new { message = "User object is null." });
            }

            try
            {
                var existingUser = await _userService.GetUserByEmail(user.Email);
                if (existingUser != null)
                {
                    return Conflict(new { message = "Email already exists. Please use a different email." });
                }

                await _userService.CreateOrganizer(user);

                return CreatedAtAction(nameof(Register), new { id = user.UserID },
                    new { message = "Organizer successfully created", userId = user.UserID });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }


    }
}
