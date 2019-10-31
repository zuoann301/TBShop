using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Ace.Exceptions;

namespace Ace.Web.Mvc
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _env;

        public HttpGlobalExceptionFilter(IHostingEnvironment env)
        {
            this._env = env;
        }

        public ContentResult FailedMsg(string msg = null)
        {
            Result retResult = new Result(ResultStatus.Failed, msg);
            string json = JsonHelper.Serialize(retResult);
            return new ContentResult() { Content = json };
        }
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            //执行过程出现未处理异常
            Exception ex = filterContext.Exception;

#if DEBUG
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                string msg = null;

                if (ex is InvalidInputException)
                {
                    msg = ex.Message;
                    filterContext.Result = this.FailedMsg(msg);
                    filterContext.ExceptionHandled = true;
                    return;
                }
            }

            this.LogException(filterContext);
            return;
#endif

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                string msg = null;

                if (ex is InvalidInputException)
                {
                    msg = ex.Message;
                }
                else
                {
                    this.LogException(filterContext);
                    msg = "服务器错误";
                }

                filterContext.Result = this.FailedMsg(msg);
                filterContext.ExceptionHandled = true;
                return;
            }
            else
            {
                //对于非 ajax 请求

                this.LogException(filterContext);
                return;
            }
        }

        /// <summary>
        ///  将错误记录进日志
        /// </summary>
        /// <param name="filterContext"></param>
        void LogException(ExceptionContext filterContext)
        {
            ILoggerFactory loggerFactory = filterContext.HttpContext.RequestServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            ILogger logger = loggerFactory.CreateLogger(filterContext.ActionDescriptor.DisplayName);

            logger.LogError("Error: {0}, {1}", ReplaceParticular(filterContext.Exception.Message), ReplaceParticular(filterContext.Exception.StackTrace));
        }

        static string ReplaceParticular(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            return s.Replace("\r", "#R#").Replace("\n", "#N#").Replace("|", "#VERTICAL#");
        }
    }
}
