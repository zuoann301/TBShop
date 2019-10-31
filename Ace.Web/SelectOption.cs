using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Ace.Web
{
    public class SelectOption
    {
        public SelectOption()
        {
        }
        public SelectOption(string value, string text)
        {
            this.Value = value;
            this.Text = text;
        }
        public string Value { get; set; }
        public string Text { get; set; }

        public static SelectOption Create(object instance, string valueProp = "Id", string textProp = "Name")
        {
            object value = instance.GetPropertyValue(valueProp);
            object text = instance.GetPropertyValue(textProp);

            SelectOption option = new SelectOption();
            option.Value = value == null ? null : value.ToString();
            option.Text = text == null ? null : text.ToString();

            return option;
        }
        public static List<SelectOption> CreateList<T>(IEnumerable<T> instanceList, string valueProp = "Id", string textProp = "Name")
        {
            List<SelectOption> options = new List<SelectOption>();
            foreach (var instance in instanceList)
            {
                SelectOption option = SelectOption.Create(instance, valueProp, textProp);
                options.Add(option);
            }

            return options;
        }
    }
}
