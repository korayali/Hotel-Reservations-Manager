namespace HotelReservationsManager.Models.Domains
{
    public class Reservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        public List<Guests> GuestList { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public bool HasBreakfast { get; set; }
        public bool IsAllInclusive { get; set; }
        public double TotalPrice { get; set; }
    }
}
