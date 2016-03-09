using System.Web;
using System.Web.Optimization;

namespace Bug_Tracker
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/vendor/js").Include(
                    "~/vendor/pacejs/pace.min.js",
                    "~/vendor/jquery/dist/jquery.min.js",
                    "~/vendor/bootstrap/js/bootstrap.min.js",
                    "~/vendor/datatables/datatables.min.js",
                    "~/vendor/toastr/toastr.min.js"));

            bundles.Add(new ScriptBundle("~/luna/js").Include(
                    "~/scripts/luna.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/vendor/css").Include(
                        "~/vendor/fontawesome/css/font-awesome.css",
                        "~/vendor/animate.css/animate.css",
                        "~/vendor/bootstrap/css/bootstrap.css",
                        "~/vendor/datatables/datatables.min.css",
                        "~/vendor/toastr/toastr.min.css"));

            //bundles.Add(new StyleBundle("~/styles/css").Include(
            //            "~/styles/pe-icons/pe-icon-7-stroke.css",
            //            "~/styles/pe-icons/helper.css",
            //            "~/styles/stroke-icons/style.css",
            //            "~/styles/style.css"));
        }
    }
}
