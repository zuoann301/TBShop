using Ace;
using Ace.VerifyCode;
using Chloe.Admin.Common;
using Ace.Application;
using Ace.Application.Common;
using Ace.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Ace.Web;
using Ace.Application.System;
using Microsoft.AspNetCore.Http;
using Ace.Entity.System;
using Ace.Web.Mvc.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace Chloe.Admin.Controllers
{
    public class AccountController : WebController
    {
        const string VerifyCodeKey = "session_verifycode";

        // GET: Account
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string loginName, string password/*经过md5加密后的密码*/, string verifyCode)
        {
            //if (verifyCode.IsNullOrEmpty())
            //    return this.FailedMsg("请输入验证码");
            if (loginName.IsNullOrEmpty() || password.IsNullOrEmpty())
                return this.FailedMsg("用户名/密码不能为空");

            //string code = WebHelper.GetSession<string>(VerifyCodeKey);
            //WebHelper.RemoveSession(VerifyCodeKey);
            //if (code.IsNullOrEmpty() || code.ToLower() != verifyCode.ToLower())
            //{
            //    return this.FailedMsg("验证码错误，请重新输入");
            //}

            loginName = loginName.Trim();
            var accountAppService = this.CreateService<IAccountService>();

            const string moduleName = "系统登录";
            string ip = this.HttpContext.GetClientIP();

            SysUser user;
            string msg;
            if (!accountAppService.CheckLogin(loginName, password, out user, out msg))
            {
                this.CreateService<ISysLogAppService>().LogAsync(null, null, ip, LogType.Login, moduleName, false, "用户[{0}]登录失败：{1}".ToFormat(loginName, msg));
                return this.FailedMsg(msg);
            }

            AdminSession session = new AdminSession();
            session.UserId = user.Id;
            session.AccountName = user.AccountName;
            session.Name = user.Name;
            session.LoginIP = ip;
            session.IsAdmin = user.AccountName.ToLower() == AppConsts.AdminUserName;

            this.CurrentSession = session;

            this.CreateService<ISysLogAppService>().LogAsync(user.Id, user.Name, ip, LogType.Login, moduleName, true, "登录成功");
            return this.SuccessMsg(msg);
        }
        public ActionResult Logout([FromServices]IMemoryCache memoryCache)
        {
            //http://www.cnblogs.com/sheng-jie/p/6970091.html
            //https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/src/Abp/Events/Bus/EventBus.cs
            memoryCache.Remove(WebPermissionFilter.USER_PERMITS_CACHE_KEY + this.CurrentSession.UserId);
            this.CurrentSession = null;
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult VerifyCode()
        {
            throw new NotImplementedException();
            //string verifyCode = VerifyCodeGenerator.GenVerifyCode();
            //byte[] bytes = VerifyCodeHelper.GetVerifyCodeByteArray(verifyCode);

            //WebHelper.SetSession(VerifyCodeKey, verifyCode);

            //return this.File(bytes, @"image/Gif");
        }


        [LoginAttribute]
        [HttpGet]
        public ActionResult Index()
        {
            var service = this.CreateService<IEntityAppService>();
            SysUser user = service.GetByKey<SysUser>(this.CurrentSession.UserId);
            //Sys_Role role = string.IsNullOrEmpty(user.RoleId) ? null : service.GetByKey<Sys_Role>(user.RoleId);

            UserModel model = new UserModel();
            model.User = user;
            //model.RoleName = role == null ? null : role.Name;

            this.ViewBag.UserInfo = model;

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldPassword">明文</param>
        /// <param name="newPassword">明文</param>
        /// <returns></returns>
        [Permission("account.change_password")]
        [LoginAttribute]
        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword)
        {
            this.CreateService<IAccountService>().ChangePassword(this.CurrentSession.UserId, oldPassword, newPassword);
            return this.SuccessMsg("密码修改成功");
        }

        [Permission("account.modify_info")]
        [LoginAttribute]
        [HttpPost]
        public ActionResult ModifyInfo(ModifyAccountInfoInput input)
        {
            this.CreateService<IAccountService>().ModifyInfo(input);
            return this.SuccessMsg("修改成功");
        }
    }

    public class UserModel
    {
        public SysUser User { get; set; }
    }
}