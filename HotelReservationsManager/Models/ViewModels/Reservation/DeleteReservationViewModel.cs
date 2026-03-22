namespace HotelReservationsManager.Models.ViewModels.Reservation
{
    public class DeleteReservationViewModel
    {
        public int RoomId { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public bool HasBreakfast { get; set; }
        public bool IsAllInclusive { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
