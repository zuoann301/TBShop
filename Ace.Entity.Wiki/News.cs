using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    [Table("News")]
    public class News
    {

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Summary { get; set; }

        public string Contents { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }

        public int IsValid { get; set; }

        public int ShopID { get; set; }

    }
}
