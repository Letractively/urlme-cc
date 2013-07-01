using System.Web;
using System.Web.Optimization;

namespace seeitornot.site
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.8.2.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/content/normalize.css",
                "~/Content/site.css"
            ));

            // mobile scripts
            bundles.Add(new ScriptBundle("~/bundles/mobile-scripts").Include(
                "~/scripts/plugins/mmenu/jquery.mmenu.min.js"        
            ));

            // mobile styles
            bundles.Add(new StyleBundle("~/bundles/mobile-styles").Include(
                "~/scripts/plugins/mmenu/mmenu.css"
            ));

            // BundleTable.EnableOptimizations = true;
        }
    }
}