namespace C8S.Functions.Models;

public class CoachAppRequest<TData>
{
    public TData? Data { get; set; } = default(TData);
    public string Code { get; set; } = default!;
}