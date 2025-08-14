using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingApi.Models
{
    public class HotelReview
    {
        public int Id { get; set; }
        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        [ForeignKey("Agent")]
        public int AgentId { get; set; }

        [Required]
        [Range(1, 5)]
        public int CleanlinessRating { get; set; }

        [Required]
        [Range(1, 5)]
        public int FacilitiesRating { get; set; }

        [Required]
        [Range(1, 5)]
        public int StaffRating { get; set; }

        [Required]
        [Range(1, 5)]
        public int LocationRating { get; set; }

        [Required]
        [Range(1, 5)]
        public int AccuracyRating { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        
        public Hotel Hotel { get; set; }
        public Agent Agent { get; set; } 
    }
}
