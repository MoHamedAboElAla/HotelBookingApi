namespace HotelBookingApi.Dtos.HotelReviewsDTOs
{
    public class DisplayReviewDTO
    {
        public int Id { get; set; }
        public int CleanlinessRating { get; set; }
        public int FacilitiesRating { get; set; }
        public int StaffRating { get; set; }
        public int LocationRating { get; set; }
        public int AccuracyRating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Agent { set; get; }
        public string HotelName { get; set; }
    }
}
