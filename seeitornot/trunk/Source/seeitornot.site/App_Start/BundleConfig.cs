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
                "~/Scripts/jquery-1.8.3.min.js",
                "~/scripts/knockout-2.2.1.js",
                "~/scripts/knockout.mapping.js",
                "~/scripts/plugins/jquery.history.js",
                "~/scripts/plugins/bootstrap-dialog/bootstrap-dialog.min.js",
                "~/scripts/plugins/iscroll.js",
                "~/scripts/ianhd.js"
            ));

            // other stuff? in @import's in .less
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
                "~/Content/ratings.css"
            ));

            // BundleTable.EnableOptimizations = true;
        }
    }
}