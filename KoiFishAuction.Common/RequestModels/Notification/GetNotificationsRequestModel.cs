using KoiFishAuction.Common.RequestModels.AuctionHistory;
using System.ComponentModel.DataAnnotations;

namespace KoiFishAuction.Common.RequestModels.Notification; 
public sealed class GetNotificationsRequestModel : PaginationRequest {
    public string? SearchTerm { get; set; }

    public string? Type { get; set; }

    public string? Message {  get; set; }   

    public DateTime? StartDate { get; set; }

    [DateGreaterThan("StartDate", ErrorMessage = "EndDate must be greater than StartDate.")]
    public DateTime EndDate { get; set; } = DateTime.Now;

    public string? FishPriceRange { get; set; }

    public string? FishAgeRange { get; set; }

    public string? SortOrder { get; set; }
}

public class DateGreaterThanAttribute : ValidationAttribute {
    private readonly string _comparisonProperty;

    public DateGreaterThanAttribute(string comparisonProperty) {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        var currentValue = value as DateTime?;

        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
        if (property == null)
            throw new ArgumentException($"Property '{_comparisonProperty}' not found");

        var comparisonValue = property.GetValue(validationContext.ObjectInstance) as DateTime?;

        if (!comparisonValue.HasValue)
            return ValidationResult.Success;

        if (currentValue.HasValue && currentValue <= comparisonValue)
            return new ValidationResult(ErrorMessage);

        return ValidationResult.Success;
    }
}