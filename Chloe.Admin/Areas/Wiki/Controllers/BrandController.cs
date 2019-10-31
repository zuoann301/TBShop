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
    [Permission("wiki.Brand")]
    public class BrandController : WebController<IBrandService>
    {
        public IActionResult Index()
        {
            
            return View();
        }

        public ActionResult GetModels(Pagination pagination,int IsTop, string keyword)
        {
            PagedData<Brand> pagedData = this.Service.GetPageData(pagination, IsTop,keyword);
            return this.SuccessData(pagedData);
        }


        [Permission("wiki.Brand.delete")]
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
                Brand modle = this.Service.GetModel(Id);
                if (modle == null)
                {
                    //RedirectToAction("Brand", "Index");
                    Response.Redirect("/Wiki/Brand/Index");
                }
                

                ISortService SortService = this.CreateService<ISortService>();
                List<Sort> ListSort = SortService.GetList((int)EnumSort.Brand, "");
                ViewBag.ListSort = ListSort;

                ViewBag.BrandModle = modle;
            }


            return View();

        }


        [Permission("wiki.Brand.edit")]
        [HttpPost]
        public ActionResult Edit(UpdateBrandInput modle)
        {            
            this.Service.Update(modle);             
            return  RedirectToAction("Index");
        }

        [Permission("wiki.Brand.add")]
        [HttpPost]
        public ActionResult Add(AddBrandInput modle)
        {
            
            this.Service.Add(modle);
                          
            return RedirectToAction("Index");
        }

    }
}