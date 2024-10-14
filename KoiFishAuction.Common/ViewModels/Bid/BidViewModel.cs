namespace KoiFishAuction.Common.ViewModels.Bid
{
    public class BidViewModel
    {
        public int Id { get; set; }

        public int AuctionSessionId { get; set; }
        public int BidderId { get; set; }

        public decimal Amount { get; set; }
        public string Note { get; set; }
        public bool IsWinning { get; set; }
        public string Currency { get; set; }
        public DateTime Timestamp { get; set; }
        public string Location { get; set; }
    }
}
