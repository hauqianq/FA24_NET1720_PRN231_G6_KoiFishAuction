namespace KoiFishAuction.Common.ViewModels.Bid
{
    public class BidViewModel
    {
        public string BidderName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsWinning { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
    }
}
