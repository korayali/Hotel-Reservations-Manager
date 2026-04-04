using HotelReservationsManager.Models;
using HotelReservationsManager.Models.ViewModels.Guest;

namespace HotelReservationsManager.Services.Interfaces
{
    public interface IGuestService
    {
        Task<GuestListViewModel> GetAllAsync(GuestFilterViewModel filters, int page, int pageSize);
        Task<DetailsGuestViewModel?> GetDetailsByIdAsync(int id, int reservationsPage, int reservationsPageSize);
        Task<EditGuestViewModel?> GetForEditAsync(int id);
        Task<DeleteGuestViewModel?> GetForDeleteAsync(int id);
        Task<ServiceResult> CreateAsync(CreateGuestViewModel model);
        Task<ServiceResult> UpdateAsync(EditGuestViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
