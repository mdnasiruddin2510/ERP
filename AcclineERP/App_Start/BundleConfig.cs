using System.Web;
using System.Web.Optimization;

namespace PoIpaWeb
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/App_Themes/Theme1/blue/assets/js/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            //<!--Plugins-->
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                       "~/App_Themes/Theme1/blue/assets/js/detect.js",
                        "~/App_Themes/Theme1/blue/assets/js/fastclick.js",
                         "~/App_Themes/Theme1/blue/assets/js/jquery.slimscroll.js",
                          "~/App_Themes/Theme1/blue/assets/js/jquery.blockUI.js",
                           "~/App_Themes/Theme1/blue/assets/js/waves.js",
                            "~/App_Themes/Theme1/blue/assets/js/wow.min.js",
                             "~/App_Themes/Theme1/blue/assets/js/jquery.nicescroll.js",
                              "~/App_Themes/Theme1/blue/assets/js/jquery.scrollTo.min.js"));
            // <!-- Counter Up  -->
            bundles.Add(new ScriptBundle("~/App_Themes/Theme1/plugins/waypoints").Include(
                "~/App_Themes/Theme1/plugins/waypoints/lib/jquery.waypoints.js"));
            bundles.Add(new ScriptBundle("~/App_Themes/Theme1/plugins/counterup").Include(
                "~/App_Themes/Theme1/plugins/counterup/jquery.counterup.min.js"));
            //  <!-- circliful Chart -->
            bundles.Add(new ScriptBundle("~/App_Themes/Theme1/plugins/jquery-circliful/js").Include(
                "~/App_Themes/Theme1/plugins/jquery-circliful/js/jquery.circliful.min.js"));
            bundles.Add(new ScriptBundle("~/App_Themes/Theme1/plugins/jquery-sparkline").Include(
                "~/App_Themes/Theme1/plugins/jquery-sparkline/jquery.sparkline.min.js"));
            // <!-- skycons -->
            bundles.Add(new ScriptBundle("~/App_Themes/Theme1/plugins/skyicons").Include(
                "~/App_Themes/Theme1/plugins/skyicons/skycons.min.js"));
            //<!-- Page js  -->
            bundles.Add(new ScriptBundle("~/App_Themes/Theme1/blue/assets/pages").Include(
                "~/App_Themes/Theme1/blue/assets/pages/jquery.dashboard.js"));
            // <!-- Custom main Js -->
            bundles.Add(new ScriptBundle("~/App_Themes/Theme1/blue/assets/js").Include(
                "~/App_Themes/Theme1/blue/assets/js/jquery.core.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/App_Themes/Theme1/blue/assets/js/bootstrap.min.js"));



            //E:\All Subversion Project\Shah\Main\ShahcementWeb\PoIpaWeb\App_Themes\Theme1\blue\assets\css\bootstrap.min.css
            bundles.Add(new StyleBundle("~/App_Themes/Theme1/plugins/switchery").Include(
                     "~/App_Themes/Theme1/plugins/switchery/switchery.min.css"));
            bundles.Add(new StyleBundle("~/App_Themes/Theme1/plugins/jquery-circliful").Include(
                     "~/App_Themes/Theme1/plugins/jquery-circliful/css/jquery.circliful.css"));
            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css"));E:\All Subversion Project\Shah\Main\ShahcementWeb\PoIpaWeb\App_Themes\Theme1\blue\assets\js\bootstrap.min.js
                        bundles.Add(new StyleBundle("~/App_Themes/Theme1/blue/assets/css").Include(
                      "~/App_Themes/Theme1/blue/assets/css/bootstrap.min.css"));
            // "~/Content/site.css"
            bundles.Add(new StyleBundle("~/App_Themes/Theme1/blue/assets/css").Include(
                      "~/App_Themes/Theme1/blue/assets/css/core.css",
                      "~/App_Themes/Theme1/blue/assets/css/icons.css",
                      "~/App_Themes/Theme1/blue/assets/css/components.css",
                      "~/App_Themes/Theme1/blue/assets/css/pages.css",
                      "~/App_Themes/Theme1/blue/assets/css/menu.css",
                      "~/App_Themes/Theme1/blue/assets/css/responsive.css"));
        }
    }
}