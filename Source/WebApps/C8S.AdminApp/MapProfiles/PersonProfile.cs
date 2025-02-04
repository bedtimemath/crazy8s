using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Features.Persons.Models;

namespace C8S.AdminApp.MapProfiles;

internal class PersonProfile: Profile
{
    public PersonProfile()
    {
        CreateMap<PersonDb, PersonAbstract>()
            .Include<PersonDb, PersonDetails>()
            .Include<PersonDb, PersonListItem>();
        
        CreateMap<PersonDb, PersonDetails>();
        CreateMap<PersonDb, PersonListItem>();
    }
}
