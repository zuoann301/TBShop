using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;
using System.Security.Claims;

namespace Ace.Web.Mvc.Authorization
{
    //https://www.cnblogs.com/freeliver54/p/6247810.html
    public class PermissionFilter : IAuthorizationFilter
    {
        static IEnumerable<TAttribute> GetAttributes<TAttribute>(MemberInfo memberInfo)
        {
            return memberInfo.GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>();
        }
        public virtual void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext.Result != null)
                return;

            if (this.SkipAuthorize(filterContext.ActionDescriptor))
            {
                return;
            }

            //ControllerActionDescriptor controllerActionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;

            //if (controllerActionDescriptor == null)
            //    return;

            //List<string> permits = GetAttributes<PermissionAttribute>(controllerActionDescriptor.MethodInfo).Concat(GetAttributes<PermissionAttribute>(controllerActionDescriptor.ControllerTypeInfo)).Where(a => a.Permit.IsNotNullOrEmpty()).Select(a => a.Permit).ToList();

            List<string> permissionCodes = filterContext.ActionDescriptor.FilterDescriptors.Where(a => a.Filter is PermissionAttribute).Select(a => a.Filter as PermissionAttribute).Select(a => a.Code).ToList();

            if (permissionCodes.Count == 0)
            {
                return;
            }

            ResultStatus status = ResultStatus.OK;
            string msg = null;
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //说明处于登录状态，则开始验证当前登录用户是否拥有访问权限
                if (this.HasExecutePermission(filterContext, permissionCodes))
                {
                    return;
                }
                status = ResultStatus.Unauthorized;
                msg = "抱歉，您无当前操作权限";
            }
            else
            {
                status = ResultStatus.NotLogin;
                msg = "未登录或登录超时，请重新登录";
            }

            HttpRequest httpRequest = filterContext.HttpContext.Request;
            if (status == ResultStatus.Unauthorized || httpRequest.IsAjaxRequest())
            {
                Result result = Result.CreateResult(status, msg);
                string json = JsonHelper.Serialize(result);
                ContentResult contentResult = new ContentResult() { Content = json };
                filterContext.Result = contentResult;
                return;
            }
            else
            {
                string url = filterContext.HttpContext.Content("~/Account/Login");
                url = string.Concat(url, "?returnUrl=", httpRequest.Path);

                RedirectResult redirectResult = new RedirectResult(url);
                filterContext.Result = redirectResult;
                return;
            }
        }

        protected virtual bool SkipAuthorize(ActionDescriptor actionDescriptor)
        {
            bool skipAuthorize = actionDescriptor.FilterDescriptors.Where(a => a.Filter is SkipPermissionAttribute || a.Filter is AllowNotLoginAttribute).Any();
            if (skipAuthorize)
            {
                return true;
            }

            return false;
        }
        protected virtual bool HasExecutePermission(AuthorizationFilterContext filterContext, List<string> permissionCodes)
        {
            return true;
        }
    }
}
