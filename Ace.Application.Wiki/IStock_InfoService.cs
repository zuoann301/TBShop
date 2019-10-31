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
    public interface IStock_InfoService : IAppService
    {
        List<Stock_Info> GetList(string StockCode = "");
        void Add(AddStock_InfoInput input);
        void Update(UpdateStock_InfoInput input);
        void Delete(string id );

        PagedData<Stock_Info> GetPageData(Pagination page, string StockCode );
    }

    public class Stock_InfoService : AppServiceBase<Stock_Info>, IStock_InfoService
    {
        public Stock_InfoService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<Stock_Info> GetList(string StockCode = "")
        {
            var q = this.Query.Where(a => a.StockCode == StockCode);          

            var ret = q.OrderBy(a => a.Quantity).ToList();
            return ret;
        }
        public void Add(AddStock_InfoInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdateStock_InfoInput input)
        {
            this.UpdateFromDto(input);
        }

        public void Delete(string id)
        {
            this.DbContext.Delete<Stock_Info>(a => a.Id == id);
        }


        public PagedData<Stock_Info> GetPageData(Pagination page,string StockCode)
        {
            var q = this.DbContext.Query<Stock_Info>();

            q = q.WhereIfNotNullOrEmpty(StockCode, a => a.StockCode== StockCode);

            
           

            q = q.OrderByDesc(a => a.Quantity);

            PagedData<Stock_Info> pagedData = q.TakePageData(page);
            
            return pagedData;
        }


    }

}
