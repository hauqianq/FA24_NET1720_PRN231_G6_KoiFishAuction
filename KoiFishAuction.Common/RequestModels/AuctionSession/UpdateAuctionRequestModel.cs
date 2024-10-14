namespace JewelryAuction.Business.RequestModels.Auction
{
    public class UpdateAuctionRequestModel
    {
        public int AuctionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
