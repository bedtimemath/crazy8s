using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Obsolete.DTOs;
using C8S.UtilityApp.Models;
using SC.Common;

namespace C8S.UtilityApp.Profiles;

internal class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<ApplicationSql, ApplicationDTO>()
            .ForMember(m => m.ApplicantLastName, opt => opt.NullSubstitute(SoftCrowConstants.Display.NotSet))
            .ForMember(m => m.ApplicantEmail, opt => opt.NullSubstitute(SoftCrowConstants.Display.NotSet));
        
        CreateMap<RequestDb, ApplicationDTO>()
            .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.Display, opt => opt.DoNotValidate())
            .ReverseMap()
            .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.Display, opt => opt.DoNotValidate());
    }

}