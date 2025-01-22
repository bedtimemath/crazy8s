using C8S.Domain.Enums;
using System.Text.Json.Serialization;

namespace C8S.Domain.Features.Requests.Models;

public record RequestDetails: RequestAbstract
{
    public ApplicantType? PersonType { get; init; }
    public string? PersonPhone { get; init; }
    public string? PersonTimeZone { get; init; }
    
    public string? PlaceAddress1 { get; init; }
    public string? PlaceAddress2 { get; init; }
    public string? PlacePostalCode { get; init; }

    public string? ReferenceSource { get; init; }
    public string? ReferenceSourceOther { get; init; }

    public string? Comments { get; init; }
    
    public int? PlaceId { get; set; }
    public int? PersonId { get; set; }

    [JsonIgnore]
    public string PlaceCityStateZip =>
        $"{PlaceCityState} {PlacePostalCode}".Trim();
}
