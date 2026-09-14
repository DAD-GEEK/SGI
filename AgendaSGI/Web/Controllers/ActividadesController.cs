using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CoreBusiness;
using Models;

namespace Web.Controllers
{
    public class ActividadesController : Controller
    {
        ActividadesActaCoreBusiness _actividadesActaCoreBusiness;
        LogsExCoreBusiness _logsExCoreBusiness;

        [HttpPost]
        public async Task<ActionResult> UpdateEjecutaActividadAsync(ActividadesActa modelo, int actaID)
        {
            try
            {
                _actividadesActaCoreBusiness = new ActividadesActaCoreBusiness();

                var actividad = await _actividadesActaCoreBusiness.FindAsync(x => x.IntActividadID == modelo.IntActividadID);

                if (actividad != null)
                {
                    actividad = _actividadesActaCoreBusiness.UnescapeActividadesActa(actividad);

                    modelo.IntActaID = actividad.IntActaID;
                    modelo.StrDescripcion = actividad.StrDescripcion;
                    modelo.StrResponsable = actividad.StrResponsable;
                    modelo.DatFecha = actividad.DatFecha;

                    string retur = await _actividadesActaCoreBusiness.UpdateAsync(modelo);

                    if (string.IsNullOrEmpty(retur))
                        return Json(new { msn = "success", actaID = actaID }, JsonRequestBehavior.AllowGet);                    

                    return Json(new { error = retur }, JsonRequestBehavior.AllowGet);

                }

                return Json(new { error = "Recurso no existe." }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                string mensaje = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}