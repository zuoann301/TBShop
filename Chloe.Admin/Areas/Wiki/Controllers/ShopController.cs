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

        public ActionResult GetModels(Pagination pagination, int Brand =0, string keyword="")
        {
            PagedData<Shop> pagedData = this.Service.GetPageData(pagination, Brand, keyword);
            return this.SuccessData(pagedData);
        }


        [Permission("wiki.shop.delete")]
        [HttpPost]
        public ActionResult Delete(int ID)
        { 
            this.Service.Delete(ID);
            return this.SuccessMsg("删除成功");
        }

        [HttpGet]
        public ActionResult Add()
        {

            ISortService SortService = this.CreateService<ISortService>();
            List<Sort> ListSort = SortService.GetList((int)EnumSort.Brand, "");
            ViewBag.ListSort = ListSort;

            IDistrictService districtService = this.CreateService<IDistrictService>();
            List<District> ListDistrict = districtService.GetList(1, "");
            ViewBag.ListDistrict = ListDistrict;


            return View();
        }


        [HttpGet]
        public ActionResult Edit(int ID=0)
        {

            if (ID==0)
            {
                return Redirect("Index");
            }
            else
            {
                Shop modle = this.Service.GetModel(ID);
                if (modle == null)
                {
                    return RedirectToAction("Index");
                }


                IDistrictService districtService = this.CreateService<IDistrictService>();
                List<District> ListDistrict = districtService.GetList(1, "");
                ViewBag.ListDistrict = ListDistrict;

                List<District> ListDistrict2 = new List<District>();
                if (modle.ProvinceID > 0)
                {
                    ListDistrict2= districtService.GetList(modle.ProvinceID, "");
                }
                ViewBag.ListDistrict2 = ListDistrict2;

                List<District> ListDistrict3 = new List<District>();
                if (modle.CityID > 0)
                {
                    ListDistrict3 = districtService.GetList(modle.CityID, "");
                }
                ViewBag.ListDistrict3 = ListDistrict3;

                ISortService SortService = this.CreateService<ISortService>();
                List<Sort> ListSort = SortService.GetList((int)EnumSort.Brand, "");
                ViewBag.ListSort = ListSort;

                ViewBag.ShopModle = modle;
            }


            return View();

        }


        [Permission("wiki.shop.edit")]
        [HttpPost]
        public IActionResult Edit(UpdateShopInput modle)
        {
            modle.CreateDate = DateTime.Now;
            this.Service.Update(modle);

            return RedirectToAction("Index");
            //return View("Index");
        }

        [Permission("wiki.shop.add")]
        [HttpPost]
        public ActionResult Add(AddShopInput modle)
        {
            modle.CreateDate = DateTime.Now;
            this.Service.Add(modle);
                          
            return View("Index");
        }



        public List<Select2Item> GetShopList()
        {
            List<Select2Item> list = new List<Select2Item>();



            return list;
        }



    }
}