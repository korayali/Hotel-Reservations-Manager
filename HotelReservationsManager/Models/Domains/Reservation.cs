namespace HotelReservationsManager.Models.Domains
{
    public class Reservation : BaseEntity
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public List<Guest> GuestList { get; set; } = [];
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public bool HasBreakfast { get; set; }
        public bool IsAllInclusive { get; set; }
        public double TotalPrice { get; set; }
    }
}
