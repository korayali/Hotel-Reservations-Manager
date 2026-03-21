namespace HotelReservationsManager.Models.Domains
{
    public class Reservation : BaseEntity
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public bool HasBreakfast { get; set; }
        public bool IsAllInclusive { get; set; }
        public decimal TotalPrice { get; set; }

        public ICollection<ReservationGuest> ReservationGuests { get; set; } = [];
        public Room Rooms { get; set; } = new Room();
        public User Users { get; set; } = new User();
    }
}
