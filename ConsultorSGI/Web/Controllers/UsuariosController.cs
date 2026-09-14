using CoreBusiness;
using CoreBusiness.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private ITercerosUsuariosCoreBusiness _iTercerosUsuariosCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public UsuariosController()
        {
            _iTercerosUsuariosCoreBusiness = new TercerosUsuariosCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }

        #region Vistas
        [HttpPost]
        public async Task<ActionResult> GetAllUsuariosAsync(int terceroID)
        {
            try
            {
                var listaUsuarios = await _iTercerosUsuariosCoreBusiness.GetAllAsync();
                listaUsuarios = listaUsuarios.Where(x => x.IntTerceroID == terceroID).ToList();

                return PartialView("_GetAllUsuarios", listaUsuarios);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllUsuarios", new AspNetUsers());
            }
        }
        #endregion

    }
}