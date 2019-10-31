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
            var q = this.DbContext.Query<Product_rcd>()
                .LeftJoin<Product>((col, product) => col.ProductID == product.Id);

            IQuery<Product_rcdInfo> db_set = q.Select<Product_rcdInfo>((col, product) => new Product_rcdInfo
            {
                Id = col.Id,
                ProductID = col.ProductID,
                ProductName = product.ProductName,
                Summary=product.Summary,
                Hit=col.Hit,
                Price=product.Price,
                ImageUrl = product.ImageUrl,
                CreateDate = col.CreateDate,
                UpdateTime=col.UpdateTime,
                UserID = col.UserID
            }).Where(a => a.UserID == UserID);
            PagedData<Product_rcdInfo> pagedData = db_set.TakePageData(page);
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
