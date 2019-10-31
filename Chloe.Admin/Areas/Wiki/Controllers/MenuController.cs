using Chloe.Admin.Common.Tree;
using Ace.Entity;
using Chloe.Admin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ace.Application.Wiki;
using Ace.Entity.Wiki;
using Ace.Web.Mvc.Authorization;

namespace Chloe.Admin.Areas.Wiki.Controllers
{
    [Area(AreaNames.Wiki)]
    [Permission("wiki.menu")]
    public class MenuController : WebController<IWikiMenuItemService>
    {
        // GET: Wiki
        public ActionResult Index()
        {
            List<WikiMenuItem> rootMenuItems = this.Service.GetRootWikiMenuItems();
            this.ViewBag.RootMenuItems = rootMenuItems;

            var docs = this.CreateService<IWikiDocumentService>().GetAll();
            this.ViewBag.Documents = docs;

            return View();
        }
        public ActionResult GetModels(string keyword)
        {
            var data = this.Service.GetWikiMenuItems();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = TreeHelper.TreeWhere(data, a => a.Name.Contains(keyword), a => a.Id, a => a.ParentId);
            }

            List<DataTableTree> ret = new List<DataTableTree>();
            DataTableTree.AppendChildren(data, ref ret, null, 0, a => a.Id, a => a.ParentId, a => a.SortCode);

            return this.SuccessData(ret);
        }

        [Permission("wiki.menu.add")]
        [HttpPost]
        public ActionResult Add(AddWikiMenuItemInput input)
        {
            IWikiMenuItemService service = this.Service;
            WikiMenuItem entity = service.Add(input);
            return this.AddSuccessData(entity);
        }

        [Permission("wiki.menu.update")]
        [HttpPost]
        public ActionResult Update(UpdateWikiMenuItemInput input)
        {
            IWikiMenuItemService service = this.Service;
            service.Update(input);
            return this.UpdateSuccessMsg();
        }

        [Permission("wiki.menu.delete")]
        [HttpPost]
        public ActionResult Delete(string id)
        {
            IWikiMenuItemService service = this.Service;
            service.Delete(id);
            return this.DeleteSuccessMsg();
        }
    }
}