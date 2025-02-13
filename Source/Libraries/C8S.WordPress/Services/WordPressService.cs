using C8S.Domain.Features.Persons.Models;
using C8S.WordPress.Abstractions.Interfaces;
using Microsoft.Extensions.Logging;

namespace C8S.WordPress.Services;

public class WordPressService(
    ILoggerFactory loggerFactory): IWordPressService
{
    private readonly ILogger<WordPressService> _logger = loggerFactory.CreateLogger<WordPressService>();
    
    public async Task CreateWordPressUserFromPerson(PersonDetails person)
    {
        _logger.LogDebug("Creating from Person: {@Person}", person);
    }

}