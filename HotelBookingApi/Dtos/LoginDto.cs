using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.Dtos
{
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
