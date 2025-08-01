namespace HotelBookingApi.Services
{
    public class ImageUrlService : IImageUrlService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ImageUrlService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

     
        private string GenerateImageUrl(string folderName, string fileName)
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            if (request == null || string.IsNullOrWhiteSpace(fileName))
                return string.Empty;

            return $"{request.Scheme}://{request.Host}/{folderName}/{fileName}";
        }

        public string GenerateHotelImageUrl(string fileName)
        {
            return GenerateImageUrl("images/hotel", fileName);
        }

        public string GenerateRoomImageUrl(string fileName)
        {
            return GenerateImageUrl("uploads", fileName);
        }

    }
}
