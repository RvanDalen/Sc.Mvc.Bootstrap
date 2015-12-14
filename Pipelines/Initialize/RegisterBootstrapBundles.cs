using System.Web.Optimization;
using Sitecore.Pipelines;

namespace Sc.Mvc.Bootstrap.Pipelines.Initialize
{
    public class RegisterBootstrapBundles
    {
        public virtual void Process(PipelineArgs args)
        {
            // Add @Styles.Render("~/Content/bootstrap") in the <head/> of your _Layout.cshtml view
            // Add @Scripts.Render("~/bundles/bootstrap") after jQuery in your _Layout.cshtml view
            // When <compilation debug="true" />, MVC4 will render the full readable version. When set to <compilation debug="false" />, the minified version will be rendered automatically
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/jquery.js").Include("~/Scripts/jquery-{version}.js",
                                                                                    "~/Scripts/jquery.validate*"));
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/bootstrap.js").Include("~/Scripts/bootstrap.js"));

            BundleTable.Bundles.Add(new StyleBundle("~/Content/bootstrap.css").Include("~/Content/bootstrap.css"));
        }
    }
}
