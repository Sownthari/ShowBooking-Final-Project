using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShowBooking.Models
{
    public class ShowSeat
    {
        [Key]
        public int ShowSeatID { get; set; }

        [ForeignKey("TheatreShow")]
        public int ShowID { get; set; }
        public TheatreShow? TheatreShow { get; set; }

        [ForeignKey("Seat")]
        public int SeatID { get; set; }
        public Seat? Seat { get; set; }
        public bool IsBooked { get; set; } = false;
        public string SeatNumber { get; set; }
        public string SeatRow { get; set; }     
        public int SeatColumn { get; set; }     
        public string TicketTypeName { get; set; }
        public decimal TicketPrice { get; set; }
    }

}
