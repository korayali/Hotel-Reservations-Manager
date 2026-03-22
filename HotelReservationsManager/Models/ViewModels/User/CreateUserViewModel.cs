using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.User
{
    public class CreateUserViewModel
    {
        [Required]
        [Display(Name = "Display Name")]
        [StringLength(100)]
        public string DisplayName { get; set; } = null!;

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [Display(Name = "Middle Name")]
        [StringLength(50)]
        public string MiddleName { get; set; } = null!;

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        [Display(Name = "EGN")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "EGN must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "EGN must contain only digits.")]
        public string EGN { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Hire Date")]
        public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
