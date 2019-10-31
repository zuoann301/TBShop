using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Entity.Wiki
{
    [Chloe.Annotations.Table("WikiDocument")]
    public class WikiDocumentDetail : WikiDocument
    {
        public string HtmlContent { get; set; }
        public string MarkdownCode { get; set; }
    }
}
