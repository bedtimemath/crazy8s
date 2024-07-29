using AutoMapper;
using C8S.Database.Abstractions.DTOs;
using C8S.UtilityApp.Models;

namespace C8S.UtilityApp.Profiles;

internal class ApplicationClubProfile: Profile
{
    public ApplicationClubProfile()
    {
        CreateMap<ApplicationClubSql, ApplicationClubDTO>();
    }

}