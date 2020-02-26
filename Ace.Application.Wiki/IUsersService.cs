using Ace.Application;
using Ace.Application.Common;
using Ace.Data;
using Ace.Entity;
using Ace.Entity.Wiki;
using Ace.Exceptions;
using Ace.IdStrategy;
using Ace.Security;
using Chloe;
using Chloe.MySql;
using Chloe.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Application.Wiki
{
    public interface IUsersService : IAppService
    {
        List<Users> GetList(int ShopID = 0, int RoleID = 0, string keyword = "");
        Users Add(AddUsersInput input);
        void Update(UpdateUsersInput input);
        void Delete(string id );

        Users GetModel(string Id);
        Users GetModelByOpenID(string OpenID);

        PagedData<Users> GetPageData(Pagination page, int ShopID = 0, int RoleID = 0, string keyword = "");


        //--------------------------------------------------------------------------------------
        bool CheckLogin(string loginName, string password, out Users user, out string msg);

        void RevisePassword(string userId, string pwdText);

        bool CheckMobile(string Mobile);
        bool CheckOpenID(string OpenID);

        void UpdateUserInfo(string OpenID, string Mobile, string UserIcon, string UserName);
    }

    public class UsersService : AppServiceBase<Users>, IUsersService
    {
        public UsersService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }


        public void RevisePassword(string userId, string pwdText)
        {
            userId.NotNullOrEmpty("用户 Id 不能为空");
            PasswordHelper.EnsurePasswordLegal(pwdText);

            var user = this.DbContext.QueryByKey<Users>(userId);
            if (user == null)
                throw new InvalidInputException("用户不存在");
            if (user.ST == (int)AccountState.Closed)
                throw new InvalidInputException("无法修改已注销用户");

            string md5Pass = Ace.Security.PasswordHelper.Md5String(pwdText);
            string userSecretkey = UserHelper.GenUserSecretkey();
            string encryptedPassword = PasswordHelper.Encrypt(md5Pass, userSecretkey);

            this.DbContext.DoWithTransaction(() =>
            {
                this.DbContext.Update<Users>(a => a.Id == userId, a => new Users() { UserSecretkey = userSecretkey, UserPass = encryptedPassword });
            });
        }


        public List<Users> GetList(int ShopID=0, int RoleID = 0, string keyword = "")
        {
            var q = this.Query;

            if (ShopID > 0)
            {
                q = q.Where(a => a.RoleID== ShopID);
            }

            if (RoleID > 0)
            {
                q = q.Where(a => a.RoleID == RoleID);
            }

            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.Mobile.Contains(keyword));
            }

            var ret = q.ToList();
            return ret;
        }

        void Trim(AddUsersInput input)
        {
            if (input.UserName.IsNotNullOrEmpty())
                input.UserName = input.UserName.Trim();
            if (input.Mobile.IsNotNullOrEmpty())
                input.Mobile = input.Mobile.Trim();
            if (input.Email.IsNotNullOrEmpty())
                input.Email = input.Email.Trim();
        }

        public Users Add(AddUsersInput input)
        {
            //this.InsertFromDto(input);
            this.Trim(input);

            input.Validate();

            //if (input.UserName.IsNullOrEmpty() && input.Mobile.IsNullOrEmpty() && input.Email.IsNullOrEmpty())
            //{
            //    throw new InvalidInputException("用户名/手机号码/邮箱至少填一个");
            //}

            //string userName = null;
            //if (input.UserName.IsNotNullOrEmpty())
            //{
            //    userName = input.UserName.ToLower();
            //    AceUtils.EnsureAccountNameLegal(userName);
            //    bool exists = this.DbContext.Query<Users>().Where(a => a.UserName == userName).Any();
            //    if (exists)
            //        throw new InvalidInputException("用户名[{0}]已存在".ToFormat(input.UserName));
            //}

            //string mobilePhone = null;
            //if (input.Mobile.IsNotNullOrEmpty())
            //{
            //    mobilePhone = input.Mobile;
            //    if (AceUtils.IsMobilePhone(mobilePhone) == false)
            //        throw new InvalidInputException("请输入正确的手机号码");

            //    bool exists = this.DbContext.Query<Users>().Where(a => a.Mobile == mobilePhone).Any();
            //    if (exists)
            //        throw new InvalidInputException("手机号码[{0}]已存在".ToFormat(mobilePhone));
            //}

            //string email = null;
            //if (input.Email.IsNotNullOrEmpty())
            //{
            //    email = input.Email.ToLower();
            //    if (AceUtils.IsEmail(email) == false)
            //        throw new InvalidInputException("请输入正确的邮箱地址");

            //    bool exists = this.DbContext.Query<Users>().Where(a => a.Email == email).Any();
            //    if (exists)
            //        throw new InvalidInputException("邮箱地址[{0}]已存在".ToFormat(input.Email));
            //}
            string userSecretkey = UserHelper.GenUserSecretkey();
            string encryptedPassword = PasswordHelper.Encrypt(input.UserPass, userSecretkey);

            Users user = new Users ();
            user.CreateDate = DateTime.Now;
            user.Email = input.Email;
            user.FromID =input.FromID;
            user.LastLoginDate = DateTime.Now;
            user.Mobile = input.Mobile;
            user.RoleID = 0;
            user.Sex = 0;
            user.ST = 0;
            user.UserName = input.UserName;
            user.UserPass = encryptedPassword;
            user.UserSecretkey = userSecretkey;
            user.OpenID = input.OpenID;
            user.ShopID = input.ShopID;
            user.UserIcon = input.UserIcon;
            user.Id= IdHelper.CreateStringSnowflakeId();
            return this.DbContext.Insert(user);
        }
        public void Update(UpdateUsersInput input)
        {
            this.UpdateFromDto(input);
        }

        public Users GetModelByOpenID(string OpenID)
        {
            return  this.Query.Where(a => a.OpenID == OpenID).FirstOrDefault();
        }

        public Users GetModel(string Id)
        {
            return this.Query.Where(a => a.Id == Id).FirstOrDefault();
        }

        public void Delete(string id)
        {
            this.DbContext.Delete<Users>(a => a.Id == id);
        }


        public PagedData<Users> GetPageData(Pagination page,int ShopID=0, int RoleID = 0, string keyword="")
        {
            var q = this.DbContext.Query<Users>();

            if (ShopID > 0)
            {
                q = q.Where(a => a.RoleID == ShopID);
            }

            if (RoleID > 0)
            {
                q = q.Where(a => a.RoleID == RoleID);
            }

            q = q.WhereIfNotNullOrEmpty(keyword, a => a.Mobile.Contains(keyword) );
              
            PagedData<Users> pagedData = q.TakePageData(page);
            
            return pagedData;
        }


        //----前台用户功能---------------------------------------------------------------------------------------------

        public bool CheckLogin(string loginName, string password, out Users user, out string msg)
        {
            user = null;
            msg = null;

            loginName.NotNullOrEmpty();
            password.NotNullOrEmpty();

            var view = this.DbContext.Query<Users>().Where(a => a.Mobile == loginName);                        

            view = view.Where(a => a.ST != (int)AccountState.Closed);

            var viewEntity = view.FirstOrDefault();

            if (viewEntity == null)
            {
                msg = "账户不存在，请重新输入";
                return false;
            }
             
            if (viewEntity.ST == (int)AccountState.Disabled)
            {
                msg = "账户被禁用，请联系管理员";
                return false;
            }
           
            string dbPassword = PasswordHelper.Encrypt(password, viewEntity.UserSecretkey);
            if (dbPassword != viewEntity.UserPass)
            {
                msg = "密码不正确，请重新输入";
                return false;
            }

            DateTime lastVisitTime = DateTime.Now;
            this.DbContext.Update<Users>(a => a.Id == viewEntity.Id, a => new Users() { LastLoginDate = lastVisitTime });
            user = viewEntity;
            return true;
        }

        public bool CheckMobile(string Mobile)
        {
            return this.Query.Any(a => a.Mobile == Mobile);
        }

        public bool CheckOpenID(string OpenID)
        {
            return this.Query.Any(a => a.OpenID == OpenID);
        }

        public void UpdateUserInfo(string OpenID,string Mobile,string UserIcon,string UserName)
        {
            this.DbContext.Update<Users>(a => a.OpenID == OpenID, a => new Users()
            {
                Mobile = Mobile,
                UserIcon = UserIcon,
                UserName= UserName
            });
        }


    }

}
