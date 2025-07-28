using HotelBookingApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Domain.Models
{
    public class Season
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }
        [Range(0.1, 1.0, ErrorMessage = "Price factor must be between 0.1 and 1.0 .")]
        public decimal PriceFactor { get; set; }

        [ForeignKey("Hotel")]
        public int? HotelId { get; set; }
        public Hotel? Hotel { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();


    }
}
