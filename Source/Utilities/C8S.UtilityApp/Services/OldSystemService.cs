using System.Data;
using C8S.UtilityApp.Extensions;
using C8S.UtilityApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace C8S.UtilityApp.Services;

public class OldSystemService(
    ILogger<OldSystemService> logger,
    string connectionString)
{
    public string ConnectionString => connectionString;

    public async Task<CoachSql?> GetDeletedCoach(Guid id)
    {
        await using var connection = new SqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();
            await using var command = new SqlCommand(CoachSql.SqlGetDeleted, connection);
            command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier);
            command.Parameters["@Id"].Value = id;
            await using var reader = await command.ExecuteReaderAsync();

            if (reader.Read())
                return reader.ConvertToObject<CoachSql>();
        }
        catch (Exception exception)
        {
            logger.LogCritical(exception, "Could not read database: {Message}", exception.Message);
        }
        finally
        {
            await connection.CloseAsync();
        }
        return null;
    }

    public async Task<List<AddressSql>> GetAddresses()
    {
        var addresses = new List<AddressSql>();

        await using var connection = new SqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();
            await using var command = new SqlCommand(AddressSql.SqlGet, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var address = reader.ConvertToObject<AddressSql>();
                addresses.Add(address);
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

        return addresses;
    }
    
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
    
    public async Task<List<ApplicationClubSql>> GetApplicationClubs()
    {
        var applicationClubs = new List<ApplicationClubSql>();

        await using var connection = new SqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();
            await using var command = new SqlCommand(ApplicationClubSql.SqlGet, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var applicationClub = reader.ConvertToObject<ApplicationClubSql>();
                applicationClubs.Add(applicationClub);
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

        return applicationClubs;
    }
    
    public async Task<List<ClubSql>> GetClubs()
    {
        var clubs = new List<ClubSql>();

        await using var connection = new SqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();
            await using var command = new SqlCommand(ClubSql.SqlGet, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var club = reader.ConvertToObject<ClubSql>();
                clubs.Add(club);
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

        return clubs;
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
    
    public async Task<List<OrderSql>> GetOrders()
    {
        var orders = new List<OrderSql>();

        await using var connection = new SqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();
            await using var command = new SqlCommand(OrderSql.SqlGet, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var order = reader.ConvertToObject<OrderSql>();
                orders.Add(order);
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

        return orders;
    }
    
    public async Task<List<OrderTrackerSql>> GetOrderTrackers()
    {
        var trackers = new List<OrderTrackerSql>();

        await using var connection = new SqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();
            await using var command = new SqlCommand(OrderTrackerSql.SqlGet, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var trackerSql = reader.ConvertToObject<OrderTrackerSql>();
                trackers.Add(trackerSql);
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

        return trackers;
    }
    
    public async Task<List<OrderSkuSql>> GetOrderSkus()
    {
        var orderskus = new List<OrderSkuSql>();

        await using var connection = new SqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();
            await using var command = new SqlCommand(OrderSkuSql.SqlGet, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var ordersku = reader.ConvertToObject<OrderSkuSql>();
                orderskus.Add(ordersku);
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

        return orderskus;
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
    
    public async Task<List<SkuSql>> GetSkus()
    {
        var skus = new List<SkuSql>();

        await using var connection = new SqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();
            await using var command = new SqlCommand(SkuSql.SqlGet, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var sku = reader.ConvertToObject<SkuSql>();
                skus.Add(sku);
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

        return skus;
    }
}