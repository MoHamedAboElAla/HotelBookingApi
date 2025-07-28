namespace HotelBookingApi.Services
{
    public class ImageUrlService : IImageUrlService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ImageUrlService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateHotelImageUrl(string fileName)
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            if (request == null || string.IsNullOrWhiteSpace(fileName))
                return string.Empty;

            return $"{request.Scheme}://{request.Host}/images/hotel/{fileName}";
        }
    }
}
