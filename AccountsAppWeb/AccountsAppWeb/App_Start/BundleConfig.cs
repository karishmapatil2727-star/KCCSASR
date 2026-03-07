using System.Web;
using System.Web.Optimization;

namespace AccountsAppWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/masterjs").Include(
                          "~/Scripts/jquery-3.3.1.min.js", "~/Scripts/jquery-ui.min.js",
                          "~/Scripts/bootstrap.min.js", "~/Scripts/jquery.validate.min.js",
                          "~/Scripts/jquery.validate.unobtrusive.min.js",
                          "~/Scripts/jquery.unobtrusive-ajax.min.js", "~/Scripts/DataTables/jquery.dataTables.min.js",
                          "~/Scripts/DataTables/dataTables.buttons.min.js", "~/Scripts/DataTables/buttons.flash.min.js",
                          "~/Scripts/DataTables/jszip.min.js", "~/Scripts/DataTables/pdfmake.min.js",
                          "~/Scripts/DataTables/vfs_fonts.js", "~/Scripts/DataTables/buttons.html5.min.js",
                          "~/Scripts/DataTables/buttons.print.min.js", "~/Scripts/bootstrap-datepicker.js", "~/Scripts/CustomScripts/jquery-ui.combobox.js",
                          "~/Scripts/metisMenu.min.js", "~/Scripts/sb-admin-2.js", "~/Scripts/CustomScripts/master.js"));

            bundles.Add(new StyleBundle("~/Content/mastercss").Include(
                "~/Content/jquery-ui.css", "~/Content/bootstrap.min.css",
                "~/Content/font-awesome.min.css", "~/Content/DataTables/css/jquery.dataTables.min.css",
                "~/Content/DataTables/css/buttons.dataTables.min.css", "~/Contents/bootstrap-datepicker3.min.css",
                "~/Content/sb-admin-2.min.css", "~/Content/metisMenu.min.css", "~/Content/custom.css"));
        }
    }
}
