using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Functions.DTOs;

namespace C8S.Functions.MapProfiles;

internal class ShipmentProfile: Profile
{
    public ShipmentProfile()
    {
        CreateMap<ShipmentDb, ShipmentDTO>()
            .ReverseMap();
    }
}