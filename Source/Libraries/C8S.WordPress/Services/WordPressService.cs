using AutoMapper;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Custom;
using Microsoft.Extensions.Logging;
using WordPressPCL;
using WordPressPCL.Models;
using WordPressPCL.Models.Exceptions;
using WordPressPCL.Utility;

namespace C8S.WordPress.Services;

public class WordPressService
{
    private const int DefaultPerPage = 100;
    private const string SkuBaseUrl = "/wp-json/wp/v2/sku";

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
        var queryBuilder = new UsersQueryBuilder()
        {
            Context = Context.Edit,
            PerPage = DefaultPerPage
        };

        // we do this here so that we won't re-enumerate the roles within the while loop
        if (includeRoles != null) queryBuilder.Roles = includeRoles.ToList();
        
        var allUsers = new List<User>();
        var page = 1;
        while (true) {
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

        return new WPUsersListResults()
        {
            Items = allUsers.Select(_mapper.Map<WPUserDetails>).ToList(),
            Total = allUsers.Count
        };
    }

    public async Task<WPSkusListResults> GetWordPressSkus()
    {
        var allSkus = new List<CustomSku>();
        var page = 1;
        while (true) {
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

        return new WPSkusListResults()
        {
            Items = allSkus.Select(_mapper.Map<WPSkuDetails>).ToList(),
            Total = allSkus.Count
        };
    }
    #endregion

    #region Create
    public async Task<WPUserDetails> CreateWordPressUser(WPUserDetails userDetails)
    {
        if (!userDetails.Roles.Contains("subscriber")) userDetails.Roles.Add("subscriber");
        if (!userDetails.Roles.Contains("coach")) userDetails.Roles.Add("coach");
        
        var user = _mapper.Map<User>(userDetails);
        var output = await _wordPressClient.Users.CreateAsync(user);
        return _mapper.Map<WPUserDetails>(output);
    }

    public async Task<WPSkuDetails> CreateWordPressSku(WPSkuCreate skuCreate)
    {
        var customSku = _mapper.Map<CustomSkuCreate>(skuCreate);
        var output = await _wordPressClient.CustomRequest
            .CreateAsync<CustomSkuCreate, CustomSku>(SkuBaseUrl, customSku);
        return _mapper.Map<WPSkuDetails>(output);
    }
    #endregion

}