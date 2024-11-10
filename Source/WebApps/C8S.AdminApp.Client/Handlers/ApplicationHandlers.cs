using C8S.Domain.Enums;
using C8S.Domain.Models;
using C8S.Domain.Queries.List;
using MediatR;
using SC.Common.Interfaces;
using SC.Common.Extensions;
using C8S.Domain.Queries;

namespace C8S.AdminApp.Client.Handlers;

public class ListApplicationsQueryHandler(
    ILoggerFactory loggerFactory,
    IDateTimeHelper dateTimeHelper,
    IRandomizer randomizer) : IRequestHandler<ListApplicationsQuery, ApplicationListResults>
{
    public const int TotalApplications = 500;
    
    private readonly ILogger<ListApplicationsQuery> _logger = loggerFactory.CreateLogger<ListApplicationsQuery>();
    private readonly List<ApplicationListDisplay> _applications = new();

    public Task<ApplicationListResults> Handle(ListApplicationsQuery request, CancellationToken cancellationToken)
    {
        if (!_applications.Any())
        {
            for (int index = 0; index < TotalApplications; index++)
            {
                _applications.Add(new ApplicationListDisplay()
                {
                    ApplicationId = index,
                    Status = (ApplicationStatus)randomizer.GetIntLessThan(6),
                    ApplicantEmail = String.Empty.AppendRandomAlphaOnly(),
                    ApplicantFirstName = String.Empty.AppendRandomAlphaOnly(),
                    ApplicantLastName = String.Empty.AppendRandomAlphaOnly(),
                    SubmittedOn = dateTimeHelper.Now.AddDays(randomizer.GetIntBetween(50,1000))
                });
            }
        }

        var results = new ApplicationListResults()
        {
            Items = _applications.Skip(request.StartIndex).Take(request.Count).ToList(),
            Total = TotalApplications
        };
        _logger.LogInformation("Request: {@Request}; {Count:#,##0} returned.", request, results.Items.Count);
        return Task.FromResult(results);
    }
}