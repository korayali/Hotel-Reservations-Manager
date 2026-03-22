using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.Reservation
{
    public class ReservationFilterViewModel
    {
        [Display(Name = "Room Number")]
        public string? RoomNumber { get; set; }

        [Display(Name = "Booked By")]
        public string? BookedByUserName { get; set; }

        [Display(Name = "Check-in From")]
        [DataType(DataType.Date)]
        public DateTime? CheckInFrom { get; set; }

        [Display(Name = "Check-in To")]
        [DataType(DataType.Date)]
        public DateTime? CheckInTo { get; set; }

        [Display(Name = "Status")]
        public ReservationStatusFilter StatusFilter { get; set; } = ReservationStatusFilter.All;

        [Display(Name = "Breakfast")]
        public ReservationBoolFilter BreakfastFilter { get; set; } = ReservationBoolFilter.All;

        [Display(Name = "All Inclusive")]
        public ReservationBoolFilter AllInclusiveFilter { get; set; } = ReservationBoolFilter.All;

        public bool HasActiveFilters() =>
            !string.IsNullOrWhiteSpace(RoomNumber) ||
            !string.IsNullOrWhiteSpace(BookedByUserName) ||
            CheckInFrom.HasValue ||
            CheckInTo.HasValue ||
            StatusFilter != ReservationStatusFilter.All ||
            BreakfastFilter != ReservationBoolFilter.All ||
            AllInclusiveFilter != ReservationBoolFilter.All;
    }

    public enum ReservationStatusFilter
    {
        All,
        Upcoming,
        Active,
        Completed,
        Cancelled
    }

    public enum ReservationBoolFilter
    {
        All,
        Yes,
        No
    }
}
