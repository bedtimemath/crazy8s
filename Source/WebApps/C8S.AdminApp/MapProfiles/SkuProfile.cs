using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Features.Skus.Models;

namespace C8S.AdminApp.MapProfiles;

internal class SkuProfile: Profile
{
    public SkuProfile()
    {
        CreateMap<SkuDb, Sku>();
    }
}
