using System.Text.Json;
using C8S.AdminApp.Extensions;
using C8S.AdminApp.Hubs;
using C8S.Domain;
using C8S.WordPress.Abstractions.Commands;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Abstractions.Queries;
using C8S.WordPress.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SC.Common.PubSub;
using SC.Common.Responses;

namespace C8S.AdminApp.Controllers
{
    [ApiController]
    public class WordPressController(
        ILoggerFactory loggerFactory,
        IHubContext<CommunicationHub> hubContext,
        WordPressService wordPressService
        ) : ControllerBase
    {
        private readonly ILogger<WordPressController> _logger = loggerFactory.CreateLogger<WordPressController>();

        #region GET LIST
        [HttpPost]
        [Route("api/[controller]/users")]
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
        [HttpPost]
        [Route("api/[controller]/roles")]
        public async Task<WrappedListResponse<WPRoleDetails>> GetWordPressRoles(
            [FromBody] WPRolesListQuery query)
        {
            try
            {
                return await wordPressService.GetWordPressRoles();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while executing query: {Query}", JsonSerializer.Serialize(query));
                return WrappedListResponse<WPRoleDetails>.CreateFailureResponse(exception);
            }

        }
        #endregion

        #region PUT
        [HttpPut]
        [Route("api/[controller]/users")]
        public async Task<WrappedResponse<WPUserDetails>> PutWordPressUser(
            [FromBody] WPUserAddCommand command)
        {
            try
            {
                var wordPressUser = await wordPressService.CreateWordPressUser(
                    command.Email,
                    command.Name,
                    command.FirstName,
                    command.LastName,
                    command.UserName,
                    command.Password,
                    command.Roles);
                
                // tell the world
                await hubContext.SendDataChange(
                    DataChangeAction.Added, C8SConstants.Entities.WPUser, 0, wordPressUser);

                return WrappedResponse<WPUserDetails>.CreateSuccessResponse(wordPressUser);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while adding WordPress user: {Json}", JsonSerializer.Serialize(command));
                return WrappedResponse<WPUserDetails>.CreateFailureResponse(exception);
            }
        }
        #endregion

        #region PATCH
        [HttpPatch]
        [Route("api/[controller]/users/{username}")]
        public async Task<WrappedResponse<WPUserDetails>> PatchWordPressUser(string username,
            [FromBody] WPUserUpdateRolesCommand command)
        {
            try
            {
                var wordPressUser = await wordPressService.UpdateWordPressUserRoles(
                    command.UserName,
                    command.Roles);
                
                // tell the world
                await hubContext.SendDataChange(
                    DataChangeAction.Modified, C8SConstants.Entities.WPUser, 0, wordPressUser);

                return WrappedResponse<WPUserDetails>.CreateSuccessResponse(wordPressUser);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while adding WordPress user: {Json}", JsonSerializer.Serialize(command));
                return WrappedResponse<WPUserDetails>.CreateFailureResponse(exception);
            }
        }
        #endregion
        
        #region DELETE
        [HttpDelete]
        [Route("api/[controller]/users/{username}")]
        public async Task<WrappedResponse> DeleteWordPressUser(string username)
        {
            try
            {
                await wordPressService.DeleteWordPressUser(username);
                
                // tell the world
                await hubContext.SendDataChange(
                    DataChangeAction.Deleted, C8SConstants.Entities.WPUser);

                return WrappedResponse.CreateSuccessResponse();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while deleting WordPress user: {Username}", username);
                return WrappedResponse.CreateFailureResponse(exception);
            }
        }
        #endregion
    }
}
