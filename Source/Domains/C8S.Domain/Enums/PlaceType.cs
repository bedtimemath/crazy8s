using SC.Common.Attributes;

namespace C8S.Domain.Enums;

public enum PlaceType
{
    [Label("Organization")]
    Organization,
    [Label("School")]
    School,
    [Label("Library")]
    Library,
    [Label("Home School Co-Op")]
    HomeSchool,
    [Label("Boys and Girls Club")]
    BoysGirlsClub,
    [Label("YMCA")]
    YMCA,
    [Label("Other")]
    Other,

}