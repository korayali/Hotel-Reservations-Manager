namespace HotelReservationsManager.Models.ViewModels.Guest
{
    public class DeleteGuestViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int ReservationCount { get; set; }
    }
}