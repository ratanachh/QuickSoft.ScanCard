using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuickSoft.ScanCard.Utils
{
    public static class HtmlUtil
    {
        public static string IsSelected(this IHtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {
            if (string.IsNullOrEmpty(cssClass))
                cssClass = "active";

            var currentAction = (string)html.ViewContext.RouteData.Values["action"];
            var currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (string.IsNullOrEmpty(controller))
                controller = currentController;

            if (string.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ? cssClass : string.Empty;
        }

        public static string PageClass(this IHtmlHelper htmlHelper)
        {
            return (string)htmlHelper.ViewContext.RouteData.Values["action"];;
        }
    }
}