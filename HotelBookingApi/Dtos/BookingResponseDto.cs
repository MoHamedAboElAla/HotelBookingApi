namespace HotelBookingApi.Dtos
{
    public class BookingResponseDto
    {
        public int RoomId { get; set; }
        public int RoomNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AgentName { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
