using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PolyOmniaLayer.Models.Page;

namespace PolyOmniaLayer.TagHelpers
{
    public class PageLinkTagHelper(IUrlHelperFactory urlHelperFactory) : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = null!;

        public PageViewModel? PageModel { get; set; }
        public string PageAction { get; set; } = "";

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = [];

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (PageModel is null)
                throw new ArgumentNullException(nameof(PageModel), "Модель сторінки не встановлена");

            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
 
            output.TagName = "nav";
            output.Attributes.SetAttribute("class", "mt-5");

            TagBuilder ulTag = new("ul");

            ulTag.AddCssClass("pagination justify-content-center pagination-sm");

            if (PageModel.HasPreviousPage)
            {
                ulTag.InnerHtml.AppendHtml(CreateItem(PageModel.PageNumber - 1, "«", false, urlHelper));
            }

            int startPage = Math.Max(1, PageModel.PageNumber - 2);
            int endPage = Math.Min(PageModel.TotalPages, PageModel.PageNumber + 2);

            if (startPage > 1)
            {
                ulTag.InnerHtml.AppendHtml(CreateItem(1, "1", false, urlHelper));
                if (startPage > 2)
                {
                    ulTag.InnerHtml.AppendHtml(CreateDisabledItem("..."));
                }
            }

            for (int i = startPage; i <= endPage; i++)
            {
                bool isCurrent = (i == PageModel.PageNumber);
                ulTag.InnerHtml.AppendHtml(CreateItem(i, i.ToString(), isCurrent, urlHelper, isCurrent));
            }

            if (endPage < PageModel.TotalPages)
            {
                if (endPage < PageModel.TotalPages - 1)
                {
                    ulTag.InnerHtml.AppendHtml(CreateDisabledItem("..."));
                }
                ulTag.InnerHtml.AppendHtml(CreateItem(PageModel.TotalPages, PageModel.TotalPages.ToString(), false, urlHelper));
            }
            if (PageModel.HasNextPage)
            {
                ulTag.InnerHtml.AppendHtml(CreateItem(PageModel.PageNumber + 1, "»", false, urlHelper));
            }

            output.Content.AppendHtml(ulTag);
        }

        private TagBuilder CreateItem(int pageNumber, string text, bool isActive, IUrlHelper urlHelper, bool isCurrent = false)
        {
            TagBuilder item = new("li");
            item.AddCssClass("page-item");
            if (isCurrent)
            {
                item.AddCssClass("active");
            }

            TagBuilder link = new("a");
            link.AddCssClass("page-link");

            if (isCurrent)
            {
                link.Attributes["aria-current"] = "page";
                link.Attributes["href"] = "#";
            }
            else
            {
                var routeValues = new Dictionary<string, object>(PageUrlValues)
                {
                    ["page"] = pageNumber
                };
                link.Attributes["href"] = urlHelper.Action(PageAction, routeValues);
            }

            link.InnerHtml.Append(text);
            item.InnerHtml.AppendHtml(link);

            return item;
        }

        private TagBuilder CreateDisabledItem(string text)
        {
            TagBuilder item = new("li");
            item.AddCssClass("page-item disabled");

            TagBuilder span = new("span");
            span.AddCssClass("page-link");
            span.InnerHtml.Append(text);

            item.InnerHtml.AppendHtml(span);
            return item;
        }
    }
}
