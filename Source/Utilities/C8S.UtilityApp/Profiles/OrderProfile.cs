using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Obsolete.DTOs;
using C8S.UtilityApp.Models;

namespace C8S.UtilityApp.Profiles;

internal class OrderProfile: Profile
{
    public OrderProfile()
    {
        CreateMap<OrderSql, OrderDTO>();
        CreateMap<OrderDb, OrderDTO>()
            .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.Display, opt => opt.DoNotValidate())
            .ReverseMap()
            .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.Display, opt => opt.DoNotValidate());
    }
}