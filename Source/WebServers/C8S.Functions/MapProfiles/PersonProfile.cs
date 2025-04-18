﻿using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Functions.DTOs;

namespace C8S.Functions.MapProfiles;

internal class PersonProfile: Profile
{
    public PersonProfile()
    {
        CreateMap<PersonDb, PersonDTO>()
            .ReverseMap();
    }
}