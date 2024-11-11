using MediatR;

namespace C8S.Domain.Queries.List;

public class ListApplicationsQuery : IRequest<ApplicationListResults>
{
    public int? StartIndex { get; set; }
    public int? Count { get; set; }
}