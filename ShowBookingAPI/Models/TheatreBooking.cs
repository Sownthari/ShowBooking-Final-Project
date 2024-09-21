using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShowBooking.Models
{
    public class TheatreBooking
    {
        [Key]
        public int BookingID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User? User { get; set; }

        [ForeignKey("TheatreShow")]
        public int ShowID { get; set; }
        public TheatreShow? TheatreShow { get; set; }

        [Required]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Required]
        public decimal TotalAmount { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Confirmed";

        // Navigation properties
        public ICollection<TheatreTicket>? TheatreTickets { get; set; }
    }

}
