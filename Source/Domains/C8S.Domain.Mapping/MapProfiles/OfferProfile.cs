using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Features.Offers.Models;

namespace C8S.Domain.Mapping.MapProfiles;

internal class OfferProfile: Profile
{
    public OfferProfile()
    {
        CreateMap<OfferDb, Offer>();
    }
}
