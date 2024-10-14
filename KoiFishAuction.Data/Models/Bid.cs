namespace KoiFishAuction.Data.Models;

public class Bid
{
    public int Id { get; set; }

    public int AuctionSessionId { get; set; }
    public int BidderId { get; set; }

    public decimal Amount { get; set; }
    public string Note { get; set; }
    public bool IsWinning { get; set; }
    public string Currency { get; set; }
    public DateTime Timestamp { get; set; }
    public string Location { get; set; }

    public AuctionSession AuctionSession { get; set; }
    public User Bidder { get; set; }

    public List<Notification> Notifications { get; set; }
}
