using System.ComponentModel.DataAnnotations;

namespace ShowBooking.Models
{
    public class Movie
    {
        [Key]
        public int MovieID { get; set; }

        [Required, MaxLength(150)]
        public string MovieName { get; set; }

        [Required, MaxLength(50)]
        public string Genre { get; set; }

        [Required, MaxLength(50)]
        public string Language { get; set; }

        [Required]
        public int DurationMinutes { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public string Image { get; set; }

        // Navigation properties
        public ICollection<MovieInTheatre>? MoviesInTheatre { get; set; }
    }

}
