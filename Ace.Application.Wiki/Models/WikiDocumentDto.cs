using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    [MapToType(typeof(WikiDocumentDetail))]
    public class AddDocumentInput : AddOrUpdateDocumentInputBase
    {
    }

    public class AddOrUpdateDocumentInputBase : Ace.Application.ValidationModel
    {
        [RequiredAttribute(ErrorMessage = "标题不能为空")]
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Tag { get; set; }
        public string HtmlContent { get; set; }
        public string MarkdownCode { get; set; }
    }

    [MapToType(typeof(WikiDocumentDetail))]
    public class UpdateDocumentInput : AddOrUpdateDocumentInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }

    public class WikiDocumentDetailModel
    {
        public string Id { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }

        public string HtmlContent { get; set; }
    }

}
