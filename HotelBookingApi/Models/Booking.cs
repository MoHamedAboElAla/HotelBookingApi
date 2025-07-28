using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int? HotelId { get; set; }
        public int? RoomId { get; set; }
        public int? AgentId { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        public decimal? TotalPrice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Hotel? Hotel { get; set; }
        public Room? Room { get; set; }
        public Agent? Agent { get; set; }
    }
}
