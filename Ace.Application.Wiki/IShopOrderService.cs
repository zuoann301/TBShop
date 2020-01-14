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
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Application.Wiki
{
    public interface IShopOrderService : IAppService
    {
        List<ShopOrder> GetList( string CreateID);
        string Add(AddShopOrderInput input);
        void Update(UpdateShopOrderInput input);
        void Delete(string id );

        ShopOrder GetModel(string Id);

        PagedData<ShopOrderInfo2> GetPageData(Pagination page,   string CreateID);


        List<ShopOrderItemInfo> GetOrderItemList(string OrderID);



        PagedData<ShopOrderInfo> GetPageOrderList(Pagination page, int ShopID = 0, string CreateID = "", int ST = -1);

        string SubmitOrder(  string CreateID, string AddressID,int ShopID);
        string GetST_Name(int ST);

        int UpdateOrderStatus(string Id, int ST);
    }

    public class ShopOrderService : AppServiceBase<ShopOrder>, IShopOrderService
    {
        public ShopOrderService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<ShopOrder> GetList(string CreateID)
        {
            var q = this.Query;
             
            q = q.WhereIfNotNullOrEmpty(CreateID, a => a.CreateID == CreateID);
            var ret = q.ToList();
            return ret;
        }
        public string Add(AddShopOrderInput input)
        {
            ShopOrder shopOrder=  this.InsertFromDto(input);
            return shopOrder.Id;
        }
        public void Update(UpdateShopOrderInput input)
        {
            this.UpdateFromDto(input);
        }

        public ShopOrder GetModel(string Id)
        {
            return  this.Query.Where(a => a.Id == Id).FirstOrDefault();
        }

        public void Delete(string id)
        {
            this.DbContext.Delete<ShopOrder>(a => a.Id == id);
        }


        public PagedData<ShopOrderInfo2> GetPageData(Pagination page,string CreateID )
        {
            var q = this.DbContext.Query<ShopOrder>();
             
            q = q.WhereIfNotNullOrEmpty(CreateID, a => a.CreateID == CreateID);
            PagedData<ShopOrder> pagedData = q.TakePageData(page);

            List<ShopOrderInfo2> list = new List<ShopOrderInfo2>();
            foreach(var item in pagedData.Models)
            {
                ShopOrderInfo2 m = new ShopOrderInfo2();
                m.AddressID = item.AddressID;
                m.CreateDate = item.CreateDate;
                m.CreateID = item.CreateID;
                m.Freight = item.Freight;
                m.Id = item.Id;
                m.OrderCode = item.OrderCode;
                m.OrderItem = GetOrderItemList(item.Id);
                m.ProTotal = item.ProTotal;
                m.ST = item.ST;
                m.STName = GetST_Name(m.ST);
                m.Total = item.Total;
                m.UpdateTime = item.UpdateTime;
                list.Add(m);
            }

            PagedData<ShopOrderInfo2> pg = new PagedData<ShopOrderInfo2>();
            pg.CurrentPage = pagedData.CurrentPage;
            pg.Models = list;
            pg.PageSize = pagedData.PageSize;
            pg.TotalCount = pagedData.TotalCount;
            pg.TotalPage = pagedData.TotalPage;
            

            return pg;
        }


        public List<ShopOrderItemInfo> GetOrderItemList(string OrderID)
        {
            string sql = " select a.Id,a.OrderID,a.ProductID,a.ProSizeID,a.ItemNum,a.Price,b.ProductName,b.ProSize,b.ImageUrl from ShopOrderItem a ";
            sql += " left join Product b on a.ProductID=b.Id  ";
            //sql += "  left join Product_Size c on a.ProSizeID=c.Id ";

            sql += " where a.OrderID=?OrderID";

            DbParam[] dbParams = new DbParam[] {
                new DbParam("?OrderID",OrderID)
            };

            List<ShopOrderItemInfo> db_set = this.DbContext.SqlQuery<ShopOrderItemInfo>(sql, dbParams).ToList();

            return db_set;
        }

        


 


        public PagedData<ShopOrderInfo> GetPageOrderList(Pagination page, int ShopID = 0, string CreateID="",int ST=-1)
        {
            string strFileds = " a.Id,a.OrderCode,a.Total,a.ProTotal,a.Freight,a.ST,a.CreateID,a.CreateDate,a.UpdateTime,a.AddressID,a.ShopID,b.UserName,b.Mobile,c.ShopName ";

            string strWhere = " a.ST>-1 ";//b.GPS_X>0
            if (ShopID > 0)
            {
                strWhere += " and a.ShopID=" + ShopID;
            }

            if (!string.IsNullOrEmpty(CreateID))
            {
                strWhere += " and  a.CreateID='" + CreateID + "'";
            }
            if(ST>-1)
            {
                strWhere += " and a.ST=" + ST;
            }


            DbParam _totalcount = new DbParam("?_totalcount", null, typeof(int)) { Direction = ParamDirection.Output };
            DbParam _pagecount = new DbParam("?_pagecount", null, typeof(int)) { Direction = ParamDirection.Output };

            DbParam[] dbs = new DbParam[] {
                new DbParam("?_fields", strFileds),
                new DbParam("?_tables", "  shoporder a LEFT JOIN users b ON a.CreateID=b.Id LEFT JOIN shop c ON a.ShopID=c.ID "),
                new DbParam("?_where",strWhere),
                new DbParam("?_orderby", "a.CreateDate desc"),
                new DbParam("?_pageindex", page.Page),
                new DbParam("?_pageSize", page.PageSize),
                _totalcount,
                _pagecount
        };

            List<ShopOrderInfo> list = this.DbContext.SqlQuery<ShopOrderInfo>("sp_viewPage", System.Data.CommandType.StoredProcedure, dbs).ToList();

            PagedData<ShopOrderInfo> pagedData = new PagedData<ShopOrderInfo>();
            pagedData.CurrentPage = page.Page;
            pagedData.Models = list;
            pagedData.PageSize = page.PageSize;
            pagedData.TotalCount = Convert.ToInt32(_totalcount.Value);
            pagedData.TotalPage = Convert.ToInt32(_pagecount.Value);

            return pagedData;
        }


        public string SubmitOrder(string CreateID,string AddressID,int ShopID)
        {  
            DbParam id = new DbParam("?_createid", CreateID);
            DbParam aid = new DbParam("?_addressid", AddressID);
            DbParam shopid = new DbParam("?_shopid", ShopID);
            DbParam outputName = new DbParam("?_re","", typeof(string)) { Direction = ParamDirection.Output };
            this.DbContext.Session.ExecuteNonQuery("SubmitOrder", CommandType.StoredProcedure, id, aid, shopid, outputName);

            return  outputName.Value.ToString();
        }

        public string GetST_Name(int ST)
        {
            string s = string.Empty;
            switch (ST)
            {
                case 0:
                    s = "待付款";
                    break;

                case 1:
                    s = "已付款";
                    break;

                case 2:
                    s = "已发货";
                    break;

                case 3:
                    s = "已收货";
                    break;

                case 4:
                    s = "订单结束";
                    break;

                case 10:
                    s = "取消订单";
                    break;

            }
            return s;
        }



        public int UpdateOrderStatus(string Id, int ST)
        {
           int n=  this.DbContext.Update<ShopOrder>(a => a.Id == Id, a => new ShopOrder()
            {
                ST = ST,
                UpdateTime = DateTime.Now
            });

            return n;
        }




    }

}
