namespace HotelBookingApi.Models
{
    public class Favorite
    {
        public int Id { get; set; }

        public int UserId { get; set; } 
        public int RoomId { get; set; }

        public Agent? User { get; set; }
        public Room? Room { get; set; }
    }
}
