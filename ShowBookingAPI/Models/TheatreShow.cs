using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShowBooking.Models
{
    public class TheatreShow
    {
        [Key]
        public int ShowID { get; set; }

        [ForeignKey("Movie")]
        public int MovieID { get; set; }
        public Movie? Movie { get; set; }

        [ForeignKey("TheatreScreen")]
        public int ScreenID { get; set; }
        public TheatreScreen? TheatreScreen { get; set; }

        [Required, MaxLength(150)]
        public string ShowName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime ShowDate { get; set; }

        [Required]
        public TimeSpan ShowTime { get; set; }

        [Required]
        public int DurationMinutes { get; set; }

        [Required]
        public int AvailableSeats { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; }

        // Navigation properties
        public ICollection<ShowSeat>? ShowSeats { get; set; }
    }

}
