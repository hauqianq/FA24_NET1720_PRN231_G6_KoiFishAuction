using KoiFishAuction.Common.ViewModels.Bid;

namespace KoiFishAuction.Common.ViewModels.AuctionSession
{
    public class AuctionSessionDetailViewModel : AuctionSessionViewModel
    {
        public List<string> Images { get; set; }
        public string Note { get; set; }
        public string? WinnerUsername { get; set; }
        public decimal MinIncrement { get; set; }
        public List<BidViewModel> BidViewModels { get; set; }
    }
}
