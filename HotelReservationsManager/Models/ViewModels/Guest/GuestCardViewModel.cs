namespace HotelReservationsManager.Models.ViewModels.Guest
{
    public class GuestCardViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsAdult { get; set; }
    }
}
