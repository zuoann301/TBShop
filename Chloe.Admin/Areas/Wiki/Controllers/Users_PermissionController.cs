using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ace;
using Ace.Application.Wiki;
using Ace.Entity.Wiki;
using Ace.Web.Mvc;
using Ace.Web.Mvc.Authorization;
using Chloe.Admin.Common;
using Chloe.Admin.Common.Tree;
using Microsoft.AspNetCore.Mvc;

namespace Chloe.Admin.Areas.Wiki.Controllers
{
    [Area(AreaNames.Wiki)]
    [Permission("wiki.permission")]
    public class Users_PermissionController : WebController<IUsers_PermissionService>
    {
        public IActionResult Index()
        {
            List<PermissionModel> menuGroups = this.GetPermissionModels();
            this.ViewBag.MenuGroups = menuGroups;
            return View();
        }

        public ActionResult Models(string keyword)
        {
            var data = this.Service.GetList();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string lowerKeyword = keyword.ToLower().Trim();
                data = TreeHelper.TreeWhere(data, a => a.Name.Contains(lowerKeyword, true) || a.Code.Contains(lowerKeyword, true), a => a.Id, a => a.ParentId);
            }

            List<DataTableTree> ret = new List<DataTableTree>();
            DataTableTree.AppendChildren(data, ref ret, null, 0, a => a.Id, a => a.ParentId);

            return this.SuccessData(ret);
        }

        public ActionResult MenuGroups()
        {
            List<PermissionModel> models = this.GetPermissionModels();
            return this.SuccessData(models);
        }

        List<PermissionModel> GetPermissionModels()
        {
            List<Users_Permission> data = this.Service.GetList(PermissionType.节点组);

            foreach (var item in data)
            {
                item.Children.AddRange(data.Where(a => a.ParentId == item.Id));
            }

            var rootGroups = data.Where(a => a.ParentId == null).ToList();

            List<PermissionModel> models = new List<PermissionModel>();
            this.Fill(models, rootGroups, 0);

            return models;
        }
        void Fill(List<PermissionModel> container, List<Users_Permission> permissions, int level)
        {
            foreach (var permission in permissions)
            {
                container.Add(PermissionModel.Create(permission, level));
                Fill(container, permission.Children, level + 1);
            }
        }

        [Permission("wiki.permission.add")]
        [HttpPost]
        public ActionResult Add(AddUsers_PermissionInput input)
        {
            this.Service.Add(input);
            return this.AddSuccessMsg();
        }

        [Permission("wiki.permission.update")]
        [HttpPost]
        public ActionResult Update(UpdateUsers_PermissionInput input)
        {
            this.Service.Update(input);
            return this.UpdateSuccessMsg();
        }

        [Permission("wiki.permission.delete")]
        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (id.IsNullOrEmpty())
                return this.FailedMsg("id 不能为空");

            this.Service.DeleteById(id);
            return this.DeleteSuccessMsg();
        }

        [Permission("wiki.permission.update")]
        [HttpPost]
        public ActionResult UpdateOrder(string data)
        {
            Dictionary<string, int> orderInfo = JsonHelper.Deserialize<Dictionary<string, int>>(data);
            this.Service.UpdateOrder(orderInfo);
            return this.UpdateSuccessMsg();
        }
    }


    public class PermissionModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public static PermissionModel Create(Users_Permission entity, int level = 0)
        {
            PermissionModel ret = new PermissionModel() { Id = entity.Id, Name = entity.Name };

            for (int i = 0; i < level; i++)
            {
                ret.Name = "　" + ret.Name;
            }

            return ret;
        }
    }
}