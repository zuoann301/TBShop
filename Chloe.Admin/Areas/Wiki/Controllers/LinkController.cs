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
    [Permission("wiki.Link")]
    public class LinkController : WebController<ILinkService>
    {
        public IActionResult Index()
        {
            ISortService SortService = this.CreateService<ISortService>();
            List<Sort> ListSort = SortService.GetList((int)EnumSort.Link, "");
            ViewBag.ListSort = ListSort;

            return View();
        }

        public ActionResult GetModels(Pagination pagination,int SortID, string keyword)
        {
            PagedData<Link> pagedData = this.Service.GetPageData(pagination, SortID,keyword);
            return this.SuccessData(pagedData);
        }


        [Permission("wiki.Link.delete")]
        [HttpPost]
        public ActionResult Delete(string id)
        { 
            this.Service.Delete(id);
            return this.SuccessMsg("删除成功");
        }

        [HttpGet]
        public ActionResult Add()
        {

            ISortService SortService = this.CreateService<ISortService>();
            List<Sort> ListSort = SortService.GetList((int)EnumSort.Link, "");
            ViewBag.ListSort = ListSort;

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
                Link modle = this.Service.GetModel(Id);
                if (modle == null)
                {
                    //RedirectToAction("Link", "Index");
                    Response.Redirect("/Wiki/Link/Index");
                }
                

                ISortService SortService = this.CreateService<ISortService>();
                List<Sort> ListSort = SortService.GetList((int)EnumSort.Link, "");
                ViewBag.ListSort = ListSort;

                ViewBag.LinkModle = modle;
            }


            return View();

        }


        [Permission("wiki.Link.edit")]
        [HttpPost]
        public ActionResult Edit(UpdateLinkInput modle)
        {            
            this.Service.Update(modle);             
            return  RedirectToAction("Index");
        }

        [Permission("wiki.Link.add")]
        [HttpPost]
        public ActionResult Add(AddLinkInput modle)
        {
            
            this.Service.Add(modle);
                          
            return RedirectToAction("Index");
        }

    }
}