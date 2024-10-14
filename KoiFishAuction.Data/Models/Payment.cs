namespace KoiFishAuction.Data.Models;

public class Payment
{
    public int Id { get; set; }

    public int HistoryId { get; set; }
    public int UserId { get; set; }

    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Method { get; set; }
    public string Status { get; set; }
    public string Confirmation { get; set; }
    public string Remarks { get; set; }
    public string Currency { get; set; }

    public AuctionHistory History { get; set; }
    public User User { get; set; }
}
