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

            BundleTable.EnableOptimizations = false;
        }
    }
}
