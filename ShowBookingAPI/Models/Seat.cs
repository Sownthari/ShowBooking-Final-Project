using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShowBooking.Models
{
    public class Seat
    {
        [Key]
        public int SeatID { get; set; }

        [ForeignKey("TheatreScreen")]
        public int ScreenID { get; set; }
        public TheatreScreen? TheatreScreen { get; set; }

        [Required, MaxLength(10)]
        public string SeatNumber { get; set; }

        [Required, MaxLength(5)]
        public string SeatRow { get; set; }

        [Required]
        public int SeatColumn { get; set; }

        [ForeignKey("TicketType")]
        public int TicketTypeID { get; set; }
        public TicketType? TicketType { get; set; }

        public bool IsActive { get; set; } = true;
    }

}
