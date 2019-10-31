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
    public interface IDistrictService : IAppService
    {
        List<District> GetList(int Pid = 0, string keyword = "");
        void Add(AddDistrictInput input);
        void Update(UpdateDistrictInput input);
        void Delete(int ID);

        District GetModel(int ID);

        PagedData<District> GetPageData(Pagination page, int Pid, string keyword);
    }

    public class DistrictService : AppServiceBase<District>, IDistrictService
    {
        public DistrictService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<District> GetList(int Pid = 0, string keyword = "")
        {
            var q = this.Query;

            if (Pid > 0)
            {
                q = q.Where(a => a.Pid== Pid);
            }

            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.Name.Contains(keyword));
            }

            var ret = q.ToList();
            return ret;
        }
        public void Add(AddDistrictInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdateDistrictInput input)
        {
            this.UpdateFromDto(input);
        }

        public District GetModel(int ID)
        {
            return  this.Query.Where(a => a.ID == ID).FirstOrDefault();
        }

        public void Delete(int ID)
        {
            this.DbContext.Delete<District>(a => a.ID == ID);
        }


        public PagedData<District> GetPageData(Pagination page,int Pid = 0, string keyword="")
        {
            var q = this.DbContext.Query<District>();

            q = q.WhereIfNotNullOrEmpty(keyword, a => a.Pid == Pid);

            q = q.WhereIfNotNullOrEmpty(keyword, a => a.Name.Contains(keyword) );

            

           

            PagedData<District> pagedData = q.TakePageData(page);
            
            return pagedData;
        }

         



    }

}
