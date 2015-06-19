using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Bazam.Slugging;
using CommonMark;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bazam.Mvc
{
    public static class BazamHtmlHelpers
    {
        private static UrlHelper GetUrlHelper(HtmlHelper helper)
        {
            return new UrlHelper(helper.ViewContext.RequestContext);
        }

        public static IHtmlString DeclareJSVariables(this HtmlHelper helper, params object[] variablePairs)
        {
            if (variablePairs.Length % 2 != 0) throw new InvalidOperationException("An invalid number of variable/pair arguments was passed to DeclareJSVariables, an html helper.");

            string declarationStatement = "<script type=\"text/javascript\">{0}</script>";
            StringBuilder varsBuilder = new StringBuilder();
            string tempVarStatement = string.Empty;

            // do default serialization of objects to json (TODO: expose these as params at some point?)
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());

            for (int i = 0; i < variablePairs.Length; i++) {
                if (i % 2 == 0) tempVarStatement = "var " + variablePairs[i].ToString();
                else {
                    tempVarStatement += " = " + JsonConvert.SerializeObject(variablePairs[i], Formatting.None, settings) + ";";
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
            return ObjectToJson(helper, input, Formatting.None);
        }
        
        public static string ObjectToJson(this HtmlHelper helper, object input, Formatting jsonFormatting, params JsonConverter[] converters)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            if (converters != null) {
                foreach (JsonConverter converter in converters) {
                    settings.Converters.Add(converter);
                }
            }

            return JsonConvert.SerializeObject(input, jsonFormatting, settings);
        }

        public static string Slugify(this HtmlHelper helper, string input)
        {
            return Slugger.Slugify(input);
        }

        /// <summary>
        /// I'm not sure if this'll help anyone but me, and maybe I'm going about this the wrong way, but I often find it necessary (?) to have a javascript
        /// file for a particular view.
        /// </summary>
        /// <param name="scriptRoot">The path to your view scripts - I put mine in "~/Scripts/ViewScripts", but your mileage may vary. Use ~ to have the path mapped for you.</param>
        /// <returns></returns>
        public static IHtmlString ViewScript(this HtmlHelper helper, string scriptRoot = "~/Scripts")
        {
            string fileName = "/" + helper.ViewContext.RouteData.Values["controller"].ToString().ToLower() + "-" + helper.ViewContext.RouteData.Values["action"].ToString().ToLower() +".js";
            string scriptUrl = GetUrlHelper(helper).Content(scriptRoot + fileName);

            return MvcHtmlString.Create(string.Format("<script type=\"text/javascript\" src=\"" + scriptUrl + "\"></script>"));
        }
    }
}