using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShowBooking.Models
{
    public class TicketType
    {
        [Key]
        public int TicketTypeID { get; set; }

        [Required, MaxLength(50)]
        public string TicketTypeName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [ForeignKey("TheatreScreen")]
        public int ScreenID { get; set; }
        public TheatreScreen? TheatreScreen { get; set; }

        // Navigation properties
        public ICollection<Seat>? Seats { get; set; }
    }

}
