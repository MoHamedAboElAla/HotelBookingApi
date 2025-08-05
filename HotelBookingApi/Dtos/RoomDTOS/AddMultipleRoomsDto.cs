using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HotelBookingApi.Dtos.RoomDTOS
{
    public class AddMultipleRoomsDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Start room number must be at least 1.")]
        public int StartRoomNumber { get; set; }

        [Required]
        [Range(1, 500, ErrorMessage = "Count must be between 1 and 500.")]
        public int Count { get; set; }

        [Required, StringLength(30)]
        public string? RoomType { get; set; }

        [Required, StringLength(700, MinimumLength = 20, ErrorMessage = "Description must be between 20 and 700 characters.")]
        public string? Description { get; set; }

        [Required, Range(500.00, 10000.00)]
        public decimal PricePerNight { get; set; }

        public bool IsAvailable { get; set; } = true;

        public IFormFile? Image { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "HotelId must be a positive number.")]
        public int HotelId { get; set; }
    }
}
