using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Features.OrderSkus.Models;

namespace C8S.Domain.Mapping.MapProfiles;

internal class OrderSkuProfile: Profile
{
    public OrderSkuProfile()
    {
        CreateMap<OrderSkuDb, OrderSku>();
    }
}
