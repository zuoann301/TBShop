using Ace.Application.System;
using Ace.Web.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Chloe.Admin.Common
{
    public class WebPermissionFilter : PermissionFilter
    {
        public const string USER_PERMITS_CACHE_KEY = "_UPERMITS_";

        IMemoryCache _cache;

        public WebPermissionFilter(IMemoryCache cache)
        {
            this._cache = cache;
        }

        protected override bool HasExecutePermission(AuthorizationFilterContext filterContext, List<string> permissionCodes)
        {
            ClaimsPrincipal user = filterContext.HttpContext.User;
            string accountName = user.Claims.FirstOrDefault(x => x.Type == "AccountName").Value;
            if (accountName == Ace.Entity.System.SysUser.AdminAccountName)
                return true;

            List<string> usePermits = null;

            string userId = user.Claims.FirstOrDefault(x => x.Type == "UserId").Value;
            string cacheKey = USER_PERMITS_CACHE_KEY + userId;
            usePermits = this._cache.Get<List<string>>(cacheKey);

            if (usePermits == null)
            {
                IUserService userService = filterContext.HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;
                usePermits = userService.GetUserPermits(userId);
                _cache.Set(cacheKey, usePermits, TimeSpan.FromMinutes(2));//缓存2分钟，2分钟后重新加载
            }

            foreach (string permit in permissionCodes)
            {
                if (!usePermits.Any(a => a == permit))
                    return false;
            }

            return true;
        }
    }
}
