using C8S.Domain.Enums;

namespace C8S.Domain.Interfaces;

public interface IKit
{
    public string Year { get; set; }
    public int Season { get; set; }
    public AgeLevel AgeLevel { get; set; }
    public string? Version { get; set; }
}