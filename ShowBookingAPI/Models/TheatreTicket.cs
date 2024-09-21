using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShowBooking.Models
{
    public class TheatreTicket
    {
        [Key]
        public int TicketID { get; set; }

        [ForeignKey("TheatreBooking")]
        public int BookingID { get; set; }
        public TheatreBooking? TheatreBooking { get; set; }

        [ForeignKey("ShowSeat")]
        public int ShowSeatID { get; set; }
        public ShowSeat? ShowSeat { get; set; }

        [ForeignKey("TicketType")]
        public int TicketTypeID { get; set; }
        public TicketType? TicketType { get; set; }

        [Required]
        public decimal Price { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Booked";
    }

}
