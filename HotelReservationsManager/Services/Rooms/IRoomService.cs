using HotelReservationsManager.Models.ViewModels.Room;

namespace HotelReservationsManager.Services.RoomService
{
    public interface IRoomService
    {
        Task<RoomListViewModel> GetRoomsAsync(RoomFilterViewModel filters, int page, int pageSize);
        Task<DetailsRoomViewModel?> GetRoomDetailsAsync(int id);
        Task UpdateRoomAsync(DetailsRoomViewModel vm);
    }
}
