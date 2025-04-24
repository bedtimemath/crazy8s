namespace SC.Common.PubSub;

public record DataChange
{
    public DataChangeAction Action { get; init; }
    public string EntityName { get; init; } = null!;
    public int? EntityId { get; init; }
    public string? JsonDetails { get; init; }
}