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
using System.Web.Mvc;

namespace CoreBusiness
{
    public class ListasDeVerificacionCoreBusiness : CRUDGenerico<ListasDeVerificacion>, IListasDeVerificacionCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private IAuditoriasCoreBusiness _iAuditoriasCoreBusiness;
        private IAuditoriasDetalleCoreBusiness _iAuditoriasDetalleCoreBusiness;
        private IAuditorias_NormasCoreBusiness _iAuditorias_NormasCoreBusiness;
        private INormasCoreBusiness _iNormasCoreBusiness;
        private INumeralesCoreBusiness _iNumeralesCoreBusiness;
        private ITerceros_NormasCoreBusiness _iTerceros_NormasCoreBusiness;
        private IElementosComunes_DetalleCoreBusiness _iElementosComunes_DetalleCoreBusiness;
        private IProcesosCoreBusiness _iProcesosCoreBusiness;
        private IPlantillasListasDeVerificacionDetalleCoreBusiness _iPlantillasListasDeVerificacionDetalleCoreBusiness;
        private IListasDeVerificacion_RequisitosCoreBusiness _iListasDeVerificacion_RequisitosCoreBusiness;

        public ListasDeVerificacionCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iAuditoriasCoreBusiness = new AuditoriasCoreBusiness();
            this._iAuditoriasDetalleCoreBusiness = new AuditoriasDetalleCoreBusiness();
            this._iNormasCoreBusiness = new NormasCoreBusiness();
            this._iNumeralesCoreBusiness = new NumeralesCoreBusiness();
            this._iTerceros_NormasCoreBusiness = new Terceros_NormasCoreBusiness();
            this._iElementosComunes_DetalleCoreBusiness = new ElementosComunes_DetalleCoreBusiness();
            this._iAuditorias_NormasCoreBusiness = new Auditorias_NormasCoreBusiness();
            this._iProcesosCoreBusiness = new ProcesosCoreBusiness();
            this._iPlantillasListasDeVerificacionDetalleCoreBusiness = new PlantillasListasDeVerificacionDetalleCoreBusiness();
            this._iListasDeVerificacion_RequisitosCoreBusiness = new ListasDeVerificacion_RequisitosCoreBusiness();
        }

        public new async Task<string> SaveEntityAsync(ListasDeVerificacion entity)
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

        public new async Task<string> CreateAsync(ListasDeVerificacion entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.IntAuditoriaDetalleID == entity.IntAuditoriaDetalleID);
                var listaRegistrosDuplicados = listaRegistros.Where(x => x.StrTitulo.Trim().ToLower() == entity.StrTitulo.Trim().ToLower()).ToList();

                if (listaRegistrosDuplicados.Count() > 0)
                    return String.Format(RecursoListasDeVerificacion.msnRegistroYaExiste, entity.StrTitulo);

                entity.IntAuditoriaDetalleID = entity.IntAuditoriaDetalleID;
                entity.StrTitulo = entity.StrTitulo.ToUpper();
                entity.StrHallazgo = entity.StrHallazgo;
                entity.BitNoConformidad = entity.BitNoConformidad;
                entity.BitConformidad = entity.BitConformidad;
                entity.BitObservacion = entity.BitObservacion;

                await base.CreateAsync(entity);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> CreateRangeAsync(List<ListasDeVerificacion> listEntities)
        {
            try
            {
                if (listEntities.Count() == 0)
                    return string.Empty;

                await base.CreateRangeAsync(listEntities);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public new async Task DeleteAsync(ListasDeVerificacion entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<ListasDeVerificacion> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<ListasDeVerificacion, bool>> match)
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

        public new async Task<ListasDeVerificacion> FindAsync(Expression<Func<ListasDeVerificacion, bool>> match)
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

        public new async Task<List<ListasDeVerificacion>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(ListasDeVerificacion entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.StrTitulo.Trim().ToLower() == entity.StrTitulo.Trim().ToLower() && x.IntListaVerificacionID != entity.IntListaVerificacionID && x.IntAuditoriaDetalleID == entity.IntAuditoriaDetalleID);

                if (listaRegistros.Count() > 0)
                    return String.Format(RecursoListasDeVerificacion.msnRegistroYaExiste, entity.StrTitulo);

                ListasDeVerificacion ListasDeVerificacion = await this.FindAsync(x => x.IntListaVerificacionID == entity.IntListaVerificacionID);

                ListasDeVerificacion.IntAuditoriaDetalleID = entity.IntAuditoriaDetalleID;
                ListasDeVerificacion.StrTitulo = entity.StrTitulo;
                ListasDeVerificacion.DatFechaEnQueAudita = entity.DatFechaEnQueAudita;

                await base.UpdateAsync(ListasDeVerificacion);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(ListasDeVerificacion model, int procesoID = 0)
        {
            try
            {
                if (model.IntListaVerificacionID != 0)
                    return await this.UpdateAsync(model);

                var plantillaListaDeVerificacion = new PlantillasListasDeVerificacionDetalle()
                {
                    IntProcesoID = procesoID,
                    StrTitulo = model.StrTitulo,
                    BitEstado = true
                };

                var listaRegistros = await _iPlantillasListasDeVerificacionDetalleCoreBusiness.FindWhereAsync(x => x.IntProcesoID == procesoID);
                var listaRegistrosDuplicados = listaRegistros.Where(x => x.StrTitulo.Trim().ToLower() == model.StrTitulo.Trim().ToLower()).ToList();

                if (listaRegistrosDuplicados.Count() == 0)
                {
                    await _iPlantillasListasDeVerificacionDetalleCoreBusiness.SaveAllAsync(plantillaListaDeVerificacion);
                    model.IntPlantillaDetalleID = plantillaListaDeVerificacion.IntPlantillaDetalleID;
                }
                else
                    model.IntPlantillaDetalleID = listaRegistros.FirstOrDefault(x => x.StrTitulo.Trim().ToLower() == model.StrTitulo.Trim().ToLower()).IntPlantillaDetalleID;

                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ListasDeVerificacionDTO> ObtenerItemListaDeVerificacionDTOAsync(ListasDeVerificacion listasDeVerificacion)
        {
            try
            {
                List<Normas> listaNormas = await _iNormasCoreBusiness.FindWhereAsync(x => x.BitActivo == true);
                List<Auditorias_Normas> listaNormasAuditoria = listasDeVerificacion.AuditoriasDetalle.Auditorias.Auditorias_Normas.ToList();
                PlantillasListasDeVerificacionDetalle plantillaModelo = await _iPlantillasListasDeVerificacionDetalleCoreBusiness.FindAsync(x => x.IntPlantillaDetalleID == listasDeVerificacion.IntPlantillaDetalleID);

                listaNormas = listaNormas.Where(x => listaNormasAuditoria.Any(y => y.IntNormaID == x.IntNormaID)).ToList();

                //List<ElementosComunes> elementosComunesPorAuditoria = await _iAuditoriasCoreBusiness.ObtenerElementosComunesEntreNormasAsync(listaNormas);

                var listaNumeralesPorListaDeVerificacion = await _iNumeralesCoreBusiness.ObtenerNumeralesPorItemDeListaDeVerificacionAsync(listasDeVerificacion);
                listaNumeralesPorListaDeVerificacion = listaNumeralesPorListaDeVerificacion.Where(x => listaNormas.Any(y => y.IntNormaID == x.IntNormaID)).ToList();

                var listaDeVerificacionDTO = new ListasDeVerificacionDTO()
                {
                    NormasDTO = null,
                    IntListaVerificacionID = listasDeVerificacion.IntListaVerificacionID,
                    IntAuditoriaDetalleID = listasDeVerificacion.IntAuditoriaDetalleID,
                    StrTitulo = listasDeVerificacion.StrTitulo,
                    StrHallazgo = listasDeVerificacion.StrHallazgo,
                    StrHallazgoEnInforme = string.IsNullOrEmpty(listasDeVerificacion.StrHallazgoEnInforme) ? listasDeVerificacion.StrHallazgo : listasDeVerificacion.StrHallazgoEnInforme,
                    BitNoConformidad = listasDeVerificacion.BitNoConformidad,
                    BitConformidad = listasDeVerificacion.BitConformidad,
                    BitObservacion = listasDeVerificacion.BitObservacion,
                    Orden = plantillaModelo.IntOrden,
                    DatFechaEnQueAudita = listasDeVerificacion.DatFechaEnQueAudita != null ? Common.ObtenerFechaExacta( listasDeVerificacion.DatFechaEnQueAudita.Value) : listasDeVerificacion.DatFechaEnQueAudita,
                    ListasDeVerificacion_Requisitos = listasDeVerificacion.ListasDeVerificacion_Requisitos.Select(x => new ListasDeVerificacion_Requisitos()
                    {
                        IntID = x.IntID,
                        IntListaDeVerificacionID = x.IntListaDeVerificacionID,
                        IntNumeralID = x.IntNumeralID
                    }).ToList()
                };

                //if (elementosComunesPorAuditoria.Count == 0)
                listaDeVerificacionDTO.NormasDTO = await _iNumeralesCoreBusiness.ObtenerNumeralesPorNormasDTOAsync(listaNumeralesPorListaDeVerificacion);
                //else
                //    listaDeVerificacionDTO.NormasDTO = await _iElementosComunes_DetalleCoreBusiness.ObtenerNumeralesPorNormasDTODeElementosComunesAsync(listaNumeralesPorListaDeVerificacion, listaNormasAuditoria, elementosComunesPorAuditoria);

                return listaDeVerificacionDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> UpdateListaDeVerificacionAuditarAsync(ListasDeVerificacion entity, bool isVistaInforme = false)
        {
            try
            {
                ListasDeVerificacion ListasDeVerificacion = await this.FindAsync(x => x.IntListaVerificacionID == entity.IntListaVerificacionID);

                if (isVistaInforme)
                    ListasDeVerificacion.StrHallazgoEnInforme = entity.StrHallazgo;
                else
                    ListasDeVerificacion.StrHallazgo = entity.StrHallazgo;

                ListasDeVerificacion.BitNoConformidad = entity.BitNoConformidad;
                ListasDeVerificacion.BitConformidad = entity.BitConformidad;
                ListasDeVerificacion.BitObservacion = entity.BitObservacion;
                ListasDeVerificacion.DatFechaEnQueAudita = entity.DatFechaEnQueAudita != null ? Common.ObtenerFechaExacta(entity.DatFechaEnQueAudita.Value) : ListasDeVerificacion.DatFechaEnQueAudita;

                await base.UpdateAsync(ListasDeVerificacion);

                await _iListasDeVerificacion_RequisitosCoreBusiness.GuardarRequisitosSeleccionadosEnListaDeVerificacionAsync(entity.IntListaVerificacionID, entity.ListasDeVerificacion_Requisitos.ToList());

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<Auditorias> ObtenerAuditoriaAsync(int auditoriaID)
        {
            try
            {
                return await _iAuditoriasCoreBusiness.FindAsync(x => x.IntAuditoriaID == auditoriaID);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<AuditoriasDetalleDTO>> ObtenerAuditoriaDetalleDTOAsync(Auditorias auditoria)
        {
            try
            {
                return await _iAuditoriasDetalleCoreBusiness.ObtenerAuditoriaDetalleDTOAsync(auditoria);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<AuditoriasDetalle> ObtenerAuditoriaDetallePorIDAsync(int auditoriaDetalleID)
        {
            try
            {
                return await _iAuditoriasDetalleCoreBusiness.FindAsync(x => x.IntAuditoriaDetalleID == auditoriaDetalleID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<AuditoriaNoConformidadesDTO>> ObtenerNoConformidadesAuditoriaAsync(int auditoriaID)
        {
            try
            {
                Auditorias auditoria = await _iAuditoriasCoreBusiness.FindAsync(x => x.IntAuditoriaID == auditoriaID);
                List<Numerales> listaNumerales = await _iNumeralesCoreBusiness.ObtenerNumeralesPorNormasDeAuditoriaAsync(auditoria);
                List<ListasDeVerificacion> listaDeVerficacion = new List<ListasDeVerificacion>();
                List<AuditoriaNoConformidadesDTO> listaDeNoConformidades = new List<AuditoriaNoConformidadesDTO>();
                var listaNormasDTO = new List<NormasDTO>();

                foreach (var item in auditoria.AuditoriasDetalle)
                    listaDeVerficacion.AddRange(item.ListasDeVerificacion.Where(x => x.BitConformidad == false && (x.BitNoConformidad == true || x.BitObservacion == true)));

                foreach (var lista in listaDeVerficacion)
                {
                    List<Numerales> listaNumeralesPorDetalle = null;

                    if (lista.ListasDeVerificacion_Requisitos.Count() == 0)
                        listaNumeralesPorDetalle = listaNumerales.Where(x => lista.PlantillasListasDeVerificacionDetalle.PlantillasListasDeVerificacion_Numerales.Any(y => x.IntNumeralID == y.IntNumeralID)).ToList();
                    else
                        listaNumeralesPorDetalle = listaNumerales.Where(x => lista.ListasDeVerificacion_Requisitos.Any(y => x.IntNumeralID == y.IntNumeralID)).ToList();

                    var noConformidadDTO = new AuditoriaNoConformidadesDTO();

                    noConformidadDTO.Id = lista.IntListaVerificacionID;
                    noConformidadDTO.Titulo = lista.StrTitulo;
                    noConformidadDTO.NormasDTO = await _iNumeralesCoreBusiness.ObtenerNumeralesPorNormasDTOAsync(listaNumeralesPorDetalle);
                    noConformidadDTO.ProcesoID = lista.AuditoriasDetalle.IntProcesoID;
                    noConformidadDTO.ProcesoCodigo = lista.AuditoriasDetalle.Procesos.StrCodigo;
                    noConformidadDTO.ProcesoDescripcion = lista.AuditoriasDetalle.Procesos.StrDescripcion;
                    noConformidadDTO.Hallazgo = lista.StrHallazgo;
                    noConformidadDTO.HallazgoInforme = String.IsNullOrEmpty(lista.StrHallazgoEnInforme) ? lista.StrHallazgo : lista.StrHallazgoEnInforme;
                    noConformidadDTO.Observacion = lista.BitObservacion;
                    noConformidadDTO.NoConformidad = lista.BitNoConformidad;
                    noConformidadDTO.Orden = lista.PlantillasListasDeVerificacionDetalle.IntOrden;
                    noConformidadDTO.FechaQueSeAudita = lista.DatFechaEnQueAudita;


                    listaDeNoConformidades.Add(noConformidadDTO);
                    listaNormasDTO.AddRange(noConformidadDTO.NormasDTO.ToList());
                }

                return listaDeNoConformidades;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<NormasDTO>> ContadorDeNoConformidadesAsync(List<AuditoriaNoConformidadesDTO> listaNoConformidadesDTO)
        {
            try
            {
                var listaNormasDTO_NoConformidad = new List<NormasDTO>();
                var listaNormasDTO_Observacion = new List<NormasDTO>();
                var listaNormasDTORespuesta = new List<NormasDTO>();

                foreach (var item in listaNoConformidadesDTO.Where(x => x.NoConformidad == true))
                    listaNormasDTO_NoConformidad.AddRange(item.NormasDTO);

                var listaNormasDTORespuesta_NoConformidad = await _iNormasCoreBusiness.ContadorDeNormasAsync(listaNormasDTO_NoConformidad);

                foreach (var item in listaNoConformidadesDTO.Where(x => x.Observacion == true))
                    listaNormasDTO_Observacion.AddRange(item.NormasDTO);

                var listaNormasDTORespuesta_Observacion = await _iNormasCoreBusiness.ContadorDeNormasAsync(listaNormasDTO_Observacion);

                listaNormasDTORespuesta_NoConformidad.ForEach(x => x.BitNoConformidad = true);
                listaNormasDTORespuesta_Observacion.ForEach(x => x.BitObservacion = true);

                listaNormasDTORespuesta.AddRange(listaNormasDTORespuesta_NoConformidad);
                listaNormasDTORespuesta.AddRange(listaNormasDTORespuesta_Observacion);

                return listaNormasDTORespuesta;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SelectList> ObtenerNormasDeAuditoriaAsync(int listaVerificacionID, int normaID)
        {
            try
            {
                var norma = normaID.ToString();
                var itemListaDeVerificacionModelo = await this.FindAsync(x => x.IntListaVerificacionID == listaVerificacionID);
                var listaNormas = await _iNormasCoreBusiness.SelectListAsync(norma);

                if (itemListaDeVerificacionModelo != null)
                {
                    var listaNormasDeAuditoria = itemListaDeVerificacionModelo.AuditoriasDetalle.Auditorias.Auditorias_Normas;
                    var listaDropDownNormas = listaNormas.Where(x => listaNormasDeAuditoria.Any(y => y.IntNormaID.ToString() == x.Value));

                    return new SelectList(listaDropDownNormas, "Value", "Text");
                }

                return listaNormas;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ListasDeVerificacion_NumeralesDTO>> ObtenerNumeralesPorListaDeVerificacionDTOAsync(int listaDeVerificacionID)
        {
            try
            {
                var listaDeVerificacionModelo = await this.FindAsync(x => x.IntListaVerificacionID == listaDeVerificacionID);
                var listaNumerales = await _iPlantillasListasDeVerificacionDetalleCoreBusiness.FindAsync(x => x.IntPlantillaDetalleID == listaDeVerificacionModelo.IntPlantillaDetalleID);

                var listaNumeralesDTO = listaNumerales.PlantillasListasDeVerificacion_Numerales.Select(x => new ListasDeVerificacion_NumeralesDTO()
                {
                    IntID = x.IntID,
                    IntNumeralID = x.IntNumeralID,
                    IntListaVerificacionID = listaDeVerificacionID,
                    CodigoNorma = x.Numerales.Normas.StrCodigo,
                    CodigoNumeral = x.Numerales.StrCodigo,
                    DescripcionNumeral = x.Numerales.StrDescripcion
                }).ToList();

                return listaNumeralesDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<NormasDTO>> ObtenerNumeralesParaSeleccionarAsync(NumeralesGenericosDTO datosGenericos)
        {
            try
            {
                var listaNumerales = await _iNumeralesCoreBusiness.FindWhereAsync(x => x.BitActivo == true);
                var listaNumeralesPorProceso = await _iProcesosCoreBusiness.ObtenerNumeralesPorProcesoDTOAsync(datosGenericos.ProcesoID);
                listaNumerales = listaNumerales.Where(x => listaNumeralesPorProceso.Any(y => y.IntNumeralID == x.IntNumeralID)).ToList();

                var listaNormasPorAuditoria = await _iAuditorias_NormasCoreBusiness.FindWhereAsync(x => x.IntAuditoriaID == datosGenericos.AuditoriaID);
                var listaNumeralesPorListaDeVerificacion = await this.ObtenerNumeralesPorListaDeVerificacionDTOAsync(datosGenericos.RegistroID);

                var listaNormasDTO = await _iNormasCoreBusiness.ObtenerNormasDTOAsync(listaNumerales);
                listaNormasDTO = listaNormasDTO.Where(x => listaNormasPorAuditoria.Any(y => y.IntNormaID == x.IntNormaID)).ToList();

                listaNormasDTO.ForEach(normas =>
                {
                    normas.NumeralesDTO.ForEach(numeral =>
                    {
                        bool existeNumeral = listaNumeralesPorListaDeVerificacion.Exists(x => x.IntNumeralID == numeral.IntNumeralID);

                        if (existeNumeral)
                            numeral.BitSeleccionado = true;
                    });
                });

                return listaNormasDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<InformeAuditoriaDTO> ObtenerInformeListasDeVerificacionPDFAsync(int auditoriaDetalleID)
        {
            try
            {
                var auditoriaDetalle = await _iAuditoriasDetalleCoreBusiness.FindAsync(x => x.IntAuditoriaDetalleID == auditoriaDetalleID);

                InformeAuditoriaDTO informeAuditoriaDTO = new InformeAuditoriaDTO();
                informeAuditoriaDTO.Auditoria = await _iAuditoriasCoreBusiness.FindAsync(x => x.IntAuditoriaID == auditoriaDetalle.IntAuditoriaID);
                informeAuditoriaDTO.AuditoriaDetalle = auditoriaDetalle;
                informeAuditoriaDTO.ListaDeVerificacionDTO = await this.ObtenerListasDeVerificacionPorProcesoDTOAsync(auditoriaDetalle);
                informeAuditoriaDTO.Usuario = await _iAuditoriasCoreBusiness.ObtenerUsuarioQueFirmaAsync(informeAuditoriaDTO.Auditoria.StrUsuarioFirma);
                informeAuditoriaDTO.NoConformidades = await this.ObtenerNoConformidadesAuditoriaAsync(informeAuditoriaDTO.Auditoria.IntAuditoriaID);
                informeAuditoriaDTO.ListaNormasDTO = await this.ContadorDeNoConformidadesAsync(informeAuditoriaDTO.NoConformidades);

                return informeAuditoriaDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<ListasDeVerificacionDTO>> ObtenerListasDeVerificacionPorProcesoDTOAsync(AuditoriasDetalle auditoriaDetalle)
        {
            try
            {
                var listasDeVerificacion = auditoriaDetalle.ListasDeVerificacion.ToList();
                var listaNormasPorAuditoria = auditoriaDetalle.Auditorias.Auditorias_Normas.ToList();
                List<ListasDeVerificacionDTO> listasDeVerificacionDTO = new List<ListasDeVerificacionDTO>();

                foreach (var lv in listasDeVerificacion)
                {
                    var listaNumeralesPorListaDeVerificacion = await _iNumeralesCoreBusiness.ObtenerNumeralesPorItemDeListaDeVerificacionAsync(lv);
                    listaNumeralesPorListaDeVerificacion = listaNumeralesPorListaDeVerificacion.Where(x => listaNormasPorAuditoria.Any(y => y.IntNormaID == x.IntNormaID)).ToList();

                    ListasDeVerificacionDTO listasDeVerificacionModeloDTO = new ListasDeVerificacionDTO();
                    listasDeVerificacionModeloDTO.NormasDTO = await _iNumeralesCoreBusiness.ObtenerNumeralesPorNormasDTOAsync(listaNumeralesPorListaDeVerificacion);
                    listasDeVerificacionModeloDTO.StrTitulo = lv.StrTitulo;
                    listasDeVerificacionModeloDTO.StrHallazgo = lv.StrHallazgo;
                    listasDeVerificacionModeloDTO.BitConformidad = lv.BitConformidad;
                    listasDeVerificacionModeloDTO.BitNoConformidad = lv.BitNoConformidad;
                    listasDeVerificacionModeloDTO.BitObservacion = lv.BitObservacion;
                    listasDeVerificacionModeloDTO.Orden = lv.PlantillasListasDeVerificacionDetalle.IntOrden;

                    listasDeVerificacionDTO.Add(listasDeVerificacionModeloDTO);
                }

                return listasDeVerificacionDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<PlantillasListasDeVerificacionDetalle>> ObtenerListaDeVerificacionDeProcesoActualAsync(int procesoID)
        {
            try
            {
                return await _iPlantillasListasDeVerificacionDetalleCoreBusiness.FindWhereAsync(x => x.IntProcesoID == procesoID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> CopiarPlantillaEnListaDeVerificacionDeProcesoActualAsync(int auditoriaDetalleID, int plantillaDetalleID)
        {
            try
            {
                var listasDeVerificacion = await this.FindWhereAsync(x => x.IntAuditoriaDetalleID == auditoriaDetalleID);
                var plantillaListaDeVerificacion = await _iPlantillasListasDeVerificacionDetalleCoreBusiness.FindAsync(x => x.IntPlantillaDetalleID == plantillaDetalleID);

                var existeListaDeVerificacion = listasDeVerificacion.Any(x => x.IntPlantillaDetalleID == plantillaDetalleID || x.StrTitulo.ToLower() == plantillaListaDeVerificacion.StrTitulo.ToLower());

                if (existeListaDeVerificacion)
                    return RecursoListasDeVerificacion.msnItemDeListaDuplicado;

                var nuevaListaDeVerificacion = new ListasDeVerificacion();
                nuevaListaDeVerificacion.IntAuditoriaDetalleID = auditoriaDetalleID;
                nuevaListaDeVerificacion.IntPlantillaDetalleID = plantillaDetalleID;
                nuevaListaDeVerificacion.StrTitulo = plantillaListaDeVerificacion.StrTitulo.ToUpper();

                await base.CreateAsync(nuevaListaDeVerificacion);

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

        ~ListasDeVerificacionCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iAuditoriasCoreBusiness != null)
                {
                    _iAuditoriasCoreBusiness.Dispose();
                    _iAuditoriasCoreBusiness = null;
                }

                if (_iAuditoriasDetalleCoreBusiness != null)
                {
                    _iAuditoriasDetalleCoreBusiness.Dispose();
                    _iAuditoriasDetalleCoreBusiness = null;
                }

                if (_iNormasCoreBusiness != null)
                {
                    _iNormasCoreBusiness.Dispose();
                    _iNormasCoreBusiness = null;
                }

                if (_iNumeralesCoreBusiness != null)
                {
                    _iNumeralesCoreBusiness.Dispose();
                    _iNumeralesCoreBusiness = null;
                }

                if (_iTerceros_NormasCoreBusiness != null)
                {
                    _iTerceros_NormasCoreBusiness.Dispose();
                    _iTerceros_NormasCoreBusiness = null;
                }

                if (_iElementosComunes_DetalleCoreBusiness != null)
                {
                    _iElementosComunes_DetalleCoreBusiness.Dispose();
                    _iElementosComunes_DetalleCoreBusiness = null;
                }

                if (_iAuditorias_NormasCoreBusiness != null)
                {
                    _iAuditorias_NormasCoreBusiness.Dispose();
                    _iAuditorias_NormasCoreBusiness = null;
                }

                if (_iPlantillasListasDeVerificacionDetalleCoreBusiness != null)
                {
                    _iPlantillasListasDeVerificacionDetalleCoreBusiness.Dispose();
                    _iPlantillasListasDeVerificacionDetalleCoreBusiness = null;
                }

                if (_iListasDeVerificacion_RequisitosCoreBusiness != null)
                {
                    _iListasDeVerificacion_RequisitosCoreBusiness.Dispose();
                    _iListasDeVerificacion_RequisitosCoreBusiness = null;
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
