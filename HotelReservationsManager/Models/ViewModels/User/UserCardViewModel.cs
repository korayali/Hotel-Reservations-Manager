using HotelReservationsManager.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.User
{
    public class UserCardViewModel
    {
        public string Id { get; set; } = null!;

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; } = null!;

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {MiddleName} {LastName}".Replace("  ", " ").Trim();
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "Phone")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Role")]
        public UserRole Role { get; set; }

        public string RoleBadgeClass => Role switch
        {
            UserRole.Admin => "badge bg-danger",
            UserRole.Employee => "badge bg-info",
            _ => "badge bg-secondary"
        };

        [Display(Name = "Hire Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateOnly HireDate { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        public string StatusBadgeClass => IsActive ? "badge bg-success" : "badge bg-danger";
        public string StatusDisplay => IsActive ? "Active" : "Inactive";

        [Display(Name = "Total Reservations")]
        public int ReservationCount { get; set; }
    }
}
