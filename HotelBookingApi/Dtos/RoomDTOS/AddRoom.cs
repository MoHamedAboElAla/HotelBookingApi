using HotelBookingApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.Dtos.RoomDTOS
{
    public class AddRoom
    {
        public int RoomNumber { get; set; }

        [Required, StringLength(30)]
        public string? RoomType { get; set; }

        [Required, Range(500.00, 10000.00)]
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; } = true;

        public int HotelId { get; set; }
    }
}
