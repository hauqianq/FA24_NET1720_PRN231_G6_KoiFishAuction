namespace KoiFishAuction.Common.RequestModels.Notification; 
public class CreateNotificationRequestModel {
    public int? UserId { get; set; }
    public int? ItemId { get; set; }
    public required string Message { get; set; }
    public required string Type { get; set; }
    public int? BidId { get; set; }
    public string? Remarks { get; set; }
}
