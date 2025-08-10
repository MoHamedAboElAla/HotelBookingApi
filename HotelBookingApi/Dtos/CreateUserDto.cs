namespace HotelBookingApi.Dtos
{
    public class CreateUserDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CommercialRegister { get; set; }
        public string? TaxVisa { get; set; }
        public string? Role { get; set; } 
        public string? Password { get; set; }
    }
}
