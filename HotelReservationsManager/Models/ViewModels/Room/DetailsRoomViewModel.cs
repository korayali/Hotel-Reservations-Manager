using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.Room
{
    public class DetailsRoomViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Capacity")]
        public int Capacity { get; set; }

        [Required]
        [Display(Name = "Type")]
        public string Type { get; set; } = null!;

        [Display(Name = "Available")]
        public bool IsFree { get; set; } = true;

        [Required]
        [Display(Name = "Price Per Adult")]
        public double PricePerAdult { get; set; }

        [Required]
        [Display(Name = "Price Per Child")]
        public double PricePerChild { get; set; }

        [Required]
        [Display(Name = "Room Number")]
        public int RoomNumber { get; set; }


    }
}
