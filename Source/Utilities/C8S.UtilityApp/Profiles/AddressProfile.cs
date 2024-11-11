using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Obsolete.DTOs;
using C8S.UtilityApp.Models;

namespace C8S.UtilityApp.Profiles;

internal class AddressProfile: Profile
{
    public AddressProfile()
    {
        CreateMap<AddressSql, AddressDTO>();
        CreateMap<AddressDb, AddressDTO>()
            .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.Display, opt => opt.DoNotValidate())
            .ReverseMap()
            .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.Display, opt => opt.DoNotValidate());
    }

}