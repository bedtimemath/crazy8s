﻿using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Features.Orders.Models;

namespace C8S.AdminApp.MapProfiles;

internal class OrderProfile: Profile
{
    public OrderProfile()
    {
        CreateMap<OrderDb, Order>();
    }
}
