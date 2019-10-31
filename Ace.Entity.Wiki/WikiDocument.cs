using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Entity.Wiki
{
    public class WikiDocument
    {
        public string Id { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }
        public string Tag { get; set; }

        public DateTime CreationTime { get; set; }
        public DateTime? UpdationTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
