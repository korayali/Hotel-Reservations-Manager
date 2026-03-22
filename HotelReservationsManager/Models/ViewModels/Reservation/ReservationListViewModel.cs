using HotelReservationsManager.Models.ViewModels.Shared;

namespace HotelReservationsManager.Models.ViewModels.Reservation
{
    public class ReservationListViewModel : PageResultViewModel<ReservationCardViewModel>
    {
        public ReservationFilterViewModel Filters { get; set; } = new();

        public ReservationListViewModel() : base() { }

        public ReservationListViewModel(IEnumerable<ReservationCardViewModel> items, int currentPage, int pageSize, int totalItems)
            : base(items, currentPage, pageSize, totalItems) { }
    }
}
