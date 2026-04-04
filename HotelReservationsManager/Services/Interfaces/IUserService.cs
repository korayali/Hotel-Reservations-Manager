using HotelReservationsManager.Models;
using HotelReservationsManager.Models.ViewModels.User;

namespace HotelReservationsManager.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserListViewModel> GetUsersAsync(UserFilterViewModel filters, int page, int pageSize);
        Task<DetailsUserViewModel?> GetUserDetailsAsync(string id);
        Task<ServiceResult> UpdateUserAsync(DetailsUserViewModel vm, string currentUserId);
        Task ToggleAdminAsync(string id, string currentUserId);
    }
}