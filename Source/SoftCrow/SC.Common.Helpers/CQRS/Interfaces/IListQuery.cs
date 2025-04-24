namespace SC.Common.Helpers.CQRS.Interfaces;

public interface IListQuery
{
    int? StartIndex { get; }
    int? Count { get; }
    string? Query { get; }
    string? SortDescription { get; }
}