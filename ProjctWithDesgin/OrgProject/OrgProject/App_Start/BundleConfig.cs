using System.Web;
using System.Web.Optimization;

namespace OrgProject
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // my All style
            bundles.Add(new StyleBundle("~/Content/Mycss").Include(
                "~/Content/alertifyjs/alertify.min.css",
                "~/Content/font-awesome.css",
                "~/Content/themes/base/autocomplete.css",
                "~/Content/jquery-ui.theme.css",
                "~/Content/font-awesome.min.css"

                ));






            // my all script
            bundles.Add(new ScriptBundle("~/myScript/jsfile").Include(
                "~/Scripts/jquery-3.3.1.min.js",
                "~/Scripts/alertify.min.js",
                "~/Scripts/jquery-ui.js"
                //,
                //"~/Scripts/jquery.validate.min.js",
                //"~/Scripts/jquery.validate.unobtrusive.js"
                
                ));

            //BundleTable.EnableOptimizations = true;






            // my All style New Layout
            bundles.Add(new StyleBundle("~/Content/MycssLayout2").Include(
                "~/Content/alertifyjs/alertify.min.css",
                "~/Content/bootstrap.min.css",
                "~/Content/themes/base/autocomplete.css",
                "~/Content/jquery-ui.theme.css",
                //"~/Content/font-awesome.min.css",
                "~/Content/VendorForAdminPage/font-awesome-5/css/fontawesome-all.min.css",
                "~/Content/MyStyle/main_styles.css",
                "~/Content/MyStyle/responsive.css",
                "~/Content/chosen.css"
                ));


            // my all script New Layout
            bundles.Add(new ScriptBundle("~/myScript/jsfileLayout2").Include(
                "~/Scripts/jquery-3.3.1.min.js",
               "~/Scripts/bootstrap.min.js",
                "~/Scripts/alertify.min.js",
                "~/Scripts/jquery-ui.js",
                 "~/Scripts/respond.js"
                ,"~/Scripts/MyScript/custom.js"
                ,"~/Scripts/MyScript/ScrollMagic.min.js" 
                , "~/Scripts/MyScript/popper.js",
                 "~/Scripts/chosen.jquery.min.js"

                //,"~/Scripts/MyScript/owl.carousel.js"


                ));








        }
    }
}
