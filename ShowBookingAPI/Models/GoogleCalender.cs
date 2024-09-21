namespace ShowBooking.Models
{
    public class GoogleCalender
    {
        public int UserId {  get; set; } 
        public string Summary { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
