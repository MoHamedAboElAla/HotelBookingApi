namespace HotelBookingApi.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public int BookingId { get; set; }

        public Booking Booking { get; set; } = null!;
        public Agent Agent { get; set; } = null!;
    }
}
