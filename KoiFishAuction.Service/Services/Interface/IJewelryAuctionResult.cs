namespace JewelryAuction.Business.Business.Interface
{
    public interface IJewelryAuctionResult
    {
        int Status { get; set; }
        string? Message { get; set; }
        object? Data { get; set; }
    }
}
