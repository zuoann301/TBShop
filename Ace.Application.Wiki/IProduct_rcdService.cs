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
    public interface IProduct_rcdService : IAppService
    { 
        void Add(AddProduct_rcdInput input);
        void Update(UpdateProduct_rcdInput input);
        void Delete(string id );

        Product_rcd GetModel(string Id);
        Product_rcd GetModel(string UserID, string ProductID);
        void UpdateHit(string Id, int Hit);

        PagedData<Product_rcd> GetPageData(Pagination page, string UserID);
        PagedData<Product_rcdInfo> GetPageList(Pagination page, string UserID);
    }

    public class Product_rcdService : AppServiceBase<Product_rcd>, IProduct_rcdService
    {
        public Product_rcdService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

         
        public void Add(AddProduct_rcdInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdateProduct_rcdInput input)
        {
            this.UpdateFromDto(input);
        }

        public Product_rcd GetModel(string Id)
        {
            return  this.Query.Where(a => a.Id == Id).FirstOrDefault();
        }

        public Product_rcd GetModel(string UserID,string ProductID)
        {
            return this.Query.Where(a => a.UserID == UserID && a.ProductID == ProductID ).FirstOrDefault();
        }

        public void Delete(string id)
        {
            this.DbContext.Delete<Product_rcd>(a => a.Id == id);
        }


        public PagedData<Product_rcd> GetPageData(Pagination page,string UserID)
        {
            var q = this.DbContext.Query<Product_rcd>();
            q = q.WhereIfNotNullOrEmpty(UserID, a => a.UserID== UserID);            
            PagedData<Product_rcd> pagedData = q.TakePageData(page);
            return pagedData;
        }



        

        public PagedData<Product_rcdInfo> GetPageList(Pagination page, string UserID)
        {
            string strFileds = " a.*,b.ProductName,b.ImageUrl,b.Price,c.ShopName ";

            string strWhere = " 1=1 ";//b.GPS_X>0
            

            if (!string.IsNullOrEmpty(UserID))
            {
                strWhere += " and  a.UserID='" + UserID + "'";
            }
             
            DbParam _totalcount = new DbParam("?_totalcount", null, typeof(int)) { Direction = ParamDirection.Output };
            DbParam _pagecount = new DbParam("?_pagecount", null, typeof(int)) { Direction = ParamDirection.Output };

            DbParam[] dbs = new DbParam[] {
                new DbParam("?_fields", strFileds),
                new DbParam("?_tables", "  product_rcd a left join product b on a.ProductID=b.Id left join shop c on a.ShopID=c.ID "),
                new DbParam("?_where",strWhere),
                new DbParam("?_orderby", "a.UpdateTime desc"),
                new DbParam("?_pageindex", page.Page),
                new DbParam("?_pageSize", page.PageSize),
                _totalcount,
                _pagecount
        };

            List<Product_rcdInfo> list = this.DbContext.SqlQuery<Product_rcdInfo>("sp_viewPage", System.Data.CommandType.StoredProcedure, dbs).ToList();

            PagedData<Product_rcdInfo> pagedData = new PagedData<Product_rcdInfo>();
            pagedData.CurrentPage = page.Page;
            pagedData.Models = list;
            pagedData.PageSize = page.PageSize;
            pagedData.TotalCount = Convert.ToInt32(_totalcount.Value);
            pagedData.TotalPage = Convert.ToInt32(_pagecount.Value);

            return pagedData;
        }


        public void UpdateHit(string Id,int Hit)
        {
            this.DbContext.Update<Product_rcd>(a => a.Id == Id, a => new Product_rcd()
            {
                Hit = Hit,
                UpdateTime = DateTime.Now
            });
        }


    }

}
