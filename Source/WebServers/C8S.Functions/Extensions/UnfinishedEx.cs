using System.Diagnostics;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Helpers.PassThrus;
using SC.Common.Interfaces;

namespace C8S.Functions.Extensions;

public static class UnfinishedEx
{
    public static RequestDb ToRequest(this UnfinishedDb unfinished,
        IDateTimeHelper? dateTimeHelper = null)
    {
        dateTimeHelper ??= new PassthruDateTimeHelper();

        var request = new RequestDb()
        {
            Status = RequestStatus.Received,
            PersonType = unfinished.PersonType,
            PersonFirstName = unfinished.PersonFirstName,
            PersonLastName = unfinished.PersonLastName ?? SoftCrowConstants.Display.NotSet,
            PersonEmail = unfinished.PersonEmail ?? SoftCrowConstants.Display.NotSet,
            PersonPhone = unfinished.PersonPhone,
            PersonTimeZone = unfinished.PersonTimeZone ?? SoftCrowConstants.Display.NotSet,
            PlaceName = unfinished.PlaceName,
            PlaceAddress1 = unfinished.PlaceAddress1,
            PlaceAddress2 = unfinished.PlaceAddress2,
            PlaceCity = unfinished.PlaceCity,
            PlaceState = unfinished.PlaceState,
            PlacePostalCode = unfinished.PlacePostalCode,
            PlaceType = unfinished.PlaceType,
            PlaceTypeOther = unfinished.PlaceTypeOther,
            PlaceTaxIdentifier = unfinished.PlaceTaxIdentifier,
            WorkshopCode = unfinished.WorkshopCode,
            ReferenceSource = unfinished.ReferenceSource,
            ReferenceSourceOther = unfinished.ReferenceSourceOther,
            Comments = unfinished.Comments,
            SubmittedOn = dateTimeHelper.Now,
            PersonId = unfinished.PersonId,
            PlaceId = unfinished.PlaceId
        };

        var clubStrings = unfinished.ClubsString?.Split(' ') ?? [];
        foreach (var clubString in clubStrings)
        {
            var parts = clubString.Split(':');
            if (parts.Length != 3) throw new UnreachableException($"ClubString cannot be parsed: {clubString}");

            var applicationClub = new RequestedClubDb()
            {
                Request = request,
                ClubSize = ClubSize.Size16,
                AgeLevel = parts[0] switch
                {
                    "K2" => AgeLevel.GradesK2,
                    "35" => AgeLevel.Grades35,
                    _ => throw new UnreachableException($"Unrecognizable AgeLevel: {parts[0]}")
                },
                Season = parts[1] switch
                {
                    "Season1" => 1,
                    "Season2" => 2,
                    "Season3" => 3,
                    _ => throw new UnreachableException($"Unrecognizable Season: {parts[1]}")
                },
                StartsOn = DateOnly.Parse(parts[2])
            };
            request.RequestedClubs ??= new List<RequestedClubDb>();
            request.RequestedClubs.Add(applicationClub);
        }

        return request;
    }
}