using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ace;
using Ace.Application.Common;
using Ace.Application.Wiki;
using Ace.Entity.Wiki;
using Ace.IdStrategy;
using Ace.Web.Mvc;
using Ace.Web.Mvc.Authorization;
using Ace.Web.Mvc.Models;
using Chloe.Admin.Common;
using Chloe.Admin.Common.Tree;
using Microsoft.AspNetCore.Mvc;

namespace Chloe.Admin.Areas.Wiki.Controllers
{

    [Area(AreaNames.Wiki)]
    [Permission("wiki.users")]
    public class UsersController : WebController<IUsersService>
    {
        public IActionResult Index()
        {
            IShopService SortService = this.CreateService<IShopService>();
            List<SimpleShop2> ListShop = SortService.GetCacheList2();
            ViewBag.ListShop = ListShop;

            IUsers_RoleService RoleService = this.CreateService<IUsers_RoleService>();
            List<Users_Role> ListRole = RoleService.GetList("");
            ViewBag.ListRole = ListRole;

            return View();
        }

        [HttpGet]
        public ActionResult GetModels(Pagination pagination,int ShopID,int RoleID, string keyword)
        {
            PagedData<Users> pagedData = this.Service.GetPageData(pagination, ShopID, RoleID, keyword);
            return this.SuccessData(pagedData);
        }


        [Permission("wiki.users.delete")]
        [HttpPost]
        public ActionResult Delete(string id)
        { 
            this.Service.Delete(id);
            return this.SuccessMsg("删除成功");
        }

        [HttpGet]
        public ActionResult Add()
        {
             

            return View();
        }


        [HttpGet]
        public ActionResult Edit(string Id="")
        {
            if (string.IsNullOrEmpty(Id))
            {
                return Redirect("Index");
            }
            else
            {
                Users modle = this.Service.GetModel(Id);
                if (modle == null)
                {
                    return RedirectToAction( "Index");                     
                }                 
                ViewBag.UsersModle = modle;
            }
            return View();
        }


        [Permission("wiki.users.edit")]
        [HttpPost]
        public ActionResult Edit(UpdateUsersInput modle)
        {            
            this.Service.Update(modle);             
            return  RedirectToAction("Index");
        }

        [Permission("wiki.users.add")]
        [HttpPost]
        public ActionResult Add(AddUsersInput modle)
        {
            modle.CreateDate = DateTime.Now;
            modle.LastLoginDate = DateTime.Now;
            this.Service.Add(modle);                          
            return RedirectToAction("Index");
        }


        
        [HttpPost]
        public ActionResult RevisePassword(string userId, string newPassword)
        {
            if (userId.IsNullOrEmpty())
                return this.FailedMsg("userId 不能为空");

            this.Service.RevisePassword(userId, newPassword);
            return this.SuccessMsg("重置密码成功");
        }


    }
}