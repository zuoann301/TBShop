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
    [Permission("wiki.shoporder")]
    public class ShopOrderController : WebController<IShopOrderService>
    {
        public IActionResult Index()
        {
            IShopService SortService = this.CreateService<IShopService>();
            List<SimpleShop2> ListShop = SortService.GetCacheList2();
            ViewBag.ListShop = ListShop;

            //IUsersService UserService = this.CreateService<IUsersService>();
            //List<Users> ListUser = UserService.GetList();
            //this.ViewBag.Users = ListUser.Select(a => new Select2Item() { id = a.Id, text = a.UserName });

            return View();
        }

        [HttpGet]
        public ActionResult GetModels(Pagination pagination,string CreateID="",int ST=-1)
        {
            int ShopID = 0;
            if (this.CurrentSession.IsAdmin)
            {

            }
            else
            {
                ShopID = this.CurrentSession.ShopID;
            }

            PagedData<ShopOrderInfo> pagedData = this.Service.GetPageOrderList(pagination, ShopID, CreateID, ST);
            return this.SuccessData(pagedData);
        }


        [Permission("wiki.shoporder.delete")]
        [HttpPost]
        public ActionResult Delete(string id)
        { 
            this.Service.Delete(id);
            return this.SuccessMsg("删除成功");
        }

        


        [Permission("wiki.shoporder.edit")]
        [HttpPost]
        public ActionResult Update(UpdateShopOrderInput modle)
        {
            this.Service.Update(modle);
            return this.UpdateSuccessMsg();
        }

        [Permission("wiki.shoporder.add")]
        [HttpPost]
        public ActionResult Add(AddShopOrderInput modle)
        {
            modle.CreateDate = DateTime.Now;

            this.Service.Add(modle);
            return this.AddSuccessMsg();
        }


        public ActionResult Item(string Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpGet]
        public ActionResult GetItemModels(Pagination pagination,string Id)
        {
            IShopOrderItemService shopOrderItemService = this.CreateService<IShopOrderItemService>();
            //PagedData<ShopOrderItem> pagedData = shopOrderItemService.GetPageData(pagination, Id);
            List<ShopOrderItemInfo> list = shopOrderItemService.GetOrderItemList(Id);
            PagedData<ShopOrderItemInfo> pagedData = new PagedData<ShopOrderItemInfo>();
            pagedData.CurrentPage = pagination.Page;
            pagedData.Models = list;
            pagedData.PageSize = pagination.PageSize;
            pagedData.TotalCount = list.Count;

             
            return this.SuccessData(pagedData);
        }


        [Permission("wiki.shoporder.delete")]
        [HttpPost]
        public ActionResult DeleteItem(string id)
        {
            IShopOrderItemService shopOrderItemService = this.CreateService<IShopOrderItemService>();
            shopOrderItemService.Delete(id);
            return this.SuccessMsg("删除成功");
        }

        [HttpPost]
        public ActionResult UpdateItem(UpdateShopOrderInput modle)
        {
            this.Service.Update(modle);
            return this.UpdateSuccessMsg();
        }
    }
}