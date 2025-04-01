using System.Diagnostics;
using System.Net;
using AutoMapper;
using C8S.WordPress.Abstractions.MagicLogin;
using C8S.WordPress.Abstractions.Models;
using Microsoft.Extensions.Logging;
using SC.Common;
using SC.Common.Extensions;
using SC.Common.Responses;
using WordPressPCL;
using WordPressPCL.Models;
using WordPressPCL.Models.Exceptions;
using WordPressPCL.Utility;

namespace C8S.WordPress.Services;

public class WordPressService
{
    private const int DefaultPerPage = 100;
    private const string KitBaseUrl = "/wp-json/wp/v2/kit-page";
    private const string GetRolesUrl = "/wp-json/c8s/v1/get-roles";
    private const string CreateRoleUrl = "/wp-json/c8s/v1/add-role";
    private const string MagicLoginUrl = "/wp-json/magic-login/v1/token";

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

        //var httpHandler = new HttpClientHandler() { Proxy = new WebProxy(new Uri("http://localhost:8866")) };
        //var httpClient = new HttpClient(httpHandler) { BaseAddress = new Uri(endpoint) };
        //_wordPressClient = new WordPressClient(httpClient);
        _wordPressClient = new WordPressClient(endpoint);
        _wordPressClient.Auth.UseBasicAuth(username, password);
    }

    #region GET LIST
    public async Task<List<WPUserDetails>> GetWordPressUsers(
        IEnumerable<string>? includeRoles = null)
    {
        var queryBuilder = new UsersQueryBuilder()
        {
            Context = Context.Edit,
            PerPage = DefaultPerPage
        };

        // we do this here so that we won't re-enumerate the roles within the while loop
        if (includeRoles != null) queryBuilder.Roles = includeRoles.ToList();

        var allUsers = new List<User>();
        var page = 1;
        while (true)
        {
            try
            {
                queryBuilder.Page = page;
                var users = await _wordPressClient.Users.QueryAsync(queryBuilder, useAuth: true);
                allUsers.AddRange(users);

                if (users.Count < DefaultPerPage) break;
                page++;
            }
            // catches the edge case when the # of items exactly matches a multiple of per_page
            //  and the code takes you one page too far
            // see: https://github.com/wp-net/WordPressPCL/issues/375
            catch (WPException)
            {
                break;
            }
        }

        return allUsers.Select(_mapper.Map<WPUserDetails>).ToList();
    }

    public async Task<List<WPKitPageDetails>> GetWordPressKitPages()
    {
        var allKitPages = new List<WPKitPageDetails>();
        var page = 1;
        while (true)
        {
            try
            {
                var kitPages = (await _wordPressClient.CustomRequest
                               .GetAsync<List<WPKitPageDetails>>(
                                   $"{KitBaseUrl}?page={page}&per_page={DefaultPerPage}", useAuth: true));
                allKitPages.AddRange(kitPages);

                if (kitPages.Count < DefaultPerPage) break;
                page++;
            }
            // catches the edge case when the # of items exactly matches a multiple of per_page
            //  and the code takes you one page too far
            // see: https://github.com/wp-net/WordPressPCL/issues/375
            catch (WPException)
            {
                break;
            }
        }

        return allKitPages.Select(_mapper.Map<WPKitPageDetails>).ToList();
    }

    public async Task<List<WPRoleDetails>> GetWordPressRoles()
    {
        var rolesResponse = await _wordPressClient.CustomRequest
            .GetAsync<WrappedListResponse<WPRoleDetails>>(GetRolesUrl, useAuth: true);
        if (rolesResponse is not { Success: true } or { Result: null })
            throw rolesResponse.Exception?.ToException() ?? new UnreachableException("Missing exception");
        
        return rolesResponse.Result.ToList();
    }
    #endregion

    #region GET
    public async Task<WPUserDetails?> GetWordPressUserById(int id)
    {
        var allUsers = await GetWordPressUsers();
        return allUsers.FirstOrDefault(u => u.Id == id);
    }
    #endregion

    #region Create
    public async Task<WPUserDetails> CreateWordPressUser(
        string email,
        string? name = null,
        string? firstName = null,
        string? lastName = null,
        string? userName = null,
        string? password = null,
        IList<string>? roleSlugs = null)
    {
        name ??= GenerateName(firstName, lastName);
        userName ??= GenerateUserName(email, name);
        password ??= userName.Reverse() + "_C8s!";
        try
        {
            var user = new User()
            {
                Email = email,
                Name = name,
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Password = password,
                Roles = roleSlugs ?? []
            };
            var output = await _wordPressClient.Users.CreateAsync(user);
            return _mapper.Map<WPUserDetails>(output);
        }
        catch (Exception ex)
        {
            // Would be nice if the WordPressPCL library threw a more specific exception
            if (ex.Message == "Sorry, that username already exists!")
                return await CreateWordPressUser(userName.IncrementFinalDigits(),
                    name, firstName, lastName, userName, password, roleSlugs);

            _logger.LogError(ex, "Error creating WordPress user: {@UserName}", userName);
            throw;
        }
    }

    public async Task<WPKitPageDetails> CreateWordPressKitPage(WPKitPageCreate create)
    {
        try
        {
            return await _wordPressClient.CustomRequest
                .CreateAsync<WPKitPageCreate, WPKitPageDetails>(KitBaseUrl, create);
        }
        catch (WPException ex)
        {
            _logger.LogError(ex, "Error creating WordPress kit page: {@Create}", create);
            throw;
        }
    }

    public async Task<WPKitPageDetails> UpdateWordPressKitPage(int pageId,
        WPKitPageProperties properties) => 
        await _wordPressClient.CustomRequest
            .UpdateAsync<WPKitPageDetails, WPKitPageDetails>(KitBaseUrl + $"/{pageId}", 
                new WPKitPageDetails() { Properties = properties });

    public async Task<WPRoleDetails> CreateWordPressRole(WPRoleDetails details)
    {
        var wrappedResponse = await _wordPressClient.CustomRequest
            .CreateAsync<WPRoleDetails, WrappedResponse<WPRoleDetails>>(CreateRoleUrl, details);
        if (!wrappedResponse.Success)
            throw wrappedResponse.Exception!.ToException();

        return wrappedResponse.Result!;
    }

    public async Task<string> CreateMagicLinkForUser(int id,
        string? redirectUrl = null)
    {
        var user = await GetWordPressUserById(id) ?? throw new Exception("User not found");
        return await CreateMagicLinkForUser(user.Email);
    }

    public async Task<string> CreateMagicLinkForUser(string email)
    {
        var response = await _wordPressClient.CustomRequest
            .CreateAsync<WPMagicLoginRequest, WPMagicLoginResponse>(
                MagicLoginUrl, new WPMagicLoginRequest() { user = email });

        if (!String.IsNullOrEmpty(response.code))
            throw new Exception($"{response.code}: {response.message}");
        if (response == null || String.IsNullOrEmpty(response.link))
            throw new UnreachableException("Unknown error requesting magic link");

        return response.link;
    }
    #endregion

    #region Update
    public async Task<WPUserDetails> UpdateWordPressUserRoles(int id, IEnumerable<string> roles)
    {
        var users = await GetWordPressUsers();
        var foundUser = users.FirstOrDefault(u => u.Id == id) ??
                        throw new Exception($"Could not find user for ID#: {id}");
        var updatedUser = foundUser with { RoleSlugs = roles.ToList() };

        var output = await _wordPressClient.Users.UpdateAsync(_mapper.Map<User>(updatedUser));
        return _mapper.Map<WPUserDetails>(output);
    }
    #endregion

    #region Delete
    public async Task DeleteWordPressUser(int id)
    {
        var users = (await GetWordPressUsers()).OrderBy(u => u.Id).ToList();
        var userToDelete = users.FirstOrDefault(u => u.Id == id) ??
                           throw new Exception($"Could not find user with ID#: {id}");
        var admin = users.FirstOrDefault(u => u.RoleSlugs.Contains("administrator")) ??
                    throw new Exception("Could not find administrator");

        var result = await _wordPressClient.Users.Delete(userToDelete.Id, admin.Id);
        if (!result) throw new Exception("Failed to delete user");
    }
    #endregion

    #region Private Methods
    private static string GenerateUserName(string email, string? name = null) =>
        String.IsNullOrWhiteSpace(name) ? email.ToFirstWord('@')!.RemoveNonAlphanumeric() :
            String.Join("", name.Split(' ').Select(s => s.RemoveNonAlphanumeric()));

    private static string GenerateName(string? firstName, string? lastName) =>
        !String.IsNullOrWhiteSpace(firstName) && !String.IsNullOrWhiteSpace(lastName)
            ? $"{firstName} {lastName}"
            : firstName ?? lastName ?? SoftCrowConstants.Display.AnonymousWord;
    #endregion

}