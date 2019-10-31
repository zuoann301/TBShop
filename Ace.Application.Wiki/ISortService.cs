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
    public interface ISortService : IAppService
    {
        List<Sort> GetList(int Pid = 0, string keyword = "");
        void Add(AddSortInput input);
        void Update(UpdateSortInput input);
        void Delete(int Id);

        int GetProSortID(string Name = "");

        List<Sort> GetAllList();
    }

    public class SortService : AppServiceBase<Sort>, ISortService
    {
        public SortService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<Sort> GetList(int Pid =0, string keyword = "")
        {
            var q = this.Query.Where(a=>a.Pid==Pid);           

            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.Title.Contains(keyword));
            }

            var ret = q.ToList();
            return ret;
        }


        public List<Sort> GetAllList()
        {
            var q = this.Query.ToList();
            return q;
        }

        public void Add(AddSortInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdateSortInput input)
        {
            this.UpdateFromDto(input);
        }

        public void Delete(int id)
        {
            this.DbContext.Delete<Sort>(a => a.Id == id);
        }


        public int GetProSortID(string Name = "")
        {
            int val =0;
            Sort modle = this.Query.Where(a => a.Title == Name).FirstOrDefault();// list.Where(a => a.Name == Name).Take(1).FirstOrDefault();
            if (modle != null)
            {
                val = modle.Id;
            }
            return val;
        }

    }

}
