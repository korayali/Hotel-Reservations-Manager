namespace HotelReservationsManager.Models.Domains
{
    public class Reservation : BaseEntity
    {
        public int RoomId { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public bool HasBreakfast { get; set; }
        public bool IsAllInclusive { get; set; }
        public decimal TotalPrice { get; set; }

        public ICollection<ReservationGuest> ReservationGuests { get; set; } = [];
        public Room Room { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
