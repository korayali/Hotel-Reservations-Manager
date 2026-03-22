using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.Reservation
{
    public class EditReservationViewModel
    {
        [Required]
        public int RoomId { get; set; }
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }
        [Required]
        public bool HasBreakfast { get; set; }
        [Required]
        public bool IsAllInclusive { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
    }
}
