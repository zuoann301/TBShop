using Ace;
using Ace.Application;
using Ace.Application.Common;
using Ace.Entity;
using Ace.Entity.System;
using Ace.Exceptions;
using Ace.IdStrategy;
using Ace.Security;
using Ace.Web.Mvc.Models;
using Chloe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ace.Application.System
{
    public interface IUserService : IAppService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwdText">明文</param>
        void RevisePassword(string userId, string pwdText);

        void Add(AddUserInput input);
        void Update(UpdateUserInput input);
        void ChangeState(string id, AccountState newState);
        void ChangeUserOrgPermissionState(string userId, string orgId, bool newState);

        List<SysUserPermission> GetPermissions(string id);
        void SetPermission(string id, List<string> permissionList);
        PagedData<SysUser> GetPageData(Pagination page, string keyword);

        List<SysPermission> GetUserPermissions(string id);
        List<string> GetUserPermits(string id);

        List<SysUser> GetAllList();
    }

    public class UserService : AppServiceBase<SysUser>, IUserService
    {
        public UserService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public void RevisePassword(string userId, string pwdText)
        {
            userId.NotNullOrEmpty("用户 Id 不能为空");
            PasswordHelper.EnsurePasswordLegal(pwdText);

            var user = this.DbContext.QueryByKey<SysUser>(userId);
            if (user == null)
                throw new InvalidInputException("用户不存在");
            if (user.State == AccountState.Closed)
                throw new InvalidInputException("无法修改已注销用户");

            user.EnsureIsNotAdmin();

            string userSecretkey = UserHelper.GenUserSecretkey();
            string encryptedPassword = PasswordHelper.Encrypt(pwdText, userSecretkey);

            this.DbContext.DoWithTransaction(() =>
            {
                this.DbContext.Update<SysUserLogOn>(a => a.UserId == userId, a => new SysUserLogOn() { UserSecretkey = userSecretkey, UserPassword = encryptedPassword });
            });
        }

        public void Add(AddUserInput input)
        {
            this.Trim(input);

            input.Validate();

            if (input.AccountName.IsNullOrEmpty() && input.MobilePhone.IsNullOrEmpty() && input.Email.IsNullOrEmpty())
            {
                throw new InvalidInputException("用户名/手机号码/邮箱至少填一个");
            }

            string accountName = null;
            if (input.AccountName.IsNotNullOrEmpty())
            {
                accountName = input.AccountName.ToLower();
                AceUtils.EnsureAccountNameLegal(accountName);
                bool exists = this.DbContext.Query<SysUser>().Where(a => a.AccountName == accountName).Any();
                if (exists)
                    throw new InvalidInputException("用户名[{0}]已存在".ToFormat(input.AccountName));
            }

            string mobilePhone = null;
            if (input.MobilePhone.IsNotNullOrEmpty())
            {
                mobilePhone = input.MobilePhone;
                if (AceUtils.IsMobilePhone(mobilePhone) == false)
                    throw new InvalidInputException("请输入正确的手机号码");

                bool exists = this.DbContext.Query<SysUser>().Where(a => a.MobilePhone == mobilePhone).Any();
                if (exists)
                    throw new InvalidInputException("手机号码[{0}]已存在".ToFormat(mobilePhone));
            }

            string email = null;
            if (input.Email.IsNotNullOrEmpty())
            {
                email = input.Email.ToLower();
                if (AceUtils.IsEmail(email) == false)
                    throw new InvalidInputException("请输入正确的邮箱地址");

                bool exists = this.DbContext.Query<SysUser>().Where(a => a.Email == email).Any();
                if (exists)
                    throw new InvalidInputException("邮箱地址[{0}]已存在".ToFormat(input.Email));
            }

            SysUser user = this.CreateEntity<SysUser>(null, input.CreatorId);
            user.AccountName = accountName;
            user.Name = input.Name;
            user.Gender = input.Gender;
            user.MobilePhone = mobilePhone;
            user.Birthday = input.Birthday;
            user.WeChat = input.WeChat;
            user.Email = email;
            user.Description = input.Description;
            user.State = AccountState.Normal;
            user.ShopID = input.ShopID;

            string userSecretkey = UserHelper.GenUserSecretkey();
            string encryptedPassword = PasswordHelper.Encrypt(input.Password, userSecretkey);

            SysUserLogOn logOnEntity = new SysUserLogOn();
            logOnEntity.Id = IdHelper.CreateStringSnowflakeId();
            logOnEntity.UserId = user.Id;
            logOnEntity.UserSecretkey = userSecretkey;
            logOnEntity.UserPassword = encryptedPassword;

            List<string> roleIds = input.GetRoles();
            List<SysUserRole> userRoles = roleIds.Select(a =>
             {
                 return new SysUserRole()
                 {
                     Id = IdHelper.CreateStringSnowflakeId(),
                     UserId = user.Id,
                     RoleId = a,
                 };
             }).ToList();

            user.RoleIds = string.Join(",", roleIds);

            List<string> orgIds = input.GetOrgs();
            List<SysUserOrg> userOrgs = orgIds.Select(a =>
            {
                return new SysUserOrg()
                {
                    Id = IdHelper.CreateStringSnowflakeId(),
                    UserId = user.Id,
                    OrgId = a,
                    DisablePermission = false
                };
            }).ToList();

            user.OrgIds = string.Join(",", orgIds);

            List<string> postIds = input.GetPosts();
            List<SysUserPost> userPosts = postIds.Select(a =>
            {
                return new SysUserPost()
                {
                    Id = IdHelper.CreateStringSnowflakeId(),
                    UserId = user.Id,
                    PostId = a
                };
            }).ToList();

            user.PostIds = string.Join(",", postIds);

            this.DbContext.DoWithTransaction(() =>
            {
                this.DbContext.Insert(user);
                this.DbContext.Insert(logOnEntity);
                this.DbContext.InsertRange(userRoles);
                this.DbContext.InsertRange(userOrgs);
                this.DbContext.InsertRange(userPosts);
            });
        }
        public void Update(UpdateUserInput input)
        {
            this.Trim(input);

            input.Validate();

            SysUser user = this.Query.Where(a => a.Id == input.Id).AsTracking().First();

            user.EnsureIsNotAdmin();
            if (user.State == AccountState.Closed)
                throw new InvalidInputException("无法修改已注销用户");

            string accountName = null;
            if (user.AccountName.IsNullOrEmpty())
            {
                //用户名设置后不能修改
                if (input.AccountName.IsNotNullOrEmpty())
                {
                    accountName = input.AccountName.ToLower();
                    AceUtils.EnsureAccountNameLegal(accountName);
                    bool exists = this.DbContext.Query<SysUser>().Where(a => a.AccountName == accountName).Any();
                    if (exists)
                        throw new InvalidInputException("用户名[{0}]已存在".ToFormat(input.AccountName));
                }
            }
            else
                accountName = user.AccountName;

            string mobilePhone = null;
            if (user.MobilePhone.IsNotNullOrEmpty() && input.MobilePhone.IsNullOrEmpty())
            {
                //手机号码设置后不能再改为空
                throw new InvalidInputException("请输入手机号码");
            }
            if (input.MobilePhone.IsNotNullOrEmpty())
            {
                mobilePhone = input.MobilePhone;
                if (AceUtils.IsMobilePhone(mobilePhone) == false)
                    throw new InvalidInputException("请输入正确的手机号码");

                if (user.MobilePhone != mobilePhone)//不等说明手机号码有变
                {
                    bool exists = this.DbContext.Query<SysUser>().Where(a => a.MobilePhone == mobilePhone).Any();
                    if (exists)
                        throw new InvalidInputException("手机号码[{0}]已存在".ToFormat(mobilePhone));
                }
            }

            string email = null;
            if (user.Email.IsNotNullOrEmpty() && input.Email.IsNullOrEmpty())
            {
                //邮箱地址设置后不能再改为空
                throw new InvalidInputException("请输入邮箱地址");
            }
            if (input.Email.IsNotNullOrEmpty())
            {
                email = input.Email.ToLower();
                if (AceUtils.IsEmail(email) == false)
                    throw new InvalidInputException("请输入正确的邮箱地址");

                if (user.Email != email)//不等说明邮箱有变
                {
                    bool exists = this.DbContext.Query<SysUser>().Where(a => a.Email == email).Any();
                    if (exists)
                        throw new InvalidInputException("邮箱地址[{0}]已存在".ToFormat(input.Email));
                }
            }

            user.AccountName = accountName;
            user.Name = input.Name;
            user.Gender = input.Gender;
            user.MobilePhone = mobilePhone;
            user.Birthday = input.Birthday;
            user.WeChat = input.WeChat;
            user.Email = email;
            user.Description = input.Description;
            user.ShopID = input.ShopID;

            List<string> roleIds = input.GetRoles();
            List<SysUserRole> userRoles = this.DbContext.Query<SysUserRole>().Where(a => a.UserId == input.Id).ToList();
            List<string> userRolesToDelete = userRoles.Where(a => !roleIds.Contains(a.Id)).Select(a => a.Id).ToList();
            List<SysUserRole> userRolesToAdd = roleIds.Where(a => !userRoles.Any(r => r.Id == a)).Select(a =>
            {
                return new SysUserRole()
                {
                    Id = IdHelper.CreateStringSnowflakeId(),
                    UserId = input.Id,
                    RoleId = a,
                };
            }).ToList();

            user.RoleIds = string.Join(",", roleIds);

            List<string> orgIds = input.GetOrgs();
            List<SysUserOrg> userOrgs = this.DbContext.Query<SysUserOrg>().Where(a => a.UserId == input.Id).ToList();
            List<string> userOrgsToDelete = userOrgs.Where(a => !orgIds.Contains(a.Id)).Select(a => a.Id).ToList();
            List<SysUserOrg> userOrgsToAdd = orgIds.Where(a => !userOrgs.Any(r => r.Id == a)).Select(a =>
            {
                return new SysUserOrg()
                {
                    Id = IdHelper.CreateStringSnowflakeId(),
                    UserId = input.Id,
                    OrgId = a,
                    DisablePermission = false
                };
            }).ToList();

            user.OrgIds = string.Join(",", orgIds);

            List<string> postIds = input.GetPosts();
            List<SysUserPost> userPosts = postIds.Select(a =>
            {
                return new SysUserPost()
                {
                    Id = IdHelper.CreateStringSnowflakeId(),
                    UserId = input.Id,
                    PostId = a
                };
            }).ToList();

            user.PostIds = string.Join(",", postIds);

            this.DbContext.DoWithTransaction(() =>
            {
                this.DbContext.Delete<SysUserRole>(a => a.Id.In(userRolesToDelete));
                this.DbContext.InsertRange(userRolesToAdd);

                this.DbContext.Delete<SysUserOrg>(a => a.Id.In(userOrgsToDelete));
                this.DbContext.InsertRange(userOrgsToAdd);

                this.DbContext.Delete<SysUserPost>(a => a.UserId == input.Id);
                this.DbContext.InsertRange(userPosts);

                this.DbContext.Update<SysUser>(user);
            });
        }
        void Trim(AddUpdateUserInputBase input)
        {
            if (input.AccountName.IsNotNullOrEmpty())
                input.AccountName = input.AccountName.Trim();
            if (input.MobilePhone.IsNotNullOrEmpty())
                input.MobilePhone = input.MobilePhone.Trim();
            if (input.Email.IsNotNullOrEmpty())
                input.Email = input.Email.Trim();
        }

        public void ChangeState(string id, AccountState newState)
        {
            id.NotNullOrEmpty();
            SysUser user = this.Query.Where(a => a.Id == id).AsTracking().FirstOrDefault();
            user.EnsureIsNotAdmin();
            if (user == null)
                throw new InvalidInputException("用户不存在");
            if (user.State == AccountState.Closed)
                throw new InvalidInputException("用户已注销，无法操作");

            List<AccountState> list = new List<AccountState>() { AccountState.Normal, AccountState.Disabled, AccountState.Closed };
            if (newState.In(list) == false)
                throw new InvalidInputException("状态无效：" + newState.ToString());

            user.State = newState;

            this.DbContext.DoWithTransaction(() =>
            {
                this.DbContext.Update(user);
                //this.Log(LogType.Update, "User", true, $"用户[{this.Session.UserId}]修改用户[{id}]状态为：{newState}");
            });
        }
        public void ChangeUserOrgPermissionState(string userId, string orgId, bool newState)
        {
            userId.NotNullOrEmpty();
            orgId.NotNullOrEmpty();

            this.DbContext.Update<SysUserOrg>(a => a.UserId == userId && a.OrgId == orgId, a => new SysUserOrg() { DisablePermission = newState });
        }

        public List<SysUserPermission> GetPermissions(string id)
        {
            return this.DbContext.Query<SysUserPermission>().Where(t => t.UserId == id).ToList();
        }
        public void SetPermission(string id, List<string> permissionList)
        {
            id.NotNullOrEmpty();

            List<SysUserPermission> rolePermissions = permissionList.Select(a => new SysUserPermission()
            {
                Id = IdHelper.CreateStringSnowflakeId(),
                UserId = id,
                PermissionId = a
            }).ToList();

            this.DbContext.DoWithTransaction(() =>
            {
                this.DbContext.Delete<SysUserPermission>(a => a.UserId == id);
                this.DbContext.InsertRange(rolePermissions);
            });
        }

        public List<SysUser> GetAllList()
        {
            List<SysUser> list= this.DbContext.Query<SysUser>().ToList();
            return list;
        }

        public PagedData<SysUser> GetPageData(Pagination page, string keyword)
        {
            var q = this.DbContext.Query<SysUser>();

            q = q.WhereIfNotNullOrEmpty(keyword, a => a.AccountName.Contains(keyword) || a.Name.Contains(keyword) || a.MobilePhone.Contains(keyword));
            q = q.Where(a => a.AccountName != AppConsts.AdminUserName);
            q = q.OrderByDesc(a => a.CreationTime);

            PagedData<SysUser> pagedData = q.TakePageData(page);

            List<string> userIds = pagedData.Models.Select(a => a.Id).ToList();
            List<string> postIds = pagedData.Models.SelectMany(a => a.PostIds.SplitString()).Distinct().ToList();
            List<string> roleIds = pagedData.Models.SelectMany(a => a.RoleIds.SplitString()).Distinct().ToList();

            List<SysPost> posts = this.DbContext.Query<SysPost>().Where(a => a.Id.In(postIds)).ToList();
            List<SysRole> roles = this.DbContext.Query<SysRole>().Where(a => a.Id.In(roleIds)).ToList();

            List<SysUserOrg> userOrgs = this.DbContext.Query<SysUserOrg>().InnerJoin<SysOrg>((a, b) => a.OrgId == b.Id)
                    .Where((a, b) => userIds.Contains(a.UserId))
                    .Select((a, b) => new SysUserOrg() { Id = a.Id, UserId = a.UserId, OrgId = a.OrgId, DisablePermission = a.DisablePermission, Org = b }).ToList();

            foreach (SysUser user in pagedData.Models)
            {
                user.UserOrgs.AddRange(userOrgs.Where(a => a.UserId == user.Id));

                List<string> userPostIds = user.PostIds.SplitString();
                user.Posts.AddRange(posts.Where(a => a.Id.In(userPostIds)));

                List<string> userRoleIds = user.RoleIds.SplitString();
                user.Roles.AddRange(roles.Where(a => a.Id.In(userRoleIds)));
            }

            return pagedData;
        }

        public List<SysPermission> GetUserPermissions(string id)
        {
            List<SysPermission> ret = new List<SysPermission>();

            List<string> userPermissionIds = this.DbContext.Query<SysUserPermission>().Where(a => a.UserId == id).Select(a => a.PermissionId).ToList();

            List<string> rolePermissionIds = this.DbContext.JoinQuery<SysRolePermission, SysRole, SysUserRole>((rolePermission, role, userRole) => new object[] {
                JoinType.InnerJoin,rolePermission.RoleId==role.Id,
                JoinType.InnerJoin,role.Id==userRole.RoleId
            })
            .Where((rolePermission, role, userRole) => userRole.UserId == id)
            .Select((rolePermission, role, userRole) => rolePermission.PermissionId).ToList();

            List<string> orgPermissionIds = this.DbContext.JoinQuery<SysOrgPermission, SysOrg, SysUserOrg>((orgPermission, org, userOrg) => new object[] {
                JoinType.InnerJoin,orgPermission.OrgId==org.Id,
                JoinType.InnerJoin,org.Id==userOrg.OrgId
            })
            .Where((orgPermission, org, userOrg) => userOrg.UserId == id && userOrg.DisablePermission == false)
            .Select((orgPermission, org, userOrg) => orgPermission.PermissionId).ToList();

            List<string> permissionIds = userPermissionIds.Concat(rolePermissionIds).Concat(orgPermissionIds).Distinct().ToList();

            ret = this.DbContext.Query<SysPermission>().Where(a => permissionIds.Contains(a.Id)).ToList();

            return ret;
        }
        public List<string> GetUserPermits(string id)
        {
            var ret = this.GetUserPermissions(id).Where(a => a.Code.IsNotNullOrEmpty()).Select(a => a.Code).ToList();
            return ret;
        }
    }
}
