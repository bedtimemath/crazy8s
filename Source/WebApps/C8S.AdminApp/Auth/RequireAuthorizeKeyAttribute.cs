using C8S.Domain.AppConfigs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace C8S.AdminApp.Auth;

public class RequireAuthorizeKeyAttribute() : 
    TypeFilterAttribute(typeof(RequireAuthorizeKeyFilter));

public class RequireAuthorizeKeyFilter(
    IConfiguration configuration) : IAuthorizationFilter
{
    protected string AuthorizeKey
    {
        get
        {
            if (!string.IsNullOrEmpty(_authorizeKey)) return _authorizeKey;
            _authorizeKey =
                configuration.GetSection(ApiKeys.SectionName).Get<ApiKeys>()?.C8SAdmin ??
                throw new Exception($"Missing configuration {nameof(ApiKeys)}:{nameof(ApiKeys.C8SAdmin)}");
            return _authorizeKey;
        }
    }

    private static string? _authorizeKey;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Request.Headers.
            TryGetValue("X-AUTHORIZE-KEY", out StringValues authKey))
        {
            if (authKey != AuthorizeKey)
            {
                context.Result = new UnauthorizedResult();
            }
        }
        else
        {
            context.Result = new UnauthorizedResult();
        }
    }
}