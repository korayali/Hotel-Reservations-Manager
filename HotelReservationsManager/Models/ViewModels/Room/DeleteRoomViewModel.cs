namespace HotelReservationsManager.Models.ViewModels.Room
{
    public class DeleteRoomViewModel
    {
        public int Capacity { get; set; }
        public string Type { get; set; } = null!;
        public bool IsFree { get; set; } = true;
        public double PricePerAdult { get; set; }
        public double PricePerChild { get; set; }
        public int RoomNumber { get; set; }
    }
}
