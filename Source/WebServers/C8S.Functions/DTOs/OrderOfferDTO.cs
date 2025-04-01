using System.Text.Json.Serialization;

namespace C8S.Functions.DTOs;

public record OrderOfferDTO
{
    [JsonPropertyName("order_sku_id")]
    public int OrderOfferId { get; init; }
    
    [JsonPropertyName("ordinal")]
    public int Ordinal { get; set; }

    [JsonPropertyName("quantity")]
    public short Quantity { get; set; }

    [JsonPropertyName("offer")]
    public OfferDTO Offer { get; init; } = null!;
}