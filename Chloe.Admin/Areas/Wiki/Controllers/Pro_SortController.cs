using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ace;
using Ace.Application.Wiki;
using Ace.Entity.Wiki;
using Ace.Web.Mvc;
using Ace.Web.Mvc.Authorization;
using Ace.Web.Mvc.Models;
using Chloe.Admin.Common;
using Chloe.Admin.Common.Tree;
using Microsoft.AspNetCore.Mvc;

namespace Chloe.Admin.Areas.Wiki.Controllers
{
    [Area(AreaNames.Wiki)]
    [Permission("wiki.pro_sort")]
    public class Pro_SortController : WebController<IPro_SortService>
    {
        public ActionResult Index(string Pid="0")
        {
            //List<SysOrg> orgs = this.CreateService<IOrgService>().GetList();
            //this.ViewBag.Orgs = orgs;

            ViewBag.SortList = this.Service.GetAllList();
            ViewBag.Pid = Pid;

            return View();
        }

        [HttpGet]
        public ActionResult Models(string Pid="0", string keyword="")
        {
            int ShopID = 0;
            if(this.CurrentSession.IsAdmin)
            {
                 
            }
            else
            {
                ShopID = this.CurrentSession.ShopID;
            }
            List<Pro_Sort> data = this.Service.GetList(Pid, keyword, ShopID);
            return this.SuccessData(data);
        }

        [Permission("wiki.pro_sort.add")]
        [HttpPost]
        public ActionResult Add(AddPro_SortInput input)
        {
            if (string.IsNullOrEmpty(input.Pid))
            {
                input.Pid = "0";
            }
            input.ShopID = this.CurrentSession.ShopID;
            this.Service.Add(input);
            return this.AddSuccessMsg();
        }

        [Permission("wiki.pro_sort.update")]
        [HttpPost]
        public ActionResult Update(UpdatePro_SortInput input)
        {
            if (string.IsNullOrEmpty(input.Pid))
            {
                input.Pid = "0";
            }
            this.Service.Update(input);
            return this.UpdateSuccessMsg();
        }

        [Permission("wiki.pro_sort.delete")]
        [HttpPost]
        public ActionResult Delete(string id)
        {
            this.Service.Delete(id);
            return this.DeleteSuccessMsg();
        }

        
        

    }
}