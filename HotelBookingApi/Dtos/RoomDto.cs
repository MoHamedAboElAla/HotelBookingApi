namespace HotelBookingApi.Dtos
{
    public class RoomDto
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public string? RoomType { get; set; }
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
