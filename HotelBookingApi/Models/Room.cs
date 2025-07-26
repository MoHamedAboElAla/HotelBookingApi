using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HotelBookingApi.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        public int RoomNumber { get; set; }
        
        [Required, StringLength(30)]
        public string? RoomType { get; set; }
      
        [Required, Range(500.00, 10000.00)]
        public decimal PricePerNight { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
       
        [Required, StringLength(700, MinimumLength = 20, ErrorMessage = "Description must be between 20 and 700 characters.")]
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public Hotel? Hotel { get; set; }
        

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
       
    }
}
