using System.ComponentModel.DataAnnotations;

namespace ShowBooking.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required, MaxLength(100)]
        public string RoleType { get; set; }

        // Navigation properties
        public ICollection<User>? Users { get; set; }
    }
}
