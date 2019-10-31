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
    public interface IPro_CollectionService : IAppService
    {
        List<Pro_CollectionInfo> GetList(string UserID);
        void Add(AddPro_CollectionInput input);
        void Delete(string id );
        void Delete(string UserID, string ProductID);

        int Count(string UserID, string ProductID);

        PagedData<Pro_CollectionInfo> GetPageData(Pagination page, string UserID);
    }

    public class Pro_CollectionService : AppServiceBase<Pro_Collection>, IPro_CollectionService
    {
        public Pro_CollectionService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<Pro_CollectionInfo> GetList(string UserID)
        {
            var db_conn = this.DbContext.Query<Pro_Collection>()
                .LeftJoin<Product>((col, product) => col.ProductID == product.Id);

            List<Pro_CollectionInfo> db_set = db_conn.Select<Pro_CollectionInfo>((col, product) => new Pro_CollectionInfo
            {
                Id = col.Id,
                ProductID = col.ProductID,
                ProductName = product.ProductName,
                ProductCode = product.ProductCode,
                Price = product.Price,               
                ImageUrl = product.ImageUrl,
                CreateDate=col.CreateDate,
                UserID=col.UserID,
                Summary=product.Summary
            }).Where(a => a.UserID == UserID).ToList();
            
            return db_set;
        }

        public PagedData<Pro_CollectionInfo> GetPageData(Pagination page, string UserID)
        {
            //var q = this.DbContext.Query<Pro_Collection>();
            //q = q.WhereIfNotNullOrEmpty(UserID, a => a.UserID.Contains(UserID));            
            //PagedData<Pro_Collection> pagedData = q.TakePageData(page);
            //return pagedData;
            var q = this.DbContext.Query<Pro_Collection>()
                .LeftJoin<Product>((col, product) => col.ProductID == product.Id);

            IQuery<Pro_CollectionInfo> db_set = q.Select<Pro_CollectionInfo>((col, product) => new Pro_CollectionInfo
            {
                Id = col.Id,
                ProductID = col.ProductID,
                ProductName = product.ProductName,
                ProductCode = product.ProductCode,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CreateDate = col.CreateDate,
                UserID = col.UserID
            }).Where(a => a.UserID == UserID); 
            PagedData<Pro_CollectionInfo> pagedData = db_set.TakePageData(page);
            return pagedData;
        }


        public void Add(AddPro_CollectionInput input)
        {
            this.InsertFromDto(input);
        }
         

        public void Delete(string id)
        {
            this.DbContext.Delete<Pro_Collection>(a => a.Id == id);
        }

        public void Delete(string UserID, string ProductID)
        {
            this.DbContext.Delete<Pro_Collection>(a => a.UserID == UserID && a.ProductID==ProductID);
        }

        public int Count(string UserID,string ProductID)
        {
            int count= this.Query.Where(a => a.UserID == UserID && a.ProductID==ProductID).Count();
            return count;
        }


       



    }

}
