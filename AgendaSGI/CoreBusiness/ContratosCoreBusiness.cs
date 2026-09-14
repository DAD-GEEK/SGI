using Across.Interfaces;
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
    public class ContratosCoreBusiness : CRUDGenerico<Contratos>, IContratos
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public ContratosCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(Contratos entity)
        {
            try
            {

                entity.IntNumeroContrato = entity.IntNumeroContrato;
                entity.DatFechaInicial = entity.DatFechaInicial;
                entity.DatFechaFinal = entity.DatFechaFinal;
                entity.IntValor = entity.IntValor;
                entity.IntHoras = entity.IntHoras;
                entity.StrEmail = entity.StrEmail;
                entity.IntClienteID = entity.IntClienteID;
                entity.OpcEstado = entity.OpcEstado;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Contratos entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Contratos> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Contratos, bool>> match)
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

        public new async Task<Contratos> FindAsync(Expression<Func<Contratos, bool>> match)
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

        public new async Task<List<Contratos>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Contratos entity)
        {
            try
            {

                Contratos Contratos = await this.FindAsync(x => x.IntContratoID == entity.IntContratoID);

                Contratos.IntContratoID = entity.IntContratoID;
                Contratos.IntNumeroContrato = entity.IntNumeroContrato;
                Contratos.DatFechaInicial = entity.DatFechaInicial;
                Contratos.DatFechaFinal = entity.DatFechaFinal;
                Contratos.IntValor = entity.IntValor;
                Contratos.IntHoras = entity.IntHoras;
                Contratos.StrEmail = entity.StrEmail;               
                Contratos.IntClienteID = entity.IntClienteID;
                Contratos.OpcEstado = entity.OpcEstado;

                await base.UpdateAsync(Contratos);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Contratos ConvertContratosFromDTO(ContratosDTO contratoDTO) 
        {
            try
            {
                Contratos contrato = new Contratos();

                contrato.IntContratoID = contratoDTO.IntContratoID;
                contrato.IntNumeroContrato = contratoDTO.IntNumeroContrato;
                contrato.DatFechaInicial = contratoDTO.DatFechaInicial;
                contrato.DatFechaFinal = contratoDTO.DatFechaFinal;
                contrato.IntValor = contratoDTO.IntValor;
                contrato.IntHoras = contratoDTO.IntHoras;
                contrato.StrEmail = contratoDTO.StrEmail;
                contrato.IntClienteID = contratoDTO.IntClienteID;
                contrato.OpcEstado = contratoDTO.OpcEstado;

                return contrato;
            }
            catch (Exception)
            {

                throw;
            }
        
        }


        public async Task<string> ValidarNumeracionContratos(int clienteID)
        {
            try
            {
                var contratos = this.GetAll().Where(x => x.IntClienteID == clienteID).OrderBy(x => x.IntContratoID);

                if (contratos.Count() != 0)
                {
                    var count = 1;
                    foreach (var item in contratos)
                    {
                        item.IntNumeroContrato = count;
                        count++;

                        await this.UpdateAsync(item);
                    }
                }

                return string.Empty;
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

        ~ContratosCoreBusiness()
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
