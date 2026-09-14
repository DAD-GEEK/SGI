using Across;
using CoreBusiness.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Across.Enumeraciones;

namespace CoreBusiness
{
    public class CommonCoreBusiness : ICommonCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        IAspNetUsersCoreBusiness _iAspNetUsersCoreBusiness;
        ITercerosCoreBusiness _iTercerosCoreBusiness;
        ITerceros_ClientesCoreBusiness _iTerceros_ClientesCoreBusiness;

        private int IntTerceroID { get; set; }

        public void AsignarTerceroID(int terceroID)
        {
            this.IntTerceroID = terceroID;
        }

        public int ObtenerTerceroID()
        {
            return this.IntTerceroID;
        }

        public CommonCoreBusiness()
        {
            this._iAspNetUsersCoreBusiness = new AspNetUsersCoreBusiness();
            this._iTercerosCoreBusiness = new TercerosCoreBusiness();
        }

        public async Task<int> GetTerceroIDFromCurrentUserAsync()
        {
            try
            {
                return await _iAspNetUsersCoreBusiness.GetTerceroIDFromCurrentUserAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetTerceroIDFromCurrentUser()
        {
            try
            {
                return _iAspNetUsersCoreBusiness.GetTerceroIDFromCurrentUser();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Terceros> GetTerceroModelFromCurrentUser()
        {
            try
            {
                var terceroID = await this.GetTerceroIDFromCurrentUserAsync();
                var terceroModel = await _iTercerosCoreBusiness.FindAsync(x => x.IntTerceroID == terceroID);

                return terceroModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<AspNetUsers> GetCurrentUser()
        {
            try
            {
                var usuarioName = HttpContext.Current.User.Identity.Name;
                return await _iAspNetUsersCoreBusiness.FindAsync(x => x.UserName == usuarioName);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetUrlPrincipal()
        {
            try
            {
                return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GenerarTerceroClienteGenericoAsync()
        {
            try
            {
                this._iTerceros_ClientesCoreBusiness = new Terceros_ClientesCoreBusiness();

                var terceroGenerico = new Terceros_Clientes();

                var terceroGeneral = ((int)enumGenericos.General).ToString();

                var existeGenerico = await _iTerceros_ClientesCoreBusiness.FindAsync(x => x.StrIdentificacion == terceroGeneral);

                if (existeGenerico is null)
                {
                    terceroGenerico.StrIdentificacion = ((int)enumGenericos.General).ToString();
                    terceroGenerico.StrNombre = "TERCERO GENÉRICO";
                    terceroGenerico.IntCiudadID = 1;
                    terceroGenerico.BitEstado = true;

                    await _iTerceros_ClientesCoreBusiness.GuardarRegistroAsync(terceroGenerico);
                }
                else
                    terceroGenerico.IntTerceroClienteID = existeGenerico.IntTerceroClienteID;

                return terceroGenerico.IntTerceroClienteID;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public SelectList DropDownListEnumerable<T>(string valueSelected = null)
        {
            try
            {
                var listaEnumerable = Common.GetEnumToSelectList<T>();

                var entidad = (from c in listaEnumerable
                               orderby c.Value
                               select new { CodigoID = c.Value, Descripcion = $"{c.Text}" });

                if (string.IsNullOrEmpty(valueSelected))
                    return new SelectList(entidad, "CodigoID", "Descripcion");

                return new SelectList(entidad, "CodigoID", "Descripcion", valueSelected);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SelectListItem> SeleccionarRegistroEnDropDownList(string id, string text)
        {
            try
            {
                List<SelectListItem> registroSeleccionado = new List<SelectListItem>();
                registroSeleccionado.Add(new SelectListItem { Value = id.ToString(), Text = text, Selected = true });

                return registroSeleccionado;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DatatableParamsDTO GetParametrosDataTable(HttpRequestBase Request)
        {
            try
            {
                DatatableParamsDTO datatableParamsDTO = new DatatableParamsDTO();
                datatableParamsDTO.draw = Request.Form.GetValues("draw").FirstOrDefault();
                datatableParamsDTO.start = Request.Form.GetValues("start").FirstOrDefault();
                datatableParamsDTO.length = Request.Form.GetValues("length").FirstOrDefault();
                datatableParamsDTO.sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                datatableParamsDTO.sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                datatableParamsDTO.searchValue = Request.Form["search[value]"];

                datatableParamsDTO.recordsTotal = 0;

                return datatableParamsDTO;
            }
            catch (Exception)
            {
                DatatableParamsDTO datatableParamsDTO = new DatatableParamsDTO();
                return datatableParamsDTO;
            }
        }

        #region Dispose

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CommonCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iAspNetUsersCoreBusiness != null)
                {
                    _iAspNetUsersCoreBusiness.Dispose();
                    _iAspNetUsersCoreBusiness = null;
                }

                if (_iTercerosCoreBusiness != null)
                {
                    _iTercerosCoreBusiness.Dispose();
                    _iTercerosCoreBusiness = null;
                }

            }

            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }
        }


        #endregion Dispose

    }
}
