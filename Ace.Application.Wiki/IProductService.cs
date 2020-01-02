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
    public interface IProductService : IAppService
    {
        int GetProductCount(string SortID = "");
        List<ProductSearchKey> GetSearchKeys(string keyword, int ShopID, int Num = 5);
        List<Product> GetList(int Num = 0, string ProSortID = "", string keyword = "");

        List<Product> GetShopProductList(int ShopID, int Num = 0, string ProSortID = "", string keyword = "");

        List<Product> GetTopList(int n = 4, int IsTop = 0, int ShopID = 0);
        List<Product> GetHotList(int n = 4, int IsHot = 0, int ShopID = 0);
        List<Product> GetProductListByBrandID(string BrandID);

        void Add(AddProductInput input);
        void Update(UpdateProductInput input);
        void Delete(string id );

        bool BatchInsert(List<Product> list);

        Product GetModle(string Id);

        PagedData<Product> GetPageData(Pagination page, string ProSortID, string keyword,int ShopID=-1);

        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="page"></param>
        /// <param name="IsTop"></param>
        /// <param name="ProSortID"></param>
        /// <param name="keyword"></param>
        /// <param name="OrderType"></param>
        /// <returns></returns>
        PagedData<Product> GetTopPageData(Pagination page, int IsTop, string ProSortID, string keyword, int OrderType = 0);

        /// <summary>
        /// 推荐
        /// </summary>
        /// <param name="page"></param>
        /// <param name="IsHot"></param>
        /// <param name="ProSortID"></param>
        /// <param name="keyword"></param>
        /// <param name="OrderType"></param>
        /// <returns></returns>
        PagedData<Product> GetHotPageData(Pagination page, int IsHot, string ProSortID, string keyword, int OrderType = 0);

        PagedData<Product> GetPageList(Pagination page, string ProSortID, string keyword, int OrderType = 0);

        PagedData<Product> GetShopProductPageList(Pagination page, int ShopID, string ProSortID, string keyword, int OrderType = 0);

        bool SetPrice(decimal PerBatchPrice, decimal PerPrice,   decimal PerSharePercent);

        void UpDateHit(string Id, int Hit = 1);
    }

    public class ProductService : AppServiceBase<Product>, IProductService
    {
        public ProductService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public int GetProductCount(string SortID="")
        {
            int count = 0;
            if (SortID.IsNotNullOrEmpty())
            {
                count = this.Query.Where(a => a.ProSortID == SortID).Count();
            }
            else
            {
                count= this.Query.Count();
            }
            return count;
        }


        public List<ProductSearchKey> GetSearchKeys(string keyword, int ShopID,int Num=5)
        {
            var q = this.Query.Where(a => a.ShopID == ShopID);
            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.ProductName.Contains(keyword));
            }
            List<ProductSearchKey> searchKeys= q.Select(a => new ProductSearchKey() { ProductName = a.ProductName }).Take(Num).ToList();
            return searchKeys;
        }


        public List<Product> GetList(int Num=0,string ProSortID="", string keyword = "")
        {
            var q = this.Query;

            if (ProSortID.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.ProSortID== ProSortID);
            }

            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.ProductName.Contains(keyword));
            }
            if(Num>0)
            {
                return q.OrderBy(a => a.CreateDate).Take(Num).ToList();
            }
            else
            {
                return q.OrderBy(a => a.CreateDate).ToList();
            }
        }

        public List<Product> GetShopProductList(int ShopID, int Num = 0, string ProSortID = "", string keyword = "")
        {
            var q = this.Query;
            q = q.Where(a => a.ShopID == ShopID);
            if (ProSortID.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.ProSortID == ProSortID);
            }

            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.ProductName.Contains(keyword));
            }
            if (Num > 0)
            {
                return q.OrderBy(a => a.CreateDate).Take(Num).ToList();
            }
            else
            {
                return q.OrderBy(a => a.CreateDate).ToList();
            }
        }

        public List<Product> GetProductListByBrandID(string BrandID)
        {
            var q = this.Query;
            if (BrandID.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.BrandID == BrandID);
            }
            var ret = q.OrderBy(a => a.CreateDate).ToList();
            return ret;
        }

        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="n"></param>
        /// <param name="IsTop"></param>
        /// <returns></returns>
        public List<Product> GetTopList(int n=4,int IsTop=0,int ShopID=0)
        {
            var q = this.Query;
            q = q.Where(a => a.IsTop == IsTop);
            q = q.Where(a => a.ShopID == ShopID);
            var ret = q.OrderBy(a => a.CreateDate).Take(n).ToList();
            return ret;
        }

        /// <summary>
        /// 推荐
        /// </summary>
        /// <param name="n"></param>
        /// <param name="IsHot"></param>
        /// <returns></returns>
        public List<Product> GetHotList(int n = 4, int IsHot = 0, int ShopID = 0)
        {
            var q = this.Query;
            q = q.Where(a => a.IsHot == IsHot);
            q = q.Where(a => a.ShopID == ShopID);
            var ret = q.OrderBy(a => a.CreateDate).Take(n).ToList();
            return ret;
        }


        public void Add(AddProductInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdateProductInput input)
        {
            this.UpdateFromDto(input);
        }

        public Product GetModle(string Id)
        {
            return this.Query.Where(a => a.Id == Id).FirstOrDefault();
        }

        public void Delete(string id)
        {
            this.DbContext.Delete<Product>(a => a.Id == id);
        }


        public PagedData<Product> GetPageData(Pagination page,string ProSortID, string keyword, int ShopID =0)
        {
            var q = this.DbContext.Query<Product>();

            q = q.WhereIfNotNullOrEmpty(keyword, a => a.ProductName.Contains(keyword) || a.ProductCode.Contains(keyword) );

            q = q.WhereIfNotNullOrEmpty(ProSortID, a => a.ProSortID== ProSortID);

            if(ShopID>0)
            {
                q = q.Where(a => a.ShopID == ShopID);
            }

            q = q.OrderByDesc(a => a.CreateDate);

            PagedData<Product> pagedData = q.TakePageData(page);
            
            return pagedData;
        }

        public PagedData<Product> GetTopPageData(Pagination page, int IsTop, string ProSortID, string keyword, int OrderType = 0)
        {
            var q = this.DbContext.Query<Product>();
            q = q.Where(a => a.IsTop == IsTop);
            q = q.WhereIfNotNullOrEmpty(keyword, a => a.ProductName.Contains(keyword) || a.ProductCode.Contains(keyword));

            q = q.WhereIfNotNullOrEmpty(ProSortID, a => a.ProSortID == ProSortID);

            if (OrderType == 1)
            {
                q = q.OrderByDesc(a => a.Price);
            }
            else if (OrderType == 2)
            {
                q = q.OrderBy(a => a.Price);
            }
            else if (OrderType == 3)
            {
                q = q.OrderByDesc(a => a.Hit);
            }
            else
            {
                q = q.OrderByDesc(a => a.CreateDate);
            }
            PagedData<Product> pagedData = q.TakePageData(page);
            return pagedData;
        }

        public PagedData<Product> GetHotPageData(Pagination page, int IsHot, string ProSortID, string keyword, int OrderType = 0)
        {
            var q = this.DbContext.Query<Product>();
            q = q.Where(a => a.IsHot == IsHot);
            q = q.WhereIfNotNullOrEmpty(keyword, a => a.ProductName.Contains(keyword) || a.ProductCode.Contains(keyword));

            q = q.WhereIfNotNullOrEmpty(ProSortID, a => a.ProSortID == ProSortID);

            if (OrderType == 1)
            {
                q = q.OrderByDesc(a => a.Price);
            }
            else if (OrderType == 2)
            {
                q = q.OrderBy(a => a.Price);
            }
            else if (OrderType == 3)
            {
                q = q.OrderByDesc(a => a.Hit);
            }
            else
            {
                q = q.OrderByDesc(a => a.CreateDate);
            }
            PagedData<Product> pagedData = q.TakePageData(page);
            return pagedData;
        }


        public PagedData<Product> GetPageList(Pagination page,string ProSortID, string keyword, int OrderType = 0)
        {
            var q = this.DbContext.Query<Product>();
            q = q.WhereIfNotNullOrEmpty(keyword, a => a.ProductName.Contains(keyword) || a.ProductCode.Contains(keyword));

            q = q.WhereIfNotNullOrEmpty(ProSortID, a => a.ProSortID == ProSortID);

            if (OrderType == 1)
            {
                q = q.OrderByDesc(a => a.Price);
            }
            else if (OrderType == 2)
            {
                q = q.OrderBy(a => a.Price);
            }
            else if (OrderType == 3)
            {
                q = q.OrderByDesc(a => a.Hit);
            }
            else
            {
                q = q.OrderByDesc(a => a.CreateDate);
            }
            PagedData<Product> pagedData = q.TakePageData(page);
            return pagedData;
        }


        public PagedData<Product> GetShopProductPageList(Pagination page,int ShopID, string ProSortID, string keyword, int OrderType = 0)
        {
            var q = this.DbContext.Query<Product>();
            q = q.Where(a => a.ShopID == ShopID);
            q = q.WhereIfNotNullOrEmpty(keyword, a => a.ProductName.Contains(keyword) || a.ProductCode.Contains(keyword));

            q = q.WhereIfNotNullOrEmpty(ProSortID, a => a.ProSortID == ProSortID);

            if (OrderType == 1)
            {
                q = q.OrderByDesc(a => a.Price);
            }
            else if (OrderType == 2)
            {
                q = q.OrderBy(a => a.Price);
            }
            else if (OrderType == 3)
            {
                q = q.OrderByDesc(a => a.Hit);
            }
            else
            {
                q = q.OrderByDesc(a => a.CreateDate);
            }
            PagedData<Product> pagedData = q.TakePageData(page);
            return pagedData;
        }


        public bool BatchInsert(List<Product> list)
        {
            bool v = false;

            string DbType = Globals.Configuration["db:DbType"].ToString();
            string ConnString = Globals.Configuration["db:ConnString"].ToString();
            if (DbType == "MySql")
            {
                using (MySqlContext context = new MySqlContext(new MySqlConnectionFactory(ConnString)))
                {
                    try
                    {
                        context.Session.BeginTransaction();

                        foreach (var item in list)
                        {
                            //int row = context.Session.ExecuteNonQuery("select 1 from  yd_fdtz where DKBH = '"+item.DKBH+"' limit 1;");
                            if (item.Id == "0")
                            {
                                item.Id = IdHelper.CreateStringSnowflakeId();
                                context.Insert<Product>(item);
                            }
                            else
                            {
                                context.Update<Product>(item);
                            }
                        }
                        context.Session.CommitTransaction();
                    }
                    catch
                    {
                        if (context.Session.IsInTransaction)
                            context.Session.RollbackTransaction();
                        throw;
                    }
                }
            }

            if (DbType == "SqlServer")
            {
                using (MsSqlContext context = new MsSqlContext(ConnString))
                {
                    try
                    {
                        context.Session.BeginTransaction();

                        foreach (var item in list)
                        {
                            //int row = context.Session.ExecuteNonQuery("select count(1) from  yd_fdtz where DKBH = '" + item.DKBH + "'");
                            if (item.Id == "0")
                            {
                                item.Id = IdHelper.CreateStringSnowflakeId();
                                context.Insert<Product>(item);
                            }
                            else
                            {
                                context.Update<Product>(item);
                            }
                        }
                        context.Session.CommitTransaction();
                    }
                    catch
                    {
                        if (context.Session.IsInTransaction)
                            context.Session.RollbackTransaction();
                        throw;
                    }
                }
            }
            return v;
        }



        public bool SetPrice(decimal PerBatchPrice, decimal PerPrice,   decimal PerSharePercent)
        {
            bool v = false;

            string DbType = Globals.Configuration["db:DbType"].ToString();
            string ConnString = Globals.Configuration["db:ConnString"].ToString();
            if (DbType == "MySql")
            {
                using (MySqlContext context = new MySqlContext(new MySqlConnectionFactory(ConnString)))
                {
                    try
                    {
                        context.Session.BeginTransaction();
                        if (PerBatchPrice > 0)
                        {
                            context.Session.ExecuteNonQuery("update `Product` set `BatchPrice`=BasePrice*" + PerBatchPrice);
                            context.Session.ExecuteNonQuery("update `Product_Size` set `BatchPrice`=BasePrice*" + PerBatchPrice);
                        }

                        if (PerPrice > 0)
                        {
                            context.Session.ExecuteNonQuery("update `Product` set `Price`=BasePrice*" + PerPrice);
                            context.Session.ExecuteNonQuery("update `Product_Size` set `Price`=BasePrice*" + PerPrice);
                        }

                        

                        if (PerSharePercent > 0)
                        {
                            context.Session.ExecuteNonQuery("update `Product` set `SharePercent`=" + PerSharePercent);
                            context.Session.ExecuteNonQuery("update `Product_Size` set `SharePercent`=" + PerSharePercent);
                        }
                        context.Session.CommitTransaction();
                    }
                    catch
                    {
                        if (context.Session.IsInTransaction)
                            context.Session.RollbackTransaction();
                        throw;
                    }
                }
            }

            if (DbType == "SqlServer")
            {
                using (MsSqlContext context = new MsSqlContext(ConnString))
                {
                    try
                    {
                        context.Session.BeginTransaction();
                        if (PerBatchPrice > 0)
                        {
                            context.Session.ExecuteNonQuery("update Product set BatchPrice=BasePrice*" + PerBatchPrice);
                            context.Session.ExecuteNonQuery("update Product_Size set BatchPrice=BasePrice*" + PerBatchPrice);
                        }

                        if (PerPrice > 0)
                        {
                            context.Session.ExecuteNonQuery("update Product set Price=BasePrice*" + PerPrice);
                            context.Session.ExecuteNonQuery("update Product_Size set Price=BasePrice*" + PerPrice);
                        }
 

                        if (PerSharePercent > 0)
                        {
                            context.Session.ExecuteNonQuery("update Product set SharePercent=" + PerSharePercent);
                            context.Session.ExecuteNonQuery("update Product_Size set SharePercent=" + PerSharePercent);
                        }
                        context.Session.CommitTransaction();
                    }
                    catch
                    {
                        if (context.Session.IsInTransaction)
                            context.Session.RollbackTransaction();
                        throw;
                    }
                }
            }
            return v;
        }


        public void UpDateHit(string Id, int Hit = 1)
        {
            this.DbContext.Update<Product>(a => a.Id == Id, a => new Product()
            {
                Hit = Hit
            });
        }


    }

}
