using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.User
{
    public class DeleteUserViewModel
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

        [Display(Name = "Total Reservations")]
        public int ReservationCount { get; set; }
    }
}
