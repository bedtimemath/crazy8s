using C8S.Domain.Enums;
using C8S.Domain.Features.Notes.Commands;
using C8S.Domain.Features.Notes.Models;
using C8S.Domain.Features.Notes.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace C8S.AdminApp.Client.Services.Coordinators.Notes;

public sealed class NotesListCoordinator(
    ILoggerFactory loggerFactory,
    IMediator mediator)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<NotesListCoordinator> _logger = loggerFactory.CreateLogger<NotesListCoordinator>();
    #endregion

    #region Public Events
    public event EventHandler? ListUpdated;
    public void RaiseListUpdated() => ListUpdated?.Invoke(this, EventArgs.Empty);
    #endregion

    #region Public Properties
    public NoteReference NotesSource { get; set; } = default;
    public int SourceId { get; set; }

    public List<NoteDetails> Notes { get; private set; } = [];
    public int TotalCount { get; set; } 
    #endregion

    #region Public Methods
    public async Task DeleteNote(int noteId)
    {
        try
        {
            var backendResponse = await mediator.Send(new NoteDeleteCommand()
            {
                NoteId = noteId
            });
            if (!backendResponse.Success) throw backendResponse.Exception!.ToException();

            // todo: do I need this if the notification is coming from the backend later?
            RaiseListUpdated();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting notes list");
            throw;
        }
    }

    public async Task Refresh()
    {
        try
        {
            var backendResponse = await mediator.Send(new NotesListQuery()
            {
                NotesSource = NotesSource,
                SourceId = SourceId
            });
            if (!backendResponse.Success) throw backendResponse.Exception!.ToException();

            var results = backendResponse.Result!;
            Notes = results.Items;
            TotalCount = results.Total;

            // todo: do I need this if the notification is coming from the backend later?
            RaiseListUpdated();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting notes list");
            throw;
        }
    }
    #endregion
}