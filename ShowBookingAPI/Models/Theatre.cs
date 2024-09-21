using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShowBooking.Models
{
    public class Theatre
    {
        [Key]
        public int TheatreID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User? User { get; set; }

        [Required, MaxLength(100)]
        public string TheatreName { get; set; }

        [MaxLength(200)]
        public string TheatreAddress { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }

        [Required, MaxLength(100)]
        public string State { get; set; }

        // Navigation properties
        public ICollection<TheatreScreen>? TheatreScreens { get; set; }
        public ICollection<MovieInTheatre>? MoviesInTheatre { get; set; }
    }

}
