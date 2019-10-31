using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ace;
using Ace.Application;
using Ace.Application.System;
using Ace.Entity;
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
    [Permission("adm.user")]
    public class UserController : WebController<IUserService>
    {
        public ActionResult Index()
        {
            IOrgService orgService = this.CreateService<IOrgService>();
            List<SysOrgType> orgTypes = orgService.GetOrgTypes();

            List<Select2Group> orgGroups = new List<Select2Group>();
            List<SysOrg> orgs = orgService.GetList();
            foreach (var item in orgs.GroupBy(a => a.OrgType).OrderBy(a => a.Key))
            {
                int? orgType = item.Key;
                string orgTypeName = orgTypes.Where(a => a.Id == orgType).Select(a => a.Name).FirstOrDefault();
                Select2Group group = new Select2Group(orgTypeName);
                group.children.AddRange(item.Select(a => new Select2Item(a.Id, a.Name)));
                orgGroups.Add(group);
            }

            this.ViewBag.Orgs = orgGroups;

            List<SysRole> roles = this.CreateService<IRoleService>().GetList();
            this.ViewBag.Roles = roles.Select(a => new Select2Item() { id = a.Id, text = a.Name });
            return View();
        }

        [HttpGet]
        public ActionResult Models(Pagination pagination, string keyword)
        {
            PagedData<SysUser> pagedData = this.Service.GetPageData(pagination, keyword);
            return this.SuccessData(pagedData);
        }

        [Permission("adm.user.add")]
        [HttpPost]
        public ActionResult Add(AddUserInput input)
        {
            input.CreatorId = this.CurrentSession.UserId;
            this.Service.Add(input);
            return this.AddSuccessMsg();
        }

        [Permission("adm.user.update")]
        [HttpPost]
        public ActionResult Update(UpdateUserInput input)
        {
            this.Service.Update(input);
            return this.UpdateSuccessMsg();
        }

        [Permission("adm.user.revise_password")]
        [HttpPost]
        public ActionResult RevisePassword(string userId, string newPassword)
        {
            if (userId.IsNullOrEmpty())
                return this.FailedMsg("userId 不能为空");

            this.Service.RevisePassword(userId, newPassword);
            return this.SuccessMsg("重置密码成功");
        }

        public ActionResult GetPermissionTree(string id)
        {
            List<SysPermission> authList = this.CreateService<IPermissionService>().GetList();
            List<SysUserPermission> authorizedata = new List<SysUserPermission>();
            if (!string.IsNullOrEmpty(id))
            {
                authorizedata = this.Service.GetPermissions(id);
            }
            var treeList = new List<TreeViewModel>();
            foreach (SysPermission auth in authList.Where(a => a.Type != PermissionType.公共菜单))
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

        [Permission("adm.user.set_permission")]
        [HttpPost]
        public ActionResult SetPermission(string id, string permissions)
        {
            List<string> permissionList = JsonHelper.DeserializeToList<string>(permissions);
            this.Service.SetPermission(id, permissionList);
            return this.SuccessMsg("保存成功");
        }

        [Permission("adm.user.change_state")]
        [HttpPost]
        public ActionResult ChangeState(string id, AccountState newState)
        {
            if (id == this.CurrentSession.UserId)
                return this.FailedMsg("亲，别折腾自己哟~");

            this.Service.ChangeState(id, newState);
            return this.SuccessMsg();
        }

        [Permission("adm.user.change_user_org_permission_state")]
        [HttpPost]
        public ActionResult ChangeUserOrgPermissionState(string userId, string orgId, bool newState)
        {
            this.Service.ChangeUserOrgPermissionState(userId, orgId, newState);
            return this.SuccessMsg();
        }
    }
}