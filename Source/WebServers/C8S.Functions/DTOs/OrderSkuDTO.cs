using System.Text.Json.Serialization;

namespace C8S.Functions.DTOs;

public record OrderSkuDTO
{
    [JsonPropertyName("order_sku_id")]
    public int OrderSkuId { get; init; }
    
    [JsonPropertyName("ordinal")]
    public int Ordinal { get; set; }

    [JsonPropertyName("quantity")]
    public short Quantity { get; set; }

    [JsonPropertyName("sku")]
    public SkuDTO Sku { get; init; } = null!;
}