﻿using System.Web;
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
                "~/scripts/plugins/bootstrap-dialog/bootstrap-dialog.min.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
                "~/Content/ratings.css",
                "~/scripts/plugins/bootstrap-dialog/bootstrap-dialog.min.css"
            ));

            // BundleTable.EnableOptimizations = true;
        }
    }
}