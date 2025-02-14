using System.Text.Json;
using C8S.FullSlate.Services;
using C8S.WordPress.Abstractions.Commands;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Services;
using Microsoft.AspNetCore.Mvc;
using SC.Common.Extensions;
using SC.Common.Interactions;

namespace C8S.AdminApp.Controllers
{
    [ApiController]
    public class WordPressController(
        ILoggerFactory loggerFactory,
        WordPressService wordPressService
        ) : ControllerBase
    {
        private readonly ILogger<WordPressController> _logger = loggerFactory.CreateLogger<WordPressController>();

        #region PUT
        [HttpPut]
        [Route("api/[controller]")]
        public async Task<DomainResponse<WordPressUser>> PutNote(
            [FromBody] WordPressUserAddCommand command)
        {
            try
            {
                await wordPressService.CreateWordPressUserFromPerson(command.PersonId);
                return new DomainResponse<WordPressUser>()
                {
                    Result = new WordPressUser()
                    {
                        Username = "dug",
                        Email = "dug@bedtimemath.org"
                    }
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while adding WordPress user: {Json}", JsonSerializer.Serialize(command));
                return new DomainResponse<WordPressUser>()
                {
                    Exception = exception.ToSerializableException()
                };
            }
        }
        #endregion
    }
}
