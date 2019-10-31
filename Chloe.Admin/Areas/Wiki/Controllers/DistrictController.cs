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
    [Permission("wiki.district")]
    public class DistrictController : WebController<IDistrictService>
    {
        public IActionResult Index()
        { 
            return View();
        }

        public ActionResult GetModels(Pagination pagination,int SortID, string keyword)
        {
            PagedData<District> pagedData = this.Service.GetPageData(pagination, SortID,keyword);
            return this.SuccessData(pagedData);
        }

        [HttpGet]
        public ActionResult GetList(int Pid)
        {
            List<District> pagedData = this.Service.GetList(Pid, "");
            return this.SuccessData(pagedData);
        }



        [Permission("wiki.district.delete")]
        [HttpPost]
        public ActionResult Delete(int ID)
        { 
            this.Service.Delete(ID);
            return this.SuccessMsg("删除成功");
        }

        [HttpGet]
        public ActionResult Add()
        { 
            return View();
        }


        [HttpGet]
        public ActionResult Edit(int ID)
        {

            if (ID<1)
            {
                return Redirect("Index");
            }
            else
            {
                District modle = this.Service.GetModel(ID);
                if (modle == null)
                {
                    //RedirectToAction("District", "Index");
                    Response.Redirect("/Wiki/District/Index");
                }
                 

                ViewBag.DistrictModle = modle;
            }


            return View();

        }


        [Permission("wiki.District.edit")]
        [HttpPost]
        public ActionResult Edit(UpdateDistrictInput modle)
        {            
            this.Service.Update(modle);             
            return  RedirectToAction("Index");
        }

        [Permission("wiki.District.add")]
        [HttpPost]
        public ActionResult Add(AddDistrictInput modle)
        {
            
            this.Service.Add(modle);
                          
            return RedirectToAction("Index");
        }

    }
}