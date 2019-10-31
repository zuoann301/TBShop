using Ace;
using Ace.Application;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chloe.Admin.Common
{
    public static class WebUtils
    {
        public const string STokenName = "stoken";
        public static AdminSession GetCurrentSession(this HttpContext context)
        {
            throw new NotImplementedException();




            //FormsAuthenticationTicket ticket = WebHelper.GetTicketByCookieName(WebUtils.STokenName, true);
            //if (ticket == null)
            //    return null;

            //string userData = ticket.UserData;

            //try
            //{
            //    AdminSession session = JsonHelper.Deserialize<AdminSession>(userData);
            //    return session;
            //}
            //catch
            //{
            //    return null;
            //}
        }
        public static void SetSession(this HttpContext context, AdminSession session)
        {
            throw new NotImplementedException();
            //if (session != null)
            //{
            //    string encryptedTicket = WebHelper.CreateEncryptedTicket(session.UserId, DateTime.Now.AddMinutes(60 * 24), JsonHelper.Serialize(session));
            //    WebHelper.SetCookie(WebUtils.STokenName, encryptedTicket);
            //}
        }
        public static void AbandonSession()
        {
            throw new NotImplementedException();
            //HttpCookie authCookie = WebHelper.GetCookie(WebUtils.STokenName);
            //if (authCookie != null)
            //{
            //    authCookie.Expires = DateTime.Now.AddDays(-1);
            //    WebHelper.SetCookie(authCookie);
            //}
        }
    }
}