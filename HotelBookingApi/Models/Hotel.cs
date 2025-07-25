using HotelBookingApi.CustomAttribute;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        [Required, StringLength(200, MinimumLength = 10, ErrorMessage = "Hotel name must be between 10 and 200 characters.")]
        public string? Name { get; set; }
        [Required, StringLength(300, MinimumLength = 10, ErrorMessage = "Location must be between 10 and 300 characters.")]
        public string? Location { get; set; }
        [Required, StringLength(50, MinimumLength = 5, ErrorMessage = "Country must be between 2 and 50 characters.")]
        public string? Country { get; set; }
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" })]
        public string? ImageUrl { get; set; }

        [Required, StringLength(700, MinimumLength = 20, ErrorMessage = "Description must be between 20 and 700 characters.")]
        public string? Description { get; set; }
        [Required, Range(1, 5, ErrorMessage = "Stars must be between 1 and 5.")]
        public byte Stars { get; set; }
        //public ICollection<Room> Rooms { get; set; } = new HashSet<Room>();
        //public ICollection<Season> Seasons { get; set; } = new List<Season>();
        //public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        //public ICollection<Agent> Agents { get; set; } = new List<Agent>();
    }
}
