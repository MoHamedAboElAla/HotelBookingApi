using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingApi.Dtos.HotelReviewsDTOs
{
    public class AddReviewDTO
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Cleanliness rating must be between 1 and 5.")]
        public int CleanlinessRating { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Facilities rating must be between 1 and 5.")]
        public int FacilitiesRating { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Staff rating must be between 1 and 5.")]
        public int StaffRating { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Location rating must be between 1 and 5.")]
        public int LocationRating { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Accuracy rating must be between 1 and 5.")]
        public int AccuracyRating { get; set; }

        [StringLength(1000, ErrorMessage = "Comment cannot be longer than 1000 characters.")]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public int AgentId { get; set; }

        [Required]
        public int HotelId { get; set; }
    }
}
