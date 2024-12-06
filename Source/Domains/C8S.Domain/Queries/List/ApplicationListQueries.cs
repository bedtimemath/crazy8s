using System.ComponentModel;
using C8S.Domain.Enums;
using MediatR;
using SC.Common.Interactions;

namespace C8S.Domain.Queries.List;

public class ListApplicationsQuery : IRequest<BackendResponse<ApplicationListResults>>
{
    public int? StartIndex { get; set; }
    public int? Count { get; set; }
    public string? SortDescription { get; set; }
    public IList<RequestStatus>? Statuses { get; set; }
}