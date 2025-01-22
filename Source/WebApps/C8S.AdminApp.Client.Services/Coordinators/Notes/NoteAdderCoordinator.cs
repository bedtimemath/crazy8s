using C8S.Domain.Enums;
using C8S.Domain.Features.Notes.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace C8S.AdminApp.Client.Services.Coordinators.Notes;

public sealed class NoteAdderCoordinator(
    ILoggerFactory loggerFactory,
    IMediator mediator)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<NoteAdderCoordinator> _logger = loggerFactory.CreateLogger<NoteAdderCoordinator>();
    #endregion

    #region Public Properties
    public NoteReference NotesSource { get; set; } = default;
    public int SourceId { get; set; }
    #endregion

    #region Public Properties
    public string Content { get; set; } = null!;
    #endregion

    #region Public Methods
    public async Task AddNote()
    {
        try
        {
            var backendResponse = await mediator.Send(new NoteAddCommand()
            {
                Reference = NotesSource,
                ParentId = SourceId,
                Content = Content
            });
            if (!backendResponse.Success) throw backendResponse.Exception!.ToException();

            Content = String.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not add note.");
            throw;
        }
    }
    #endregion

}