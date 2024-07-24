﻿using C8S.Database.Abstractions.DTOs;
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
        "SELECT c.[Id] AS [OldSystemCompanyId], o.[Id] AS [OldSystemOrganizationId], c.[Name], c.[TimeZoneId] AS [TimeZone], c.[CultureId] As [Culture], t.[Name] AS [OldSystemType], o.[OrganizationTypeOther] AS [TypeOther], CASE WHEN LEN(o.[TaxId]) = 0 THEN NULL ELSE o.[TaxId] END AS [TaxIdentifier],  o.[Notes] As [OldSystemNotes], o.[Created] As [CreatedOn] FROM [Crazy8s].[Organization] o LEFT JOIN [Crazy8s].[OrganizationType] t ON t.[Id] = o.[OrganizationTypeId] LEFT JOIN [Bits].[Company] c ON o.[CompanyId] = c.[Id] WHERE c.[DeletedBy] IS NULL AND o.[DeletedBy] IS NULL";

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
}