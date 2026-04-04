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

        // Delegates to ReservationMappingExtensions to avoid duplication
        public static GuestReservationCardViewModel ToCardViewModel(this ReservationGuest rg) =>
            rg.ToReservationCardViewModel();

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