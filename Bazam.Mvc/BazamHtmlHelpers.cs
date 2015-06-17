using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Bazam.Slugging;
using CommonMark;
using Newtonsoft.Json;

namespace Bazam.Mvc
{
    public static class BazamHtmlHelpers
    {
        private static UrlHelper GetUrlHelper(HtmlHelper helper)
        {
            return new UrlHelper(helper.ViewContext.RequestContext);
        }

        public static IHtmlString DeclareJSVariables(this HtmlHelper helper, params string[] variablePairs)
        {
            if (variablePairs.Length % 2 != 0) throw new InvalidOperationException("An invalid number of variable/pair arguments was passed to DeclareJSVariables, an html helper.");

            string declarationStatement = "<script type=\"text/javascript\">{0}</script>";
            StringBuilder varsBuilder = new StringBuilder();
            string tempVarStatement = string.Empty;

            for (int i = 0; i < variablePairs.Length; i++) {
                if (i % 2 == 0) tempVarStatement += "var " + variablePairs[i];
                else {
                    tempVarStatement += " = " + variablePairs[i] + ";";
                    varsBuilder.Append(tempVarStatement);
                }
            }

            return MvcHtmlString.Create(string.Format(declarationStatement, varsBuilder.ToString()));
        }

        public static IHtmlString MarkdownToHtml(this HtmlHelper helper, string markdownData)
        {
            return MvcHtmlString.Create(CommonMarkConverter.Convert(markdownData));
        }

        public static string ObjectToJson(this HtmlHelper helper, object input)
        {
            return JsonConvert.SerializeObject(input);
        }

        public static string Slugify(this HtmlHelper helper, string input)
        {
            return Slugger.Slugify(input);
        }

        /// <summary>
        /// I'm not sure if this'll help anyone but me, and maybe I'm going about this the wrong way, but I often find it necessary (?) to have a javascript
        /// file for a particular view.
        /// </summary>
        /// <param name="scriptRoot">The path to your view scripts - I put mine in "/Scripts/ViewScripts", but your mileage may vary.</param>
        /// <returns></returns>
        public static IHtmlString ViewScript(this HtmlHelper helper, string scriptRoot = "/Scripts")
        {
            string actionName = helper.ViewContext.RouteData.Values["action"].ToString();
            string controllerName = helper.ViewContext.RouteData.Values["controller"].ToString();

            string scriptUrl = GetUrlHelper(helper).Content(scriptRoot + "/ViewScripts/" + controllerName.ToLower() + "-" + actionName.ToLower() + ".js");
            return MvcHtmlString.Create(string.Format("<script type=\"text/javascript\" src=\"" + scriptUrl + "\"></script>"));
        }
    }
}