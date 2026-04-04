using HotelReservationsManager.Enums;

namespace HotelReservationsManager.Models.Domains
{
    public class Room : BaseEntity
    {
        public int Capacity { get; set; }
        public RoomType Type { get; set; }
        public bool IsFree { get; set; } = true;
        public double PricePerAdult { get; set; }
        public double PricePerChild { get; set; }
        public int RoomNumber { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = [];
    }
}
