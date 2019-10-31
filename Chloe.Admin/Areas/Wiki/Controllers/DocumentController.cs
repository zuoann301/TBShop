using Ace;
using Ace.Entity;
using Chloe.Admin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ace.Application.Wiki;
using Ace.Entity.Wiki;
using Ace.Application;
using Ace.Web.Mvc.Authorization;

namespace Chloe.Admin.Areas.Wiki.Controllers
{
    [Area(AreaNames.Wiki)]
    [Permission("wiki.document")]
    public class DocumentController : WebController<IWikiDocumentService>
    {
        // GET: Wiki
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Document(string id)
        {
            DocumentModel doc = new DocumentModel();

            if (id != null)
            {
                WikiDocumentDetail detail = this.Service.GetDocumentDetail(id);

                if (detail == null)
                {
                    /* 404 */
                }

                doc.Id = detail.Id;
                doc.Title = detail.Title;
                doc.Tag = detail.Tag;
                doc.Summary = detail.Summary;
                doc.MarkdownCode = detail.MarkdownCode;
            }

            this.ViewBag.Doc = doc;

            return View();
        }
        public ActionResult GetModels(Pagination pagination, string keyword)
        {
            PagedData<WikiDocument> pagedData = this.Service.GetPageData(pagination, keyword);
            return this.SuccessData(pagedData);
        }
        public ActionResult GetDocument(string id)
        {
            WikiDocumentDetail doc = this.Service.GetDocumentDetail(id);
            return this.SuccessData(doc);
        }

        [Permission("wiki.document.add")]
        [HttpPost]
        //[System.Web.Mvc.ValidateInput(false)]
        public ActionResult Add(AddDocumentInput input)
        {
            string id = this.Service.Add(input);
            return this.SuccessData(id);
        }

        [Permission("wiki.document.update")]
        [HttpPost]
        //[System.Web.Mvc.ValidateInput(false)]
        public ActionResult Update(UpdateDocumentInput input)
        {
            this.Service.Update(input);
            return this.SuccessMsg("更新成功");
        }

        [Permission("wiki.document.delete")]
        [HttpPost]
        public ActionResult Delete(string id)
        {
            IEntityAppService service = this.CreateService<IEntityAppService>();
            service.SoftDelete<WikiDocument>(id);
            return this.SuccessMsg("删除成功");
        }
    }

    public class DocumentModel
    {
        public string Id { get; set; }/* 由于 */
        public string Title { get; set; }
        public string Tag { get; set; }
        public string Summary { get; set; }
        public string MarkdownCode { get; set; }
    }
}