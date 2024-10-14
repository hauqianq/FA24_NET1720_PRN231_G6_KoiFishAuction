namespace KoiFishAuction.Common.ViewModels.AuctionSession
{
    public class AuctionSessionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public string? WinnerUsername { get; set; }
    }
}
