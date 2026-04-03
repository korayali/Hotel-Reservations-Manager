using HotelReservationsManager.Models.ViewModels.Shared;

public class PageResultViewModel<T> : PagingViewModel
{
    public IEnumerable<T> Items { get; set; } = [];

    public PageResultViewModel() { }

    public PageResultViewModel(IEnumerable<T> items)
    {
        Items = items;
    }

    public PageResultViewModel(IEnumerable<T> items, int currentPage, int pageSize, int totalItems)
        : base(currentPage, pageSize, totalItems)
    {
        Items = items;
    }
}