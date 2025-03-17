using AutoMapper;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Custom;
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
    private const string SkuBaseUrl = "/wp-json/wp/v2/sku";
    private const string GetRolesUrl = "/wp-json/c8s/v1/get-roles";
    private const string CreateRoleUrl = "/wp-json/c8s/v1/add-role";

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

    #region Get
    public async Task<WrappedListResponse<WPUserDetails>> GetWordPressUsers(
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

        var items = allUsers.Select(_mapper.Map<WPUserDetails>).ToList();
        var count = allUsers.Count;
        return WrappedListResponse<WPUserDetails>.CreateSuccessResponse(items, count);
    }

    public async Task<WrappedListResponse<WPSkuDetails>> GetWordPressSkus()
    {
        var allSkus = new List<CustomSku>();
        var page = 1;
        while (true)
        {
            try
            {
                var skus = (await _wordPressClient.CustomRequest
                               .GetAsync<List<CustomSku>>(
                                   $"{SkuBaseUrl}?page={page}&per_page={DefaultPerPage}", useAuth: true));
                allSkus.AddRange(skus);

                if (skus.Count < DefaultPerPage) break;
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

        var items = allSkus.Select(_mapper.Map<WPSkuDetails>).ToList();
        var count = allSkus.Count;
        return WrappedListResponse<WPSkuDetails>.CreateSuccessResponse(items, count);
    }

    public async Task<WrappedListResponse<WPRoleDetails>> GetWordPressRoles() =>
        await _wordPressClient.CustomRequest
            .GetAsync<WrappedListResponse<WPRoleDetails>>(GetRolesUrl, useAuth: true);
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
        password ??= String.Empty.AppendRandom(12);
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
                return await CreateWordPressUser(IncrementUserName(userName),
                    name, firstName, lastName, userName, password, roleSlugs);

            _logger.LogError(ex, "Error creating WordPress user: {@UserName}", userName);
            throw;
        }
    }

    public async Task<WPSkuDetails> CreateWordPressSku(WPSkuCreate skuCreate)
    {
        var customSku = _mapper.Map<CustomSkuCreate>(skuCreate);
        var output = await _wordPressClient.CustomRequest
            .CreateAsync<CustomSkuCreate, CustomSku>(SkuBaseUrl, customSku);
        return _mapper.Map<WPSkuDetails>(output);
    }

    public async Task<WPRoleDetails> CreateWordPressRole(WPRoleDetails details)
    {
        var wrappedResponse = await _wordPressClient.CustomRequest
            .CreateAsync<WPRoleDetails, WrappedResponse<WPRoleDetails>>(CreateRoleUrl, details);
        if (!wrappedResponse.Success) 
            throw  wrappedResponse.Exception!.ToException();

        return wrappedResponse.Result!;
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

    private static string IncrementUserName(string userName)
    {
        var endingMatch = SoftCrowRegex.GetMatchForEndingDigits(userName);
        return (!endingMatch.Success) ? userName + "1" :
                endingMatch.Groups["start"].Value + (Int32.Parse(endingMatch.Groups["digits"].Value) + 1);
    }
    #endregion

}