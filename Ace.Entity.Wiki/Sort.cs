using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    [Table("Sort")]
    public class Sort
    {

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Pid { get; set; }

        

    }
}
