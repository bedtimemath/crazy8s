using AutoMapper;
using C8S.Database.Abstractions.DTOs;
using C8S.UtilityApp.Models;

namespace C8S.UtilityApp.Profiles;

internal class SkuProfile: Profile
{
    public SkuProfile()
    {
        CreateMap<SkuSql, SkuDTO>();
    }
}