using System.Diagnostics;
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
                var users = await wordPressService.GetWordPressUsers(query.IncludeRoles);
                return WrappedListResponse<WPUserDetails>.CreateSuccessResponse(users, users.Count);
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
                var roles = await wordPressService.GetWordPressRoles();
                return WrappedListResponse<WPRoleDetails>.CreateSuccessResponse(roles, roles.Count);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while executing query: {Query}", JsonSerializer.Serialize(query));
                return WrappedListResponse<WPRoleDetails>.CreateFailureResponse(exception);
            }

        }
        #endregion

        #region GET
        [HttpGet]
        [Route("api/[controller]/users/{id:int}")]
        public async Task<WrappedResponse<WPUserDetails?>> GetUserById(int id)
        {
            try
            {
                var user = await wordPressService.GetWordPressUserById(id);
                return WrappedResponse<WPUserDetails?>.CreateSuccessResponse(user);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while getting user ID#: {Id}", id);
                return WrappedResponse<WPUserDetails?>.CreateFailureResponse(exception);
            }
        }

        [HttpGet]
        [Route("api/[controller]/users/magic-link/{id:int}")]
        public async Task<WrappedResponse<string>> GetMagicLinkForUser(int id)
        {
            try
            {
                var magicLink = await wordPressService.CreateMagicLinkForUser(id);
                return WrappedResponse<string>.CreateSuccessResponse(magicLink);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while getting magic link ID#: {Id}", id);
                return WrappedResponse<string>.CreateFailureResponse(exception);
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
                    DataChangeAction.Added, C8SConstants.Entities.WPUser, wordPressUser.Id, wordPressUser);

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
        [Route("api/[controller]/users/{id:int}")]
        public async Task<WrappedResponse<WPUserDetails>> PatchWordPressUserRoles(int id,
            [FromBody] WPUserUpdateRolesCommand command)
        {
            try
            {
                var wordPressUser = await wordPressService.UpdateWordPressUserRoles(id, command.Roles);
                
                // tell the world
                await hubContext.SendDataChange(
                    DataChangeAction.Modified, C8SConstants.Entities.WPUser, id, wordPressUser);

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
        [Route("api/[controller]/users/{id:int}")]
        public async Task<WrappedResponse> DeleteWordPressUser(int id)
        {
            try
            {
                await wordPressService.DeleteWordPressUser(id);
                
                // tell the world
                await hubContext.SendDataChange(
                    DataChangeAction.Deleted, C8SConstants.Entities.WPUser);

                return WrappedResponse.CreateSuccessResponse();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error creating WordPress user ID#: {@Id}", id);
                return WrappedResponse.CreateFailureResponse(exception);
            }
        }
        #endregion
    }
}
