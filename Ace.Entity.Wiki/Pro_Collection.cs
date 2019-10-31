using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    public class Pro_Collection
    {
        public string Id { get; set; }

        public string ProductID { get; set; }

        public string UserID { get; set; }

        public DateTime CreateDate { get; set; } 

    }

    public class Pro_CollectionInfo
    {
        public string Id { get; set; }

        public string ProductID { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreateDate { get; set; }

        public string UserID { get; set; }

        public string Summary { get; set; }

    }

}
