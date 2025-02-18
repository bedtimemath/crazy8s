using System.Net;
using System.Web;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.EFCore.Models;
using C8S.Functions.Extensions;
using HttpMultipartParser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace C8S.Functions.Functions
{
    public class GetCoachOrders(
        ILoggerFactory loggerFactory,
        IDbContextFactory<C8SDbContext> dbContextFactory)
    {
        #region ReadOnly Constructor Variables

        private readonly ILogger<GetCoachOrders> _logger = loggerFactory.CreateLogger<GetCoachOrders>();
        #endregion

        [Function("GetCoachOrders")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "coach")] HttpRequestData req)
        {
            HttpResponseData httpResponse;
            try
            {
                _logger.LogInformation("GetCoachOrders triggered");
            
                await using var dbContext = await dbContextFactory.CreateDbContextAsync();

                var order = new 
                {
                    Number = 1234,
                    Status = "Shipped",
                    ContactName = "Dug Steen",
                    ContactEmail = "drsteen@drsteen.com",
                    ContactPhone = "(970) 821-8128",
                    Line1 = "1601 Trailwood Dr",
                    City = "Fort Collins",
                    State = "CO",
                    ZIPCode = "80525",
                    OrderedOn = new DateTimeOffset(2025,1,1,0,0,0,TimeSpan.Zero),
                    ArriveBy = new DateOnly(2025,2,3),
                    ShippedOn = new DateTimeOffset(2025,2,1,0,0,0,TimeSpan.Zero)
                };
                httpResponse = await req.CreateSuccessResponse(order);
            }
            catch (Exception ex)
            {
                httpResponse = await req.CreateFailureResponse(ex);
            }

            return httpResponse;
        }
    }
}
