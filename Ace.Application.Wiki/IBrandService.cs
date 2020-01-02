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
    public interface IBrandService : IAppService
    {
        List<Brand> GetList(int IsTop = 0, string keyword = "");

        List<Brand> GetShopBrandList(int ShopID, int IsTop = 0, string keyword = "");

        void Add(AddBrandInput input);
        void Update(UpdateBrandInput input);
        void Delete(string id );

        Brand GetModel(string Id);

        string GetBrandID(string Name = "");

        PagedData<Brand> GetPageData(Pagination page, int IsTop, string keyword, int ShopID = 0);
    }

    public class BrandService : AppServiceBase<Brand>, IBrandService
    {
        public BrandService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<Brand> GetList(int IsTop=0, string keyword = "")
        {
            var q = this.Query;

            if (IsTop > 0)
            {
                q = q.Where(a => a.IsTop == IsTop);
            }

            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.Title.Contains(keyword));
            }

            var ret = q.ToList();
            return ret;
        }

        public List<Brand> GetShopBrandList(int ShopID, int IsTop = 0, string keyword = "")
        {
            var q = this.Query;
            if (ShopID > 0)
            {
                q = q.Where(a => a.ShopID == ShopID);
            }
            if (IsTop > 0)
            {
                q = q.Where(a => a.IsTop == IsTop);
            }

            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.Title.Contains(keyword));
            }

            var ret = q.ToList();
            return ret;
        }

        public void Add(AddBrandInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdateBrandInput input)
        {
            this.UpdateFromDto(input);
        }

        public Brand GetModel(string Id)
        {
            return  this.Query.Where(a => a.Id == Id).FirstOrDefault();
        }

        public void Delete(string id)
        {
            this.DbContext.Delete<Brand>(a => a.Id == id);
        }


        public PagedData<Brand> GetPageData(Pagination page,int IsTop = 0, string keyword="",int ShopID=0)
        {
            var q = this.DbContext.Query<Brand>();
            q = q.WhereIfNotNullOrEmpty(keyword, a => a.Title.Contains(keyword) );
            if(IsTop > 0)
            {
                q = q.Where(a => a.IsTop == IsTop);
            }
            if(ShopID>0)
            {
                q = q.Where(a => a.ShopID == ShopID);
            }
            PagedData<Brand> pagedData = q.TakePageData(page);
            return pagedData;
        }


        public string GetBrandID(string Name = "")
        {
            string val = string.Empty;
            Brand modle = this.Query.Where(a => a.Title == Name).FirstOrDefault();// list.Where(a => a.Name == Name).Take(1).FirstOrDefault();
            if (modle != null)
            {
                val = modle.Id;
            }
            return val;
        }


    }

}
