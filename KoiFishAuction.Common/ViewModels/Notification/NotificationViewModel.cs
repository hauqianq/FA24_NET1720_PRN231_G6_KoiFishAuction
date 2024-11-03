namespace KoiFishAuction.Common.ViewModels.Notification; 
public class NotificationViewModel {
    public int Id { get; set; }
    public required string Message { get; set; }
    public required string Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime Date { get; set; }
    public int? BidId { get; set; }
    public string? Remarks { get; set; }
    public required string FishName { get; set; }
    public decimal FishPrice { get; set; }
    public int FishAge { get; set; }
    public required string SenderUsername { get; set; }
    public required string Username { get; set; }
}
