using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.Dtos
{
    public class UpdatePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; }
    }
}
