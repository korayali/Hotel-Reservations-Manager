using HotelReservationsManager.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.Room
{
    public class CreateRoomViewModel
    {
        [Required]
        [Display(Name = "Room Number")]
        [Range(1, 10000)]
        public int RoomNumber { get; set; }

        [Required]
        [Display(Name = "Type")]
        public RoomType Type { get; set; }

        [Required]
        [Display(Name = "Capacity")]
        [Range(1, 20, ErrorMessage = "Capacity must be between 1 and 20.")]
        public int Capacity { get; set; }

        [Display(Name = "Available")]
        public bool IsFree { get; set; } = true;

        [Required]
        [Display(Name = "Price Per Adult")]
        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000.")]
        [DataType(DataType.Currency)]
        public decimal PricePerAdult { get; set; }

        [Required]
        [Display(Name = "Price Per Child")]
        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000.")]
        [DataType(DataType.Currency)]
        public decimal PricePerChild { get; set; }
    }
}
