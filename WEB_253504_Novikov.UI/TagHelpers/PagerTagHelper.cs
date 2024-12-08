using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Reflection;

namespace WEB_253504_Novikov.UI.TagHelpers
{
    public class PagerTagHelper : TagHelper
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PagerTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HtmlAttributeName("current-page")]
        public int CurrentPage { get; set; }

        [HtmlAttributeName("total-pages")]
        public int TotalPages { get; set; }

        [HtmlAttributeName("vehicle-type")]
        public string? VehicleType { get; set; }

        [HtmlAttributeName("admin")]
        public bool Admin { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";
            output.Attributes.Add("class", "pagination");
            output.Content.AppendHtml(CreatePageItem((CurrentPage == 1) ? 1 : CurrentPage - 1, "«"));

            for (int i = 1; i <= TotalPages; i++)
            {
                output.Content.AppendHtml(CreatePageItem(i, i.ToString()));
            }

            output.Content.AppendHtml(CreatePageItem((CurrentPage == TotalPages) ? TotalPages : CurrentPage + 1, "»"));
        }

        private TagBuilder CreatePageItem(int page, string text)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("page-item");
            if (page == CurrentPage && text != "«" && text != "»")
            {
                li.AddCssClass("active");
            }

            var a = new TagBuilder("a");
            a.AddCssClass("page-link");
            if (!Admin) a.AddCssClass("add-async-load");
            a.Attributes["href"] = GeneratePageLink(page);
            //a.InnerHtml.Append(text);

            var span = new TagBuilder("span");
            span.Attributes["aria-hidden"] = "true";
            span.InnerHtml.Append(text);
            a.InnerHtml.AppendHtml(span);

            li.InnerHtml.AppendHtml(a);
            return li;
        }

        private string GeneratePageLink(int page)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is null.");
            }

            string? url = null;


            var values = new RouteValueDictionary
                {
                    { "pageNo", page }
                };

            if (Admin)
            {
                url = _linkGenerator.GetPathByPage(
                    page: "Index",
                    values: values,
                    httpContext: httpContext);
            }
            else
            {
                if (!string.IsNullOrEmpty(VehicleType))
                {
                    values["vehicleType"] = VehicleType;
                }


                url = _linkGenerator.GetPathByAction(
                    action: "Index",
                    controller: "VehicleCatalog",
                    values: values,
                    httpContext: httpContext);
            }


            return url ?? "#";
        }
    }
}
