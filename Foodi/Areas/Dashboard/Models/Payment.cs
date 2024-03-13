using System.ComponentModel.DataAnnotations;
using System;

namespace Foodi.Areas.Dashboard.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Card number is required.")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card number must be 16 digits.")]
        public string CardNo { get; set; }

        [Required(ErrorMessage = "Expiration Month is required.")]
        [Range(1, 12, ErrorMessage = "Invalid Expiration Month.")]
        [RegularExpression(@"^\d{2}$", ErrorMessage = "Expiration Month number must be 2 digits.")]
        public int ExpirationMonth { get; set; }

        [Required(ErrorMessage = "Expiration Year is required.")]
        [CurrentYearRange(ErrorMessage = "Invalid Expiration Year.")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Expiration Year number must be 4 digits.")]
        public int ExpirationYear { get; set; }


        [Required(ErrorMessage = "Expiry date is required.")]
        public string ExpiryDate { get; set; }

        [Required(ErrorMessage = "CVV number is required.")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV number must be 3 digits.")]
        public string CvvNo { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Payment mode is required.")]
        public string PaymentMode { get; set; }
    }

    public class CurrentYearRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int expirationYear = (int)value;

            if (expirationYear >= DateTime.Now.Year && expirationYear <= 9999)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid Expiration Year.");
        }
    }
}
