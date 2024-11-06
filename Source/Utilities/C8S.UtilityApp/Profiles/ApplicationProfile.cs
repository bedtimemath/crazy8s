using AutoMapper;
using C8S.Applications;
using C8S.Applications.Models;
using C8S.Database.Abstractions.DTOs;
using C8S.Database.Abstractions.Enumerations;
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
                    (src) => src.OrganizationType == CoachAppConstants.OrganizationTypes.School ? OrganizationType.School :
                            (src.OrganizationType == CoachAppConstants.OrganizationTypes.Library ? OrganizationType.Library :
                            (src.OrganizationType == CoachAppConstants.OrganizationTypes.HomeSchool ? OrganizationType.HomeSchool :
                            (src.OrganizationType == CoachAppConstants.OrganizationTypes.BoysGirlsClub ? OrganizationType.BoysGirlsClub :
                            (src.OrganizationType == CoachAppConstants.OrganizationTypes.YMCA ? OrganizationType.YMCA : 
                                OrganizationType.Other))))))
            .ForMember(m => m.SubmittedOn, opt => opt.MapFrom(src => src.CreatedOn))
            ;
    }

}