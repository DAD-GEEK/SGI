using Across.CacheStorage;
using DataAccess.Interfaces;
using Models;
using Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using static Across.Enumeraciones;

namespace DataAccess
{
    public class AusentismoIndicadoresDataAccess : IAusentismoIndicadoresDataAccess
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private gestioni_consultorNetEntities _modelo;
        private ICacheStorage _iCacheStorage;

        public AusentismoIndicadoresDataAccess()
        {
            this._iCacheStorage = new CacheStorage();
        }

        public List<AusentismoIndicadoresDTO> ObtenerListaIndicadoresGenerales(int anio = 0)
        {
            try
            {
                List<AusentismoIndicadoresDTO> listaIndicadores = null;

                string listaIndicadoresJSON = _iCacheStorage.Get<string>(CacheNames.ObtenerTodosLosIndicadoresGenerales);

                if (string.IsNullOrEmpty(listaIndicadoresJSON))
                {
                    IQueryable<AusentismoIndicadoresDTO> sentencia = null;

                    //Se utiliza sentencia Iquerable para no cargar todos los registros en memoria
                    using (gestioni_consultorNetEntities db = new gestioni_consultorNetEntities())
                    {
                        sentencia = (from d in db.AusentismoIndicadores
                                     select new AusentismoIndicadoresDTO
                                     {

                                         StrId = d.StrId,
                                         IntAnio = anio,
                                         StrCodigo = d.StrCodigo,
                                         StrNombreIndicador = d.StrNombreIndicador,
                                         StrTipoEventoAusentismo = d.StrTipoEventoAusentismo,
                                         StrFormula = d.StrFormula,
                                         StrFuente = d.StrFuente,
                                         StrObjetivo = d.StrObjetivo,
                                         IntOrdenamiento = d.IntOrdenamiento,
                                         StrObservaciones = d.StrObservaciones,
                                         StrCanvasID = enumCodigosAusentismoIndicadores.ausentismoPorCausaMedica.ToString() == d.StrCodigo ? "canvas_mga" :
                                                       enumCodigosAusentismoIndicadores.indiceDeFrecuenciaPorCausaMedica.ToString() == d.StrCodigo ? "canvas_ifcm" :
                                                       enumCodigosAusentismoIndicadores.indiceDeSeveridadPorCausaMedica.ToString() == d.StrCodigo ? "canvas_iscm" :
                                                       enumCodigosAusentismoIndicadores.tasaDeAusentismoPorCausaMedica.ToString() == d.StrCodigo ? "canvas_tacm" :
                                                       enumCodigosAusentismoIndicadores.incidenciaYPrevalenciaDeAusentismoPorCausaMedica.ToString() == d.StrCodigo ? "canvas_ipacm" :

                                                        enumCodigosAusentismoIndicadores.ausentismoPor_AT_EL.ToString() == d.StrCodigo ? "canvas_apat" :
                                                        enumCodigosAusentismoIndicadores.reporteEInvestigacionAT_EL.ToString() == d.StrCodigo ? "canvas_riat" :
                                                        enumCodigosAusentismoIndicadores.indiceDeFrecuenciaAT.ToString() == d.StrCodigo ? "canvas_ifat" :
                                                        enumCodigosAusentismoIndicadores.indiceDeFrecuenciaEL.ToString() == d.StrCodigo ? "canvas_ifel" :
                                                        enumCodigosAusentismoIndicadores.mortalidad.ToString() == d.StrCodigo ? "canvas_morta" :
                                                        enumCodigosAusentismoIndicadores.indiceDeSeveridadAT_EL.ToString() == d.StrCodigo ? "canvas_isatel" :
                                                        enumCodigosAusentismoIndicadores.tasaDeAccidentalidad.ToString() == d.StrCodigo ? "canvas_tacc" :
                                                        enumCodigosAusentismoIndicadores.incidenciaYPrevalenciaDeAccidentes.ToString() == d.StrCodigo ? "canvas_ipdae" :
                                                        enumCodigosAusentismoIndicadores.lesionesIncapacitantes.ToString() == d.StrCodigo ? "canvas_leinc" :

                                                        string.Empty,

                                     });

                        listaIndicadores = sentencia.ToList();

                        listaIndicadoresJSON = JsonConvert.SerializeObject(listaIndicadores);
                        _iCacheStorage.Insert(CacheNames.ObtenerTodosLosIndicadoresGenerales, listaIndicadoresJSON, new TimeSpan(8, 0, 0));
                    }
                }
                else
                    listaIndicadores = JsonConvert.DeserializeObject<List<AusentismoIndicadoresDTO>>(listaIndicadoresJSON);

                return listaIndicadores;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EliminarAusentismoIndicadoresEnCachePorEmpresa()
        {
            try
            {
                using (gestioni_consultorNetEntities db = new gestioni_consultorNetEntities())
                {
                    var listaIDs = (from d in db.AusentismoIndicadores_InformacionPorEmpresa
                                    group d by d.IntTerceroID into listaTercerosIDs
                                    select listaTercerosIDs).ToList();

                    foreach (var item in listaIDs)                    
                        _iCacheStorage.Clear($"{CacheNames.ObtenerIndicadoresPorEmpresa}_{item.Key}");                    
                }
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

        ~AusentismoIndicadoresDataAccess()
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
