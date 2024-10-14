namespace KoiFishAuction.Data.Models;

public class AuctionHistory
{
    public int Id { get; set; }

    public int AuctionSessionId { get; set; } 
    public int WinnerId { get; set; }
    public int OwnerId { get; set; }

    public decimal WinningAmount { get; set; }
    public DateTime WinningDate { get; set; }
    public string PaymentStatus { get; set; }
    public string DeliveryStatus { get; set; }
    public string FeedbackStatus { get; set; }
    public string Remarks { get; set; }

    public AuctionSession AuctionSession { get; set; }
    public User Owner { get; set; }
    public User Winner { get; set; }

    public List<Payment> Payments { get; set; }
}
