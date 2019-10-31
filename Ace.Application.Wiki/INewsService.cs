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
    public interface INewsService : IAppService
    {
        List<News> GetList(int Num=4,int SortID = 0, int IsValid=-1, string keyword = "");

        List<News> GetShopNewsList(int Num = 4, int SortID = 0, int ShopID = 0, int IsValid = -1, string keyword = "");

        void Add(AddNewsInput input);
        void Update(UpdateNewsInput input);
        void Delete(string id );

        News GetModel(string Id);

        bool BatchInsert(List<News> list);

        PagedData<News> GetPageData(Pagination page, int SortID, int IsValid=-1, string keyword="");

        PagedData<News> GetCommentPageData(Pagination page, int SortID, int IsValid = -1, string keyword = "");


    }

    public class NewsService : AppServiceBase<News>, INewsService
    {
        public NewsService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<News> GetList(int Num=4, int SortID =0,int IsValid=-1, string keyword = "")
        {
            var q = this.Query;
            if (SortID>0)
            {
                q = q.Where(a => a.SortID== SortID);
            }
            if (IsValid > -1)
            {
                q = q.Where(a => a.IsValid == IsValid);
            }
            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.Title.Contains(keyword));
            }
            var ret = q.OrderBy(a => a.CreateDate).Take(Num).ToList();
            return ret;
        }

        public List<News> GetShopNewsList(int Num = 4, int SortID = 0,int ShopID=0, int IsValid = -1, string keyword = "")
        {
            var q = this.Query;
            if (SortID > 0)
            {
                q = q.Where(a => a.SortID == SortID);
            }

            if (ShopID > 0)
            {
                q = q.Where(a => a.ShopID == ShopID);
            }

            if (IsValid > -1)
            {
                q = q.Where(a => a.IsValid == IsValid);
            }
            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.Title.Contains(keyword));
            }
            var ret = q.OrderBy(a => a.CreateDate).Take(Num).ToList();
            return ret;
        }


        public void Add(AddNewsInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdateNewsInput input)
        {
            this.UpdateFromDto(input);
        }

        public News GetModel(string Id)
        {
            return  this.Query.Where(a => a.Id == Id).FirstOrDefault();
        }

        public void Delete(string id)
        {
            this.DbContext.Delete<News>(a => a.Id == id);
        }


        public PagedData<News> GetPageData(Pagination page, int SortID, int IsValid = -1, string keyword="")
        {
            var q = this.DbContext.Query<News>();

            q = q.WhereIfNotNullOrEmpty(keyword, a => a.Title.Contains(keyword) );
            if (SortID > 0)
            {
                q = q.Where(a => a.SortID == SortID);
            }
            else
            {
                List<int> SortList = this.DbContext.Query<Sort>().Where(a => a.Pid == (int)Ace.EnumSort.News).Select(c => c.Id).ToList();
                q = q.Where(a => SortList.Contains(a.SortID));
            }

            if (IsValid > -1)
            {
                q = q.Where(a => a.IsValid == IsValid);
            }

            q = q.OrderByDesc(a => a.CreateDate);

            PagedData<News> pagedData = q.TakePageData(page);
            
            return pagedData;
        }

         



        public PagedData<News> GetCommentPageData(Pagination page, int SortID, int IsValid = -1, string keyword = "")
        {
            var q = this.DbContext.Query<News>();

            q = q.WhereIfNotNullOrEmpty(keyword, a => a.Title.Contains(keyword));
            if (SortID > 0)
            {
                q = q.Where(a => a.SortID == SortID);
            }
            else
            {
                List<int> SortList = this.DbContext.Query<Sort>().Where(a => a.Pid == (int)Ace.EnumSort.Comment).Select(c => c.Id).ToList();
                q = q.Where(a => SortList.Contains(a.SortID));
            }

            if (IsValid > -1)
            {
                q = q.Where(a => a.IsValid == IsValid);
            }

            q = q.OrderByDesc(a => a.CreateDate);

            PagedData<News> pagedData = q.TakePageData(page);

            return pagedData;
        }



        public bool BatchInsert(List<News> list)
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
                                context.Insert<News>(item);
                            }
                            else
                            {
                                context.Update<News>(item);
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
                                context.Insert<News>(item);
                            }
                            else
                            {
                                context.Update<News>(item);
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
