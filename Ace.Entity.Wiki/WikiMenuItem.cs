using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Entity.Wiki
{
    public class WikiMenuItem
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string DocumentId { get; set; }
        public int? SortCode { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
