using HotelReservationsManager.Models.ViewModels.Shared;

namespace HotelReservationsManager.Models.ViewModels.Guest
{
    public class GuestListViewModel : PageResultViewModel<GuestCardViewModel>
    {
        public GuestFilterViewModel Filters { get; set; } = new();

        public GuestListViewModel() : base() { }

        public GuestListViewModel(IEnumerable<GuestCardViewModel> items, int currentPage, int pageSize, int totalItems)
            : base(items, currentPage, pageSize, totalItems) { }
    }
}
