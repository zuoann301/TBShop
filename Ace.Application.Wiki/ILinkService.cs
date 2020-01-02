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
    public interface ILinkService : IAppService
    {
        List<Link> GetList(int SortID = 0, string keyword = "");

        List<Link> GetShopLinkList(int ShopID, int SortID = 0, string keyword = "");

        void Add(AddLinkInput input);
        void Update(UpdateLinkInput input);
        void Delete(string id );

        Link GetModel(string Id);

        PagedData<Link> GetPageData(Pagination page, int SortID, string keyword, int ShopID = 0);
    }

    public class LinkService : AppServiceBase<Link>, ILinkService
    {
        public LinkService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<Link> GetList(int SortID=0, string keyword = "")
        {
            var q = this.Query;

            if (SortID>0)
            {
                q = q.Where(a => a.SortID== SortID);
            }

            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.Title.Contains(keyword));
            }

            var ret = q.ToList();
            return ret;
        }

        public List<Link> GetShopLinkList(int ShopID, int SortID = 0, string keyword = "")
        {
            var q = this.Query;

            if (ShopID > 0)
            {
                q = q.Where(a => a.ShopID == ShopID);
            }

            if (SortID > 0)
            {
                q = q.Where(a => a.SortID == SortID);
            }

            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.Title.Contains(keyword));
            }

            var ret = q.ToList();
            return ret;
        }

        public void Add(AddLinkInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdateLinkInput input)
        {
            this.UpdateFromDto(input);
        }

        public Link GetModel(string Id)
        {
            return  this.Query.Where(a => a.Id == Id).FirstOrDefault();
        }

        public void Delete(string id)
        {
            this.DbContext.Delete<Link>(a => a.Id == id);
        }


        public PagedData<Link> GetPageData(Pagination page,int SortID=0, string keyword="",int ShopID=0)
        {
            var q = this.DbContext.Query<Link>();

            q = q.WhereIfNotNullOrEmpty(keyword, a => a.Title.Contains(keyword) );
            if(SortID>0)
            {
                q = q.Where(a => a.SortID == SortID);
            }
            if (ShopID > 0)
            {
                q = q.Where(a => a.ShopID == ShopID);
            }



            PagedData<Link> pagedData = q.TakePageData(page);
            
            return pagedData;
        }

         



    }

}
