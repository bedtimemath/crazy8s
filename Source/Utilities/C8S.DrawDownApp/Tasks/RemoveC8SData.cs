using System.Diagnostics;
using C8S.DrawDownApp.Base;
using C8S.DrawDownApp.Extensions;
using C8S.DrawDownApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace C8S.DrawDownApp.Tasks;

internal class RemoveC8SData(
    ILogger<RemoveC8SData> logger,
    RemoveC8SDataOptions options,
    IConfiguration configuration) : IActionLauncher
{
    public async Task<int> Launch()
    {
        const string readBitsTables =
            "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Bits' AND TABLE_TYPE = 'BASE TABLE' AND EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Bits' AND TABLE_NAME = INFORMATION_SCHEMA.TABLES.TABLE_NAME AND COLUMN_NAME = 'CreatedBy' );";
        const string readCrazy8sTables =
            "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Crazy8s' AND TABLE_TYPE = 'BASE TABLE' AND EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Crazy8s' AND TABLE_NAME = INFORMATION_SCHEMA.TABLES.TABLE_NAME AND COLUMN_NAME = 'CreatedBy' );";

        // load the configurations that we need
        var connections = configuration.GetSection(Connections.SectionName).Get<Connections>() ??
                          throw new Exception($"Missing configuration section: {Connections.SectionName}");

        logger.LogInformation("=== {Name} ===", nameof(RemoveC8SData));
        logger.LogInformation("Platform: {Platform}", options.Platform);
        logger.LogInformation("Database: {Database}", connections.Database);
        logger.LogInformation("OldSystem: {OldSystem}", connections.OldSystem);

        logger.LogInformation("Continue? Y/N");
        var checkContinue = Console.ReadKey();

        var firstChar = Char.ToLower(checkContinue.KeyChar);
        if (firstChar != 'y') return 0;
        Console.WriteLine();

        var tables = new HashSet<string>();

        await using var cnn = new SqlConnection(connections.OldSystem);
        await cnn.OpenAsync();

        var cmd = new SqlCommand(readBitsTables, cnn);
        var reader = await cmd.ExecuteReaderAsync();
        int added = 0, skipped = 0;
        while (reader.Read())
        {
            var tableName = reader.GetString(0);
            if (tables.Add($"[Bits].[{tableName}]")) added++;
            else skipped++;
        }
        logger.LogInformation("Bits: Added {Added:#,##0}; skipped {Skipped:#,##0}.", added, skipped);
        reader.Close();

        cmd = new SqlCommand(readCrazy8sTables, cnn);
        reader = await cmd.ExecuteReaderAsync();
        added = 0; skipped = 0;
        while (reader.Read())
        {
            var tableName = reader.GetString(0);
            if (tables.Add($"[Crazy8s].[{tableName}]")) added++;
            else skipped++;
        }
        logger.LogInformation("C8S: Added {Added:#,##0}; skipped {Skipped:#,##0}.", added, skipped);
        reader.Close();

        var activeIds = new HashSet<Guid>();
        
        cmd = new SqlCommand("SELECT COUNT(DISTINCT [AppSessionId]) FROM [Bits].[AccessAttempt] WHERE [AppSessionId] IS NOT NULL;", cnn);
        var total = (int)((await cmd.ExecuteScalarAsync()) ?? throw new UnreachableException());

        ConsoleEx.StartProgress($"Checking {total:#,##0} in [Bits].[AccessAttempt]... ");
        cmd = new SqlCommand("SELECT DISTINCT [AppSessionId] FROM [Bits].[AccessAttempt] WHERE [AppSessionId] IS NOT NULL;", cnn);
        reader = await cmd.ExecuteReaderAsync();
        var count = 0;
        while (reader.Read())
        {
            var appSessionId = reader.GetGuid(0);
            activeIds.Add(appSessionId);
            ConsoleEx.ShowProgress((float)++count / total);
        }
        reader.Close();
        ConsoleEx.EndProgress();

        foreach (var table in tables)
        {
            cmd = new SqlCommand($"SELECT COUNT(DISTINCT [CreatedBy]) FROM {table} WHERE [CreatedBy] IS NOT NULL;", cnn); 
            total = (int)((await cmd.ExecuteScalarAsync()) ?? throw new UnreachableException());

            ConsoleEx.StartProgress($"Checking {total:#,##0} in {table}... ");
            cmd = new SqlCommand($"SELECT DISTINCT [CreatedBy] FROM {table} WHERE [CreatedBy] IS NOT NULL;", cnn);
            reader = await cmd.ExecuteReaderAsync();
            count = 0;
            while (reader.Read())
            {
                var appSessionId = reader.GetGuid(0);
                activeIds.Add(appSessionId);
                ConsoleEx.ShowProgress((float)++count / total);
            }
            reader.Close();
            ConsoleEx.EndProgress();
        }

        logger.LogInformation("Legitimate AppSession Ids: {Total:#,##0}", activeIds.Count);

        var removeIds = new HashSet<Guid>() { Guid.NewGuid() };
        
        cmd = new SqlCommand($"SELECT COUNT(*) FROM [Bits].[AppSession];", cnn); 
        total = (int)((await cmd.ExecuteScalarAsync()) ?? throw new UnreachableException());
        
        ConsoleEx.StartProgress($"Reading {total:#,##0} from [Bits].[AppSession]... ");
        cmd = new SqlCommand($"SELECT [Id] FROM [Bits].[AppSession];", cnn);
        reader = await cmd.ExecuteReaderAsync();
        count = 0;
        while (reader.Read())
        {
            var appSessionId = reader.GetGuid(0);
            removeIds.Add(appSessionId);
            ConsoleEx.ShowProgress((float)++count / total);
        }
        reader.Close();
        ConsoleEx.EndProgress();

        removeIds.ExceptWith(activeIds);
        logger.LogInformation("AppSession Ids to Remove: {Total:#,##0}", removeIds.Count);

        var startDateTime = DateTimeOffset.UtcNow;
        logger.LogInformation("Started: {Start:O}", startDateTime);

        ConsoleEx.StartProgress($"Removing {removeIds.Count:#,##0} from [Bits].[AppSession]... ");
        int index = 0;
        for (; index < removeIds.Count - 10; index += 10)
        {
            cmd = new SqlCommand($"DELETE FROM [Bits].[AppSession] WHERE [Id] IN (@Id00,@Id01,@Id02,@Id03,@Id04,@Id05,@Id06,@Id07,@Id08,@Id09);", cnn);
            cmd.Parameters.Add(new SqlParameter("@Id00", removeIds.ElementAt(index)));
            cmd.Parameters.Add(new SqlParameter("@Id01", removeIds.ElementAt(index+1)));
            cmd.Parameters.Add(new SqlParameter("@Id02", removeIds.ElementAt(index+2)));
            cmd.Parameters.Add(new SqlParameter("@Id03", removeIds.ElementAt(index+3)));
            cmd.Parameters.Add(new SqlParameter("@Id04", removeIds.ElementAt(index+4)));
            cmd.Parameters.Add(new SqlParameter("@Id05", removeIds.ElementAt(index+5)));
            cmd.Parameters.Add(new SqlParameter("@Id06", removeIds.ElementAt(index+6)));
            cmd.Parameters.Add(new SqlParameter("@Id07", removeIds.ElementAt(index+7)));
            cmd.Parameters.Add(new SqlParameter("@Id08", removeIds.ElementAt(index+8)));
            cmd.Parameters.Add(new SqlParameter("@Id09", removeIds.ElementAt(index+9)));
            await cmd.ExecuteNonQueryAsync();
            ConsoleEx.ShowProgress((float)index / removeIds.Count);
        }
        for (; index < removeIds.Count; index++)
        {
            cmd = new SqlCommand($"DELETE FROM [Bits].[AppSession] WHERE [Id] = @Id;", cnn);
            cmd.Parameters.Add(new SqlParameter("@Id", removeIds.ElementAt(index)));
            await cmd.ExecuteNonQueryAsync();
            ConsoleEx.ShowProgress((float)index / removeIds.Count);
        }
        ConsoleEx.EndProgress();
        
        var endedDateTime = DateTimeOffset.UtcNow;
        logger.LogInformation("Ended: {End:O}", endedDateTime);
        var elapsed = endedDateTime - startDateTime;
        logger.LogInformation("Minutes: {Minutes}", elapsed.TotalMinutes);


        logger.LogInformation("{Name}: complete.", nameof(RemoveC8SData));
        return 0;
    }
}