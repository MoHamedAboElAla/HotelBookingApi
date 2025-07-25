using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.CustomAttribute
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        public AllowedExtensionsAttribute(string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not string fileName)
            {
                return new ValidationResult("Invalid file name.");
            }

            var extension = System.IO.Path.GetExtension(fileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
            {
                return new ValidationResult($"File extension '{extension}' is not allowed. Allowed extensions are: {string.Join(", ", _allowedExtensions)}");
            }

            return ValidationResult.Success!;
        }
    }
}
