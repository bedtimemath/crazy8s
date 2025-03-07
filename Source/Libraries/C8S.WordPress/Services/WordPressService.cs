﻿using System.Text;
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
        
        var items = allUsers.Select(_mapper.Map<WPUserDetails>).ToList();
        var count = allUsers.Count;
        return WrappedListResponse<WPUserDetails>.CreateSuccessResponse(items, count);
    }

    public async Task<WrappedListResponse<WPSkuDetails>> GetWordPressSkus()
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

        var items = allSkus.Select(_mapper.Map<WPSkuDetails>).ToList();
        var count = allSkus.Count;
        return WrappedListResponse<WPSkuDetails>.CreateSuccessResponse(items, count);
    }
    #endregion

    #region Create
    public async Task<WPUserDetails> CreateWordPressUser(WPUserDetails userDetails,
        string? userName = null)
    {
        userName ??= GenerateUserName(userDetails);
        try
        {
            var user = _mapper.Map<User>(userDetails);
            user.UserName = userName;
            user.Password = String.Empty.AppendRandomAlphaOnly(12);
            var output = await _wordPressClient.Users.CreateAsync(user);
            return _mapper.Map<WPUserDetails>(output);
        }
        catch (Exception ex)
        {
            // Would be nice if the WordPressPCL library threw a more specific exception
            if (ex.Message == "Sorry, that username already exists!")
                return await CreateWordPressUser(userDetails, IncrementUserName(userName));

            _logger.LogError(ex, "Error creating WordPress user: {@UserDetails}", userDetails);
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
    #endregion

    #region Private Methods

    private static string GenerateUserName(WPUserDetails userDetails)
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(userDetails.FirstName))
            parts.Add(userDetails.FirstName.RemoveNonAlphanumeric());
        if (!string.IsNullOrWhiteSpace(userDetails.LastName))
            parts.Add(userDetails.LastName.RemoveNonAlphanumeric());
        if (parts.Count == 0)
            parts.Add(userDetails.Email.RemoveNonAlphanumeric());

        return String.Join('_', parts);
    }
    private static string IncrementUserName(string userName)
    {
        var endingMatch = SoftCrowRegex.GetMatchForEndingDigits(userName);
        return (!endingMatch.Success) ? userName + "_1" :
                endingMatch.Groups["start"].Value + (Int32.Parse(endingMatch.Groups["digits"].Value) + 1);
    }
    #endregion

}