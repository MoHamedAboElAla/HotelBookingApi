using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
       // [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;


        [StringLength(20, MinimumLength = 5)]
       // [RegularExpression(@"^[a-zA-Z0-9\-]+$", ErrorMessage = "Only letters, numbers, and dashes allowed.")]
        public string? CommercialRegister { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 8)]
      //  [RegularExpression(@"^[a-zA-Z0-9\-]+$", ErrorMessage = "Only letters, numbers, and dashes allowed.")]
        public string Password { get; set; }
        [Required]
       // [RegularExpression(@"^[a-zA-Z0-9\-]+$", ErrorMessage = "Only letters, numbers, and dashes allowed.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string PasswordConfirmation { get; set; }


        [StringLength(20, MinimumLength = 5)]
     //   [RegularExpression(@"^[a-zA-Z0-9\-]+$", ErrorMessage = "Only letters, numbers, and dashes allowed.")]
        public string? TaxVisa { get; set; }
      
    }
}
