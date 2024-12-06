using AutoMapper;
using C8S.Applications;
using C8S.Applications.Models;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Enums;
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

        CreateMap<SubmittedApplication, ApplicationDTO>()
            .ForMember(m => m.ApplicantType,
                opt => opt.MapFrom(
                    (src) => src.IsSupervisor ? ApplicantType.Supervisor : ApplicantType.Coach))
            .ForMember(m => m.OrganizationType,
                opt => opt.MapFrom(
                    (src) => src.OrganizationType == CoachAppConstants.OrganizationTypes.School ? PlaceType.School :
                            (src.OrganizationType == CoachAppConstants.OrganizationTypes.Library ? PlaceType.Library :
                            (src.OrganizationType == CoachAppConstants.OrganizationTypes.HomeSchool ? PlaceType.HomeSchool :
                            (src.OrganizationType == CoachAppConstants.OrganizationTypes.BoysGirlsClub ? PlaceType.BoysGirlsClub :
                            (src.OrganizationType == CoachAppConstants.OrganizationTypes.YMCA ? PlaceType.YMCA : 
                                PlaceType.Other))))))
            .ForMember(m => m.SubmittedOn, opt => opt.MapFrom(src => src.CreatedOn))
            ;
        
        CreateMap<RequestDb, ApplicationDTO>()
            .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.Display, opt => opt.DoNotValidate())
            .ReverseMap()
            .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.Display, opt => opt.DoNotValidate());
    }

}