using HotelReservationsManager.Models.ViewModels.Room;

namespace HotelReservationsManager.Services.Interfaces
{
    public interface IRoomService
    {
        Task<RoomListViewModel> GetAllAsync(RoomFilterViewModel filters, int page, int pageSize);
        Task<DetailsRoomViewModel?> GetDetailsByIdAsync(int id);
        Task<EditRoomViewModel?> GetForEditAsync(int id);
        Task<DeleteRoomViewModel?> GetForDeleteAsync(int id);
        Task CreateAsync(CreateRoomViewModel model);
        Task<bool> UpdateAsync(EditRoomViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}