using C8S.Domain.Enums;
using C8S.Domain.Features.Notes.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using SC.Common.Extensions;

namespace C8S.AdminApp.Client.Services.Coordinators.Notes;

public sealed class NoteAdderCoordinator(
    ILoggerFactory loggerFactory,
    IMediator mediator)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<NoteAdderCoordinator> _logger = loggerFactory.CreateLogger<NoteAdderCoordinator>();
    #endregion

    #region Public Methods
    public async Task AddNote()
    {
        try
        {
            // todo: use real note
            var backendResponse = await mediator.Send(new NoteAddCommand()
            {
                Reference = NoteReference.Request,
                ParentId = 36232,
                Content = String.Empty.AppendRandomAlphaOnly(15)
            });
            if (!backendResponse.Success) throw backendResponse.Exception!.ToException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not add note.");
            throw;
        }
    }
    #endregion

}