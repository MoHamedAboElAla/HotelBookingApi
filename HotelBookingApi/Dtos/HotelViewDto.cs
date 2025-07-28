namespace HotelBookingApi.Dtos
{
    public class HotelViewDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }
        public byte Stars { get; set; }
        // public string ImageFileName { get; set; }
        public string ImageUrl { get; set; } = "";
        public List<RoomDto>? Rooms { get; set; }
        public List<SeasonDto>? Seasons { get; set; }
    }
}
