using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShowBooking.Models
{
    public class TheatreScreen
    {
        [Key]
        public int ScreenID { get; set; }

        [ForeignKey("Theatre")]
        public int TheatreID { get; set; }
        public Theatre? Theatre { get; set; }

        [Required, MaxLength(100)]
        public string ScreenName { get; set; }

        [Required]
        public int SeatingCapacity { get; set; }

        [MaxLength(50)]
        public string ScreenType { get; set; }

        // Navigation properties
        public ICollection<Seat>? Seats { get; set; }
        public ICollection<TheatreShow>? TheatreShows { get; set; }
        public ICollection<TicketType>? TicketTypes { get; set; }
    }

}
