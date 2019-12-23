using System.Web;
using System.Web.Optimization;

namespace ArchiveProject2019
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

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-arabic.css",
                      "~/Content/site.css"));


            bundles.Add(new StyleBundle("~/Content/DashBoardCss").Include(
              // Cutom Chosen
              "~/Content/chosen.css",
              //Data Tabel Css :
              "~/Content/DataTables/css/dataTables.bootstrap.css",
              //Bootstrap:
              "~/Content/DashBoard/vendors/bootstrap/dist/css/bootstrap.min.css",
              "~/Content/DashBoard/vendors/bootstrap-rtl/dist/css/bootstrap-rtl.min.css",
              //Font Awesome
              "~/Content/DashBoard/vendors/font-awesome/css/font-awesome.min.css",
              //NProgress
           //   "~/Content/DashBoard/vendors/nprogress/nprogress.css",
              //bootstrap-progressbar
              "~/Content/DashBoard/vendors/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css",
              //icheck
             "~/Content/DashBoard/vendors/iCheck/skins/flat/green.css",
              //DataPacker
              "~/Content/DashBoard/vendors/bootstrap-daterangepicker/daterangepicker.css",
              //jQuery custom content scroller
              "~/Content/DashBoard/vendors/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.min.css",
              
              //Custom Theme Style
              "~/Content/DashBoard/build/css/custom.min.css",

              // Custom Scanner css
              "~/Content/scanner.css"

             ));





            bundles.Add(new ScriptBundle("~/Content/DashBoardjs").Include(
               //Jquerey
               //"~/Content/DashBoard/vendors/jquery/dist/jquery.min.js",
               //"~/Scripts/jquery-1.10.2.min.js",
               // Cutom Chosen
               "~/Scripts/chosen.jquery.min.js",
               "~/Scripts/chosen.proto.min.js",
              "~/Scripts/jquery-{version}.js",
              "~/Scripts/jquery.unobtrusive-ajax.min.js",

              "~/Scripts/ jquery.validate.unobtrusive.min.js",

               //Bootstrap
               "~/Content/DashBoard/vendors/bootstrap/dist/js/bootstrap.min.js",
               //FastClick
               "~/Content/DashBoard/vendors/fastclick/lib/fastclick.js",
               //nprogress
            //   "~/Content/DashBoard/vendors/nprogress/nprogress.js",
               //bootstrap-progressbar
               "~/Content/DashBoard/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js",
               //icheck
               //bootstrap-daterangepicker
               "~/Content/DashBoard/vendors/moment/min/moment.min.js",
               "~/Content/DashBoard/vendors/bootstrap-daterangepicker/daterangepicker.js",
               //jquery.mCustomScrollbar
              "~/Content/DashBoard/vendors/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.concat.min.js",

               "~/Content/DashBoard/build/js/custom.min.js",
               "~/Content/DashBoard/vendors/iCheck/icheck.min.js",

               //DataTables:
               "~/Scripts/DataTables/jquery.dataTables.min.js",
               "~/Scripts/DataTables/dataTables.bootstrap.js",

               // Custom Scanner js
               "~/Scripts/scanner.js"



              ));

        }
    }
}
