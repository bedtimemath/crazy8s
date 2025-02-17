using System.Text.Json;
using AutoMapper;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Enums;
using C8S.Domain.Features.Notes.Models;
using C8S.Domain.Features.Notes.Queries;
using C8S.FullSlate.Services;
using C8S.WordPress.Abstractions.Commands;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Abstractions.Queries;
using C8S.WordPress.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Common.Extensions;
using SC.Common.Interactions;

namespace C8S.AdminApp.Controllers
{
    [ApiController]
    public class WordPressController(
        ILoggerFactory loggerFactory,
        IDbContextFactory<C8SDbContext> dbContextFactory,
        WordPressService wordPressService
        ) : ControllerBase
    {
        private readonly ILogger<WordPressController> _logger = loggerFactory.CreateLogger<WordPressController>();

        #region GET LIST
        [HttpPost]
        [Route("api/[controller]")]
        public async Task<DomainResponse<WPUsersListResults>> GetWordPressUsers(
            [FromBody] WPUsersListQuery query)
        {
            try
            {
                var listResults = await wordPressService.GetWordPressUsers(query.IncludeRoles);
                return DomainResponse<WPUsersListResults>.CreateSuccessResponse(listResults);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while executing query: {Query}", JsonSerializer.Serialize(query));
                return DomainResponse<WPUsersListResults>.CreateFailureResponse(exception);
            }

        }
        #endregion

        #region PUT
        [HttpPut]
        [Route("api/[controller]")]
        public async Task<DomainResponse<WPUserDetails>> PutWordPressUser(
            [FromBody] WPUserAddCommand command)
        {
            try
            {
                await using var dbContext = await dbContextFactory.CreateDbContextAsync();

                var person = await dbContext.Persons.FirstOrDefaultAsync(p => p.PersonId == command.PersonId) ??
                             throw new Exception($"Could not find person: #{command.PersonId}");

                throw new NotImplementedException();
                //var wordPressUser = await wordPressService.CreateWordPressUser(
                //    person.LastName, person.FirstName ?? "person_name",
                //    person.Email ?? "person_email", String.Empty.AppendRandomAlphaOnly(6) + "1a!",
                //    person.FirstName, person.LastName);
                //return DomainResponse<WPUserDetails>.CreateSuccessResponse(wordPressUser);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while adding WordPress user: {Json}", JsonSerializer.Serialize(command));
                return DomainResponse<WPUserDetails>.CreateFailureResponse(exception);
            }
        }
        #endregion
    }
}
