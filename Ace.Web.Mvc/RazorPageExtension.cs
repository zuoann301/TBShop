using Ace;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;

namespace Microsoft.AspNetCore.Mvc.Razor
{
    public static class RazorPageExtension
    {
        //public static IHtmlContent RefStyle(this RazorPage razorPage, string contentPath)
        //{
        //    string contentUrl = razorPage.Href(contentPath);
        //    string html = string.Format("<link rel=\"stylesheet\" href=\"{0}\">", contentUrl);
        //    return new HtmlString(html);
        //}

        //public static IHtmlContent RefScript(this RazorPage razorPage, string contentPath)
        //{
        //    string contentUrl = razorPage.Href(contentPath);
        //    string html = string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", contentUrl);
        //    return new HtmlString(html);
        //}

        //public static string Serialize(this RazorPage razorPage, object obj)
        //{
        //    return JsonHelper.Serialize(obj);
        //}
        //public static IHtmlContent RawSerialize(this RazorPage razorPage, object obj)
        //{
        //    return razorPage.Raw(razorPage.Serialize(obj));
        //}
        //public static IHtmlContent Raw(this RazorPage razorPage, string value)
        //{
        //    return new HtmlString(value);
        //}
        ////public static IHtmlContent Raw(this RazorPage razorPage, object value)
        ////{
        ////    return razorPage.Html.Raw(value);
        ////}
        //public static IHtmlContent Partial(this RazorPage razorPage, string partialViewName)
        //{
        //    dynamic page = razorPage; //RazorPage<int>
        //    IHtmlHelper htmlHelper = page.Html;
        //    return htmlHelper.Partial(partialViewName);
        //}
    }
}
