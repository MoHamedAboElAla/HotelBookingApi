namespace HotelBookingApi.Services
{
    public interface IImageUrlService
    {
        string GenerateHotelImageUrl(string fileName);
        string GenerateRoomImageUrl(string fileName);
    }
}
