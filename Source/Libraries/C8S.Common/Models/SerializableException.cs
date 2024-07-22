using System.Text.Json.Serialization;

namespace C8S.Common.Models;

/// <summary>
/// A serializable version of the exception that can be passed around without
/// losing the original hierarchy of inner exceptions
/// </summary>
[Serializable]
public class SerializableException
{
    #region Static Variables
    public static SerializableException Generic = new SerializableException("Unknown error occurred.");
    #endregion

    #region Constants & ReadOnlys
    public const string EmptySource = SharedConstants.Display.None;
    #endregion

    #region Public Properties
    [JsonPropertyName("className")]
    public string? ClassName { get; set; } = "Generic";

    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
    
    [JsonPropertyName("stackTrace")]
    public string? StackTrace { get; set; }
    
    [JsonPropertyName("innerException")]
    public SerializableException? InnerException { get; set; }
    #endregion

    #region Constructors / Destructor
    public SerializableException() : this(String.Empty) { }

    public SerializableException(string message)
    {
        Message = message;
    }

    public SerializableException(string source, string message)
    {
        Source = source;
        Message = message;
    }

    public SerializableException(Exception? exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        ClassName = exception.GetType().Name;
        Source = exception.Source;
        Message = exception.Message;
        StackTrace = exception.StackTrace;

        if (exception.InnerException != null)
            InnerException = new SerializableException(exception.InnerException);
    }
    #endregion

    #region Public Methods
    public Exception ToException() => new(this.Message, InnerException?.ToException()) { Source = this.Source };
    #endregion
}