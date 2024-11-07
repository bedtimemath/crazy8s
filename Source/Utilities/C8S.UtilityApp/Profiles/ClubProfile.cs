﻿using AutoMapper;
using C8S.Domain.Obsolete.DTOs;
using C8S.UtilityApp.Models;

namespace C8S.UtilityApp.Profiles;

internal class ClubProfile: Profile
{
    public ClubProfile()
    {
        CreateMap<ClubSql, ClubDTO>();
    }
}