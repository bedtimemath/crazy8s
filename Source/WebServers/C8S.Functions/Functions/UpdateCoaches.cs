using AutoMapper;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.Features.Persons.Models;
using C8S.Functions.Extensions;
using C8S.WordPress.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace C8S.Functions.Functions
{
    public class UpdateCoaches(
        ILoggerFactory loggerFactory,
        IDbContextFactory<C8SDbContext> dbContextFactory,
        IMapper mapper,
        WordPressService wordPressService)
    {
        #region ReadOnly Constructor Variables

        private readonly ILogger<UpdateCoaches> _logger = loggerFactory.CreateLogger<UpdateCoaches>();
        #endregion

        [Function("UpdateCoaches")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "update-coaches")] HttpRequestData req)
        {
            HttpResponseData response;
            try
            {
                _logger.LogInformation("UpdateCoaches triggered");

                await using var dbContext = await dbContextFactory.CreateDbContextAsync();

                // read the existing coaches on word press
                var coaches = await wordPressService.GetWordPressUsers(["coach"]);

                // check that each of them has the correct word press id
                var coachesUpdated = new List<Person>();
                foreach (var coach in coaches)
                {
                    if (String.IsNullOrEmpty(coach.Email)) continue; // shouldn't happen

                    var personDb = await dbContext.Persons
                        .FirstOrDefaultAsync(p => !String.IsNullOrEmpty(p.Email) &&
                                                  p.Email.Trim().ToLower() == coach.Email.Trim().ToLower() &&
                                                  p.WordPressId != coach.Id);
                    if (personDb == null) continue;

                    // update the person with the word press id
                    personDb.WordPressId = coach.Id;
                    await dbContext.SaveChangesAsync();
                    coachesUpdated.Add(mapper.Map<Person>(personDb));
                }

                // send back the person with their orders
                response = await req.CreateSuccessResponse(coachesUpdated);
            }
            catch (Exception ex)
            {
                response = await req.CreateFailureResponse(ex);
            }

            return response;
        }
    }
}
