using HotelReservationsManager.Models.ViewModels.Shared;

namespace HotelReservationsManager.Models.ViewModels.User
{
    public class UserListViewModel : PageResultViewModel<UserCardViewModel>
    {
        public UserFilterViewModel Filters { get; set; } = new();

        public UserListViewModel() : base() { }

        public UserListViewModel(IEnumerable<UserCardViewModel> items, int currentPage, int pageSize, int totalItems)
            : base(items, currentPage, pageSize, totalItems) { }
    }
}