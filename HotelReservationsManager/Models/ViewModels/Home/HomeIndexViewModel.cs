namespace HotelReservationsManager.Models.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public int RoomsAvailable { get; set; }
        public int ActiveReservations { get; set; }
        public int RegisteredGuests { get; set; }
        public int CheckInsToday { get; set; }
    }
}