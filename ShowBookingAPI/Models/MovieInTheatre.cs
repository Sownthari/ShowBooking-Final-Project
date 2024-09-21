using System.ComponentModel.DataAnnotations.Schema;

namespace ShowBooking.Models
{
    public class MovieInTheatre
    {
        [ForeignKey("Movie")]
        public int MovieID { get; set; }
        public Movie? Movie { get; set; }

        [ForeignKey("Theatre")]
        public int TheatreID { get; set; }
        public Theatre? Theatre { get; set; }

        public bool IsActive { get; set; } = true;
    }

}
