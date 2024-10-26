namespace KoiFishAuction.Common.ViewModels.AuctionSession
{
    public class AuctionSessionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string KoiFishName { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
    }
}
