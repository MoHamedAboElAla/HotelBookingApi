using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingApi.Models
{
    public class Agent
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
      
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(20, MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9\-]+$", ErrorMessage = "Only letters, numbers, and dashes allowed.")]
        public string? CommercialRegister { get; set; }

       
        [StringLength(20, MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9\-]+$", ErrorMessage = "Only letters, numbers, and dashes allowed.")]
        public string? TaxVisa { get; set; }
        [Required]
      
        public string Role { get; set; } = "Agent";
        [Required]
        public string Password { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        
    }
}
