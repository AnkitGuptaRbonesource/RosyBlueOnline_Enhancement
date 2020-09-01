using System.Web;
using System.Web.Optimization;

namespace Rosyblueonline.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/custom/jquery.validate.additionalMethods.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/moment.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootbox.js",
                       //"~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/jquery.blockUI.js",
                        "~/Scripts/jquery.cookie.js",
                        "~/Scripts/jQuery.tmpl.js"));

            bundles.Add(new ScriptBundle("~/bundles/Admin/Script").Include(
                  "~/Scripts/custom/themeApp.js",
                  "~/Scripts/custom/uiApp.js",
            "~/Scripts/locales/pagination.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/content/font-awesome/css/font-awesome.min.css",
                      "~/Content/css/style.css",
                      "~/Content/css/custom.css",
                       "~/Content/Datepicker/jquery.ui.datepicker.css",
                        "~/Content/Datepicker/jquery-ui.css",
                         "~/Content/Datepicker/jquery-ui.min.css",
                          "~/Content/Datepicker/jquery-ui.theme.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/Admin/Css").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/AdminLTE.css",
                      "~/Content/css/AdminLTE.min.css",
                      "~/Content/css/dashboard.css",
                      "~/Content/css/_all-skins.min.css",
                      "~/Content/css/tablestyle.css",
                      "~/Content/css/custom.css",
                      "~/content/font-awesome/css/font-awesome.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/DataTable/Script").Include(
                  "~/Scripts/DataTables/jquery.dataTables.min.js",
                  "~/Scripts/DataTables/dataTables.fixedColumns.js",
                  "~/Scripts/DataTables/dataTables.bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/bundles/DataTable/Style").Include(
                  //"~/Content/DataTables/css/jquery.dataTables.min.css",
                  //"~/Content/DataTables/css/responsive.bootstrap.css",
                  "~/Content/DataTables/css/dataTables.bootstrap.min.css"));


            //BundleTable.EnableOptimizations = true;
        }
    }
}
