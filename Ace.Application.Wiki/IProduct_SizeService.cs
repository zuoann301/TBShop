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
    public interface IProduct_SizeService : IAppService
    {
        List<Product_Size> GetList(string ProductCode = "");
        void Add(AddProduct_SizeInput input);

        Product_Size GetModle(string Id);

        void Update(UpdateProduct_SizeInput input);
        void Delete(string id);
        bool BatchInsert(List<Product_Size> list);
    }

    public class Product_SizeService : AppServiceBase<Product_Size>, IProduct_SizeService
    {
        public Product_SizeService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<Product_Size> GetList(string ProductCode = "")
        {
            var q = this.Query.Where(a=>a.ProductCode== ProductCode);           
              
            var ret = q.OrderBy(a => a.Price).ToList();
            return ret;
        }

        public Product_Size GetModle(string Id)
        {
            return this.Query.Where(a => a.Id == Id).FirstOrDefault();
        }


        public void Add(AddProduct_SizeInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdateProduct_SizeInput input)
        {
            this.UpdateFromDto(input);
        }

        public void Delete(string id)
        {
            this.DbContext.Delete<Product_Size>(a => a.Id == id);
        }


        public bool BatchInsert(List<Product_Size> list)
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
                                context.Insert<Product_Size>(item);
                            }
                            else
                            {
                                context.Update<Product_Size>(item);
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
                                context.Insert<Product_Size>(item);
                            }
                            else
                            {
                                context.Update<Product_Size>(item);
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

    }

}
