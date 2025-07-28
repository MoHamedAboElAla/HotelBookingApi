namespace HotelBookingApi.Dtos
{
    public class CartItemDto
    {
        public int BookingId { get; set; }
        public string HotelName { get; set; } = null!;
        public int RoomNumber { get; set; }
        public string RoomType { get; set; } 
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
       
        public string? AgentEmail { get; set; }


    }
}
