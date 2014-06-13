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
                        "~/Scripts/jquery-1.10.2.min.js",
                        "~/scripts/knockout-3.1.0.js",
                        "~/scripts/knockout.mapping.js",
                        "~/scripts/plugins/zclip/jquery.zclip.min.js",
                        "~/scripts/plugins/datatables/jquery.dataTables.min.js",
                        "~/scripts/plugins/datatables/datatables.bootstrap.js",
                        "~/scripts/plugins/bootstrap-dialog/bootstrap-dialog.min.js",
                        //"~/scripts/plugins/jquery.timeago.js",
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
                      "~/scripts/plugins/bootstrap-dialog/bootstrap-dialog.min.css",
                      "~/scripts/plugins/datatables/datatables.bootstrap.css",
                      "~/Content/site.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}
