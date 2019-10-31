using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ace;
using Ace.Application.System;
using Ace.Entity.System;
using Ace.Web.Mvc;
using Ace.Web.Mvc.Authorization;
using Ace.Web.Mvc.Models;
using Chloe.Admin.Common;
using Chloe.Admin.Common.Tree;
using Microsoft.AspNetCore.Mvc;

namespace Chloe.Admin.Areas.Adm.Controllers
{
    [Area(AreaNames.Adm)]
    [Permission("adm.post")]
    public class PostController : WebController<IPostService>
    {
        public ActionResult Index()
        {
            List<SysOrg> orgs = this.CreateService<IOrgService>().GetList();
            this.ViewBag.Orgs = orgs;
            return View();
        }

        [HttpGet]
        public ActionResult Models(string keyword)
        {
            List<SysPost> data = this.Service.GetList(keyword);
            return this.SuccessData(data);
        }

        [Permission("adm.post.add")]
        [HttpPost]
        public ActionResult Add(AddPostInput input)
        {
            this.Service.Add(input);
            return this.AddSuccessMsg();
        }

        [Permission("adm.post.update")]
        [HttpPost]
        public ActionResult Update(UpdatePostInput input)
        {
            this.Service.Update(input);
            return this.UpdateSuccessMsg();
        }

        [Permission("adm.post.delete")]
        [HttpPost]
        public ActionResult Delete(string id)
        {
            this.Service.Delete(id, this.CurrentSession.UserId);
            return this.DeleteSuccessMsg();
        }

        public ActionResult GetModelsByOrgId(string orgs)
        {
            List<string> orgsIds = orgs.SplitString(',');

            List<SysOrg> orgList = this.CreateService<IOrgService>().GetListById(orgsIds);
            List<SysPost> posts = this.Service.GetListByOrgId(orgsIds);

            List<Select2Group> ret = new List<Select2Group>();

            foreach (var item in posts.GroupBy(a => a.OrgId))
            {
                Select2Group group = new Select2Group(orgList.Where(a => a.Id == item.Key).Select(a => a.Name).FirstOrDefault());
                group.children.AddRange(item.Select(a => new Select2Item(a.Id, a.Name)));
                ret.Add(group);
            }

            return this.SuccessData(ret);
        }
    }
}