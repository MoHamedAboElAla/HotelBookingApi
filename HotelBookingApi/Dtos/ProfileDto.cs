using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.Dtos
{
    public class ProfileDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; } = string.Empty;

        public string CommercialRegister { get; set; } = string.Empty;

        public string TaxVisa { get; set; } = string.Empty;
    }
}