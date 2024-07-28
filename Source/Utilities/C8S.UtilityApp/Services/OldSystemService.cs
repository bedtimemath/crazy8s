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

    public string ConnectionString => connectionString;
    

    public async Task<List<ApplicationSql>> GetApplications()
    {
        var applications = new List<ApplicationSql>();

        await using var connection = new SqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();
            await using var command = new SqlCommand(ApplicationSql.SqlGet, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var application = reader.ConvertToObject<ApplicationSql>();
                applications.Add(application);
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

        return applications;
    }

    public async Task<List<CoachSql>> GetCoaches()
    {
        var coaches = new List<CoachSql>();

        await using var connection = new SqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();
            await using var command = new SqlCommand(CoachSql.SqlGet, connection);
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

    public async Task<List<OrganizationSql>> GetOrganizations()
    {
        var organizations = new List<OrganizationSql>();

        await using var connection = new SqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();
            await using var command = new SqlCommand(OrganizationSql.SqlGet, connection);
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
}