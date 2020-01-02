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

        public ActionResult Models(Pagination pagination,int IsTop, string keyword)
        {
            int ShopID = 0;
            if (this.CurrentSession.IsAdmin)
            {

            }
            else
            {
                ShopID = this.CurrentSession.ShopID;
            }

            PagedData<Brand> pagedData = this.Service.GetPageData(pagination, IsTop,keyword, ShopID);
            return this.SuccessData(pagedData);
        }


        [Permission("wiki.Brand.delete")]
        [HttpPost]
        public ActionResult Delete(string id)
        { 
            this.Service.Delete(id);
            return this.SuccessMsg("删除成功");
        }

       
        [Permission("wiki.Brand.edit")]
        [HttpPost]
        public ActionResult Edit(UpdateBrandInput modle)
        {            
            this.Service.Update(modle);
            return this.SuccessMsg("编辑成功");
        }

        [Permission("wiki.Brand.add")]
        [HttpPost]
        public ActionResult Add(AddBrandInput modle)
        {
            modle.ShopID = this.CurrentSession.ShopID;
            
            this.Service.Add(modle);

            return this.SuccessMsg("添加成功");
        }

    }
}