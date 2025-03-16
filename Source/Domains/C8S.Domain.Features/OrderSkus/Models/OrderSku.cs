using C8S.Domain.Features.Orders.Models;
using C8S.Domain.Features.Skus.Models;

namespace C8S.Domain.Features.OrderSkus.Models;

public record OrderSku
{
    public int OrderSkuId { get; init; }
    public int Ordinal { get; init; }
    public short Quantity { get; init; }
    public Sku Sku { get; init; } = null!;
}