using System.Web;
using System.Web.Optimization;

namespace urlme.site
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/scripts/knockout-{version}.js",
                        "~/scripts/knockout.mapping.js",
                        "~/scripts/plugins/zeroclipboard/zeroclipboard.min.js",
                        "~/scripts/plugins/datatables/datatables.bootstrap.js",
                        "~/scripts/ianhd/ianhd.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-social.css",
                      //"~/Content/bootstrap-theme.min.css",
                      //"~/content/todc-bootstrap.min.css",
                      "~/scripts/plugins/datatables/datatables.bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
