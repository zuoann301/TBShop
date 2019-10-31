using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ace;
using Ace.Application.Wiki;
using Ace.Application.System;
using Ace.Entity.Wiki;
using Ace.Entity.System;
using Ace.Web.Mvc;
using Ace.Web.Mvc.Authorization;
using Ace.Web.Mvc.Models;
using Chloe.Admin.Common;
using Chloe.Admin.Common.Tree;
using Microsoft.AspNetCore.Mvc;

namespace Chloe.Admin.Areas.Wiki.Controllers
{
    [Area(AreaNames.Wiki)]
    [Permission("wiki.stockout")]
    public class StockOutController : WebController<IStockService>
    {
        public IActionResult Index()
        {
            IUserService UserService = this.CreateService<IUserService>();
            List<SysUser> ListUser = UserService.GetAllList();
            this.ViewBag.Users = ListUser.Select(a => new Select2Item() { id = a.Id, text = a.Name });
            return View();
        }

        public ActionResult Item(string StockCode)
        {
            ViewBag.StockCode = StockCode;

            return View();
        }

        public ActionResult GetModels(Pagination pagination, string keyword)
        {
            PagedData<Stock> pagedData = this.Service.GetPageData(pagination,"O", keyword);
            return this.SuccessData(pagedData);
        }

        public ActionResult GetItemModels(Pagination pagination, string StockCode)
        {
            IStock_InfoService stock_InfoService = this.CreateService<IStock_InfoService>();
            PagedData<Stock_Info> pagedData = stock_InfoService.GetPageData(pagination, StockCode);
            return this.SuccessData(pagedData);
        }

        [Permission("wiki.stockout.add")]
        [HttpPost]
        public ActionResult Add(AddStockInput input)
        {
            input.Type = "O";
            input.CreateID = CurrentSession.UserId;
            input.CreateDate = DateTime.Now;
            this.Service.Add(input);
            return this.AddSuccessMsg();
        }

        [Permission("adm.stockout.add")]
        [HttpPost]
        public ActionResult AddItem(AddStock_InfoInput input)
        {
            input.SubTotal = input.Quantity * input.Price;
            IStock_InfoService stock_InfoService = this.CreateService<IStock_InfoService>();
            stock_InfoService.Add(input);
            return this.AddSuccessMsg();
        }


        [Permission("wiki.stockout.update")]
        [HttpPost]
        public ActionResult Update(UpdateStockInput input)
        {
            input.Type = "O";
            input.CreateID = CurrentSession.UserId;
            input.CreateDate = DateTime.Now;
            this.Service.Update(input);
            return this.UpdateSuccessMsg();
        }


        [Permission("wiki.stockout.update")]
        [HttpPost]
        public ActionResult UpdateItem(UpdateStock_InfoInput input)
        {
            input.SubTotal = input.Quantity * input.Price;
            IStock_InfoService stock_InfoService = this.CreateService<IStock_InfoService>();
            stock_InfoService.Update(input);
            return this.UpdateSuccessMsg();
        }


        [Permission("wiki.stockout.delete")]
        [HttpPost]
        public ActionResult Delete(string id)
        {
            this.Service.Delete(id);
            return this.DeleteSuccessMsg();
        }


        [Permission("wiki.stockout.delete")]
        [HttpPost]
        public ActionResult DeleteItem(string id)
        {
            IStock_InfoService stock_InfoService = this.CreateService<IStock_InfoService>();
            stock_InfoService.Delete(id);
            return this.DeleteSuccessMsg();
        }

    }
}