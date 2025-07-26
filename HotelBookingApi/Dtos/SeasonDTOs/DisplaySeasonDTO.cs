using System.Runtime.CompilerServices;

namespace HotelBookingApi.DTOs.SeasonDTOs
{
    public class DisplaySeasonDTO
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PriceFactor {  get; set; }
        public List<int> BookingIds {  get; set; } = new List<int>();

    }
}
