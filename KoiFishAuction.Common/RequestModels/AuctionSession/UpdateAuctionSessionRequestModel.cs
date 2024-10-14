namespace KoiFishAuction.Common.RequestModels.AuctionSession
{
    public class UpdateAuctionSessionRequestModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Note { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; }
        public decimal MinIncrement { get; set; }
    }
}
