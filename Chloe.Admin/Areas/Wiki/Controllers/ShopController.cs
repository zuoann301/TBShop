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
    [Permission("wiki.shop")]
    public class ShopController : WebController<IShopService>
    {
        public IActionResult Index()
        {
            ISortService SortService = this.CreateService<ISortService>();
            List<Sort> ListSort = SortService.GetList((int)EnumSort.Brand, "");
            ViewBag.ListSort = ListSort;

            return View();
        }

        public ActionResult Models(Pagination pagination, int BrandID = 0, string keyword="")
        {
            PagedData<Shop> pagedData = this.Service.GetPageData(pagination, BrandID, keyword);
            return this.SuccessData(pagedData);
        }


        [Permission("wiki.shop.delete")]
        [HttpPost]
        public ActionResult Delete(int ID)
        { 
            this.Service.Delete(ID);
            return this.SuccessMsg("删除成功");
        }

         


        [Permission("wiki.shop.edit")]
        [HttpPost]
        public IActionResult Edit(UpdateShopInput modle)
        {
            modle.CreateDate = DateTime.Now;
            this.Service.Update(modle);

            return this.SuccessMsg();
        }

        [Permission("wiki.shop.add")]
        [HttpPost]
        public ActionResult Add(AddShopInput modle)
        {
            modle.CreateDate = DateTime.Now;
            this.Service.Add(modle);

            return this.SuccessMsg();
        }



        public List<Select2Item> GetShopList()
        {
            List<Select2Item> list = new List<Select2Item>();



            return list;
        }



    }
}