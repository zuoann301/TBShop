using Ace.Application;
using Ace.Entity.Wiki;
using Ace.Exceptions;
using Chloe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Application.Wiki
{
    public interface IPro_SortService : IAppService
    {
        List<Pro_Sort> GetList(string Pid = "", string keyword = "", int ShopID = 0);
        void Add(AddPro_SortInput input);
        void Update(UpdatePro_SortInput input);
        void Delete(string id);

        string GetProSortID(string Name = "");
        Pro_Sort GetModle(string Id);
        List<Pro_Sort> GetAllList();
    }

    public class Pro_SortService : AppServiceBase<Pro_Sort>, IPro_SortService
    {
        public Pro_SortService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<Pro_Sort> GetList(string Pid="", string keyword = "",int ShopID=0)
        {
            //var q = this.Query.Where(a=>a.Pid==Pid);   
            
            var q= this.Query.WhereIfNotNullOrEmpty(Pid, a => a.Pid== Pid);

            

            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.Title.Contains(keyword));
            }

            if(ShopID>0)
            {
                q = q.Where(a => a.ShopID == ShopID);
            }

            var ret = q.OrderBy(a => a.SortCode).ToList();
            return ret;
        }


        public Pro_Sort GetModle(string Id)
        {
            var q = this.Query.Where(a => a.Id == Id).FirstOrDefault();
            return q;
        }

        public List<Pro_Sort> GetAllList()
        {
            var q = this.Query.OrderBy(a => a.SortCode).ToList();            
            return q;
        }

        public void Add(AddPro_SortInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdatePro_SortInput input)
        {
            this.UpdateFromDto(input);
        }

        public void Delete(string id)
        {
            this.DbContext.Delete<Pro_Sort>(a => a.Id == id);
        }


        public string GetProSortID(string Name = "")
        {
            string val =string.Empty;
            Pro_Sort modle = this.Query.Where(a => a.Title == Name).FirstOrDefault();// list.Where(a => a.Name == Name).Take(1).FirstOrDefault();
            if (modle != null)
            {
                val = modle.Id;
            }
            return val;
        }

    }

}
