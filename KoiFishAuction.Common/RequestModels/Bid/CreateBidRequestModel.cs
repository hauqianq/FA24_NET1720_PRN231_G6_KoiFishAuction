namespace KoiFishAuction.Common.RequestModels.Bid
{
    public class CreateBidRequestModel
    {
        public int AuctionSessionId { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public string Currency { get; set; }
        public string Location { get; set; }
    }
}
