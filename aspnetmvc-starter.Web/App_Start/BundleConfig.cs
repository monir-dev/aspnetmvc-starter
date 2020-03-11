using System.Web.Optimization;

namespace aspnetmvc_starter.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));


            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/adminLte/dist/css/adminlte.min.css",
                //"~/Content/adminLte/plugins/fontawesome-free/css/all.css",
                "~/Content/sweetalert2/sweetalert2.min.css",
                "~/Content/alertify/css/alertify.min.css",
                "~/Content/alertify/css/semantic.min.css",
                "~/Content/css/style.css"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/adminLte/plugins/fontawesome-free/css/all.css",
            //          "~/Content/adminLte/dist/css/adminlte.min.css",
            //          "~/Content/sweetalert2/sweetalert2.min.css",
            //          "~/Content/alertify/css/alertify.min.css",
            //          "~/Content/alertify/css/semantic.min.css",
            //          "~/Content/css/style.css"));


            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //    "~/Content/bootstrap.css",
            //    "~/Content/site.css"));


            #if DEBUG
            BundleTable.EnableOptimizations = false;
            #else
                BundleTable.EnableOptimizations = true;
            #endif


            //BundleTable.EnableOptimizations = true;
            //BundleTable.EnableOptimizations = false;
        }
    }
}
