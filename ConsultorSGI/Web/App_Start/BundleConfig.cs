using System.Web;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                        "~/Content/MiStyle.css",
                        "~/Vendor/select2/dist/css/select2.min.css",//Debe estar primero que adminLTE
                        "~/Vendor/select2-theme-bootstrap4/dist/select2-bootstrap.min.css",
                        "~/AdminLTE/dist/css/adminlte.min.css",
                        "~/Vendor/tempusdominus-bootstrap-4/build/css/tempusdominus-bootstrap-4.min.css",
                        "~/content/parsley.css",
                        "~/Content/whirl.css",
                        "~/Vendor/sweetalert2/dist/sweetalert2.min.css",
                        "~/Vendor/icheck-bootstrap/icheck-bootstrap.min.css",
                        "~/Vendor/daterangepicker/daterangepicker.css",
                        "~/Vendor/datatables.net-bs4/css/dataTables.bootstrap4.min.css",
                        "~/Vendor/datatables.net-responsive-bs4/css/responsive.bootstrap4.min.css",
                        "~/Vendor/bs-stepper/dist/css/bs-stepper.min.css",
                        "~/Vendor/toastr/build/toastr.min.css",
                        "~/AdminLTE/dist/css/fileinput.min.css",
                        "~/AdminLTE/bootstrap-datetimepicker/css/bootstrap-datetimepicker.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                       "~/AdminLTE/dist/js/bootstrap.bundle.min.js",
                       "~/Vendor/select2/dist/js/select2.full.min.js",
                       "~/AdminLTE/dist/js/adminlte.min.js",
                       "~/Vendor/moment/min/moment.min.js",
                       "~/Vendor/moment/locale/es-mx.js",
                       "~/Vendor/chart.js/dist/Chart.min.js",
                       "~/Vendor/sparklines/source/sparkline.js",
                       "~/Vendor/daterangepicker/daterangepicker.js",
                       "~/Vendor/tempusdominus-bootstrap-4/build/js/tempusdominus-bootstrap-4.min.js",
                       "~/Vendor/sweetalert2/dist/sweetalert2.js",
                       "~/Vendor/toastr/build/toastr.min.js",
                       "~/Vendor/datatables.net/js/jquery.dataTables.min.js",
                       "~/Vendor/datatables.net-bs4/js/dataTables.bootstrap4.min.js",
                       "~/Vendor/datatables.net-responsive/js/dataTables.responsive.min.js",
                       "~/Vendor/datatables.net-responsive-bs4/js/responsive.bootstrap4.min.js",
                       "~/Vendor/inputmask/dist/jquery.inputmask.min.js",
                       "~/Vendor/bs-stepper/dist/js/bs-stepper.min.js",
                       "~/Scripts/propios/waitingDialog.js",
                       "~/AdminLTE/dist/js/fileinput.min.js",
                       "~/AdminLTE/dist/js/es.js",
                       "~/Scripts/appOptions.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                        "~/Scripts/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/vistasParciales").Include(
                        "~/Scripts/common/parsley.js",
                        "~/AdminLTE/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js",
                        "~/Scripts/custom/InicializadorPlugins.js",
                        "~/Scripts/modulos/Funciones_Globales/parsleyValidationsForms.js",
                        "~/Scripts/modulos/Funciones_Globales/funcionesGlobales.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
