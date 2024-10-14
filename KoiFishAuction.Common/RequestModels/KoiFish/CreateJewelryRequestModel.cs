using Microsoft.AspNetCore.Http;

namespace JewelryAuction.Business.RequestModels.Jewelry
{
    public class CreateJewelryRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public IFormFile Image { get; set; }
    }
}
