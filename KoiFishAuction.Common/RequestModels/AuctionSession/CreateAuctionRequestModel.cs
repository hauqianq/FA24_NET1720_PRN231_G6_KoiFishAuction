namespace JewelryAuction.Business.RequestModels.Auction
{
    public class CreateAuctionRequestModel
    {
        public int JewelryId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
