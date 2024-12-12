using System.Text.RegularExpressions;
using AutoMapper;
using C8S.Applications.Models;
using C8S.Applications.Services;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Enums;
using C8S.Domain.Obsolete.DTOs;
using C8S.UtilityApp.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SC.Common.Extensions;
using SC.Common.Models;

namespace C8S.UtilityApp.Tasks;

internal class ProcessApplications(
    ILogger<ProcessApplications> logger,
    ApplicationService applicationService,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    IMapper mapper)
    : IActionLauncher
{
    public async Task<int> Launch()
    {
        Console.WriteLine($"=== {nameof(ProcessApplications)} ===");
        
        Console.WriteLine("Continue? Y/N");
        var checkContinue = Console.ReadKey();

        var firstChar = Char.ToLower(checkContinue.KeyChar);
        if (firstChar != 'y') return 0;

        Console.WriteLine();

        var response = await applicationService.ProcessApplications(ProcessApplication);
        logger.LogInformation("{Processed:#,##0} processed; {Successful:#,##0} successful", response.TotalProcessed, response.TotalSuccessful);
        foreach (var error in response.Errors)
            logger.LogInformation("ERROR: {@Error}", error);

        logger.LogInformation("{Name}: complete.", nameof(ProcessApplications));
        return 0;
    }

    private async Task<SerializableException?> ProcessApplication(SubmittedApplication submitted)
    {
        var rxClubString = new Regex(@"(?<ageLevel>\w{2})\:Season(?<season>\d)\:(?<startYear>\d{4})-(?<startMonth>\d{2})-(?<startDay>\d{2})");

        try
        {
            var dto = mapper.Map<ApplicationDTO>(submitted);
            var application = mapper.Map<RequestDb>(dto);

            application.PersonTimeZone = "Eastern Standard Time";
            application.RequestedClubs ??= new List<RequestedClubDb>();

            foreach (var clubString in submitted.ClubListString.Split(" "))
            {
                if (String.IsNullOrEmpty(clubString)) continue;
                var match = rxClubString.Match(clubString.Trim());
                if (!match.Success) throw new Exception($"Could not match club string:{clubString}");

                var applicationClub = new RequestedClubDb()
                {
                    ClubSize = ClubSize.Size12,
                    Season = Int32.Parse(match.Groups["season"].Value),
                    AgeLevel = match.Groups["ageLevel"].Value switch
                    {
                        "K2" => AgeLevel.GradesK2,
                        "35" => AgeLevel.Grades35,
                        _ => throw new Exception($"Could not match age level: {match.Groups["ageLevel"].Value}")
                    },
                    StartsOn = new DateOnly(
                        int.Parse(match.Groups["startYear"].Value), 
                        int.Parse(match.Groups["startMonth"].Value), 
                        int.Parse(match.Groups["startDay"].Value))
                };
                application.RequestedClubs.Add(applicationClub);
            }

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            await dbContext.Requests.AddAsync(application);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            return exception.ToSerializableException();
        }

        return null;
    }
}