using System;
using System.Collections.Generic;
using System.Linq;

namespace Chloe.Admin.Common.Tree
{
    public class TreeViewModel
    {
        public string ParentId { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public int? Checkstate { get; set; }
        public bool Showcheck { get; set; }
        public bool Complete { get; set; }
        public bool Isexpand { get; set; }
        public bool HasChildren { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
    }

}