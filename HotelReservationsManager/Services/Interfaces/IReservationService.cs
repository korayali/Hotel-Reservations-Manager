using HotelReservationsManager.Models.Domains;
using HotelReservationsManager.Models.ViewModels.Reservation;

namespace HotelReservationsManager.Services.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationListViewModel> GetAllAsync(ReservationFilterViewModel filters, int page, int pageSize);
        Task<DetailsReservationViewModel?> GetDetailsByIdAsync(int id);
        Task<EditReservationViewModel?> GetForEditAsync(int id);
        Task<DeleteReservationViewModel?> GetForDeleteAsync(int id);
        Task<CreateReservationViewModel> BuildCreateFormAsync();
        Task<EditReservationViewModel?> BuildEditFormAsync(int id);
        Task CreateAsync(CreateReservationViewModel model, string userId);
        Task<bool> UpdateAsync(EditReservationViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<List<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut);
        Task<List<Guest>> GetAvailableGuestsAsync(DateTime checkIn, DateTime checkOut);
    }
}