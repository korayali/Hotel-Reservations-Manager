using HotelReservationsManager.Models.ViewModels.Shared;

namespace HotelReservationsManager.Models.ViewModels.Room
{
    public class RoomListViewModel : PageResultViewModel<RoomCardViewModel>
    {
        public RoomFilterViewModel Filters { get; set; } = new();

        public RoomListViewModel() : base() { }

        public RoomListViewModel(IEnumerable<RoomCardViewModel> items, int currentPage, int pageSize, int totalItems)
            : base(items, currentPage, pageSize, totalItems) { }
    }
}
