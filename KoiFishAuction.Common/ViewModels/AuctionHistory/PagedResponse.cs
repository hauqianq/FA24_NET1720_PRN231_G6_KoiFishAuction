namespace KoiFishAuction.Common.ViewModels.AuctionHistory;

public class PagedResponse<T>
{
    public PagedResponse(PagedList<T> items)
    {
        Items = items;
        CurrentPage = items.CurrentPage;
        ItemsPerPage = items.PageSize;
        TotalItems = items.TotalCount;
        TotalPages = items.TotalPages;
    }

    public List<T> Items { get; set; }
    public int CurrentPage { get; set; }

    public int ItemsPerPage { get; set; }

    public int TotalItems { get; set; }

    public int TotalPages { get; set; }

    public static PagedResponse<T> CreateResponse(PagedList<T> list)
    {
        return new PagedResponse<T>(list);
    }
}