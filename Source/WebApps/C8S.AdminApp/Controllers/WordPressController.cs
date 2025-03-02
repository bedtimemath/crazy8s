using System.Text.Json;
using C8S.Domain.EFCore.Contexts;
using C8S.WordPress.Abstractions.Commands;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Abstractions.Queries;
using C8S.WordPress.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Common.Responses;

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
        public async Task<WrappedListResponse<WPUserDetails>> GetWordPressUsers(
            [FromBody] WPUsersListQuery query)
        {
            try
            {
                return await wordPressService.GetWordPressUsers(query.IncludeRoles);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while executing query: {Query}", JsonSerializer.Serialize(query));
                return WrappedListResponse<WPUserDetails>.CreateFailureResponse(exception);
            }

        }
        #endregion

        #region PUT
        [HttpPut]
        [Route("api/[controller]")]
        public async Task<WrappedResponse<WPUserDetails>> PutWordPressUser(
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
                return WrappedResponse<WPUserDetails>.CreateFailureResponse(exception);
            }
        }
        #endregion
    }
}
