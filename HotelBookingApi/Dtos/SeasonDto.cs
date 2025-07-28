namespace HotelBookingApi.Dtos
{
    public class SeasonDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PriceFactor { get; set; }
    }
}
