using JewelryAuction.Business.Business.Interface;

namespace JewelryAuction.Business.Business.Implementation
{
    public class JewelryAuctionResult : IJewelryAuctionResult
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public JewelryAuctionResult(int status)
        {
            Status = status;
        }
        public JewelryAuctionResult(int status, string message)
        {
            Status = status;
            Message = message;
        }
        
        public JewelryAuctionResult(int status, object data)
        {
            Status = status;
            Data = data;
        }
        public JewelryAuctionResult(int status, string message, object data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
