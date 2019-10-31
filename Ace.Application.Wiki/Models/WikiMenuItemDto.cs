using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class AddOrUpdateWikiMenuItemInputBase : ValidationModel
    {
        public string ParentId { get; set; }
        [RequiredAttribute(ErrorMessage = "名称不能为空")]
        public string Name { get; set; }
        public string DocumentId { get; set; }
        public bool IsEnabled { get; set; }
        public int? SortCode { get; set; }
    }

    [MapToType(typeof(WikiMenuItem))]
    public class AddWikiMenuItemInput : AddOrUpdateWikiMenuItemInputBase
    {

    }

    [MapToType(typeof(WikiMenuItem))]
    public class UpdateWikiMenuItemInput : AddOrUpdateWikiMenuItemInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }

    public class WikiMenuItem_WikiDocument
    {
        public WikiMenuItem MenuItem { get; set; }
        public WikiDocument Document { get; set; }
    }
}
