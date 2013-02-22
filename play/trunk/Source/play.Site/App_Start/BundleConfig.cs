using System.Web;
using System.Web.Optimization;

namespace play.Site
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            /* ihdavis */
            bundles.Add(new ScriptBundle("~/bundles/ihdavis").Include(
                        "~/Scripts/ihdavis.js"));

            /* jQuery */
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            /* jQuery UI */
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Content/themes/dot-luv/js/jquery-ui-{version}.custom.js"));
            
            bundles.Add(new StyleBundle("~/Content/themes/dot-luv/css").Include(
                        "~/Content/themes/dot-luv/css/jquery-ui-{version}.custom.css"));
            
            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/*.css"));

            /* site.css */
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            /* knockout */
            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                        "~/scripts/knockout-{version}.js", 
                        "~/scripts/knockout.mapping.js"));

        }
    }
}