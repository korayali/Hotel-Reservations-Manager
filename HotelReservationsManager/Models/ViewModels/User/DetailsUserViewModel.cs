using HotelReservationsManager.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.User
{
    public class DetailsUserViewModel : IValidatableObject
    {
        public string Id { get; set; } = null!;

        [Required]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; } = null!;

        public string FullName => $"{FirstName} {MiddleName} {LastName}".Replace("  ", " ").Trim();

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; } = null!;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "EGN must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "EGN must contain only digits.")]
        [Display(Name = "EGN")]
        public string EGN { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Phone]
        [Display(Name = "Phone")]
        public string? PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Role")]
        public UserRole Role { get; set; }

        public string RoleBadgeClass => Role switch
        {
            UserRole.Admin => "badge bg-danger",
            UserRole.Employee => "badge bg-info",
            _ => "badge bg-secondary"
        };

        [Required]
        [Display(Name = "Hire Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateOnly HireDate { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        public string StatusBadgeClass => IsActive ? "badge bg-success" : "badge bg-danger";
        public string StatusDisplay => IsActive ? "Active" : "Inactive";

        [Display(Name = "Termination Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateOnly? TerminationDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsActive && TerminationDate == null)
            {
                yield return new ValidationResult(
                    "Termination date is required when the employee is inactive.",
                    [nameof(TerminationDate)]);
            }

            if (TerminationDate.HasValue && TerminationDate.Value < HireDate)
            {
                yield return new ValidationResult(
                    "Termination date cannot be before the hire date.",
                    [nameof(TerminationDate)]);
            }

            if (HireDate > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                yield return new ValidationResult(
                    "Hire date cannot be in the future.",
                    [nameof(HireDate)]);
            }
        }
    }
}