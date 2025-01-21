using MediatR;
using Microsoft.Extensions.Logging;

namespace C8S.AdminApp.Client.Services.Coordinators.Notes;

public sealed class NoteEditorCoordinator(
    ILoggerFactory loggerFactory,
    IMediator mediator)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<NoteEditorCoordinator> _logger = loggerFactory.CreateLogger<NoteEditorCoordinator>();
    #endregion

    #region Public Properties
    public string? ContentHtml { get; set; }
    #endregion
}