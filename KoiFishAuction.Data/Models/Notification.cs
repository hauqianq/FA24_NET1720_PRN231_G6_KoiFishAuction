namespace KoiFishAuction.Data.Models;

public class Notification
{
    public int Id { get; set; }

    public int? UserId { get; set; }
    public int? ItemId { get; set; }
    public string Message { get; set; }
    public string Type { get; set; }
    public bool? IsRead { get; set; }
    public DateTime Date { get; set; }
    public int? BidId { get; set; }
    public int? SenderId { get; set; }
    public string Remarks { get; set; }

    public Bid Bid { get; set; }
    public KoiFish Item { get; set; }
    public User Sender { get; set; }
    public User User { get; set; }
}
