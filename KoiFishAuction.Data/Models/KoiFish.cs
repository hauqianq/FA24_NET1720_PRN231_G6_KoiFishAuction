namespace KoiFishAuction.Data.Models;

public partial class KoiFish
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public decimal StartingPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public string ImageUrl { get; set; }
    public int Age { get; set; }
    public string Origin { get; set; }
    public decimal Weight { get; set; }
    public decimal Length { get; set; }
    public string ColorPattern { get; set; }
    public int SellerId { get; set; }

    public virtual User Seller { get; set; }

    public List<AuctionSession> AuctionSessions { get; set; }
    public List<AuctionHistory> AuctionHistories { get; set; }
    public List<Bid> Bids { get; set; }
    public List<Notification> Notifications { get; set; }
}
