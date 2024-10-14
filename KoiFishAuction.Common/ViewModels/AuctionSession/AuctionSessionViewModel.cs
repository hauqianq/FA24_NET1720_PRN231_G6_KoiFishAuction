namespace KoiFishAuction.Common.ViewModels.AuctionSession
{
    public class AuctionSessionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int KoiFishId { get; set; }
        public string KoiFishName { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public string? WinnerUsername { get; set; }
        public decimal MinIncrement { get; set; }
    }
}
