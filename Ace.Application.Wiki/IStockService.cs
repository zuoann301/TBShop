using Ace.Application;
using Ace.Entity.Wiki;
using Ace.Exceptions;
using Chloe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Application.Wiki
{
    public interface IStockService : IAppService
    {
        List<Stock> GetList(string keyword = "");
        void Add(AddStockInput input);
        void Update(UpdateStockInput input);
        void Delete(string id );

        PagedData<Stock> GetPageData(Pagination page, string Type, string keyword);
    }

    public class StockService : AppServiceBase<Stock>, IStockService
    {
        public StockService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<Stock> GetList(string keyword = "")
        {
            var q = this.Query;
            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.StockCode.Contains(keyword));
            }

            var ret = q.OrderBy(a => a.CreateDate).ToList();
            return ret;
        }
        public void Add(AddStockInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdateStockInput input)
        {
            this.UpdateFromDto(input);
        }

        public void Delete(string id)
        {
            this.DbContext.Delete<Stock>(a => a.Id == id);
        }


        public PagedData<Stock> GetPageData(Pagination page,string Type, string keyword)
        {
            var q = this.DbContext.Query<Stock>();

            q = q.WhereIfNotNullOrEmpty(Type, a =>   a.Type== Type);
            q = q.WhereIfNotNullOrEmpty(keyword, a => a.StockCode.Contains(keyword));

            q = q.OrderByDesc(a => a.CreateDate);

            PagedData<Stock> pagedData = q.TakePageData(page);
            
            return pagedData;
        }


    }

}
