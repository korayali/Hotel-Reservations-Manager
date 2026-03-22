using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.Reservation
{
    public class CreateReservationViewModel
    {
        [Required]
        [Display(Name = "Room")]
        public int RoomId { get; set; }

        [Required]
        [Display(Name = "Booked By")]
        public string UserId { get; set; } = null!;

        [Required]
        [Display(Name = "Check-in Date")]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; } = DateTime.Today;

        [Required]
        [Display(Name = "Check-out Date")]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);

        [Display(Name = "Breakfast")]
        public bool HasBreakfast { get; set; }

        [Display(Name = "All Inclusive")]
        public bool IsAllInclusive { get; set; }

        [Display(Name = "Guests")]
        public List<int> GuestIds { get; set; } = [];

        // Populated by controller for dropdowns
        public IEnumerable<SelectListItem> AvailableRooms { get; set; } = [];
        public IEnumerable<SelectListItem> AvailableUsers { get; set; } = [];
        public IEnumerable<SelectListItem> AvailableGuests { get; set; } = [];
    }
}
