using C8S.Domain.Enums;
using C8S.Domain.Features.Notes.Commands;
using C8S.Domain.Features.Notes.Models;
using C8S.Domain.Features.Notes.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace C8S.AdminApp.Client.Services.Coordinators.Notes;

public sealed class NotesListEditorCoordinator(
    ILoggerFactory loggerFactory,
    IMediator mediator)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<NotesListEditorCoordinator> _logger = loggerFactory.CreateLogger<NotesListEditorCoordinator>();
    #endregion

    #region Public Events
    public event EventHandler? ListUpdated;
    public void RaiseListUpdated() => ListUpdated?.Invoke(this, EventArgs.Empty);

    public event EventHandler? IsBusyChanged;
    public void RaiseIsBusyChanged() => IsBusyChanged?.Invoke(this, EventArgs.Empty);
    #endregion

    #region Public Properties
    public NoteReference NotesSource { get; set; } = default;
    public int SourceId { get; set; }

    public List<NoteDetails> Notes { get; private set; } = [];
    public int TotalCount { get; set; } 
    #endregion

    #region Public Properties (IsBusy)
    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (value == _isBusy) return;

            _isBusy = value; 
            RaiseIsBusyChanged();
        }
    }
    private bool _isBusy = false;
    #endregion

    #region Public Methods
    public async Task AddNote(string content)
    {
        IsBusy = true;

        try
        {
            var backendResponse = await mediator.Send(new NoteAddCommand()
            {
                Reference = NotesSource,
                ParentId = SourceId,
                Content = content
            });
            if (!backendResponse.Success) throw backendResponse.Exception!.ToException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not add note.");
            throw;
        }
        finally
        {
            IsBusy = false;
        } 
    }

    public async Task UpdateNote(NoteDetails note)
    {
        IsBusy = true;

        try
        {
            var backendResponse = await mediator.Send(new NoteUpdateCommand()
            {
                NoteId = note.NoteId,
                Content = note.Content
            });
            if (!backendResponse.Success) throw backendResponse.Exception!.ToException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating note");
            throw;
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task DeleteNote(int noteId)
    {
        IsBusy = true;

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
            _logger.LogError(ex, "Error deleting note");
            throw;
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task RefreshNotesList()
    {
        IsBusy = true;

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
        finally
        {
            IsBusy = false;
        }
    }
    #endregion
}