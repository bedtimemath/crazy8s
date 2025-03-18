using AutoMapper;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Features.Persons.Models;

namespace C8S.Domain.Mapping.MapProfiles;

internal class PersonProfile: Profile
{
    public PersonProfile()
    {
        CreateMap<PersonDb, Person>();
        CreateMap<PersonDb, PersonWithOrders>();
    }
}
