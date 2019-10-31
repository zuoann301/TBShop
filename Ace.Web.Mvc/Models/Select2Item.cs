using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Web.Mvc.Models
{
    public class Select2Item
    {
        public Select2Item()
        {
        }
        public Select2Item(object id, string text)
        {
            this.id = id;
            this.text = text;
        }
        public object id { get; set; }
        public string text { get; set; }
        public object value { get; set; }
    }
    public class Select2Group
    {
        public Select2Group()
        {
        }
        public Select2Group(string text)
        {
            this.text = text;
        }
        public string text { get; set; }
        public List<Select2Item> children { get; set; } = new List<Select2Item>();

        //public static List<Select2Group> Create<T>(List<T> items,Func<T,object> groupKeySelector,)
        //{

        //}
    }
}
