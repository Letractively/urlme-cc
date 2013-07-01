using System.Web;
using System.Web.Optimization;

namespace seeitornot.site
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            bundles.Add(new ScriptBundle("~/bundles/js/jquery").Include(
                        "~/Scripts/jquery-1.8.3.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/plugins").Include(
                        "~/Scripts/plugins/jquery.cookie.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/content/normalize.css",
                "~/Content/site.css"
            ));

            // BundleTable.EnableOptimizations = true;
        }
    }
}