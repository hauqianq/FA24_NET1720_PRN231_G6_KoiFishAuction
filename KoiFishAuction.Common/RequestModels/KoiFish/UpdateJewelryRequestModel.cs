namespace JewelryAuction.Business.RequestModels.Jewelry
{
    public class UpdateJewelryRequestModel
    {
        public int JewelryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
    }
}
