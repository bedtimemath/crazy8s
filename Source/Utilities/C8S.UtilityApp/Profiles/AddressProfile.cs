﻿using AutoMapper;
using C8S.Database.Abstractions.DTOs;
using C8S.UtilityApp.Models;

namespace C8S.UtilityApp.Profiles;

internal class AddressProfile: Profile
{
    public AddressProfile()
    {
        CreateMap<AddressSql, AddressDTO>();
    }

}