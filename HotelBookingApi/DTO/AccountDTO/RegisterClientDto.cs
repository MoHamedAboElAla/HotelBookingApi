using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.DTO.AccountDTO
{
public class RegisterClientDto
{
    public string Name { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    [Required]
    public string Password { get; set; }

    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}
}
