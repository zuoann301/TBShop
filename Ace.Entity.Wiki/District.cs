using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    [Table("District")]
    public class District
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public int Pid { get; set; }


    }
}
