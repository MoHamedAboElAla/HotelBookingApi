namespace HotelBookingApi.Dtos
{
    public class BookingDto
    {
        public int Id { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string RoomNumber { get; set; }
        public string HotelName { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
