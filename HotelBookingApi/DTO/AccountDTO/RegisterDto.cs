using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.DTO.AccountDTO
{
        public class RegisterDto
        {
            public string Name { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string CommercialRegister { get; set; }
            public string TaxVisa { get; set; }

            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Compare("Password")]
            [Display(Name = "Confirm Password")]
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }
            public int HotelId { get; set; }

    }
}
