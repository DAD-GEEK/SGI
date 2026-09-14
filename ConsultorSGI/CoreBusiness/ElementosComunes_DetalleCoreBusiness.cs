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
    public class ElementosComunes_DetalleCoreBusiness : CRUDGenerico<ElementosComunes_Detalle>, IElementosComunes_DetalleCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private INormasCoreBusiness _iNormasCoreBusiness;
        private IElementosComunes_NormasCoreBusiness _iElementosComunes_NormasCoreBusiness;
        private IElementosComunes_Detalle_NumeralesCoreBusiness _iElementosComunes_Detalle_NumeralesCoreBusiness;
        private INumeralesCoreBusiness _iNumeralesCoreBusiness;

        public ElementosComunes_DetalleCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iNormasCoreBusiness = new NormasCoreBusiness();
            this._iElementosComunes_NormasCoreBusiness = new ElementosComunes_NormasCoreBusiness();
            this._iElementosComunes_Detalle_NumeralesCoreBusiness = new ElementosComunes_Detalle_NumeralesCoreBusiness();
            this._iNumeralesCoreBusiness = new NumeralesCoreBusiness();
        }

        public async Task<ElementosComunesDTO> ObtenerElementoComunDTOPorIdAsync(int elementoComunID)
        {
            try
            {
                var listaNormas = await _iNormasCoreBusiness.FindWhereAsync(x => x.BitActivo == true);
                var listaNormasElementosComunes = await _iElementosComunes_NormasCoreBusiness.FindWhereAsync(x => x.IntElementoComunID == elementoComunID);
                var listaDetalle = await this.FindWhereAsync(x => x.IntElementoComunID == elementoComunID);

                return new ElementosComunesDTO()
                {
                    IntElementoComunID = elementoComunID,
                    Normas = listaNormas.Where(x => listaNormasElementosComunes.Any(y => y.IntNormaID == x.IntNormaID)).ToList(),
                    ElementosComunes_DetalleDTO = listaDetalle.Select(x => new ElementosComunes_DetalleDTO()
                    {
                        IntDetalleID = x.IntDetalleID,
                        ElementosComunes_Detalle_Numerales = x.ElementosComunes_Detalle_Numerales,
                        StrInterpretacion = x.StrInterpretacion,
                        BitActivo = x.BitActivo,

                    }).ToList()
                };


            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ElementosComunes_DetalleDTO> ObtenerElementosComunes_DetalleDTO(int elementoComunDetalleID)
        {
            try
            {
                var modelo = await this.FindAsync(x => x.IntDetalleID == elementoComunDetalleID);
                int elementoComunID = modelo.IntElementoComunID;
                var listaNormasNumerales = await this.ObtenerNumeralesPorNormasAsync(elementoComunID);

                return new ElementosComunes_DetalleDTO()
                {
                    listaNumeralesDTO = listaNormasNumerales,
                    ElementosComunes_Detalle_Numerales = modelo.ElementosComunes_Detalle_Numerales,
                    IntElementoComunID = elementoComunID,
                    IntDetalleID = modelo.IntDetalleID,
                    StrInterpretacion = modelo.StrInterpretacion,
                    BitActivo = modelo.BitActivo

                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<NumeralesDTO>> ObtenerNumeralesPorNormasAsync(int elementoComunID)
        {
            try
            {
                IElementosComunesCoreBusiness _iElementosComunesCoreBusiness = new ElementosComunesCoreBusiness();
                var modeloElementoComun = await _iElementosComunesCoreBusiness.FindAsync(x => x.IntElementoComunID == elementoComunID);

                var listaNumerales = await _iNumeralesCoreBusiness.FindWhereAsync(x => x.BitActivo == true);
                var listaNumeralesElementoComun = listaNumerales.Where(x => modeloElementoComun.ElementosComunes_Normas.ToList().Any(y => y.IntNormaID == x.IntNormaID)).ToList();

                var listaRespuesta = listaNumeralesElementoComun.Select(x => new NumeralesDTO()
                {
                    IntNormaID = x.IntNormaID,
                    StrCodigoNorma = x.Normas.StrCodigo,
                    StrDescripcionNorma = x.Normas.StrDescripcion,
                    IntNumeralID = x.IntNumeralID,
                    StrCodigo = x.StrCodigo,
                    StrDescripcion = $"{x.StrCodigo} - {x.StrDescripcion}"
                }).ToList();

                return listaRespuesta;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> SaveEntityAsync(ElementosComunes_Detalle entity)
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

        public new async Task<string> CreateAsync(ElementosComunes_Detalle entity)
        {
            try
            {
                entity.IntElementoComunID = entity.IntElementoComunID;
                entity.StrInterpretacion = entity.StrInterpretacion;
                entity.BitActivo = entity.BitActivo;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(ElementosComunes_Detalle entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<ElementosComunes_Detalle> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<ElementosComunes_Detalle, bool>> match)
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

        public new async Task<ElementosComunes_Detalle> FindAsync(Expression<Func<ElementosComunes_Detalle, bool>> match)
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

        public new List<ElementosComunes_Detalle> GetAll()
        {
            try
            {
                return base.GetAll().ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<List<ElementosComunes_Detalle>> GetAllAsync()
        {
            try
            {
                var listaEntidad = await base.GetAllAsync();
                return listaEntidad.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> UpdateAsync(ElementosComunes_Detalle entity)
        {
            try
            {
                var ElementosComunes_Detalle = await this.FindAsync(x => x.IntDetalleID == entity.IntDetalleID);

                ElementosComunes_Detalle.IntElementoComunID = entity.IntElementoComunID;
                ElementosComunes_Detalle.StrInterpretacion = entity.StrInterpretacion;
                ElementosComunes_Detalle.BitActivo = entity.BitActivo;

                await base.UpdateAsync(ElementosComunes_Detalle);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(ElementosComunes_Detalle model)
        {
            try
            {
                var listaNumeralesSeleccionados = model.ElementosComunes_Detalle_Numerales.ToList();

                if (model.IntDetalleID != 0)
                {
                    var modeloElementosComunesDetalle = await this.FindAsync(x => x.IntDetalleID == model.IntDetalleID);
                    var listaNumeralesEnBaseDeDatos = modeloElementosComunesDetalle.ElementosComunes_Detalle_Numerales.ToList();
                    var listaNumeralesParaEliminar = listaNumeralesEnBaseDeDatos.Where(x => !listaNumeralesSeleccionados.Any(y => y.IntNumeralID == x.IntNumeralID)).ToList();
                    var listaNumeralesParaCrear = listaNumeralesSeleccionados.Where(x => !listaNumeralesEnBaseDeDatos.Any(y => y.IntNumeralID == x.IntNumeralID)).ToList();

                    foreach (var item in listaNumeralesParaEliminar)
                    {
                        var modeloNumeral = await _iElementosComunes_Detalle_NumeralesCoreBusiness.FindAsync(x => x.IntRegistroID == item.IntRegistroID);
                        await _iElementosComunes_Detalle_NumeralesCoreBusiness.DeleteAsync(modeloNumeral);
                    }

                    foreach (var item in listaNumeralesParaCrear)
                    {
                        if (item.IntNumeralID != 0)
                        {
                            await _iElementosComunes_Detalle_NumeralesCoreBusiness.SaveAllAsync(new ElementosComunes_Detalle_Numerales()
                            {
                                IntDetalleID = item.IntDetalleID,
                                IntNumeralID = item.IntNumeralID,

                            });
                        }                        
                    }

                    return await this.UpdateAsync(model);

                }

                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public async Task<List<NormasDTO>> ObtenerNumeralesPorNormasDTODeElementosComunesAsync(List<Numerales> listaNumerales, List<Auditorias_Normas> listaNormasPorAuditoria, int elementoComunID)
        //{
        //    try
        //    {
        //        var listaElementosComunes = await this.FindWhereAsync(x => x.IntElementoComunID == elementoComunID);
        //        var listaElementosComunesNumerales = await _iElementosComunes_Detalle_NumeralesCoreBusiness.GetAllAsync();
        //        listaNumerales = listaNumerales.Where(x => listaNormasPorAuditoria.Any(y => y.IntNormaID == x.IntNormaID)).ToList();

        //        listaElementosComunesNumerales = listaElementosComunesNumerales.Where(x => listaNumerales.Any(y => y.IntNumeralID == x.IntNumeralID) && listaElementosComunes.Any(z => z.IntDetalleID == x.IntDetalleID)).ToList();
        //        listaElementosComunes = listaElementosComunes.Where(x => listaElementosComunesNumerales.Any(y => y.IntDetalleID == x.IntDetalleID)).ToList();
        //        //listaNumerales = listaNumerales.Where(x => listaElementosComunesNumerales.Any(y => y.IntNumeralID == x.IntNumeralID)).ToList();

        //        listaNumerales.ForEach(numeral => numeral.StrInterpretacion = String.Empty);

        //        listaElementosComunesNumerales.ForEach(numeral =>
        //        {
        //            listaNumerales.Find(x => x.IntNumeralID == numeral.IntNumeralID).StrInterpretacion = numeral.ElementosComunes_Detalle.StrInterpretacion;
        //        });

        //        var listaNormas = await _iNormasCoreBusiness.FindWhereAsync(x => x.BitActivo == true);

        //        List<NormasDTO> listaNormasDTO = new List<NormasDTO>();

        //        foreach (var item in listaNumerales.GroupBy(x => x.IntNormaID))
        //        {
        //            NormasDTO normasDTO = new NormasDTO();

        //            normasDTO.IntNormaID = (int)item.Key;
        //            normasDTO.StrCodigo = listaNormas.Find(norma => norma.IntNormaID == item.Key).StrCodigo;
        //            normasDTO.StrDescripcion = listaNormas.Find(norma => norma.IntNormaID == item.Key).StrDescripcion;
        //            normasDTO.Numerales = listaNumerales.Where(x => x.IntNormaID == item.Key).ToList();

        //            listaNormasDTO.Add(normasDTO);
        //        }

        //        return listaNormasDTO;

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public async Task<List<NormasDTO>> ObtenerNumeralesPorNormasDTODeElementosComunesAsync(List<Numerales> listaNumerales, List<Auditorias_Normas> listaNormasPorAuditoria, List<ElementosComunes> elementosComunesDeAuditoria)
        {
            try
            {
                var listaElementosComunes_Detalle = await this.GetAllAsync();
                listaElementosComunes_Detalle = listaElementosComunes_Detalle.Where(x => elementosComunesDeAuditoria.Any(y => y.IntElementoComunID == x.IntElementoComunID)).ToList();

                var listaElementosComunesNumerales = await _iElementosComunes_Detalle_NumeralesCoreBusiness.GetAllAsync();
                listaNumerales = listaNumerales.Where(x => listaNormasPorAuditoria.Any(y => y.IntNormaID == x.IntNormaID)).ToList();

                listaElementosComunesNumerales = listaElementosComunesNumerales.Where(x => listaNumerales.Any(y => y.IntNumeralID == x.IntNumeralID) && listaElementosComunes_Detalle.Any(z => z.IntDetalleID == x.IntDetalleID)).ToList();

                listaNumerales = listaNumerales.Where(x => listaElementosComunesNumerales.Any(y => y.IntNumeralID == x.IntNumeralID)).ToList();

                listaNumerales.ForEach(numeral => numeral.StrInterpretacion = String.Empty);

                listaElementosComunesNumerales.ForEach(numeral =>
                {
                    listaNumerales.Find(x => x.IntNumeralID == numeral.IntNumeralID).StrInterpretacion = numeral.ElementosComunes_Detalle.StrInterpretacion;
                });

                var listaNormas = await _iNormasCoreBusiness.FindWhereAsync(x => x.BitActivo == true);

                List<NormasDTO> listaNormasDTO = new List<NormasDTO>();

                foreach (var item in listaNumerales.GroupBy(x => x.IntNormaID))
                {
                    NormasDTO normasDTO = new NormasDTO();

                    normasDTO.IntNormaID = (int)item.Key;
                    normasDTO.StrCodigo = listaNormas.Find(norma => norma.IntNormaID == item.Key).StrCodigo;
                    normasDTO.StrDescripcion = listaNormas.Find(norma => norma.IntNormaID == item.Key).StrDescripcion;
                    normasDTO.Numerales = listaNumerales.Where(x => x.IntNormaID == item.Key).ToList();

                    listaNormasDTO.Add(normasDTO);
                }

                return listaNormasDTO;

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

        ~ElementosComunes_DetalleCoreBusiness()
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
