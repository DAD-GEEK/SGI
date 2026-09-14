using Across;
using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Across.Enumeraciones;

namespace CoreBusiness
{
    public class AuditoriasDetalleACCoreBusiness : CRUDGenerico<AuditoriasDetalleAC>, IAuditoriasDetalleACCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private IAuditorias_ProcesosCoreBusiness _iAuditorias_ProcesosCoreBusiness;

        public AuditoriasDetalleACCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iAuditorias_ProcesosCoreBusiness = new Auditorias_ProcesosCoreBusiness();
        }

        #region CRUD Generico
        public new async Task<string> SaveEntityAsync(AuditoriasDetalleAC entity)
        {
            try
            {
                await base.SaveEntityAsync(entity);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> CreateAsync(AuditoriasDetalleAC entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.IntAuditoriaID == entity.IntAuditoriaID && ((x.DatFechaInicial >= entity.DatFechaInicial && x.DatFechaInicial <= entity.DatFechaFinal) || (x.DatFechaFinal >= entity.DatFechaInicial && x.DatFechaFinal <= entity.DatFechaFinal) || (x.DatFechaInicial <= entity.DatFechaInicial && x.DatFechaFinal >= entity.DatFechaFinal)));
                if (listaRegistros.Count() != 0)
                    return RecursoAuditorias.msnAperturaCierreYaExiste;

                entity.IntAuditoriaID = entity.IntAuditoriaID;
                entity.StrDetalle = entity.StrDetalle;
                entity.DatFechaInicial = Common.ObtenerFechaExacta(entity.DatFechaInicial);
                entity.DatFechaFinal = Common.ObtenerFechaExacta(entity.DatFechaFinal);

                await base.CreateAsync(entity);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(AuditoriasDetalleAC entity)
        {
            try
            {
                await base.DeleteAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteRangeAsync(IEnumerable<AuditoriasDetalleAC> entity)
        {
            try
            {
                await base.DeleteRangeAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<bool> ExistAsync(Expression<Func<AuditoriasDetalleAC, bool>> match)
        {
            try
            {
                return await base.ExistAsync(match);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<AuditoriasDetalleAC> FindAsync(Expression<Func<AuditoriasDetalleAC, bool>> match)
        {
            try
            {
                return await base.FindAsync(match);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<List<AuditoriasDetalleAC>> GetAllAsync()
        {
            try
            {
                return await base.GetAllAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> UpdateAsync(AuditoriasDetalleAC entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.IntAuditoriaID == entity.IntAuditoriaID && x.IntAuditoriaDetalleACID != entity.IntAuditoriaDetalleACID && ((x.DatFechaInicial >= entity.DatFechaInicial && x.DatFechaInicial <= entity.DatFechaFinal) || (x.DatFechaFinal >= entity.DatFechaInicial && x.DatFechaFinal <= entity.DatFechaFinal) || (x.DatFechaInicial <= entity.DatFechaInicial && x.DatFechaFinal >= entity.DatFechaFinal)));
                if (listaRegistros.Count() != 0)
                    return RecursoAuditorias.msnAperturaCierreYaExiste;

                AuditoriasDetalleAC AuditoriasDetalleAC = await this.FindAsync(x => x.IntAuditoriaDetalleACID == entity.IntAuditoriaDetalleACID);

                AuditoriasDetalleAC.IntAuditoriaID = entity.IntAuditoriaID;
                AuditoriasDetalleAC.StrDetalle = entity.StrDetalle;
                AuditoriasDetalleAC.DatFechaInicial = Common.ObtenerFechaExacta(entity.DatFechaInicial);
                AuditoriasDetalleAC.DatFechaFinal = Common.ObtenerFechaExacta(entity.DatFechaFinal);

                await base.UpdateAsync(AuditoriasDetalleAC);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(AuditoriasDetalleAC model)
        {
            try
            {
                if (model.DatFechaInicial > model.DatFechaFinal)
                    return RecursoCommon.msnHoraInicialMayorHoraFinal;

                if (model.IntAuditoriaDetalleACID != 0)
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion

        #region Select List
        public async Task<MultiSelectList> SelectListAuditoriaProcesosAsync(int auditoriaID, string valorSeleccionado = null)
        {
            try
            {
                return await _iAuditorias_ProcesosCoreBusiness.SelectListAsync(auditoriaID, valorSeleccionado);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public SelectList SelectListModalidades(string valueSelected = null)
        {
            try
            {
                var listaRegistros = Common.GetEnumToSelectList<enumModalidades>();

                var entidad = listaRegistros.Select(x => new SelectListItem()
                {
                    Value = x.Text[0].ToString().ToUpper(),
                    Text = $"{x.Text}"
                }).ToList();

                if (string.IsNullOrEmpty(valueSelected))
                    return new SelectList(entidad, "Value", "Text");

                return new SelectList(entidad, "Value", "Text", valueSelected);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Dispose

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AuditoriasDetalleACCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iAuditorias_ProcesosCoreBusiness != null)
                {
                    _iAuditorias_ProcesosCoreBusiness.Dispose();
                    _iAuditorias_ProcesosCoreBusiness = null;
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
