using HotelReservationsManager.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.Reservation
{
    public class DeleteReservationViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Room")]
        public string RoomNumber { get; set; } = null!;

        [Display(Name = "Booked By")]
        public string BookedByUserName { get; set; } = null!;

        [Display(Name = "Check-in")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime CheckInDate { get; set; }

        [Display(Name = "Check-out")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime CheckOutDate { get; set; }

        [Display(Name = "Total Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Status")]
        public ReservationStatus Status { get; set; }

        [Display(Name = "Guests")]
        public int GuestCount { get; set; }
    }
}
