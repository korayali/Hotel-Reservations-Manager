using HotelReservationsManager.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.User
{
    public class UserEditViewModel
    {
        public string Id { get; set; } = null!;

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
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Role")]
        public UserRole Role { get; set; }

        [Required]
        [Display(Name = "Hire Date")]
        public DateOnly HireDate { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Termination Date")]
        public DateOnly? TerminationDate { get; set; }
    }
}
