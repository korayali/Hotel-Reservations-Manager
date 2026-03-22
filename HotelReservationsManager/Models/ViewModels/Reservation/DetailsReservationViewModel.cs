using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.Reservation
{
    public class DetailsReservationViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Room ID")]
        public int RoomId { get; set; }

        [Required]
        [Display(Name = "User ID")]
        public string UserId { get; set; } = null!;


        [Required]
        [Display(Name = "Check-in Date")]
        public DateTime CheckInDate { get; set; }


        [Required]
        [Display(Name = "Check-out Date")]
        public DateTime CheckOutDate { get; set; }


        [Required]
        [Display(Name = "Breakfast")]
        public bool HasBreakfast { get; set; }


        [Required]
        [Display(Name = "All-Inclusive")]
        public bool IsAllInclusive { get; set; }


        [Required]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }
    }
}
