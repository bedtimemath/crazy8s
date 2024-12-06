using System.Globalization;
using AutoMapper;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Obsolete.DTOs;
using C8S.UtilityApp.Base;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SC.Common.Interfaces;

namespace C8S.UtilityApp.Tasks;

internal class LoadSampleData(
    ILogger<LoadSampleData> logger,
    IRandomizer randomizer,
    LoadSampleDataOptions options,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    IMapper mapper)
    : IActionLauncher
{
    public async Task<int> Launch()
    {
        Console.WriteLine($"=== {nameof(LoadSampleData)} ===");

        if (!Directory.Exists(options.InputPath))
            throw new Exception($"Missing input path: {options.InputPath}");

        var coachesFilePath = Path.Combine(options.InputPath, options.CoachFile);
        if (!File.Exists(coachesFilePath))
            throw new Exception($"Missing group file: {coachesFilePath}");

        // check the connection quickly
        // ReSharper disable once UseAwaitUsing
        // ReSharper disable once MethodHasAsyncOverload
        using (var tempDb = dbContextFactory.CreateDbContext())
        {
            var sqlConnection = tempDb.Database.GetConnectionString();
            Console.WriteLine($"Connection: {sqlConnection}");
        }

        Console.WriteLine("Continue? Y/N");
        var checkContinue = Console.ReadKey();

        var firstChar = Char.ToLower(checkContinue.KeyChar);
        if (firstChar != 'y') return 0;

        Console.WriteLine();

        await ClearAllData();
        logger.LogInformation("All data cleared.");

        var coaches = await ImportCoaches(coachesFilePath);
        logger.LogInformation("Loaded {Count:#,##0} coaches.", coaches.Count);

        logger.LogInformation("{Name}: complete.", nameof(LoadSampleData));
        return 0;
    }

    private async Task ClearAllData()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        await dbContext.Database.ExecuteSqlAsync($"DELETE FROM [Coaches];");
    }

    private async Task<Dictionary<int,CoachDTO>> ImportCoaches(string inputFile)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        using var reader = new StreamReader(inputFile);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<CoachDTO>().ToList();

        // put all the records in a lookup, so they can reference each other
        var lookup = new Dictionary<int, Tuple<CoachDTO, PersonDb>>();

        int ignoreId = 1000000;
        foreach (var dto in records)
        {
            // start with a db that doesn't have its id or its parent id set
            var db = mapper.Map<PersonDb>(dto);
            db.PersonId = 0;

            // add the db to the data context, before hooking up parents & children
            dbContext.Persons.Add(db);

            // add both to the lookup
            lookup.Add(dto.CoachId ?? ignoreId++, new Tuple<CoachDTO, PersonDb>(dto, db));
        }

        // hook up the parents & children
        //foreach (var tempId in lookup.Keys)
        //{
        //    var tuple = lookup[tempId];
        //    var parentId = tuple.Item1.ParentId;
        //    if (parentId == null) continue;

        //    if (!lookup.ContainsKey(parentId.Value))
        //        throw new Exception($"Missing ParentId: {parentId}");

        //    var dbChild = tuple.Item2;
        //    var dbParent = lookup[parentId.Value].Item2;

        //    dbChild.Parent = dbParent;
        //    dbParent.Children ??= new List<CoachDb>();
        //    dbParent.Children.Add(dbChild);
        //}

        await dbContext.SaveChangesAsync();

        var returnLookup = new Dictionary<int, CoachDTO>();
        foreach (var tempId in lookup.Keys)
            returnLookup.Add(tempId, mapper.Map<CoachDTO>(lookup[tempId].Item2));

        return returnLookup;
    }
}