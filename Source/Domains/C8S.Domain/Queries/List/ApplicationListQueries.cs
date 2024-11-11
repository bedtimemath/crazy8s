using System.ComponentModel;
using MediatR;
using SC.Common.Interactions;

namespace C8S.Domain.Queries.List;

public class ListApplicationsQuery : IRequest<BackendResponse<ApplicationListResults>>
{
    public int? StartIndex { get; set; }
    public int? Count { get; set; }
    public string? SortDescription { get; set; }
}