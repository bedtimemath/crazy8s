using System.Text.Json.Serialization;
using C8S.Domain.Enums;

namespace C8S.Functions.DTOs;

[Serializable]
public record OrderDTO
{
    [JsonPropertyName("order_id")]
    public int OrderId { get; init; }

    [JsonPropertyName("number")]
    public int Number { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("status")]
    public OrderStatus Status { get; init; } = default!;

    [JsonPropertyName("contact_name")]
    public string? ContactName { get; init; }

    [JsonPropertyName("contact_email")]
    public string? ContactEmail { get; init; }

    [JsonPropertyName("contact_phone")]
    public string? ContactPhone { get; init; }

    [JsonPropertyName("recipient")]
    public string Recipient { get; init; } = null!;

    [JsonPropertyName("shipping_address_1")]
    public string Line1 { get; init; } = null!;

    [JsonPropertyName("shipping_address_2")]
    public string? Line2 { get; init; }

    [JsonPropertyName("city")]
    public string City { get; init; } = null!;

    [JsonPropertyName("state")]
    public string State { get; init; } = null!;

    [JsonPropertyName("zip_code")]
    public string ZIPCode { get; init; } = null!;

    [JsonPropertyName("is_military")]
    public bool IsMilitary { get; set; } = false!;

    [JsonPropertyName("ordered_on")]
    public DateTimeOffset OrderedOn { get; set; } = default!;

    [JsonPropertyName("arrive_by")]
    public DateOnly? ArriveBy { get; set; } = null!;

    [JsonPropertyName("shipped_on")]
    public DateTimeOffset? ShippedOn { get; set; } = null!;

    [JsonPropertyName("emailed_on")]
    public DateTimeOffset? EmailedOn { get; set; } = null!;

    [JsonPropertyName("shipments")]
    public List<ShipmentDTO> Shipments { get; init; } = [];
}