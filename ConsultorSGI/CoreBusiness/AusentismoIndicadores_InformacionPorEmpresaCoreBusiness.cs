using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using DataAccess.Servicios;
using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static Across.Enumeraciones;

namespace CoreBusiness
{
    public class AusentismoIndicadores_InformacionPorEmpresaCoreBusiness : CRUDGenerico<AusentismoIndicadores_InformacionPorEmpresa>, IAusentismoIndicadores_InformacionPorEmpresaCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        public AusentismoIndicadores_InformacionPorEmpresaCoreBusiness() : base(new gestioni_consultorNetEntities(new ServicioTercero())) { }

        public new async Task<string> SaveEntityAsync(AusentismoIndicadores_InformacionPorEmpresa entity)
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

        public new async Task<string> CreateAsync(AusentismoIndicadores_InformacionPorEmpresa entity)
        {
            try
            {
                var indicadorPorEmpresa = await this.FindAsync(x => x.StrAusentismoIndicadoresID == entity.StrAusentismoIndicadoresID);

                if (indicadorPorEmpresa != null)
                    return "Ya existe el indicador";

                entity.StrId = Guid.NewGuid().ToString();

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(AusentismoIndicadores_InformacionPorEmpresa entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<AusentismoIndicadores_InformacionPorEmpresa> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<AusentismoIndicadores_InformacionPorEmpresa, bool>> match)
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

        public new async Task<AusentismoIndicadores_InformacionPorEmpresa> FindAsync(Expression<Func<AusentismoIndicadores_InformacionPorEmpresa, bool>> match)
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

        public new async Task<List<AusentismoIndicadores_InformacionPorEmpresa>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(AusentismoIndicadores_InformacionPorEmpresa entity)
        {
            try
            {
                var indicadoresInformacion = await this.FindAsync(x => x.StrId == entity.StrId);

                if (indicadoresInformacion is null)
                    return RecursoAusentismoIndicadores.msnIndicadorPorEmpresaNoExiste;

                indicadoresInformacion.StrProceso = entity.StrProceso;
                indicadoresInformacion.StrCargos = entity.StrCargos;
                indicadoresInformacion.StrResponsable = entity.StrResponsable;
                indicadoresInformacion.StrTipoGrafico = entity.StrTipoGrafico;
                indicadoresInformacion.StrFrecuencia = entity.StrFrecuencia;
                indicadoresInformacion.IntMeta_1 = entity.IntMeta_1;
                indicadoresInformacion.IntMeta_2 = entity.StrFrecuencia == enumFrecuenciaMedicion.anual.ToString() ? entity.IntMeta_1 : entity.IntMeta_2;
                indicadoresInformacion.IntMeta_3 = entity.StrFrecuencia == enumFrecuenciaMedicion.anual.ToString() ? entity.IntMeta_1 : entity.IntMeta_3;
                indicadoresInformacion.IntMeta_4 = entity.StrFrecuencia == enumFrecuenciaMedicion.anual.ToString() ? entity.IntMeta_1 : entity.IntMeta_4;
                indicadoresInformacion.IntMeta_5 = entity.StrFrecuencia == enumFrecuenciaMedicion.anual.ToString() ? entity.IntMeta_1 : entity.IntMeta_5;
                indicadoresInformacion.IntMeta_6 = entity.StrFrecuencia == enumFrecuenciaMedicion.anual.ToString() ? entity.IntMeta_1 : entity.IntMeta_6;
                indicadoresInformacion.IntMeta_7 = entity.StrFrecuencia == enumFrecuenciaMedicion.anual.ToString() ? entity.IntMeta_1 : entity.IntMeta_7;
                indicadoresInformacion.IntMeta_8 = entity.StrFrecuencia == enumFrecuenciaMedicion.anual.ToString() ? entity.IntMeta_1 : entity.IntMeta_8;
                indicadoresInformacion.IntMeta_9 = entity.StrFrecuencia == enumFrecuenciaMedicion.anual.ToString() ? entity.IntMeta_1 : entity.IntMeta_9;
                indicadoresInformacion.IntMeta_10 = entity.StrFrecuencia == enumFrecuenciaMedicion.anual.ToString() ? entity.IntMeta_1 : entity.IntMeta_10;
                indicadoresInformacion.IntMeta_11 = entity.StrFrecuencia == enumFrecuenciaMedicion.anual.ToString() ? entity.IntMeta_1 : entity.IntMeta_11;
                indicadoresInformacion.IntMeta_12 = entity.StrFrecuencia == enumFrecuenciaMedicion.anual.ToString() ? entity.IntMeta_1 : entity.IntMeta_12;

                await base.UpdateAsync(indicadoresInformacion);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarAsync(AusentismoIndicadores_InformacionPorEmpresa modelo)
        {
            try
            {
                if (string.IsNullOrEmpty(modelo.StrId))
                    return await this.CreateAsync(modelo);

                return await this.UpdateAsync(modelo);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Dispose

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AusentismoIndicadores_InformacionPorEmpresaCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {


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
