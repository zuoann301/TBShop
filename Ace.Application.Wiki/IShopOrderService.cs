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

        List<ShopOrderInfo> GetOrderList( string CreateID);
        PagedData<ShopOrderInfo> GetPageOrderList(Pagination page, string CreateID); 

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
                m.AuthID = item.AuthID;
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

        public List<ShopOrderInfo> GetOrderList( string CreateID)
        {

            //var db_conn = this.DbContext.Query<ShopOrder>()
            //             .LeftJoin<Users>((order, users) => order.CreateID == users.Id)
            //             .LeftJoin<Users>((order, users, users2) => order.AuthID == users2.Id)
            //             .LeftJoin<Shop>((order, users, users2, shop) => order.ShopID == shop.ID);

            //List<ShopOrderInfo> db_set = db_conn.Select<ShopOrderInfo>((order, users, users2, shop) => new ShopOrderInfo
            //{
            //    Id = order.Id,
            //    Total = order.Total,
            //    CreateName = users.UserName,
            //    ShopName = shop.ShopName,
            //    CreateDate = order.CreateDate,
            //    AuthName = users2.UserName,
            //    UpdateTime = order.UpdateTime,
            //    ST = order.ST
            //}).Where(a => a.ShopID == ShopID && a.CreateID == CreateID).ToList();

            string sql = "select a.Id,a.Total,a.CreateID,a.ShopID,a.CreateDate,a.UpdateTime,a.ST,b.ShopName,u1.UserName as CreateName,u2.UserName as AuthName ";
            sql += " from  ShopOrder a  ";
            sql += " left join Shop b on a.ShopID=b.ID ";
            sql += " left join Users u1 on a.CreateID=u1.Id ";
            sql += " left join Users u2 on a.AuthID= u2.Id ";
            sql += " where a.CreateID=?CreateID";

            DbParam[] dbParams = new DbParam[] 
            {
                new DbParam("?CreateID",CreateID)
            };

            List<ShopOrderInfo> db_set=this.DbContext.SqlQuery<ShopOrderInfo>(sql, dbParams).ToList();

            return db_set;
        }



        public PagedData<ShopOrderInfo> GetPageOrderList(Pagination page,   string CreateID)
        {
            var q = this.DbContext.Query<ShopOrder>();
             
            q = q.WhereIfNotNullOrEmpty(CreateID, a => a.CreateID == CreateID).OrderByDesc(a=>a.CreateDate);
            PagedData<ShopOrder> pagedData = q.TakePageData(page);

            List<string> createIds= pagedData.Models.Select(a => a.CreateID).Distinct().ToList();
            List<string> authIds = pagedData.Models.Select(a => a.AuthID).Distinct().ToList();

            List<string> ids = new List<string>();
            ids.AddRange(createIds);
            ids.AddRange(authIds);
            ids = ids.Distinct().ToList();

            //List<int> shopIds = pagedData.Models.Select(a => a.ShopID).Distinct().ToList();

            //List<SimpleShop2> shops = this.DbContext.Query<Shop>().Select(a => new SimpleShop2() { ShopID = a.ID, ShopName = a.ShopName }).Where(a => a.ShopID.In(shopIds)).ToList();
            List<UsersInfo2> users = this.DbContext.Query<Users>().Select(a => new UsersInfo2() { Id = a.Id, UserName = a.UserName }).Where(a => a.Id.In(ids)).ToList();

            List<ShopOrderInfo> _list = new List<ShopOrderInfo>();
            ShopOrderInfo _newOrder;
            foreach (ShopOrder order in pagedData.Models)
            {
                _newOrder = new ShopOrderInfo();
                _newOrder.AuthName = !string.IsNullOrEmpty(order.AuthID)?  users.Where(a => a.Id == order.AuthID).FirstOrDefault().UserName:"";
                _newOrder.CreateName = !string.IsNullOrEmpty(order.CreateID) ? users.Where(a => a.Id == order.CreateID).FirstOrDefault().UserName:"";
                _newOrder.CreateDate = order.CreateDate;
                _newOrder.CreateID = order.CreateID;
                _newOrder.Id = order.Id;
                //_newOrder.ShopID = order.ShopID;
                //_newOrder.ShopName = shops.Where(a => a.ShopID == order.ShopID).FirstOrDefault().ShopName;
                _newOrder.ST = order.ST;
                _newOrder.ST_Name = GetST_Name(order.ST);
                _newOrder.Total = order.Total;
                _newOrder.UpdateTime = order.UpdateTime;
                _list.Add(_newOrder);
            }
            PagedData<ShopOrderInfo> pagedDataInfo = new PagedData<ShopOrderInfo>(_list, pagedData.TotalCount, page.Page, page.PageSize);
            return pagedDataInfo;
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
                    s = "已付款，待发货";
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
