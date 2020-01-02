using Ace.Application;
using Ace.Data;
using Ace.Entity.Wiki;
using Ace.Exceptions;
using Ace.IdStrategy;
using Ace.Web.Helpers;
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
    public interface IShopService : IAppService
    {
        List<Shop> GetList(int BrandID = 0, string keyword = "");

        List<ShopSample> GetShopList(decimal GPS_X, decimal GPS_Y);

        void Add(AddShopInput input);
        void Update(UpdateShopInput input);
        void Delete(int id );

        Shop GetModel(int Id);

        PagedData<Shop> GetPageData(Pagination page, int Brand, string keyword);

        List<SimpleShop> GetCacheList();

        List<SimpleShop2> GetCacheList2();
    }

    public class ShopService : AppServiceBase<Shop>, IShopService
    {
        public ShopService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }


        public List<SimpleShop> GetCacheList()
        {

            /*
            List<SimpleShop> list = new List<SimpleShop>();
            
            MemoryCacheHelper mc = new MemoryCacheHelper();
            object obj = mc.Get(Ace.Globals.CacheShop);
            if (obj != null)
            {
                list = obj as List<SimpleShop>;
            }
            else
            {
                list = this.DbContext.Query<Shop>().Select(a => new SimpleShop() { ShopID = a.ID, ShopUrl = a.ShopUrl }).ToList(); 
                mc.Set(Ace.Globals.CacheShop, list, System.TimeSpan.FromMinutes(30), true);//缓存30分钟 自动延长
            }
            */
            List<SimpleShop> list = this.DbContext.Query<Shop>().Select(a => new SimpleShop() { ShopID = a.ID, ShopUrl = a.ShopUrl }).ToList();
            return list;
        }

        public List<SimpleShop2> GetCacheList2()
        {
            List<SimpleShop2> list = new List<SimpleShop2>();
            MemoryCacheHelper mc = new MemoryCacheHelper();
            object obj = mc.Get(Ace.Globals.CacheShop2);
            if (obj != null)
            {
                list = obj as List<SimpleShop2>;
            }
            else
            {
                list = this.DbContext.Query<Shop>().Select(a => new SimpleShop2() { ShopID = a.ID, ShopName = a.ShopName }).ToList();
                mc.Set(Ace.Globals.CacheShop2, list, System.TimeSpan.FromMinutes(30), true);//缓存30分钟 自动延长
            }
            return list;
        }


        public List<Shop> GetList(int BrandID = 0, string keyword = "")
        {
            var q = this.Query;

            if (BrandID > 0)
            {
                q = q.Where(a => a.BrandID == BrandID);
            }

            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.ShopName.Contains(keyword));
            }

            var ret = q.ToList();
            return ret;
        }

        public List<ShopSample> GetShopList(decimal GPS_X,decimal GPS_Y)
        {
            string sql = " select ID, ShopName, ShopAddress, ShopTel, ShopUrl, Distance, GPS_X, GPS_Y from ";
            sql += " ( ";
            sql += "  SELECT ID, ShopName, ShopAddress, ShopTel, ShopUrl, f_distance(?x,?y, GPS_X, GPS_Y) as Distance, GPS_X, GPS_Y from Shop ";
            sql += " ) aa order by Distance asc LIMIT 0,5 ";
            DbParam[] dbParams = new DbParam[] {
                new DbParam("?x",GPS_X),
                new DbParam("?y",GPS_Y)
            };
            List<ShopSample> list= this.DbContext.SqlQuery<ShopSample>(sql, dbParams).ToList();
            
            return list;
        }

        public void Add(AddShopInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdateShopInput input)
        {
            this.UpdateFromDto(input);
        }

        public Shop GetModel(int ID)
        {
            return  this.Query.Where(a => a.ID == ID).FirstOrDefault();
        }

        public void Delete(int ID)
        {
            this.DbContext.Delete<Shop>(a => a.ID == ID);
        }


        public PagedData<Shop> GetPageData(Pagination page,int BrandID, string keyword)
        {
            var q = this.DbContext.Query<Shop>();

            q = q.WhereIfNotNullOrEmpty(keyword, a => a.ShopName.Contains(keyword) );

            q = q.WhereIfNotNullOrEmpty(keyword, a => a.BrandID == BrandID);

           

            PagedData<Shop> pagedData = q.TakePageData(page);
            
            return pagedData;
        }

         



    }

}
