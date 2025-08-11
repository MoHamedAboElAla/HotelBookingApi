using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.Dtos.RoomDTOS
{
    public class UpdateRoomDto
    {
        public int RoomNumber { get; set; }

        [Required, StringLength(30)]
        public string? RoomType { get; set; }

        [Required, Range(500.00, 10000.00)]
        public decimal PricePerNight { get; set; }

        [Required, StringLength(700, MinimumLength = 20, ErrorMessage = "Description must be between 20 and 700 characters.")]
        public string Description { get; set; }
        public bool IsAvailable { get; set; } = true;

        public IFormFile ?Image { get; set; }
       
        public int HotelId { get; set; }
    }
}

