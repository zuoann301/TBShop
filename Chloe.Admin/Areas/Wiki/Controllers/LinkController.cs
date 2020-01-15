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
    [Permission("wiki.link")]
    public class LinkController : WebController<ILinkService>
    {
        public IActionResult Index()
        {
            ISortService SortService = this.CreateService<ISortService>();
            List<Sort> ListSort = SortService.GetList((int)EnumSort.Link, "");
            ViewBag.ListSort = ListSort;

            return View();
        }

        public ActionResult Models(Pagination pagination,int SortID, string keyword)
        {
            int ShopID = 0;
            if (this.CurrentSession.IsAdmin)
            {

            }
            else
            {
                ShopID = this.CurrentSession.ShopID;
            }

            PagedData<Link> pagedData = this.Service.GetPageData(pagination, SortID,keyword, ShopID);
            return this.SuccessData(pagedData);
        }


        [Permission("wiki.link.delete")]
        [HttpPost]
        public ActionResult Delete(string id)
        { 
            this.Service.Delete(id);
            return this.SuccessMsg("删除成功");
        }
         
 


        [Permission("wiki.link.edit")]
        [HttpPost]
        public ActionResult Edit(UpdateLinkInput modle)
        {            
            this.Service.Update(modle);
            return this.SuccessMsg("编辑成功");
        }

        [Permission("wiki.link.add")]
        [HttpPost]
        public ActionResult Add(AddLinkInput modle)
        {
            
            this.Service.Add(modle);
            modle.ShopID = this.CurrentSession.ShopID;
            return this.SuccessMsg("添加成功");
        }

    }
}