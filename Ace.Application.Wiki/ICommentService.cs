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
    public interface ICommentService : IAppService
    {
        List<Comment> GetList(string Fid, int Num);
        void Add(AddCommentInput input);
        void Update(UpdateCommentInput input);
        void Delete(string id );

        Comment GetModel(string Id);

        PagedData<Comment> GetPageData(Pagination page, string Fid);
    }

    public class CommentService : AppServiceBase<Comment>, ICommentService
    {
        public CommentService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<Comment> GetList(string Fid,int Num)
        {
            var q = this.Query;
            if (Fid.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.Fid== Fid);
            }
            if(Num>0)
            {
                q = q.Take(Num);
            }
            var ret = q.ToList();
            return ret;
        }
        public void Add(AddCommentInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdateCommentInput input)
        {
            this.UpdateFromDto(input);
        }

        public Comment GetModel(string Id)
        {
            return  this.Query.Where(a => a.Id == Id).FirstOrDefault();
        }

        public void Delete(string id)
        {
            this.DbContext.Delete<Comment>(a => a.Id == id);
        }


        public PagedData<Comment> GetPageData(Pagination page,string Fid)
        {
            var q = this.DbContext.Query<Comment>();
            q = q.WhereIfNotNullOrEmpty(Fid, a => a.Fid== Fid);
            
            PagedData<Comment> pagedData = q.TakePageData(page);
            return pagedData;
        }

         



    }

}
