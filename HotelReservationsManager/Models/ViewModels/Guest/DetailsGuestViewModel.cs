using HotelReservationsManager.Models.ViewModels.ReservationGuest;
using HotelReservationsManager.Models.ViewModels.Shared;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.Guest
{
    public class DetailsGuestViewModel
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = null!;

        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "Adult")]
        public bool IsAdult { get; set; }

        [Display(Name = "Total Reservations")]
        public int ReservationCount { get; set; }

        public PageResultViewModel<GuestReservationCardViewModel> Reservations { get; set; } = new();
    }
}
