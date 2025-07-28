namespace HotelBookingApi.Dtos
{
    public class BookingWithAgentAndRoomDto
    {
        public int BookingId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }

        public string AgentName { get; set; } = null!;
        public string AgentEmail { get; set; } = null!;
        public decimal? TotalPrice { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; } = null!;
        public decimal RoomPrice { get; set; }
    }
}
