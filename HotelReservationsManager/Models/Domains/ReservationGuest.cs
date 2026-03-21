namespace HotelReservationsManager.Models.Domains
{
    public class ReservationGuest
    {
        public int GuestId { get; set; }
        public Guest Guest { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }
    }
}
