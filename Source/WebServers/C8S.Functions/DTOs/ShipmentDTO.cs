using System.Text.Json.Serialization;
using C8S.Domain.Enums;

namespace C8S.Functions.DTOs;

public record ShipmentDTO
{
    [JsonPropertyName("shipment_id")]
    public int ShipmentId { get; init; }

    [JsonPropertyName("tracking_number")]
    public string TrackingNumber { get; init; } = null!;

    [JsonPropertyName("ship_method")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ShipMethod ShipMethod { get; init; } = default!;

    [JsonPropertyName("ship_method_other")]
    public string ShipMethodOther { get; init; } = null!;
}