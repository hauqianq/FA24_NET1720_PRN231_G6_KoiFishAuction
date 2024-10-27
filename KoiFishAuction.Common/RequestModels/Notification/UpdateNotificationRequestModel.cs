namespace KoiFishAuction.Common.RequestModels.Notification; 
public class UpdateNotificationRequestModel {
    public int Id { get; set; }
    public required string Message { get; set; }
    public required string Type { get; set; }
    public bool IsRead { get; set; }
    public string? Remarks { get; set; }
}
