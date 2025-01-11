using C8S.Domain.Features.Requests.Enums;
using SC.Common.Interactions;
using MediatR;

namespace C8S.Domain.Features.Requests.Queries;


public class ListRequestsQuery : IRequest<BackendResponse<RequestListResults>>
{
    public int? StartIndex { get; set; }
    public int? Count { get; set; }
    public string? SortDescription { get; set; }
    public IList<RequestStatus>? Statuses { get; set; }
}