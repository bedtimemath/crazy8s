using C8S.Domain.Features.Requests.Models;
using SC.Common.Interactions;
using MediatR;

namespace C8S.Domain.Features.Requests.Queries;


public class RequestDetailsQuery : IRequest<BackendResponse<RequestDetails?>>
{
    public int RequestId { get; set; }
}