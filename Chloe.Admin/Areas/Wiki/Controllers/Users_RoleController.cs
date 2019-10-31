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
    [Permission("adm.users_role")]
    public class Users_RoleController : WebController<IUsers_RoleService>
    {
        // GET: SystemManage/Users_Role
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Models(string keyword)
        {
            List<Users_Role> data = this.Service.GetList(keyword);
            return this.SuccessData(data);
        }

        [Permission("adm.users_role.add")]
        [HttpPost]
        public ActionResult Add(AddUsers_RoleInput input)
        {
             
            this.Service.Add(input);
            return this.AddSuccessMsg();
        }

        [Permission("adm.users_role.update")]
        [HttpPost]
        public ActionResult Update(UpdateUsers_RoleInput input)
        {
            this.Service.Update(input);
            return this.UpdateSuccessMsg();
        }

        [Permission("adm.Users_Role.delete")]
        [HttpPost]
        public ActionResult Delete(int ID)
        {
            this.Service.Delete(ID);
            return this.DeleteSuccessMsg();
        }

        public ActionResult GetPermissionTree(int ID)
        {
            List<Users_Permission> authList = this.CreateService<IUsers_PermissionService>().GetList();
            List<Users_RolePermission> authorizedata = new List<Users_RolePermission>();
            if (ID>0)
            {
                authorizedata = this.Service.GetPermissions(ID);
            }
            var treeList = new List<TreeViewModel>();
            foreach (Users_Permission auth in authList.Where(a => a.Type != PermissionType.公共菜单))
            {
                string typeName = "";
                if (auth.Type == PermissionType.权限菜单)
                    typeName = $" [菜单]";

                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = authList.Any(a => a.ParentId == auth.Id);
                tree.Id = auth.Id;
                tree.Text = auth.Name + typeName;
                tree.Value = auth.Code;
                tree.ParentId = auth.ParentId;
                tree.Isexpand = true;
                tree.Complete = true;
                tree.Showcheck = true;
                tree.Checkstate = authorizedata.Count(t => t.PermissionId == auth.Id);
                tree.HasChildren = hasChildren;
                tree.Img = auth.Icon == "" ? "" : auth.Icon;
                treeList.Add(tree);
            }

            return Content(TreeHelper.ConvertToJson(treeList));
        }

        [Permission("adm.Users_Role.set_permission")]
        [HttpPost]
        public ActionResult SetPermission(int ID, string permissions)
        {
            List<string> permissionList = JsonHelper.DeserializeToList<string>(permissions);
            this.Service.SetPermission(ID, permissionList);
            return this.SuccessMsg("保存成功");
        }
    }
}