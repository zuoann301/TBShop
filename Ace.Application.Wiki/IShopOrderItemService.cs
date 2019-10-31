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
    public interface IShopOrderItemService : IAppService
    {
        List<ShopOrderItem> GetList(string OrderID);
        void Add(AddShopOrderItemInput input);
        void AddRange<ShopOrderItem>(List<ShopOrderItem> entities, bool keepIdentity = false);

        void Update(UpdateShopOrderItemInput input);
        void Delete(string id );

        ShopOrderItem GetModel(string Id);

        PagedData<ShopOrderItem> GetPageData(Pagination page,string OrderID);

        List<ShopOrderItemInfo> GetOrderItemList(string OrderID);
    }

    public class ShopOrderItemService : AppServiceBase<ShopOrderItem>, IShopOrderItemService
    {
        public ShopOrderItemService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<ShopOrderItem> GetList(string OrderID)
        {
            var q = this.Query;
            q = q.Where(a => a.OrderID == OrderID);            
            var ret = q.ToList();
            return ret;
        }
        public void Add(AddShopOrderItemInput input)
        {
            this.InsertFromDto(input);
        }

        public void AddRange<ShopOrderItem>(List<ShopOrderItem> entities, bool keepIdentity = false)
        {
            this.DbContext.InsertRange<ShopOrderItem>(entities, keepIdentity); 
        }

        public void Update(UpdateShopOrderItemInput input)
        {
            this.UpdateFromDto(input);
        }

        public ShopOrderItem GetModel(string Id)
        {
            return  this.Query.Where(a => a.Id == Id).FirstOrDefault();
        }

        public void Delete(string id)
        {
            this.DbContext.Delete<ShopOrderItem>(a => a.Id == id);
        }


        public PagedData<ShopOrderItem> GetPageData(Pagination page, string OrderID = "")
        {
            var q = this.DbContext.Query<ShopOrderItem>(); 
            q = q.WhereIfNotNullOrEmpty(OrderID, a => a.OrderID == OrderID);
            PagedData<ShopOrderItem> pagedData = q.TakePageData(page);            
            return pagedData;
        }



        public List<ShopOrderItemInfo> GetOrderItemList( string OrderID)
        { 
            string sql = " select a.Id,a.OrderID,a.ProductID,a.ProSizeID,a.ItemNum,a.Price,b.ProductName,c.ProSize,c.ImageUrl from ShopOrderItem a ";
            sql += " left join Product b on a.ProductID=b.Id  ";
            sql += "  left join Product_Size c on a.ProSizeID=c.Id ";
           
            sql += " where a.OrderID=?OrderID";

            DbParam[] dbParams = new DbParam[] {
                new DbParam("?OrderID",OrderID)
            };

            List<ShopOrderItemInfo> db_set = this.DbContext.SqlQuery<ShopOrderItemInfo>(sql, dbParams).ToList();

            return db_set;
        }










    }

}
