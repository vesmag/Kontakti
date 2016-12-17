
using System.Web.Optimization;

namespace ContactsApp
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/ContactsApp")
                .IncludeDirectory("~/Scripts/Controllers", "*.js")
                .IncludeDirectory("~/Scripts/Services", "*.js")
                .Include("~/Scripts/ContactsApp.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}