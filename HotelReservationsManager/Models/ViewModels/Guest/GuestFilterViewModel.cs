using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.Guest
{
    public class GuestFilterViewModel
    {
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Display(Name = "Email")]
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "Guest Type")]
        public GuestAgeFilter AgeFilter { get; set; } = GuestAgeFilter.All;

        [Display(Name = "Reservations")]
        public GuestReservationFilter ReservationFilter { get; set; } = GuestReservationFilter.All;

        public bool HasActiveFilters() =>
            !string.IsNullOrWhiteSpace(FirstName) ||
            !string.IsNullOrWhiteSpace(LastName) ||
            !string.IsNullOrWhiteSpace(Email) ||
            AgeFilter != GuestAgeFilter.All ||
            ReservationFilter != GuestReservationFilter.All;
    }

    public enum GuestAgeFilter
    {
        All,
        AdultsOnly,
        MinorsOnly
    }

    public enum GuestReservationFilter
    {
        All,
        WithReservations,
        WithoutReservations
    }
}
