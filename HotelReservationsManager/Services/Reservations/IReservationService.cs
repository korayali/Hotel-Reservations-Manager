using HotelReservationsManager.Models.ViewModels.Reservation;

namespace HotelReservationsManager.Services.Reservations
{
    public interface IReservationService
    {
        Task<ReservationListViewModel> GetReservationsAsync(ReservationFilterViewModel filters, int page, int pageSize);
        Task<DetailsReservationViewModel?> GetReservationDetailsAsync(int id);

        Task<int> CreateReservationAsync(CreateReservationViewModel vm);
        Task<EditReservationViewModel?> GetEditReservationAsync(int id);
        Task UpdateReservationAsync(EditReservationViewModel vm);
        Task CancelReservationAsync(int id);
    }
}
