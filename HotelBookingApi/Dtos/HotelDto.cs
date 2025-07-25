using HotelBookingApi.CustomAttribute;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.Dtos
{
    public class HotelDto
    {
        public int Id { get; set; }

        [Required, StringLength(200, MinimumLength = 10)]
        public string? Name { get; set; }

        [Required, StringLength(300, MinimumLength = 10)]
        public string? Location { get; set; }

        [Required, StringLength(50, MinimumLength = 5)]
        public string? Country { get; set; }

        [Required]
        public IFormFile? ImageFile { get; set; }

        [Required, StringLength(700, MinimumLength = 20)]
        public string? Description { get; set; }

        [Required, Range(1, 5)]
        public byte Stars { get; set; }
    }

}
