using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.Room
{
    public class DeleteRoomViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Room Number")]
        public int RoomNumber { get; set; }

        [Display(Name = "Type")]
        public string TypeDisplay { get; set; } = null!;

        [Display(Name = "Capacity")]
        public int Capacity { get; set; }

        [Display(Name = "Active Reservations")]
        public int ActiveReservationCount { get; set; }
    }
}
