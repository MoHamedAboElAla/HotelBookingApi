namespace HotelBookingApi.Models
{
    public class AskFileRequest
    {
        public IFormFile File { get; set; }
        public string Question { get; set; }
    }
}
