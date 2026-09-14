using Across;
using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CoreBusiness
{
    public class AusentismoVariablesCoreBusiness : CRUDGenerico<AusentismoVariables>, IAusentismoVariablesCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        ICommonCoreBusiness _iCommonCoreBusiness;
        IEmpleadosCoreBusiness _iEmpleadosCoreBusiness;

        public AusentismoVariablesCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            _iCommonCoreBusiness = new CommonCoreBusiness();
            _iEmpleadosCoreBusiness = new EmpleadosCoreBusiness();
        }

        #region CRUD Generico
        public async Task<List<AusentismoVariables>> ObtenerVariablesPorAnioAsync(int anio)
        {
            try
            {
                var listaVariables = await this.FindWhereAsync(x => x.IntAno == anio);

                return listaVariables;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> SaveEntityAsync(AusentismoVariables entity)
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

        private new async Task<string> CreateAsync(AusentismoVariables entity)
        {
            try
            {
                var listaVariables = await this.GetAllAsync();
                var isDuplicado = listaVariables.Any(x => x.IntAno == entity.IntAno && x.TIntPeriodo == entity.TIntPeriodo);
                if (isDuplicado) return string.Format(RecursoAusentismoVariables.msnRegistroDuplicado, entity.TIntPeriodo, entity.IntAno);

                var terceroID = await _iCommonCoreBusiness.GetTerceroIDFromCurrentUserAsync();

                entity.IntAusentismoVariablesID = entity.IntAusentismoVariablesID;
                entity.IntAno = entity.IntAno;
                entity.TIntPeriodo = entity.TIntPeriodo;
                entity.IntTotalEmpleados = entity.IntTotalEmpleados;
                entity.IntHorasHombre = entity.IntHorasHombre;
                entity.IntHorasHombreProgramadas = entity.IntHorasHombreProgramadas;
                entity.IntTerceroID = terceroID;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(AusentismoVariables entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<AusentismoVariables> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<AusentismoVariables, bool>> match)
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

        public new async Task<AusentismoVariables> FindAsync(Expression<Func<AusentismoVariables, bool>> match)
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

        public new List<AusentismoVariables> GetAll()
        {
            try
            {
                var terceroID = _iCommonCoreBusiness.GetTerceroIDFromCurrentUser();
                return base.GetAll().Where(x => x.IntTerceroID == terceroID).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<List<AusentismoVariables>> GetAllAsync()
        {
            try
            {
                var terceroID = await _iCommonCoreBusiness.GetTerceroIDFromCurrentUserAsync();
                var listaEntidad = await base.GetAllAsync();
                return listaEntidad.Where(x => x.IntTerceroID == terceroID).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private new async Task<string> UpdateAsync(AusentismoVariables entity)
        {
            try
            {
                var listaVariables = await this.GetAllAsync();
                var isDuplicado = listaVariables.Any(x => x.IntAno == entity.IntAno && x.TIntPeriodo == entity.TIntPeriodo && x.IntAusentismoVariablesID != entity.IntAusentismoVariablesID);
                if (isDuplicado) return string.Format(RecursoAusentismoVariables.msnRegistroDuplicado, entity.TIntPeriodo, entity.IntAno);

                var terceroID = await _iCommonCoreBusiness.GetTerceroIDFromCurrentUserAsync();

                var AusentismoVariables = await this.FindAsync(x => x.IntAusentismoVariablesID == entity.IntAusentismoVariablesID);

                AusentismoVariables.IntAusentismoVariablesID = entity.IntAusentismoVariablesID;
                AusentismoVariables.IntAno = entity.IntAno;
                AusentismoVariables.TIntPeriodo = entity.TIntPeriodo;
                AusentismoVariables.IntTotalEmpleados = entity.IntTotalEmpleados;
                AusentismoVariables.IntHorasHombre = entity.IntHorasHombre;
                AusentismoVariables.IntHorasHombreProgramadas = entity.IntHorasHombreProgramadas;
                AusentismoVariables.IntTerceroID = terceroID;

                await base.UpdateAsync(AusentismoVariables);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(AusentismoVariables model)
        {
            try
            {
                if (model.IntAusentismoVariablesID != 0) return await this.UpdateAsync(model);
                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        public async Task<int> GetTotalEmpleadosActivosAsync()
        {
            try
            {
                var listaEmpleados = await _iEmpleadosCoreBusiness.GetAllAsync();
                return listaEmpleados.Where(x => x.BitActivo == true).Count();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MesesDelAnioDTO> Obtener_NumeroHorasHombreTrabajadasPorPeriodo(List<AusentismoVariables> listaRegistros)
        {
            try
            {
                var listaMeses = Common.GetMesesDelAnio();
                var registroAusentismo = listaRegistros.FirstOrDefault();

                if (listaRegistros.Count() != 0)
                {
                    foreach (var item in listaMeses.OrderBy(x => x.Numero))
                    {
                        var listaAusentismoPorMes = listaRegistros.Where(x => x.IntAno == registroAusentismo.IntAno && x.TIntPeriodo == item.Numero).ToList();
                        item.Valor = (decimal)listaAusentismoPorMes.Sum(x => x.IntHorasHombre);
                    }
                }

                return listaMeses;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MesesDelAnioDTO> Obtener_NumeroHorasHombreProgramadasPorPeriodo(List<AusentismoVariables> listaRegistros)
        {
            try
            {
                var listaMeses = Common.GetMesesDelAnio();
                var registroAusentismo = listaRegistros.FirstOrDefault();

                if (listaRegistros.Count() != 0)
                {
                    foreach (var item in listaMeses.OrderBy(x => x.Numero))
                    {
                        var listaAusentismoPorMes = listaRegistros.Where(x => x.IntAno == registroAusentismo.IntAno && x.TIntPeriodo == item.Numero).ToList();
                        item.Valor = (decimal)listaAusentismoPorMes.Sum(x => x.IntHorasHombreProgramadas);
                    }
                }

                return listaMeses;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MesesDelAnioDTO> Obtener_NumeroDeTrabajadoresPorMes(List<AusentismoVariables> listaRegistros)
        {
            try
            {
                var listaMeses = Common.GetMesesDelAnio();
                var registroAusentismo = listaRegistros.FirstOrDefault();

                if (listaRegistros.Count() != 0)
                {
                    foreach (var item in listaMeses.OrderBy(x => x.Numero))
                    {
                        var listaAusentismoPorMes = listaRegistros.Where(x => x.IntAno == registroAusentismo.IntAno && x.TIntPeriodo == item.Numero).ToList();
                        item.Valor = (decimal)listaAusentismoPorMes.Sum(x => x.IntTotalEmpleados);
                    }
                }

                return listaMeses;
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

        ~AusentismoVariablesCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //if (_iCommonCoreBusiness != null)
                //{
                //    _iCommonCoreBusiness.Dispose();
                //    _iCommonCoreBusiness = null;
                //}

                if (_iEmpleadosCoreBusiness != null)
                {
                    _iEmpleadosCoreBusiness.Dispose();
                    _iEmpleadosCoreBusiness = null;
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
