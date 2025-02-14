using C8S.Domain.Features.Persons.Models;
using Microsoft.Extensions.Logging;
using WordPressPCL;

namespace C8S.WordPress.Services;

public class WordPressService(
    ILoggerFactory loggerFactory,
    WordPressClient wordPressClient)
{
    private readonly ILogger<WordPressService> _logger = loggerFactory.CreateLogger<WordPressService>();
    private readonly WordPressClient _wordPressClient = wordPressClient;

    public async Task CreateWordPressUserFromPerson(int personId)
    {
        _logger.LogDebug("Creating from Person: {PersonId}", personId);
    }

}