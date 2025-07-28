namespace HotelBookingApi.Dtos
{
    public class CartItemDto
    {
        public int BookingId { get; set; }
        public string HotelName { get; set; } = null!;
        public string RoomNumber { get; set; } = null!;
        public string RoomType { get; set; } = null!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string? SeasonName { get; set; }
        public string? RoomImageUrl { get; set; }
    }
}
