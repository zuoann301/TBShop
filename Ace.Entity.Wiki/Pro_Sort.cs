using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    [Table("Pro_Sort")]
    public class Pro_Sort
    {

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        public string Icon { get; set; }

        public string ImageUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Pid { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortCode { get; set; }


        public int ProCount { get; set; }

        public string Summary { get; set; }

        public int ShopID { get; set; }

    }


    public class ProSortInfo
    {

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        public string Summary { get; set; }

        public string Icon { get; set; }
        public string ImageUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Pid { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortCode { get; set; }


        public int ProCount { get; set; }

        public List<Pro_Sort> SubSortList { get; set; }

    }

}
