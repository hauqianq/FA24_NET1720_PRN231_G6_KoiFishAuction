namespace KoiFishAuction.Data.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public decimal Balance { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public DateTime? JoinDate { get; set; }
    public DateTime? LastLogin { get; set; }

    public List<Bid> Bids { get; set; }
    public List<KoiFish> KoiFishes { get; set; }

    // For Auction Owner and Winner
    public List<AuctionHistory> AuctionHistoryOwners { get; set; }
    public List<AuctionHistory> AuctionHistoryWinners { get; set; }

    public List<Notification> NotificationSenders { get; set; }
    public List<Notification> NotificationUsers { get; set; }

    public List<Payment> Payments { get; set; }
}
