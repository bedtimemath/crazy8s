using C8S.Domain.Features.Requests.Enums;
using SC.Common.Interactions;
using MediatR;
using C8S.Domain.Features.Requests.Models;

namespace C8S.Domain.Features.Requests.Queries;


public class RequestsListQuery : IRequest<BackendResponse<RequestListResults>>
{
    public int? StartIndex { get; set; }
    public int? Count { get; set; }
    public string? Query { get; set; }
    public string? SortDescription { get; set; }
    public DateOnly? SinceWhen { get; set; }
    public IList<RequestStatus>? Statuses { get; set; }
}