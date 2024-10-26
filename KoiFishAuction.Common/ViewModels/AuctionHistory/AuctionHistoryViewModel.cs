namespace KoiFishAuction.Common.ViewModels.AuctionHistory;

public sealed class AuctionHistoryViewModel
{
    public int Id { get; set; }
    
    public decimal WinningAmount { get; set; }
    public DateTime WinningDate { get; set; }
    public string PaymentStatus { get; set; }
    public string DeliveryStatus { get; set; }
    public string FeedbackStatus { get; set; }
    public string Remarks { get; set; }

    
    public DateTime AuctionSessionStartTime { get; set; }
    public DateTime AuctionSessionEndTime { get; set; }
    public int AuctionSessionStatus { get; set; }
    
    public string FishName { get; set; }


    public string OwnerName { get; set; }
    
    public string WinnerName { get; set; }
}