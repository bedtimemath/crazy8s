using C8S.UtilityApp.Extensions;
using C8S.UtilityApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace C8S.UtilityApp.Services;

public class OldSystemService(
    ILogger<OldSystemService> logger,
    string connectionString)
{
    private readonly ILogger<OldSystemService> _logger = logger;

    private const string SqlGetOrganizations = 
        "SELECT c.[Id] AS [OldSystemCompanyId], o.[Id] AS [OldSystemOrganizationId], c.[Name], c.[TimeZoneId] AS [TimeZone], c.[CultureId] As [Culture], t.[Name] AS [OldSystemType], o.[OrganizationTypeOther] AS [TypeOther], CASE WHEN LEN(o.[TaxId]) = 0 THEN NULL ELSE o.[TaxId] END AS [TaxIdentifier], o.[Notes] As [OldSystemNotes], o.[Created] As [CreatedOn] FROM [Crazy8s].[Organization] o LEFT JOIN [Crazy8s].[OrganizationType] t ON t.[Id] = o.[OrganizationTypeId] LEFT JOIN [Bits].[Company] c ON o.[CompanyId] = c.[Id] WHERE c.[DeletedBy] IS NULL AND o.[DeletedBy] IS NULL";

    private const string SqlGetCoaches =
        "SELECT CAST(c.[Id] AS NVARCHAR(50)) AS [OldSystemCoachIdString], CAST(c.[OrganizationId] AS NVARCHAR(50)) AS [OldSystemOrganizationIdString], CAST(u.[Id] AS NVARCHAR(50)) AS [OldSystemUserIdString], CAST(u.[CompanyId] AS NVARCHAR(50)) AS [OldSystemCompanyIdString], c.[FirstName], c.[LastName], c.[Email], c.[TimeZoneId] AS [TimeZone], c.[Phone] AS [PhoneString], c.[PhoneExt], c.[Notes] As [OldSystemNotes], c.[Created] AS [CreatedOn] FROM [Crazy8s].[Coach] c LEFT JOIN [Bits].[User] u ON u.[Id] = c.[UserId] WHERE c.[DeletedBy] IS NULL AND u.[DeletedBy] IS NULL";

    public string ConnectionString => connectionString;

    public async Task<List<OrganizationSql>> GetOrganizations()
    {
        var organizations = new List<OrganizationSql>();

        await using var connection = new SqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();
            await using var command = new SqlCommand(SqlGetOrganizations, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var organization = reader.ConvertToObject<OrganizationSql>();
                organizations.Add(organization);
            }
        }
        catch (Exception exception)
        {
            logger.LogCritical(exception, "Could not read database: {Message}", exception.Message);
        }
        finally
        {
            await connection.CloseAsync();
        }

        return organizations;
    }

    public async Task<List<CoachSql>> GetCoaches()
    {
        var coaches = new List<CoachSql>();

        await using var connection = new SqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();
            await using var command = new SqlCommand(SqlGetCoaches, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var coach = reader.ConvertToObject<CoachSql>();
                coaches.Add(coach);
            }
        }
        catch (Exception exception)
        {
            logger.LogCritical(exception, "Could not read database: {Message}", exception.Message);
        }
        finally
        {
            await connection.CloseAsync();
        }

        return coaches;
    }
}