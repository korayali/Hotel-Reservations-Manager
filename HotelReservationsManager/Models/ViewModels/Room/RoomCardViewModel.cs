using HotelReservationsManager.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.Room
{
    public class RoomCardViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Room Number")]
        public int RoomNumber { get; set; }

        [Display(Name = "Type")]
        public RoomType Type { get; set; }

        public string TypeDisplay => Type switch
        {
            RoomType.TwinBedded => "Twin Bedded",
            RoomType.Apartment => "Apartment",
            RoomType.DoubleBed => "Double Bed",
            RoomType.Penthouse => "Penthouse",
            RoomType.Maisonette => "Maisonette",
            _ => "Unknown"
        };

        [Display(Name = "Capacity")]
        public int Capacity { get; set; }

        [Display(Name = "Available")]
        public bool IsFree { get; set; }

        public string StatusBadgeClass => IsFree ? "badge bg-success" : "badge bg-danger";
        public string StatusDisplay => IsFree ? "Available" : "Occupied";

        [Display(Name = "Price Per Adult")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PricePerAdult { get; set; }

        [Display(Name = "Price Per Child")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PricePerChild { get; set; }
    }
}
