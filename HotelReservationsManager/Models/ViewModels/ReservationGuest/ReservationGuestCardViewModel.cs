using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.ReservationGuest
{
    public class ReservationGuestCardViewModel
    {
        public int GuestId { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; } = null!;

        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; } = null!;

        [Display(Name = "Adult")]
        public bool IsAdult { get; set; }
    }
}
