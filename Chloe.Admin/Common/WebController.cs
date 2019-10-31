using Ace.Application;
using Ace.Attributes;
using Ace.Exceptions;
using Ace.Web.Mvc;
using Chloe.Admin.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ace;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Chloe.Admin.Common
{
    public abstract class WebController<TService> : WebController
    {
        TService _service;
        protected TService Service
        {
            get
            {
                if (this._service == null)
                    this._service = this.CreateService<TService>();

                return this._service;
            }
        }
    }

    public abstract class WebController : BaseController
    {
        [NonAction]
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            

            ObsoleteApiAttribute obsoleteAttr = filterContext.ActionDescriptor.FilterDescriptors.Where(a => a.Filter is ObsoleteApiAttribute).Select(a => a.Filter).FirstOrDefault() as ObsoleteApiAttribute;

            if (obsoleteAttr == null)
            {
                obsoleteAttr = filterContext.Controller.GetType().GetCustomAttributes<ObsoleteApiAttribute>().FirstOrDefault() as ObsoleteApiAttribute;
            }

            if (obsoleteAttr != null)
            {
                filterContext.Result = this.FailedMsg(obsoleteAttr.Message);
            }

            base.OnActionExecuting(filterContext);
        }

        AdminSession _session;
        public AdminSession CurrentSession
        {
            get
            {
                if (this._session != null)
                    return this._session;

                if (this.HttpContext.User.Identity.IsAuthenticated == false)
                    return null;

                AdminSession session = AdminSession.Parse(this.HttpContext.User);
                this._session = session;
                return session;
            }
            set
            {
                AdminSession session = value;

                if (session == null)
                {
                    //注销登录
                    this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return;
                }

                List<Claim> claims = session.ToClaims();

                //init the identity instances 
                var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(60),
                    IsPersistent = false,
                    AllowRefresh = false
                });

                //IAuthenticationService authenticationService = this.HttpContext.RequestServices.GetService(typeof(IAuthenticationService)) as IAuthenticationService;
                //authenticationService.SignInAsync(this.HttpContext, CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                //{
                //    ExpiresUtc = DateTime.UtcNow.AddMinutes(60),
                //    IsPersistent = false,
                //    AllowRefresh = false
                //});

                this._session = session;
            }
        }
    }
}