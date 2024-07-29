using AutoMapper;
using C8S.Common;
using C8S.Database.Abstractions.DTOs;
using C8S.UtilityApp.Models;

namespace C8S.UtilityApp.Profiles;

internal class ApplicationProfile: Profile
{
    public ApplicationProfile()
    {
        CreateMap<ApplicationSql, ApplicationDTO>()
            .ForMember(m => m.ApplicantLastName, opt => opt.NullSubstitute(SharedConstants.Display.NotSet))
            .ForMember(m => m.ApplicantEmail, opt => opt.NullSubstitute(SharedConstants.Display.NotSet));
    }

}