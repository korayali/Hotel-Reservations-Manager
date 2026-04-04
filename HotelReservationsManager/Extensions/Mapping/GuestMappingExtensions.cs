using HotelReservationsManager.Models.Domains;
using HotelReservationsManager.Models.ViewModels.Guest;
using HotelReservationsManager.Models.ViewModels.ReservationGuest;

namespace HotelReservationsManager.Extensions.Mapping
{
    public static class GuestMappingExtensions
    {
        public static GuestCardViewModel ToCardViewModel(this Guest guest) =>
            new GuestCardViewModel
            {
                Id = guest.Id,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                PhoneNumber = guest.PhoneNumber,
                Email = guest.Email,
                IsAdult = guest.isAdult
            };

        // Requires: rg.Reservation, rg.Reservation.Room, rg.Reservation.User,
        //           rg.Reservation.ReservationGuests
        public static GuestReservationCardViewModel ToCardViewModel(this ReservationGuest rg) =>
            new GuestReservationCardViewModel
            {
                ReservationId = rg.ReservationId,
                CheckInDate = rg.Reservation.CheckInDate,
                CheckOutDate = rg.Reservation.CheckOutDate,
                RoomNumber = rg.Reservation.Room.RoomNumber.ToString(),
                RoomType = rg.Reservation.Room.Type,
                BookedByUserName = rg.Reservation.User.DisplayName,
                GuestCount = rg.Reservation.ReservationGuests.Count,
                HasBreakfast = rg.Reservation.HasBreakfast,
                IsAllInclusive = rg.Reservation.IsAllInclusive,
                TotalPrice = rg.Reservation.TotalPrice,
            };

        public static DetailsGuestViewModel ToDetailsViewModel(this Guest guest) =>
            new DetailsGuestViewModel
            {
                Id = guest.Id,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                PhoneNumber = guest.PhoneNumber,
                Email = guest.Email,
                IsAdult = guest.isAdult,
                ReservationCount = guest.ReservationGuests.Count
            };

        public static EditGuestViewModel ToEditViewModel(this Guest guest) =>
            new EditGuestViewModel
            {
                Id = guest.Id,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                PhoneNumber = guest.PhoneNumber,
                Email = guest.Email,
                IsAdult = guest.isAdult
            };

        public static DeleteGuestViewModel ToDeleteViewModel(this Guest guest) =>
            new DeleteGuestViewModel
            {
                Id = guest.Id,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                Email = guest.Email,
                ReservationCount = guest.ReservationGuests.Count
            };

        public static Guest ToDomain(this CreateGuestViewModel vm) =>
            new Guest
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                PhoneNumber = vm.PhoneNumber,
                Email = vm.Email,
                isAdult = vm.IsAdult
            };

        public static void ApplyFromViewModel(this Guest guest, EditGuestViewModel vm)
        {
            guest.FirstName = vm.FirstName;
            guest.LastName = vm.LastName;
            guest.PhoneNumber = vm.PhoneNumber;
            guest.Email = vm.Email;
            guest.isAdult = vm.IsAdult;
        }
    }
}