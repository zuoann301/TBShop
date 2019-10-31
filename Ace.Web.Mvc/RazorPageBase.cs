using Ace;
using Ace.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Mvc.Razor
{
    public abstract class RazorPageBase<TModel> : RazorPage<TModel>
    {
        protected RazorPageBase()
        {
        }
        
        public IHtmlContent RefStyle(string contentPath)
        {
            string contentUrl = this.Href(contentPath);
            string html = string.Format("<link rel=\"stylesheet\" href=\"{0}\">", contentUrl);
            return new HtmlString(html);
        }

        public IHtmlContent RefScript(string contentPath)
        {
            string contentUrl = this.Href(contentPath);
            string html = string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", contentUrl);
            return new HtmlString(html);
        }

        public string Serialize(object obj)
        {
            return JsonHelper.Serialize(obj);
        }
        public IHtmlContent RawSerialize(object obj)
        {
            return this.Raw(this.Serialize(obj));
        }
        public IHtmlContent Raw(string value)
        {
            return new HtmlString(value);
        }
        public IHtmlContent Raw(object value)
        {
            return new HtmlString(value == null ? null : value.ToString());
        }
        public IHtmlContent Partial(string partialViewName)
        {
            dynamic page = this; //RazorPage<TModel>
            IHtmlHelper htmlHelper = page.Html;
            return htmlHelper.Partial(partialViewName);
        }

        public HtmlString SelectOptions(IEnumerable<SelectOption> optionList, string defaultText = "--请选择--")
        {
            return this.SelectOptions((object)optionList, defaultText);
        }
        public HtmlString SelectOptions(object optionList, string defaultText = "--请选择--")
        {
            StringBuilder htmlBuilder = new StringBuilder();

            const string optionFormat = "<option value=\"{0}\">{1}</option>";
            if (!string.IsNullOrEmpty(defaultText))
            {
                htmlBuilder.AppendFormat(optionFormat, string.Empty, defaultText);
            }

            dynamic d = optionList;
            foreach (var option in d)
            {
                htmlBuilder.AppendFormat(optionFormat, option.Value, option.Text);
            }
            return new HtmlString(htmlBuilder.ToString());
        }

    }


}
