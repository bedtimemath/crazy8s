﻿using AutoMapper;
using C8S.Database.Abstractions.DTOs;
using C8S.Database.EFCore.Models;

namespace C8S.Database.Repository.Profiles;

internal class OrganizationProfile: Profile
{
    public OrganizationProfile()
    {
        CreateMap<OrganizationDb, OrganizationDTO>()
            .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.Display, opt => opt.DoNotValidate())
            .ReverseMap()
            .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.Display, opt => opt.DoNotValidate());
    }
}