using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.Dtos.RoomDTOS
{
    public class displayRoom
    {
     public   int Id { set; get; }
       public int RoomNumber { set; get; }
        public string? RoomType { get; set; }
        public string Description { get; set; }
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; }

        public string? ImageUrl { get; set; } 
        public string HotelName { get; set; }



    }
}
