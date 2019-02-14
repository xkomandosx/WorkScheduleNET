using System.Web;
using System.Web.Optimization;

namespace WorkSchedule
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-1.12.1.js",
                        "~/Scripts/jquery-ui-timepicker-addon.js",
                        "~/Scripts/jquery.stickytableheaders.min.js",
                        "~/Scripts/datepicker-pl.js",
                        "~/Scripts/jszip.min.js",
                        "~/Scripts/jquery-ui-timepicker-pl.js",
                        "~/Scripts/DataTables/jquery.dataTables.js",
                        "~/Scripts/DataTables/dataTables.bootstrap.js",
                        "~/Scripts/DataTables/dataTables.buttons.min.js",
                        "~/Scripts/DataTables/buttons.html5.min.js",
                        "~/Scripts/pdfmake/pdfmake.min.js",
                        "~/Scripts/pdfmake/vfs_fonts.js",
                        "~/Scripts/DataTables/dataTables.bootstrap4.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/umd/popper.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootbox.js",
                      "~/Scripts/fullcalendar.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-united.css"));

            bundles.Add(new StyleBundle("~/Content/cssrest").Include(
                      "~/Content/fullcalendar.css",
                      "~/Content/themes/base/jquery-ui.css",
                      "~/Content/themes/base/jquery-ui-timepicker-addon.css",
                      "~/Content/DataTables/css/dataTables.bootstrap.css",
                      "~/Content/DataTables/css/buttons.dataTables.css",
                      "~/Content/DataTables/css/jquery.dataTables.css",
                      "~/Content/DataTables/css/responsive.bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
