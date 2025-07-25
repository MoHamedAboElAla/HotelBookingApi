using Microsoft.AspNetCore.Identity;

namespace HotelBookingApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string CommercialRegister { get; set; }
        public string TaxVisa { get; set; }
        public int HotelId { get; set; }
    }
}
