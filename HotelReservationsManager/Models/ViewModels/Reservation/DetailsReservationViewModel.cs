using HotelReservationsManager.Enums;
using HotelReservationsManager.Models.ViewModels.ReservationGuest;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.Reservation
{
    public class DetailsReservationViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; } = null!;

        [Display(Name = "Room Type")]
        public string RoomTypeDisplay { get; set; } = null!;

        [Display(Name = "Capacity")]
        public int RoomCapacity { get; set; }

        [Display(Name = "Booked By")]
        public string BookedByUserName { get; set; } = null!;

        public string BookedByUserId { get; set; } = null!;

        [Display(Name = "Check-in")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime CheckInDate { get; set; }

        [Display(Name = "Check-out")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime CheckOutDate { get; set; }

        [Display(Name = "Nights")]
        public int Nights => (CheckOutDate - CheckInDate).Days;

        [Display(Name = "Breakfast")]
        public bool HasBreakfast { get; set; }

        [Display(Name = "All Inclusive")]
        public bool IsAllInclusive { get; set; }

        [Display(Name = "Total Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Status")]
        public ReservationStatus Status { get; set; }

        public string StatusBadgeClass => Status switch
        {
            ReservationStatus.Upcoming => "badge bg-primary",
            ReservationStatus.Active => "badge bg-success",
            ReservationStatus.Completed => "badge bg-secondary",
            ReservationStatus.Cancelled => "badge bg-danger",
            _ => "badge bg-light"
        };

        public IEnumerable<ReservationGuestCardViewModel> Guests { get; set; } = [];
    }
}
