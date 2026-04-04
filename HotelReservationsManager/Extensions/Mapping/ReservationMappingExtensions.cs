using HotelReservationsManager.Enums;
using HotelReservationsManager.Models.Domains;
using HotelReservationsManager.Models.ViewModels.Reservation;
using HotelReservationsManager.Models.ViewModels.ReservationGuest;

namespace HotelReservationsManager.Extensions.Mapping
{
    public static class ReservationMappingExtensions
    {
        public static ReservationStatus ComputeStatus(this Reservation reservation)
        {
            var today = DateTime.Today;
            if (reservation.CheckInDate.Date > today)  return ReservationStatus.Upcoming;
            if (reservation.CheckOutDate.Date < today) return ReservationStatus.Completed;
            return ReservationStatus.Active;
        }

        // Requires: reservation.Room, reservation.User, reservation.ReservationGuests
        public static ReservationCardViewModel ToCardViewModel(this Reservation reservation) =>
            new ReservationCardViewModel
            {
                Id                = reservation.Id,
                RoomNumber        = reservation.Room.RoomNumber.ToString(),
                RoomTypeDisplay   = reservation.Room.Type.ToString(),
                BookedByUserName  = reservation.User.DisplayName,
                CheckInDate       = reservation.CheckInDate,
                CheckOutDate      = reservation.CheckOutDate,
                GuestCount        = reservation.ReservationGuests.Count,
                HasBreakfast      = reservation.HasBreakfast,
                IsAllInclusive    = reservation.IsAllInclusive,
                TotalPrice        = reservation.TotalPrice,
                Status            = reservation.ComputeStatus()
            };

        // Requires: reservation.Room, reservation.User, reservation.ReservationGuests.Guest
        public static DetailsReservationViewModel ToDetailsViewModel(this Reservation reservation) =>
            new DetailsReservationViewModel
            {
                Id                = reservation.Id,
                RoomNumber        = reservation.Room.RoomNumber.ToString(),
                RoomTypeDisplay   = reservation.Room.Type.ToString(),
                RoomCapacity      = reservation.Room.Capacity,
                BookedByUserName  = reservation.User.DisplayName,
                BookedByUserId    = reservation.UserId,
                CheckInDate       = reservation.CheckInDate,
                CheckOutDate      = reservation.CheckOutDate,
                HasBreakfast      = reservation.HasBreakfast,
                IsAllInclusive    = reservation.IsAllInclusive,
                TotalPrice        = reservation.TotalPrice,
                Status            = reservation.ComputeStatus(),
                Guests            = reservation.ReservationGuests
                                        .Select(rg => rg.ToGuestCardViewModel())
                                        .ToList()
            };

        public static DeleteReservationViewModel ToDeleteViewModel(this Reservation reservation) =>
            new DeleteReservationViewModel
            {
                Id               = reservation.Id,
                RoomNumber       = reservation.Room.RoomNumber.ToString(),
                BookedByUserName = reservation.User.DisplayName,
                CheckInDate      = reservation.CheckInDate,
                CheckOutDate     = reservation.CheckOutDate,
                TotalPrice       = reservation.TotalPrice,
                Status           = reservation.ComputeStatus(),
                GuestCount       = reservation.ReservationGuests.Count
            };

        public static EditReservationViewModel ToEditViewModel(this Reservation reservation) =>
            new EditReservationViewModel
            {
                Id             = reservation.Id,
                RoomId         = reservation.RoomId,
                UserId         = reservation.UserId,
                CheckInDate    = reservation.CheckInDate,
                CheckOutDate   = reservation.CheckOutDate,
                HasBreakfast   = reservation.HasBreakfast,
                IsAllInclusive = reservation.IsAllInclusive,
                TotalPrice     = reservation.TotalPrice,
                GuestIds       = reservation.ReservationGuests
                                     .Select(rg => rg.GuestId)
                                     .ToList()
            };

        public static Reservation ToDomain(this CreateReservationViewModel vm) =>
            new Reservation
            {
                RoomId         = vm.RoomId,
                UserId         = vm.UserId,
                CheckInDate    = vm.CheckInDate,
                CheckOutDate   = vm.CheckOutDate,
                HasBreakfast   = vm.HasBreakfast,
                IsAllInclusive = vm.IsAllInclusive,
                TotalPrice     = 0 // calculated by service
            };

        public static void ApplyFromViewModel(this Reservation reservation, EditReservationViewModel vm)
        {
            reservation.RoomId         = vm.RoomId;
            reservation.UserId         = vm.UserId;
            reservation.CheckInDate    = vm.CheckInDate;
            reservation.CheckOutDate   = vm.CheckOutDate;
            reservation.HasBreakfast   = vm.HasBreakfast;
            reservation.IsAllInclusive = vm.IsAllInclusive;
            reservation.TotalPrice     = vm.TotalPrice;
        }

        // ReservationGuest → ReservationGuestCardViewModel (guest info on a reservation)
        // Requires: rg.Guest
        public static ReservationGuestCardViewModel ToGuestCardViewModel(this ReservationGuest rg) =>
            new ReservationGuestCardViewModel
            {
                GuestId     = rg.GuestId,
                FullName    = $"{rg.Guest.FirstName} {rg.Guest.LastName}",
                Email       = rg.Guest.Email,
                PhoneNumber = rg.Guest.PhoneNumber,
                IsAdult     = rg.Guest.isAdult
            };

        // ReservationGuest → GuestReservationCardViewModel (reservation info on a guest)
        // Requires: rg.Reservation, rg.Reservation.Room, rg.Reservation.User, rg.Reservation.ReservationGuests
        public static GuestReservationCardViewModel ToReservationCardViewModel(this ReservationGuest rg) =>
            new GuestReservationCardViewModel
            {
                ReservationId    = rg.ReservationId,
                CheckInDate      = rg.Reservation.CheckInDate,
                CheckOutDate     = rg.Reservation.CheckOutDate,
                RoomNumber       = rg.Reservation.Room.RoomNumber.ToString(),
                RoomType         = rg.Reservation.Room.Type,
                BookedByUserName = rg.Reservation.User.DisplayName,
                GuestCount       = rg.Reservation.ReservationGuests.Count,
                HasBreakfast     = rg.Reservation.HasBreakfast,
                IsAllInclusive   = rg.Reservation.IsAllInclusive,
                TotalPrice       = rg.Reservation.TotalPrice,
                Status           = rg.Reservation.ComputeStatus()
            };
    }
}
