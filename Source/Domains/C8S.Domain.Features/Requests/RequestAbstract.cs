﻿using System.Text.Json.Serialization;

namespace C8S.Domain.Features.Requests;

public record RequestAbstract : RequestBase
{
    public string? PersonFirstName { get; init; }
    public DateTimeOffset SubmittedOn { get; init; }
    public string? OrganizationName { get; init; }
    public string? OrganizationCity { get; init; }
    public string? OrganizationState { get; init; }
        
    [JsonIgnore] 
    public string PersonFullName => 
        String.Join(" ", (new List<string?> {PersonFirstName, PersonLastName})
            .Where(s => !String.IsNullOrEmpty(s)));

    [JsonIgnore] 
    public string OrganizationCityState => 
        String.Join(", ", (new List<string?> {OrganizationCity, OrganizationState})
            .Where(s => !String.IsNullOrEmpty(s)));
}