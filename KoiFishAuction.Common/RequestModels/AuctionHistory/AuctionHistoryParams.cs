namespace KoiFishAuction.Common.RequestModels.AuctionHistory;

public sealed class AuctionHistoryParams : PaginationRequest
{
    public string? FeedbackStatus { get; set; }
    public string? DeliveryStatus { get; set; }
    public string? PaymentStatus { get; set; }
    public string? OrderBy { get; set; }
};

public abstract class PaginationRequest
{
    private const int MaxPageSize = 25;

    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}