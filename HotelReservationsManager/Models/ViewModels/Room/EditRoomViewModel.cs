using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.Room
{
    public class EditRoomViewModel
    {
        [Required]
        public int Capacity { get; set; }
        [Required]
        public string Type { get; set; } = null!;
        //[Required]
        public bool IsFree { get; set; } = true;
        [Required]
        public double PricePerAdult { get; set; }
        [Required]
        public double PricePerChild { get; set; }
        [Required]
        public int RoomNumber { get; set; }
    }
}
