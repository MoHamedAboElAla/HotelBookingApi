namespace HotelBookingApi.Dtos
{
    public class CreateBookingDto
    {
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
