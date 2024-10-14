namespace KoiFishAuction.Common.RequestModels.AuctionSession
{
    public class CreateAuctionSessionRequestModel
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public int KoiFishId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal MinIncrement { get; set; }
    }
}
