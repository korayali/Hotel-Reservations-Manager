namespace HotelReservationsManager.Models.ViewModels.Shared
{
    public class PageResultViewModel<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public PagingViewModel Paging { get; set; } = new();
        public PageResultViewModel()
        {
        }
        public PageResultViewModel(IEnumerable<T> items)
        {
            Items = items;
        }
        public PageResultViewModel(IEnumerable<T> items, int currentPage, int pageSize, int totalItems)
        {
            Items = items;
            Paging = new PagingViewModel(currentPage, pageSize, totalItems);
        }
    }
}
