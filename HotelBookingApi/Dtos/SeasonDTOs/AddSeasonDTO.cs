using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.DTOs.SeasonDTOs
{
    public class AddSeasonDTO:IValidatableObject
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [Range(0.1, 1.0, ErrorMessage = "Price factor must be between 0.1 and 1.0")]
        public decimal PriceFactor { get; set; }

        public int? HotelId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "End date must be after start date.",
                    new[] { nameof(EndDate) }
                );
            }
        }
    }
}
