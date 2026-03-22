using HotelReservationsManager.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.ReservationGuest
{
    public class GuestReservationCardViewModel
    {
        public int ReservationId { get; set; }

        [Display(Name = "Check-in")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime CheckInDate { get; set; }

        [Display(Name = "Check-out")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime CheckOutDate { get; set; }

        [Display(Name = "Nights")]
        public int Nights => (CheckOutDate - CheckInDate).Days;

        [Display(Name = "Room")]
        public string RoomNumber { get; set; } = null!;

        [Display(Name = "Room Type")]
        public RoomType RoomType { get; set; }

        public string RoomTypeDisplay => RoomType switch
        {
            RoomType.TwinBedded => "Twin Bedded",
            RoomType.Apartment => "Apartment",
            RoomType.DoubleBed => "Double Bed",
            RoomType.Penthouse => "Penthouse",
            RoomType.Maisonette => "Maisonette",
            _ => "Unknown"
        };

        [Display(Name = "Booked By")]
        public string BookedByUserName { get; set; } = null!;

        [Display(Name = "Guests")]
        public int GuestCount { get; set; }

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
    }
}
