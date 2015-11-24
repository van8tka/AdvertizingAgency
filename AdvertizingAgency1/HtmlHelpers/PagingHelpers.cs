using System;
using System.Text;
using System.Web.Mvc;
using AdvertizingAgency1.Models;

namespace AdvertizingAgency1.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
            PagingInfoViewModel pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");//construct an <a> tag
                tag.MergeAttribute("href",pageUrl(i));
                tag.InnerHtml = i.ToString();
                if(i==pagingInfo.CurrentPage)
                    tag.AddCssClass("selected");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}