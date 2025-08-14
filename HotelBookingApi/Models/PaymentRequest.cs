namespace HotelBookingApi.Models
{
    public class PaymentRequest
    {
        public long Amount { get; set; }

        // اسم الغرفة أو وصف للـ product
        public string? Description { get; set; }

        public int? RoomId { get; set; }
        public string? RoomType { get; set; }
        public string? HotelName { get; set; }
    }
}
