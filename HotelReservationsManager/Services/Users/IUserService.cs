using HotelReservationsManager.Models.ViewModels.User;

namespace HotelReservationsManager.Services.Users
{
    public interface IUserService
    {
        Task<UserListViewModel> GetPagedUsersAsync(UserFilterViewModel filters, int page, int pageSize);
    }
}
