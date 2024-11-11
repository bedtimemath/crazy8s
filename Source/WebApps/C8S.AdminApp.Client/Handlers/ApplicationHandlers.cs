#if false
using C8S.AdminApp.Client.Services;
using C8S.Domain.Queries.List;
using MediatR;
using C8S.Domain.Queries;

namespace C8S.AdminApp.Client.Handlers;

public class ListApplicationsQueryHandler(
    ILoggerFactory loggerFactory,
    ICallbackService callbackService) : IRequestHandler<ListApplicationsQuery, ApplicationListResults>
{
    private readonly ILogger<ListApplicationsQuery> _logger = loggerFactory.CreateLogger<ListApplicationsQuery>();

    public Task<ApplicationListResults> Handle(
        ListApplicationsQuery request, CancellationToken cancellationToken)
    {
        return callbackService.ListApplications(request, cancellationToken);
    }
} 
#endif