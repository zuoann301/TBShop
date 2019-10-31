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
    public interface ICartService : IAppService
    {
        List<Cart> GetList(string CreateID);
        void Add(AddCartInput input);
        void Update(UpdateCartInput input);
        void Delete(string id );

        void DelateAll(string CreateID);

        Cart GetModel(string Id);
        Cart GetCartItem(string ProductID, string ProSizeID, string CreateID);

        PagedData<Cart> GetPageData(Pagination page, string CreateID);

        int Count(string ProductID, string ProSizeID, string CreateID);
        int CountByUserID(string CreateID,int ShopID);

        List<CartInfo> GetCartListItem(string CreateID,int ShopID);
        List<CartInfo> GetTempOrderItem(string CreateID);

        void UpdateCart(string Id, int ItemNum);
        void CartChecked(string Id);
        void ClearTempOrder(string UserID);

        int SetCart(int ShopID, string ProductID, string ProSizeID, int ItemNum, string CreateID);
    }

    public class CartService : AppServiceBase<Cart>, ICartService
    {
        public CartService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<Cart> GetList(string CreateID)
        {
            var q = this.Query;
            if (CreateID.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.CreateID== CreateID);
            }
            var ret = q.ToList();
            return ret;
        }
        public void Add(AddCartInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdateCartInput input)
        {
            this.UpdateFromDto(input);
        }

        public Cart GetModel(string Id)
        {
            return  this.Query.Where(a => a.Id == Id).FirstOrDefault();
        }

        public void Delete(string id)
        {
            this.DbContext.Delete<Cart>(a => a.Id == id);
        }

        public void DelateAll(string CreateID)
        {
            this.DbContext.Delete<Cart>(a=>a.CreateID== CreateID);
        }



        public PagedData<Cart> GetPageData(Pagination page,string CreateID)
        {
            var q = this.DbContext.Query<Cart>();
            if (CreateID.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.CreateID == CreateID);
            }
            PagedData<Cart> pagedData = q.TakePageData(page);
            return pagedData;
        }


        public int Count(string ProductID,string ProSizeID, string CreateID)
        {
            return this.Query.Where(a => a.ProductID == ProductID && a.ProSizeID == ProSizeID &&  a.CreateID== CreateID).Count();
        }

        public int CountByUserID( string CreateID ,int ShopID)
        {
            return this.Query.Where(a => a.CreateID == CreateID && a.ShopID== ShopID).Count();
        }

        /// <summary>
        /// GetCartItem(string ProductID, string ProSizeID, string CreateID);
        /// </summary>
        /// <param name="ProductID"></param>
        /// <param name="ProSizeID"></param>
        /// <param name="CreateID"></param>
        /// <returns></returns>
        public Cart GetCartItem(string ProductID, string ProSizeID, string CreateID)
        {
            return this.Query.Where(a => a.ProductID == ProductID && a.ProSizeID == ProSizeID && a.CreateID == CreateID).FirstOrDefault();
        }

        public List<CartInfo> GetCartListItem(string CreateID,int ShopID)
        {

            var db_conn = this.DbContext.Query<Cart>().Where(a=>a.ShopID==ShopID&&a.CreateID==CreateID)
                         .LeftJoin<Product>((cart, product) => cart.ProductID == product.Id)
                         .LeftJoin<Product_Size>((cart, product, pro_size) => cart.ProSizeID == pro_size.Id);

            List<CartInfo> db_set = db_conn.Select<CartInfo>((cart, product, pro_size) => new CartInfo
            {
                Id = cart.Id,
                ProductID = cart.ProductID,
                ProductName = product.ProductName,
                ProductCode = product.ProductCode,
                Price = cart.Price,
                ProSize= pro_size.ProSize,
                ItemNum= cart.ItemNum, 
                CreateID=cart.CreateID,
                ImageUrl= product.ImageUrl
            }).ToList();

            return db_set;
        }


        /// <summary>
        /// 确定要下单的商品列表
        /// </summary>
        /// <param name="CreateID"></param>
        /// <returns></returns>
        public List<CartInfo> GetTempOrderItem(string CreateID)
        {

            var db_conn = this.DbContext.Query<Cart>()
                         .LeftJoin<Product>((cart, product) => cart.ProductID == product.Id)
                         .LeftJoin<Product_Size>((cart, product, pro_size) => cart.ProSizeID == pro_size.Id);

            List<CartInfo> db_set = db_conn.Select<CartInfo>((cart, product, pro_size) => new CartInfo
            {
                Id = cart.Id,
                ProductID = cart.ProductID,
                ProductName = product.ProductName,
                ProductCode = product.ProductCode,
                Price = cart.Price,
                ProSize = pro_size.ProSize,
                ItemNum = cart.ItemNum,
                CreateID = cart.CreateID,
                ImageUrl = product.ImageUrl,
                ST=cart.ST
            }).Where(a => a.CreateID == CreateID&&a.ST==1).ToList();

            return db_set;
        }


        public void UpdateCart(string Id,int ItemNum)
        {
            this.DbContext.Update<Cart>(a => a.Id == Id, a => new Cart()
            {
                 ItemNum= ItemNum,
                 CreateDate= DateTime.Now
            });
        }

        public void CartChecked(string Id)
        {
            this.DbContext.Update<Cart>(a => a.Id == Id, a => new Cart()
            {
                ST = 1,
                CreateDate = DateTime.Now
            });
        }


        /// <summary>
        /// 从临时订单中清除
        /// </summary>
        /// <param name="UserID"></param>
        public void ClearTempOrder(string UserID)
        {
            this.DbContext.Update<Cart>(a => a.CreateID == UserID, a => new Cart()
            {
                ST = 0
            });
        }

        /// <summary>
        /// in _productid varchar(50),in _prosizeid varchar(50),in _itemnum int,in _createid varchar(50)
        /// </summary>
        /// <returns></returns>
        public int SetCart(int ShopID, string ProductID,string ProSizeID,int ItemNum,string CreateID)
        {
            int rs = 0;

            DbParam _productid = new DbParam("?_productid", ProductID);
            DbParam _prosizeid = new DbParam("?_prosizeid", ProSizeID);
            DbParam _itemnum = new DbParam("?_itemnum", ItemNum);
            DbParam _createid = new DbParam("?_createid", CreateID);
            DbParam _shopid = new DbParam("?_shopid", ShopID);

            //DbParam outputName = new DbParam("?_prosizeid", null, typeof(string)) { Direction = ParamDirection.Output };
            rs = this.DbContext.Session.ExecuteNonQuery("SetCart", System.Data.CommandType.StoredProcedure, _productid, _prosizeid, _itemnum, _createid, _shopid);
            
            return rs;
        }
        


    }

}
