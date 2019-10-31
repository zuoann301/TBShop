using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Ace.Web
{
    public static class WebHelper
    {
        static string[] BeReplacedStrs = new string[] { ".com", ".cn", ".com.cn", ".edu.cn", ".net.cn", ".org.cn", ".co.jp", ".gov.cn", ".co.uk", "ac.cn", ".edu", ".tv", ".info", ".ac", ".ag", ".am", ".at", ".be", ".biz", ".bz", ".cc", ".de", ".es", ".eu", ".fm", ".gs", ".hk", ".in", ".info", ".io", ".it", ".jp", ".la", ".md", ".ms", ".name", ".net", ".nl", ".nu", ".org", ".pl", ".ru", ".sc", ".se", ".sg", ".sh", ".tc", ".tk", ".tv", ".tw", ".us", ".co", ".uk", ".vc", ".vg", ".ws", ".il", ".li", ".nz" };

        static IHostingEnvironment _environment;
        public static IHostingEnvironment Environment
        {
            get
            {
                if (_environment == null)
                    _environment = Globals.Services.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;

                return _environment;
            }
        }
        public static string WebRootPath
        {
            get
            {
                return Environment.WebRootPath;//后面不带 /
            }
        }
        public static string ApplicationPath
        {
            get
            {
                return Environment.ContentRootPath;
            }
        }

        public static void SetCookie(this HttpContext httpContext, string name, string value, DateTime? expires = null, bool httpOnly = true, bool allowCrossAccess = false)
        {
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.HttpOnly = httpOnly;
            if (expires != null)
                cookieOptions.Expires = expires.Value;


            if (allowCrossAccess == true)
            {
                HttpRequest request = httpContext.Request;
                string cookieDomain = WebHelper.GetDomain(request.Host.Host);
                if (!string.IsNullOrEmpty(cookieDomain))
                {
                    cookieOptions.Domain = cookieDomain;
                }
            }

            httpContext.Response.Cookies.Append(name, value, cookieOptions);
        }
        public static string GetCookie(this HttpContext httpContext, string name)
        {
            string value = httpContext.Request.Cookies[name];
            return value;
        }


        public static string GetSessionId(this HttpContext httpContext)
        {
            return httpContext.Session.Id;
        }
        public static void SetSession(this HttpContext httpContext, string key, object value)
        {
            throw new NotImplementedException();
        }
        public static object GetSession(this HttpContext httpContext, string key)
        {
            throw new NotImplementedException();
        }
        public static T GetSession<T>(this HttpContext httpContext, string key)
        {
            throw new NotImplementedException();
        }
        public static void RemoveSession(this HttpContext httpContext, string key)
        {
            httpContext.Session.Remove(key);
        }

        public static string GetDomain(string url)
        {
            string host;
            Uri uri;
            try
            {
                uri = new Uri(url);

                if (uri.HostNameType != UriHostNameType.Dns)
                    return string.Empty;

                host = uri.Host + " ";
            }
            catch
            {
                return string.Empty;
            }

            foreach (string oneBeReplacedStr in BeReplacedStrs)
            {
                string BeReplacedStr = oneBeReplacedStr + " ";
                if (host.IndexOf(BeReplacedStr) != -1)
                {
                    host = host.Replace(BeReplacedStr, string.Empty);
                    break;
                }
            }

            int dotIndex = host.LastIndexOf(".");
            host = uri.Host.Substring(dotIndex + 1);
            return host;
        }

        /// <summary>
        /// 获取当前请求url中的虚拟目录部分，如设 url=http://192.168.1.105:81/WebApp/plan/TT1，如果应用程序部署的虚拟目录是 "/WebApp",则返回  http://192.168.1.105:81/WebApp ，如果应用程序部署的虚拟目录是 "/"，则返回  http://192.168.1.105:81
        /// </summary>
        /// <returns></returns>
        public static string GetRequestUrlApplicationPath(this HttpContext httpContext)
        {
            HttpRequest request = httpContext.Request;

            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>http://192.168.1.105:81</returns>
        static string GetRequestUrlAuthority()
        {
            throw new NotImplementedException();
        }
    }
}