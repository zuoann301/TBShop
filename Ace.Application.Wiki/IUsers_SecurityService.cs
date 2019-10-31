using Ace.Application;
using Ace.Data;
using Ace.Entity.Wiki;
using Ace.Exceptions;
using Ace.IdStrategy;
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
    public interface IUsers_SecurityService : IAppService
    {
         
        void Add(AddUsers_SecurityInput input);
        void Update(UpdateUsers_SecurityInput input);
        void Delete(string id );

        Users_Security GetModel(string Id);

        PagedData<Users_SecurityInfo> GetPageData(Pagination page, string UserID);
    }

    public class Users_SecurityService : AppServiceBase<Users_Security>, IUsers_SecurityService
    {
        public Users_SecurityService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

         
        public void Add(AddUsers_SecurityInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdateUsers_SecurityInput input)
        {
            this.UpdateFromDto(input);
        }

        public Users_Security GetModel(string Id)
        {
            return  this.Query.Where(a => a.Id == Id).FirstOrDefault();
        }

        public void Delete(string id)
        {
            this.DbContext.Delete<Users_Security>(a => a.Id == id);
        }


        public PagedData<Users_SecurityInfo> GetPageData(Pagination page,string UserID)
        { 
            var q = this.DbContext.Query<Users_Security>()
                .LeftJoin<Users>((col, u) => col.UserID == u.Id);

            IQuery<Users_SecurityInfo> db_set = q.Select<Users_SecurityInfo>((col, u) => new Users_SecurityInfo
            {
                Id = col.Id,
                UserID = col.UserID,
                GPS_X=col.GPS_X,
                GPS_Y=col.GPS_Y,
                UserName=u.UserName,
                UserIcon=u.UserIcon,
                CreateDate=col.CreateDate
            }).Where(a => a.UserID == UserID);
            PagedData<Users_SecurityInfo> pagedData = db_set.TakePageData(page);
            return pagedData;

        }

         



    }

}
