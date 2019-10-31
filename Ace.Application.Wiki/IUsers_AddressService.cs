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
    public interface IUsers_AddressService : IAppService
    {
        List<Users_Address> GetList(string CreateID);
        void Add(AddUsers_AddressInput input);
        void Update(UpdateUsers_AddressInput input);
        void Delete(string id );

        Users_Address GetModel(string Id);
        Users_Address GetDefaultAddress(string UserID);

        PagedData<Users_Address> GetPageData(Pagination page,string CreateID);
    }

    public class Users_AddressService : AppServiceBase<Users_Address>, IUsers_AddressService
    {
        public Users_AddressService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<Users_Address> GetList(string CreateID)
        {
            var q = this.Query.Where(a=>a.CreateID==CreateID).ToList();
              
            return q;
        }
        public void Add(AddUsers_AddressInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdateUsers_AddressInput input)
        {
            this.UpdateFromDto(input);
        }

        public Users_Address GetModel(string Id)
        {
            return  this.Query.Where(a => a.Id == Id).FirstOrDefault();
        }

        public void Delete(string id)
        {
            this.DbContext.Delete<Users_Address>(a => a.Id == id);
        }


        public PagedData<Users_Address> GetPageData(Pagination page, string CreateID)
        {
            var q = this.DbContext.Query<Users_Address>();
            q = q.WhereIfNotNullOrEmpty(CreateID, a => a.CreateID== CreateID);
            
            PagedData<Users_Address> pagedData = q.TakePageData(page);
            return pagedData;
        }

        public Users_Address GetDefaultAddress(string UserID)
        { 
            Users_Address modle= this.Query.Where(a => a.CreateID == UserID && a.IsDefault==1 ).FirstOrDefault();
            if(modle==null)
            {
                modle = this.Query.Where(a => a.CreateID == UserID).FirstOrDefault();
            }
            return modle;
        }




    }

}
