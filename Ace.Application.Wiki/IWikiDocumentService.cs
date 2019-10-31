using Ace;
using Ace.Application;
using Ace.AutoMapper;
using Ace.Entity.Wiki;
using Ace.IdStrategy;
using Chloe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Application.Wiki
{
    public interface IWikiDocumentService : IAppService
    {
        WikiDocumentDetailModel GetDetailModel(string id);
        string Add(AddDocumentInput input);
        void Update(UpdateDocumentInput input);
        List<WikiDocument> GetAll();
        PagedData<WikiDocument> GetPageData(Pagination page, string keyword);
        WikiDocumentDetail GetDocumentDetail(string id);
    }

    public class WikiDocumentService : AppServiceBase, IWikiDocumentService
    {
        public WikiDocumentService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public WikiDocumentDetailModel GetDetailModel(string id)
        {
            var details = this.DbContext.Query<WikiDocumentDetail>().FilterDeleted();
            WikiDocumentDetailModel model = details.Select(a => new WikiDocumentDetailModel() { Id = a.Id, Title = a.Title, Summary = a.Summary, HtmlContent = a.HtmlContent }).Where(a => a.Id == id).FirstOrDefault();

            return model;
        }

        public string Add(AddDocumentInput input)
        {
            input.Validate();

            WikiDocumentDetail detail = AceMapper.Map<WikiDocumentDetail>(input);
            detail.Id = IdHelper.CreateSnowflakeId().ToString();
            detail.IsDeleted = false;
            detail.CreationTime = DateTime.Now;

            this.DbContext.Insert(detail);

            return detail.Id;
        }
        public void Update(UpdateDocumentInput input)
        {
            input.Validate();

            this.DbContext.UpdateFromDto<WikiDocumentDetail, UpdateDocumentInput>(input);
        }

        public List<WikiDocument> GetAll()
        {
            var q = this.DbContext.Query<WikiDocument>().FilterDeleted();
            return q.ToList();
        }
        public PagedData<WikiDocument> GetPageData(Pagination page, string keyword)
        {
            var q = this.DbContext.Query<WikiDocument>().FilterDeleted().WhereIfNotNullOrEmpty(keyword, a => a.Title.Contains(keyword) || a.Tag.Contains(keyword));

            var pagedData = q.TakePageData(page);

            return pagedData;
        }
        public WikiDocumentDetail GetDocumentDetail(string id)
        {
            var doc = this.DbContext.QueryByKey<WikiDocumentDetail>(id);
            return doc;
        }
    }
}
