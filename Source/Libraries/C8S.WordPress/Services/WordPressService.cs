using AutoMapper;
using C8S.WordPress.Abstractions.Models;
using Microsoft.Extensions.Logging;
using WordPressPCL;
using WordPressPCL.Models;
using WordPressPCL.Utility;

namespace C8S.WordPress.Services;

public class WordPressService
{
    private readonly ILogger<WordPressService> _logger;
    private readonly IMapper _mapper;
    private readonly WordPressClient _wordPressClient;

    public WordPressService(
        ILoggerFactory loggerFactory,
        IMapper mapper,
        string endpoint, string username, string password)
    {
        _logger = loggerFactory.CreateLogger<WordPressService>();
        _mapper = mapper;
        
        _wordPressClient = new WordPressClient(endpoint);
        _wordPressClient.Auth.UseBasicAuth(username, password);
    }

    #region Get

    public async Task<WPUsersListResults> GetWordPressUsers(
        IEnumerable<string>? includeRoles = null)
    {
        includeRoles ??= [];
        var queryBuilder = new UsersQueryBuilder()
        {
            Context = Context.Edit,
            Roles = includeRoles.ToList()

        };
        var wpUsers = await _wordPressClient.Users
            .QueryAsync(queryBuilder, useAuth: true);
        return new WPUsersListResults()
        {
            Items = wpUsers.Select(_mapper.Map<WPUserDetails>).ToList(),
            Total = wpUsers.Count
        };
    }
    #endregion

    #region Create
    public async Task<WPUserDetails> CreateWordPressUser(
    string userName, string name, string email, string password,
    string? firstName = null, string? lastName = null,
    IList<string>? roles = null)
    {
        roles ??= [];
        if (!roles.Contains("subscriber")) roles.Add("subscriber");
        if (!roles.Contains("coach")) roles.Add("coach");

        var wpUser = await _wordPressClient.Users
            .CreateAsync(new User()
            {
                UserName = userName,
                Name = name,
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Roles = roles
            });

        return _mapper.Map<WPUserDetails>(wpUser);
    } 
    #endregion

}