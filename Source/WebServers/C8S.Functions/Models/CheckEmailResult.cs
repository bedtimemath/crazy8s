using C8S.Database.Abstractions.DTOs;

namespace C8S.Functions.Models;

public class CheckEmailResult
{
    public bool Found { get; set; } = false;
    public CoachDTO? Coach { get; set; } = null;
}