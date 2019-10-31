using Ace.Application;
using Ace.Entity;
using Ace.Entity.System;
using Ace.IdStrategy;
using Chloe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Application.System
{
    public interface IPostService : IAppService
    {
        List<SysPost> GetList(string keyword = "");
        void Add(AddPostInput input);
        void Update(UpdatePostInput input);
        void Delete(string id, string operatorId);
        List<SysPost> GetListByOrgId(List<string> orgIds);
    }

    public class PostService : AppServiceBase<SysPost>, IPostService
    {
        public PostService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<SysPost> GetList(string keyword = "")
        {
            var q = this.Query.FilterDeleted();
            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.Name.Contains(keyword));
            }

            var ret = q.OrderBy(a => a.OrgId).ToList();
            return ret;
        }
        public void Add(AddPostInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdatePostInput input)
        {
            this.UpdateFromDto(input);
        }

        public void Delete(string id, string operatorId)
        {
            this.SoftDelete(id, operatorId);
        }

        public List<SysPost> GetListByOrgId(List<string> orgIds)
        {
            if (orgIds.Count == 0)
                return new List<SysPost>();

            return this.Query.FilterDeleted().Where(a => orgIds.Contains(a.OrgId)).ToList();
        }
    }

}
