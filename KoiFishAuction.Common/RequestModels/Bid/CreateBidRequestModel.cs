namespace JewelryAuction.Business.RequestModels.Bid
{
    public class CreateBidRequestModel
    {
        public int AuctionId { get; set; }
        public int BidderId { get; set; }
        public decimal Amount { get; set; }
    }
}
