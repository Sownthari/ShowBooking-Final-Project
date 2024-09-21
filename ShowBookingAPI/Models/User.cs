using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ShowBooking.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(256)]
        public string PasswordHash { get; set; }

        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [ForeignKey("Role")]
        public int RoleID { get; set; }
        public Role? Role { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? GoogleAccessToken { get; set; }

        public string? GoogleRefreshToken { get; set; }

        // Navigation properties
        public ICollection<TheatreBooking>? TheatreBookings { get; set; }
    }

}
