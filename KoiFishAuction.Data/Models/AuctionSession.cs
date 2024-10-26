namespace KoiFishAuction.Data.Models;

public class AuctionSession
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string Note { get; set; }
    public int KoiFishId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Status { get; set; }
    public int? WinnerId { get; set; }
    public int CreatorId { get; set; }
    public decimal MinIncrement { get; set; }

    public User Winner { get; set; }
    public User Creator { get; set; }
    public KoiFish KoiFish { get; set; } 
    public List<Bid> Bids { get; set; }
    public AuctionHistory AuctionHistory { get; set; } 
}
