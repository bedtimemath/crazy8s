using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Features.OrderOffers.Models;

namespace C8S.Domain.Mapping.MapProfiles;

internal class OrderOfferProfile: Profile
{
    public OrderOfferProfile()
    {
        CreateMap<OrderOfferDb, OrderOffer>();
    }
}
