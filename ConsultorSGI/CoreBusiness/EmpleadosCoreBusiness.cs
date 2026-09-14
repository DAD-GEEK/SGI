using Across;
using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using DataAccess.Servicios;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Across.Enumeraciones;

namespace CoreBusiness
{
    public class EmpleadosCoreBusiness : CRUDGenerico<Empleados>, IEmpleadosCoreBusiness
    {
        #region Inyección de dependencias
        private string rutaArchivoDeExcel = string.Empty;

        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private ICommonCoreBusiness _iCommonCoreBusiness;
        private ICiudadesCoreBusiness _iCiudadesCoreBusiness;
        private ITipoIdentificacionCoreBusiness _iTipoIdentificacionCoreBusiness;
        private IEscolaridadesCoreBusiness _iEscolaridadesCoreBusiness;
        private ICentrosDeTrabajoCoreBusiness _iCentrosDeTrabajoCoreBusiness;
        private IEstadosCivilesCoreBusiness _iEstadosCivilesCoreBusiness;
        private ICargosCoreBusiness _iCargosCoreBusiness;
        private ITurnosCoreBusiness _iTurnosCoreBusiness;
        private IFondosCoreBusiness _iFondosCoreBusiness;
        private IEstratosCoreBusiness _iEstratosCoreBusiness;
        private ITiposContratoCoreBusiness _iTiposContratoCoreBusiness;
        private IAreasCoreBusiness _iAreasCoreBusiness;
        private IProcesosCoreBusiness _iProcesosCoreBusiness;
        private ITiposFondoCoreBusiness _iTiposFondoCoreBusiness;

        public EmpleadosCoreBusiness() : base(new gestioni_consultorNetEntities(new ServicioTercero()))
        {
            _iCommonCoreBusiness = new CommonCoreBusiness();
            _iCiudadesCoreBusiness = new CiudadesCoreBusiness();
            _iTipoIdentificacionCoreBusiness = new TipoIdentificacionCoreBusiness();
            _iEscolaridadesCoreBusiness = new EscolaridadesCoreBusiness();
            _iCentrosDeTrabajoCoreBusiness = new CentrosDeTrabajoCoreBusiness();
            _iEstadosCivilesCoreBusiness = new EstadosCivilesCoreBusiness();
            _iCargosCoreBusiness = new CargosCoreBusiness();
            _iTurnosCoreBusiness = new TurnosCoreBusiness();
            _iFondosCoreBusiness = new FondosCoreBusiness();
            _iEstratosCoreBusiness = new EstratosCoreBusiness();
            _iTiposContratoCoreBusiness = new TiposContratoCoreBusiness();
            _iAreasCoreBusiness = new AreasCoreBusiness();
            _iProcesosCoreBusiness = new ProcesosCoreBusiness();
            _iTiposFondoCoreBusiness = new TiposFondoCoreBusiness();
        }

        #endregion

        #region CRUDGenerico
        public new async Task<string> CreateAsync(Empleados entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.StrIdentificacion == entity.StrIdentificacion);
                if (listaRegistros.Count() != 0)
                    return RecursoEmpleados.msnEmpleadoYaExiste;

                entity.StrIdentificacion = entity.StrIdentificacion;
                entity.IntTipoIdentificacion = entity.IntTipoIdentificacion;
                entity.StrPrimerApellido = entity.StrPrimerApellido;
                entity.StrSegundoApellido = entity.StrSegundoApellido;
                entity.StrPrimerNombre = entity.StrPrimerNombre;
                entity.StrSegundoNombre = entity.StrSegundoNombre;
                entity.StrNombreCompleto = $"{entity.StrPrimerApellido} {entity.StrSegundoApellido} {entity.StrPrimerNombre} {entity.StrSegundoNombre}".ToUpper();
                entity.StrRutaImagen = entity.StrRutaImagen;
                entity.StrGenero = entity.StrGenero;
                entity.StrNacionalidad = entity.StrNacionalidad;
                entity.CentrosDeTrabajo = entity.CentrosDeTrabajo;
                entity.DatFechaNacimiento = entity.DatFechaNacimiento;
                entity.DatFechaIngreso = entity.DatFechaIngreso;
                entity.IntCiudadNacimiento = entity.IntCiudadNacimiento;
                entity.IntCiudadLabora = entity.IntCiudadLabora;
                entity.IntCiudadResidencia = entity.IntCiudadResidencia;
                entity.StrDireccionResidencia = entity.StrDireccionResidencia;
                entity.StrTelefono = entity.StrTelefono;
                entity.StrCelular = entity.StrCelular;
                entity.StrEmail = entity.StrEmail;
                entity.IntCargo = entity.IntCargo;
                entity.IntAreaID = entity.IntAreaID;
                entity.IntTurno = entity.IntTurno;
                entity.IntARL = entity.IntARL;
                entity.IntEPS = entity.IntEPS;
                entity.IntAFP = entity.IntAFP;
                entity.IntTipoContrato = entity.IntTipoContrato;
                entity.IntSalario = entity.IntSalario;
                entity.IntEscolaridad = entity.IntEscolaridad;
                entity.IntEstadoCivil = entity.IntEstadoCivil;
                entity.TIntNumeroHijos = entity.TIntNumeroHijos;
                entity.StrPersonasACargo = entity.StrPersonasACargo;
                entity.TIntEstrato = entity.TIntEstrato;
                entity.StrGrupoSanguineo = entity.StrGrupoSanguineo;
                entity.StrCondiciones = entity.StrCondiciones;
                entity.StrAlergias = entity.StrAlergias;
                entity.StrMedicinas = entity.StrMedicinas;
                entity.StrContacto = entity.StrContacto;
                entity.StrParentesco = entity.StrParentesco;
                entity.StrTelefonoContacto = entity.StrTelefonoContacto;
                entity.BitActivo = entity.BitActivo;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> UpdateAsync(Empleados entity)
        {
            try
            {
                var terceroID = await _iCommonCoreBusiness.GetTerceroIDFromCurrentUserAsync();

                var listaRegistros = await this.FindWhereAsync(x => x.StrIdentificacion == entity.StrIdentificacion && x.IntEmpleadoID != entity.IntEmpleadoID);
                if (listaRegistros.Count() != 0)
                    return RecursoEmpleados.msnEmpleadoYaExiste;

                Empleados Empleados = await this.FindAsync(x => x.IntEmpleadoID == entity.IntEmpleadoID);

                Empleados.StrIdentificacion = entity.StrIdentificacion;
                Empleados.IntTipoIdentificacion = entity.IntTipoIdentificacion;
                Empleados.StrPrimerApellido = entity.StrPrimerApellido;
                Empleados.StrSegundoApellido = entity.StrSegundoApellido;
                Empleados.StrPrimerNombre = entity.StrPrimerNombre;
                Empleados.StrSegundoNombre = entity.StrSegundoNombre;
                Empleados.StrNombreCompleto = $"{entity.StrPrimerApellido} {entity.StrSegundoApellido} {entity.StrPrimerNombre} {entity.StrSegundoNombre}".ToUpper();
                Empleados.StrRutaImagen = entity.StrRutaImagen;
                Empleados.StrGenero = entity.StrGenero;
                Empleados.StrNacionalidad = entity.StrNacionalidad;
                Empleados.CentrosDeTrabajo = entity.CentrosDeTrabajo;
                Empleados.DatFechaNacimiento = entity.DatFechaNacimiento;
                Empleados.DatFechaIngreso = entity.DatFechaIngreso;
                Empleados.IntCiudadNacimiento = entity.IntCiudadNacimiento;
                Empleados.IntCiudadLabora = entity.IntCiudadLabora;
                Empleados.IntCiudadResidencia = entity.IntCiudadResidencia;
                Empleados.StrDireccionResidencia = entity.StrDireccionResidencia;
                Empleados.StrTelefono = entity.StrTelefono;
                Empleados.StrCelular = entity.StrCelular;
                Empleados.StrEmail = entity.StrEmail;
                Empleados.IntAreaID = entity.IntAreaID;
                Empleados.IntCargo = entity.IntCargo;
                Empleados.IntTurno = entity.IntTurno;
                Empleados.IntARL = entity.IntARL;
                Empleados.IntEPS = entity.IntEPS;
                Empleados.IntAFP = entity.IntAFP;
                Empleados.IntTipoContrato = entity.IntTipoContrato;
                Empleados.IntSalario = entity.IntSalario;
                Empleados.IntEscolaridad = entity.IntEscolaridad;
                Empleados.IntEstadoCivil = entity.IntEstadoCivil;
                Empleados.TIntNumeroHijos = entity.TIntNumeroHijos;
                Empleados.StrPersonasACargo = entity.StrPersonasACargo;
                Empleados.TIntEstrato = entity.TIntEstrato;
                Empleados.StrGrupoSanguineo = entity.StrGrupoSanguineo;
                Empleados.StrCondiciones = entity.StrCondiciones;
                Empleados.StrAlergias = entity.StrAlergias;
                Empleados.StrMedicinas = entity.StrMedicinas;
                Empleados.StrContacto = entity.StrContacto;
                Empleados.StrParentesco = entity.StrParentesco;
                Empleados.StrTelefonoContacto = entity.StrTelefonoContacto;
                Empleados.BitActivo = entity.BitActivo;

                await base.UpdateAsync(Empleados);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public new async Task DeleteAsync(Empleados entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Empleados> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Empleados, bool>> match)
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

        public new async Task<Empleados> FindAsync(Expression<Func<Empleados, bool>> match)
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

        public new List<Empleados> GetAll()
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

        public new async Task<List<Empleados>> GetAllAsync()
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


        public async Task<string> GuardarEmpleadoAsync(Empleados modelo)
        {
            try
            {
                if (modelo.IntEmpleadoID == 0)
                    return await this.CreateAsync(modelo);
                else
                    return await this.UpdateAsync(modelo);
            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion

        #region Archivo de Excel
        public async Task<List<ExcelErrorDTO>> CargarEmpleadosPorArchivoDeExcelAsync(HttpFileCollectionBase archivoDeExcel, ParamFilesDTO paramFilesDTO)
        {
            try
            {
                var listaEmpleadosExcelConNovedades = new List<EmpleadosDTO>();
                var listaDatosArchivoExcel = await this.LeerArchivoDeExcelAsync(archivoDeExcel, paramFilesDTO);
                var listaDatosRevisados = await this.ObtenerNovedadesDeArchivoDeExcelAsync(listaDatosArchivoExcel);
                var listaDatosConNovedades = listaDatosRevisados.Where(x => x.ErrorArchivoExcel != string.Empty).ToList();
                var listaDatosSinNovedades = listaDatosRevisados.Where(x => x.ErrorArchivoExcel == string.Empty).ToList();

                var listaDatosConNovedadesAlGuardar = await this.GuardarEmpleadosDesdeExcelAsync(listaDatosSinNovedades);

                listaEmpleadosExcelConNovedades.AddRange(listaDatosConNovedades);
                listaEmpleadosExcelConNovedades.AddRange(listaDatosConNovedadesAlGuardar);

                var listaNovedadesDTO = listaEmpleadosExcelConNovedades.Select(x => new ExcelErrorDTO()
                {
                    NumeroRegistro = x.NumeroRegistro,
                    MensajeError = x.ErrorArchivoExcel

                }).ToList();

                return listaNovedadesDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<EmpleadosDTO>> LeerArchivoDeExcelAsync(HttpFileCollectionBase archivoDeExcel, ParamFilesDTO paramFilesDTO)
        {
            try
            {
                var modeloTercero = await _iCommonCoreBusiness.GetTerceroModelFromCurrentUser();
                paramFilesDTO.nombreArchivo = $"Empleados_{modeloTercero.StrIdentificacion.Trim()}";

                var nombreArchivo = Archivos.GuardarArchivo(archivoDeExcel, paramFilesDTO);
                rutaArchivoDeExcel = Archivos.ObtenerRutaDeArchivoTemporal(nombreArchivo);

                var excelFile = new LinqToExcel.ExcelQueryFactory(rutaArchivoDeExcel);

                var empleadosExcel =
                    from row in excelFile.Worksheet("Empleados")

                    let item = new
                    {
                        Identificacion = row["Identificacion"].Cast<string>(),
                        TipoIdentificacion = row["TipoIdentificacion"].Cast<string>(),
                        PrimerApellido = row["PrimerApellido"].Cast<string>(),
                        SegundoApellido = row["SegundoApellido"].Cast<string>(),
                        PrimerNombre = row["PrimerNombre"].Cast<string>(),
                        SegundoNombre = row["SegundoNombre"].Cast<string>(),
                        Genero = row["Genero"].Cast<string>(),
                        Nacionalidad = row["Nacionalidad"].Cast<string>(),
                        CentroDeTrabajo = row["CentroDeTrabajo"].Cast<string>(),
                        FechaNacimiento = row["FechaNacimiento"].Cast<DateTime>(),
                        FechaIngreso = row["FechaIngreso"].Cast<DateTime>(),
                        CiudadNacimiento = row["CiudadNacimiento"].Cast<string>(),
                        CiudadLabora = row["CiudadLabora"].Cast<string>(),
                        CiudadResidencia = row["CiudadResidencia"].Cast<string>(),
                        DireccionResidencia = row["DireccionResidencia"].Cast<string>(),
                        Telefono = row["Telefono"].Cast<string>(),
                        Celular = row["Celular"].Cast<string>(),
                        Email = row["Email"].Cast<string>(),
                        Cargo = row["Cargo"].Cast<string>(),
                        Turno = row["Turno"].Cast<string>(),
                        Area = row["Area"].Cast<string>(),
                        Proceso = row["Proceso"].Cast<string>(),
                        ARL = row["ARL"].Cast<string>(),
                        EPS = row["EPS"].Cast<string>(),
                        AFP = row["AFP"].Cast<string>(),
                        TipoContrato = row["TipoContrato"].Cast<string>(),
                        Salario = row["Salario"].Cast<int>(),
                        Escolaridad = row["Escolaridad"].Cast<string>(),
                        EstadoCivil = row["EstadoCivil"].Cast<string>(),
                        NumeroHijos = row["NumeroHijos"].Cast<byte>(),
                        PersonasACargo = row["PersonasACargo"].Cast<byte>(),
                        Estrato = row["Estrato"].Cast<byte>(),
                        GrupoSanguineo = row["GrupoSanguineo"].Cast<string>(),
                        Condiciones = row["Condiciones"].Cast<string>(),
                        Alergias = row["Alergias"].Cast<string>(),
                        Medicinas = row["Medicinas"].Cast<string>(),
                        Contacto = row["Contacto"].Cast<string>(),
                        Parentesco = row["Parentesco"].Cast<string>(),
                        TelefonoContacto = row["TelefonoContacto"].Cast<string>()
                    }

                    select new EmpleadosDTO
                    {
                        Identificacion = item.Identificacion,
                        TipoIdentificacion = item.TipoIdentificacion,
                        PrimerApellido = item.PrimerApellido,
                        SegundoApellido = item.SegundoApellido,
                        PrimerNombre = item.PrimerNombre,
                        SegundoNombre = item.SegundoNombre,
                        Genero = item.Genero,
                        Nacionalidad = item.Nacionalidad,
                        CentroDeTrabajo = item.CentroDeTrabajo,
                        FechaDeNacimiento = item.FechaNacimiento,
                        FechaDeIngreso = item.FechaIngreso,
                        CiudadNacimiento = item.CiudadNacimiento,
                        CiudadLaboral = item.CiudadLabora,
                        CiudadResidencia = item.CiudadResidencia,
                        DireccionResidencia = item.DireccionResidencia,
                        Telefono = item.Telefono,
                        Celular = item.Celular,
                        Email = item.Email,
                        Turno = item.Turno,
                        Area = item.Area,
                        Cargo = item.Cargo,
                        Proceso = item.Proceso,
                        ARL = item.ARL,
                        EPS = item.EPS,
                        AFP = item.AFP,
                        TipoContrato = item.TipoContrato,
                        Salario = item.Salario,
                        Escolaridad = item.Escolaridad,
                        EstadoCivil = item.EstadoCivil,
                        NumeroHijos = item.NumeroHijos,
                        PersonasACargo = item.PersonasACargo,
                        Estrato = item.Estrato,
                        GrupoSanquineo = item.GrupoSanguineo,
                        Condiciones = item.Condiciones,
                        Alergias = item.Alergias,
                        Medicinas = item.Medicinas,
                        Contacto = item.Contacto,
                        Parentesco = item.Parentesco,
                        TelefonoContacto = item.TelefonoContacto,
                        Activo = true

                    };

                var listaEmpleados = empleadosExcel.ToList();

                if (listaEmpleados.Count() == 0)
                    throw new Exception(RecursoCommon.msnArchivoSinDatos);

                int contadorRegistros = 1;
                listaEmpleados.ForEach(registro =>
                {
                    registro.NumeroRegistro = contadorRegistros + 1;
                    contadorRegistros++;
                });

                Archivos.EliminarArchivo(rutaArchivoDeExcel);

                //this.ValidarModelosDeExcel(listaEmpleados);

                return listaEmpleados;

            }
            catch (Exception)
            {
                Archivos.EliminarArchivo(rutaArchivoDeExcel);
                throw;
            }
        }

        private List<EmpleadosDTO> ValidarModelosDeExcel(List<EmpleadosDTO> listaRegistrosExcel)
        {
            try
            {
                listaRegistrosExcel.ForEach(registro =>
                {
                    if (string.IsNullOrEmpty(registro.Identificacion))
                        registro.ErrorArchivoExcel = String.Format("El campo Identificacion no puede estar vacio.", registro.Identificacion);

                });

                return listaRegistrosExcel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<EmpleadosDTO>> ObtenerNovedadesDeArchivoDeExcelAsync(List<EmpleadosDTO> registrosExcel)
        {
            try
            {
                List<EmpleadosDTO> registrosConNovedades = new List<EmpleadosDTO>();

                var terceroUsuarioID = await _iCommonCoreBusiness.GetTerceroIDFromCurrentUserAsync();

                var listaEmpleados = await this.GetAllAsync();
                var listaTiposDeIndentificacion = await _iTipoIdentificacionCoreBusiness.GetAllAsync();
                var listaCentrosDeTrabajo = await _iCentrosDeTrabajoCoreBusiness.GetAllByCurrenTerceroAsync();
                var listaCiudades = await _iCiudadesCoreBusiness.GetAllAsync();
                var listaCargos = await _iCargosCoreBusiness.GetAllAsync();
                var listaTurnos = await _iTurnosCoreBusiness.GetAllAsync();
                var listaAreas = await _iAreasCoreBusiness.FindWhereAsync(x => x.IntTerceroID == terceroUsuarioID);
                var listaProcesos = await _iProcesosCoreBusiness.FindWhereAsync(x => x.IntTerceroClienteID == terceroUsuarioID);
                var listaTiposFondos = await _iTiposFondoCoreBusiness.GetAllAsync();
                var listaFondos = await _iFondosCoreBusiness.GetAllAsync();
                var listaTipoContrato = await _iTiposContratoCoreBusiness.GetAllAsync();
                var listaEscolaridades = await _iEscolaridadesCoreBusiness.GetAllAsync();
                var listaEstadosCiviles = await _iEstadosCivilesCoreBusiness.GetAllAsync();

                registrosExcel.Where(x => x.Identificacion == null || x.Identificacion == string.Empty).ToList()
                    .ForEach(registro =>
                    {
                        registro.ErrorArchivoExcel = String.Format("El campo Identificación no puede estar vacio.", registro.Identificacion);
                    });

                registrosExcel.Where(x => listaEmpleados.Any(y => y.StrIdentificacion == x.Identificacion)).ToList()
                    .ForEach(registro =>
                    {
                        registro.ErrorArchivoExcel = String.Format("Ya existe un empleado con la identificación {0} en la base de datos", registro.Identificacion);
                    });

                registrosExcel.Where(x => !listaTiposDeIndentificacion.Any(y => y.StrCodigo == x.TipoIdentificacion)).ToList()
                    .ForEach(registro =>
                    {
                        registro.ErrorArchivoExcel = String.Format("El tipo de identificación {0} no existe en la base de datos", registro.TipoIdentificacion);
                    });

                registrosExcel.Where(x => !listaCentrosDeTrabajo.Any(y => y.StrCodigo.ToLower() == Common.TextoEnMinuscula(x.CentroDeTrabajo))).ToList()
                    .ForEach(registro =>
                    {
                        registro.ErrorArchivoExcel = String.Format("El centro de trabajo {0} no existe en la base de datos", registro.CentroDeTrabajo);
                    });

                registrosExcel.Where(x => !listaCiudades.Any(y => Common.TextoSinTildes(y.StrDescripcion.ToLower()) == Common.TextoSinTildes(Common.TextoEnMinuscula(x.CiudadNacimiento)))).ToList()
                   .ForEach(registro =>
                   {
                       registro.ErrorArchivoExcel = String.Format("La ciudad de nacimiento {0} no existe en la base de datos", registro.CiudadNacimiento);
                   });

                registrosExcel.Where(x => !listaCiudades.Any(y => Common.TextoSinTildes(y.StrDescripcion.ToLower()) == Common.TextoSinTildes(Common.TextoEnMinuscula(x.CiudadLaboral)))).ToList()
                   .ForEach(registro =>
                   {
                       registro.ErrorArchivoExcel = String.Format("La ciudad laboral {0} no existe en la base de datos", registro.CiudadLaboral);
                   });

                registrosExcel.Where(x => !listaCiudades.Any(y => Common.TextoSinTildes(y.StrDescripcion.ToLower()) == Common.TextoSinTildes(Common.TextoEnMinuscula(x.CiudadResidencia)))).ToList()
                   .ForEach(registro =>
                   {
                       registro.ErrorArchivoExcel = String.Format("La ciudad de residencia {0} no existe en la base de datos", registro.CiudadResidencia);
                   });

                registrosExcel.Where(x => !listaCargos.Any(y => y.StrCodigo.ToLower() == Common.TextoEnMinuscula(x.Cargo))).ToList()
                  .ForEach(registro =>
                  {
                      registro.ErrorArchivoExcel = String.Format("El cargo {0} no existe en la base de datos", registro.Cargo);
                  });

                registrosExcel.Where(x => !listaTurnos.Any(y => y.StrDescripcion.ToLower() == Common.TextoEnMinuscula(x.Turno))).ToList()
                  .ForEach(registro =>
                  {
                      registro.ErrorArchivoExcel = String.Format("El turno {0} no existe en la base de datos", registro.Turno);
                  });

                registrosExcel.Where(x => !listaAreas.Any(y => y.StrCodigo.ToLower() == Common.TextoEnMinuscula(x.Area))).ToList()
                  .ForEach(registro =>
                  {
                      registro.ErrorArchivoExcel = String.Format("El área {0} no existe en la base de datos", registro.Area);
                  });

                registrosExcel.Where(x => !listaProcesos.Any(y => y.StrCodigo.ToLower() == Common.TextoEnMinuscula(x.Proceso))).ToList()
                  .ForEach(registro =>
                  {
                      registro.ErrorArchivoExcel = String.Format("El proceso {0} no existe en la base de datos", registro.Proceso);
                  });

                registrosExcel.Where(x => !listaFondos.Any(y => y.StrCodigo.ToLower() == Common.TextoEnMinuscula(x.EPS) && y.TiposFondo.StrCodigo == enumTiposFondos.EPS.ToString())).ToList()
                  .ForEach(registro =>
                  {
                      registro.ErrorArchivoExcel = String.Format("El código de EPS {0} no existe en la base de datos", registro.EPS);
                  });

                registrosExcel.Where(x => !listaFondos.Any(y => y.StrCodigo.ToLower() == Common.TextoEnMinuscula(x.AFP) && y.TiposFondo.StrCodigo == enumTiposFondos.AFP.ToString())).ToList()
                  .ForEach(registro =>
                  {
                      registro.ErrorArchivoExcel = String.Format("El código de AFP {0} no existe en la base de datos", registro.AFP);
                  });

                registrosExcel.Where(x => !listaFondos.Any(y => y.StrCodigo.ToLower() == Common.TextoEnMinuscula(x.ARL) && y.TiposFondo.StrCodigo == enumTiposFondos.ARL.ToString())).ToList()
                  .ForEach(registro =>
                  {
                      registro.ErrorArchivoExcel = String.Format("El código de ARL {0} no existe en la base de datos", registro.ARL);
                  });

                registrosExcel.Where(x => !listaTipoContrato.Any(y => y.StrDescripcion.ToLower() == Common.TextoEnMinuscula(x.TipoContrato))).ToList()
                  .ForEach(registro =>
                  {
                      registro.ErrorArchivoExcel = String.Format("El tipo de contrato {0} no existe en la base de datos", registro.TipoContrato);
                  });

                registrosExcel.Where(x => !listaEscolaridades.Any(y => y.StrDescripcion.ToLower() == Common.TextoEnMinuscula(x.Escolaridad))).ToList()
                .ForEach(registro =>
                {
                    registro.ErrorArchivoExcel = String.Format("El código de escolaridad {0} no existe en la base de datos", registro.Escolaridad);
                });

                registrosExcel.Where(x => !listaEstadosCiviles.Any(y => y.StrDescripcion.ToLower() == Common.TextoEnMinuscula(x.EstadoCivil))).ToList()
                .ForEach(registro =>
                {
                    registro.ErrorArchivoExcel = String.Format("El código de estado civil {0} no existe en la base de datos", registro.EstadoCivil);
                });

                //datos sin entidad
                registrosExcel.Where(x => x.Genero != "M" && x.Genero != "F").ToList()
                .ForEach(registro =>
                {
                    registro.ErrorArchivoExcel = String.Format("El código de género {0} no existe en la base de datos", registro.Genero);
                });

                registrosExcel.Where(x => x.Telefono.Length > 15 || x.Celular.Length > 15).ToList()
                .ForEach(registro =>
                {
                    registro.ErrorArchivoExcel = String.Format("El teléfono no puede tener mas de {0} caracteres.", "15");
                });

                registrosExcel.Where(x => x.Salario < 0 || x.Salario > 99999999).ToList()
                   .ForEach(registro =>
                   {
                       registro.ErrorArchivoExcel = "El salario está fuera del rango correcto.";
                   });

                registrosExcel.Where(x => x.NumeroHijos < 0 || x.NumeroHijos > 20).ToList()
                .ForEach(registro =>
                {
                    registro.ErrorArchivoExcel = "El número de hijos está fuera del rango.";
                });

                registrosExcel.Where(x => x.PersonasACargo < 0 || x.PersonasACargo > 20).ToList()
               .ForEach(registro =>
               {
                   registro.ErrorArchivoExcel = "El número de personas a cargo está fuera del rango.";
               });

                registrosExcel.Where(x => x.Estrato < 1 || x.Estrato > 6).ToList()
                 .ForEach(registro =>
                 {
                     registro.ErrorArchivoExcel = "El estrato social está fuera del rango.";
                 });

                registrosExcel.Where(x => x.ErrorArchivoExcel == string.Empty).ToList()
                    .ForEach(registro =>
                    {
                        registro.TerceroID = terceroUsuarioID;
                        registro.Identificacion = registro.Identificacion;
                        registro.TipoIdentificacionID = listaTiposDeIndentificacion.Find(x => x.StrCodigo == registro.TipoIdentificacion).IntTipoIdentificacionID;
                        registro.PrimerApellido = registro.PrimerApellido;
                        registro.SegundoApellido = registro.SegundoApellido;
                        registro.PrimerNombre = registro.PrimerNombre;
                        registro.SegundoNombre = registro.SegundoNombre;
                        registro.Genero = registro.Genero;
                        registro.Nacionalidad = registro.Nacionalidad;
                        registro.CentroDeTrabajoID = listaCentrosDeTrabajo.Find(x => x.StrCodigo == registro.CentroDeTrabajo).IntCentroDeTrabajoID;
                        registro.FechaDeNacimiento = registro.FechaDeNacimiento;
                        registro.FechaDeIngreso = registro.FechaDeIngreso;
                        registro.CiudadNacimientoID = listaCiudades.Find(x => Common.TextoSinTildes(x.StrDescripcion.ToLower()) == Common.TextoSinTildes(registro.CiudadNacimiento.ToLower())).IntCiudadID;
                        registro.CiudadLaboralID = listaCiudades.Find(x => Common.TextoSinTildes(x.StrDescripcion.ToLower()) == Common.TextoSinTildes(registro.CiudadLaboral.ToLower())).IntCiudadID;
                        registro.CiudadResidenciaID = listaCiudades.Find(x => Common.TextoSinTildes(x.StrDescripcion.ToLower()) == Common.TextoSinTildes(registro.CiudadResidencia.ToLower())).IntCiudadID;
                        registro.DireccionResidencia = registro.DireccionResidencia;
                        registro.Telefono = registro.Telefono;
                        registro.Celular = registro.Celular;
                        registro.Email = registro.Email;
                        registro.CargoID = listaCargos.Find(x => x.StrCodigo.ToLower() == registro.Cargo.ToLower()).IntCargoID;
                        registro.TurnoID = listaTurnos.Find(x => x.StrDescripcion.ToLower() == registro.Turno.ToLower()).IntTurnoID;
                        registro.AreaID = listaAreas.Find(x => x.StrCodigo.ToLower() == registro.Area.ToLower()).IntAreaID;
                        registro.ProcesoID = listaProcesos.Find(x => x.StrCodigo.ToLower() == registro.Proceso.ToLower()).IntProcesoID;
                        registro.ARLID = listaFondos.Find(x => x.StrCodigo.ToLower() == registro.ARL.ToLower() && x.TiposFondo.StrCodigo == enumTiposFondos.ARL.ToString()).IntFondoID;
                        registro.EPSID = listaFondos.Find(x => x.StrCodigo.ToLower() == registro.EPS.ToLower() && x.TiposFondo.StrCodigo == enumTiposFondos.EPS.ToString()).IntFondoID;
                        registro.AFPID = listaFondos.Find(x => x.StrCodigo.ToLower() == registro.AFP.ToLower() && x.TiposFondo.StrCodigo == enumTiposFondos.AFP.ToString()).IntFondoID;
                        registro.TipoContratoID = listaTipoContrato.Find(x => x.StrDescripcion.ToLower() == registro.TipoContrato.ToLower()).IntTipoContratoID;
                        registro.Salario = registro.Salario;
                        registro.EscolaridadID = listaEscolaridades.Find(x => x.StrDescripcion.ToLower() == registro.Escolaridad.ToLower()).IntEscolaridadID;
                        registro.EstadoCivilID = listaEstadosCiviles.Find(x => x.StrDescripcion.ToLower() == registro.EstadoCivil.ToLower()).IntEstadoCivilID;
                        registro.NumeroHijos = registro.NumeroHijos;
                        registro.PersonasACargo = registro.PersonasACargo;
                        registro.Estrato = registro.Estrato;
                        registro.GrupoSanquineo = registro.GrupoSanquineo;
                        registro.Condiciones = registro.Condiciones;
                        registro.Alergias = registro.Alergias;
                        registro.Medicinas = registro.Medicinas;
                        registro.Contacto = registro.Contacto;
                        registro.Parentesco = registro.Parentesco;
                        registro.TelefonoContacto = registro.TelefonoContacto;
                        registro.Activo = registro.Activo;

                    });


                return registrosExcel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<EmpleadosDTO>> GuardarEmpleadosDesdeExcelAsync(List<EmpleadosDTO> empleadosExcelDTO)
        {
            try
            {
                if (empleadosExcelDTO.Count() != 0)
                {
                    var listaEmpleadosParaCrear = this.MapearEmpleadosDTO_Empleados(empleadosExcelDTO);

                    if (listaEmpleadosParaCrear.Count() != 0)
                    {
                        foreach (var item in listaEmpleadosParaCrear)
                        {
                            var respuesta = await this.GuardarEmpleadoAsync(item);

                            if (!string.IsNullOrEmpty(respuesta))
                                empleadosExcelDTO.FirstOrDefault(x => x.Identificacion == item.StrIdentificacion).ErrorArchivoExcel = respuesta;
                        }
                    }
                }

                return empleadosExcelDTO.Where(x => x.ErrorArchivoExcel != string.Empty).ToList();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<Empleados> MapearEmpleadosDTO_Empleados(List<EmpleadosDTO> listaEmpleadosDTO)
        {
            try
            {
                var listaEmpleados = listaEmpleadosDTO.Select(x => new Empleados()
                {
                    IntTerceroID = x.TerceroID,
                    StrIdentificacion = x.Identificacion,
                    IntTipoIdentificacion = x.TipoIdentificacionID,
                    StrPrimerApellido = x.PrimerApellido,
                    StrSegundoApellido = x.SegundoApellido,
                    StrPrimerNombre = x.PrimerNombre,
                    StrSegundoNombre = x.SegundoNombre,
                    StrNombreCompleto = x.NombreCompleto,
                    StrGenero = x.Genero,
                    StrNacionalidad = x.Nacionalidad,
                    IntCentroDeTrabajo = x.CentroDeTrabajoID,
                    DatFechaNacimiento = x.FechaDeNacimiento,
                    DatFechaIngreso = x.FechaDeIngreso,
                    IntCiudadNacimiento = x.CiudadNacimientoID,
                    IntCiudadLabora = x.CiudadLaboralID,
                    IntCiudadResidencia = x.CiudadResidenciaID,
                    StrDireccionResidencia = x.DireccionResidencia,
                    StrTelefono = x.Telefono,
                    StrCelular = x.Celular,
                    StrEmail = x.Email,
                    IntCargo = x.CargoID,
                    IntTurno = x.TurnoID,
                    IntAreaID = x.AreaID,
                    IntARL = x.ARLID,
                    IntEPS = x.EPSID,
                    IntAFP = x.AFPID,
                    IntTipoContrato = x.TipoContratoID,
                    IntSalario = x.Salario,
                    IntEscolaridad = x.EscolaridadID,
                    IntEstadoCivil = x.EstadoCivilID,
                    TIntNumeroHijos = x.NumeroHijos,
                    StrPersonasACargo = x.PersonasACargo.ToString(),
                    TIntEstrato = x.Estrato,
                    StrGrupoSanguineo = x.GrupoSanquineo,
                    StrCondiciones = x.Condiciones,
                    StrAlergias = x.Alergias,
                    StrMedicinas = x.Medicinas,
                    StrContacto = x.Contacto,
                    StrParentesco = x.Parentesco,
                    StrTelefonoContacto = x.TelefonoContacto,
                    BitActivo = x.Activo

                }).ToList();

                return listaEmpleados;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Otras entidades

        #region Select List
        public async Task<SelectList> SelectListAsync(string valueSelected = null)
        {
            try
            {
                var entidad = (from tabla in await this.GetAllAsync()
                               orderby tabla.StrIdentificacion
                               select new { CodigoID = tabla.IntEmpleadoID, Descripcion = $"{tabla.StrIdentificacion} - {tabla.StrNombreCompleto}" });

                if (string.IsNullOrEmpty(valueSelected)) return new SelectList(entidad, "CodigoID", "Descripcion");

                return new SelectList(entidad, "CodigoID", "Descripcion", valueSelected);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SelectList> SelectListCiudadesAsync(string valueSelected = null)
        {
            try
            {
                return await _iCiudadesCoreBusiness.DropDownListCiudadesAsync(valueSelected);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SelectList> SelectListTipoIdentificacionAsync(string valueSelected = null)
        {
            try
            {
                return await _iTipoIdentificacionCoreBusiness.SelectListAsync(valueSelected);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SelectList> SelectListEscolaridadesAsync(string valueSelected = null)
        {
            try
            {
                return await _iEscolaridadesCoreBusiness.SelectListAsync(valueSelected);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SelectList> SelectListCentrosDeTrabajoAsync(string valueSelected = null)
        {
            try
            {
                return await _iCentrosDeTrabajoCoreBusiness.SelectListAsync(valueSelected);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SelectList> SelectListEstadosCivilesAsync(string valueSelected = null)
        {
            try
            {
                return await _iEstadosCivilesCoreBusiness.SelectListAsync(valueSelected);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SelectList> SelectListCargosAsync(string valueSelected = null)
        {
            try
            {
                return await _iCargosCoreBusiness.SelectListAsync(valueSelected);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SelectList> SelectListTurnosAsync(string valueSelected = null)
        {
            try
            {
                return await _iTurnosCoreBusiness.SelectListAsync(valueSelected);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SelectList> SelectListFondosAsync(string tipoFondo, string valueSelected = null)
        {
            try
            {
                return await _iFondosCoreBusiness.SelectListAsync(tipoFondo, valueSelected);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SelectList> SelectListEstratosAsync(string valueSelected = null)
        {
            try
            {
                return await _iEstratosCoreBusiness.SelectListAsync(valueSelected);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SelectList> SelectListTiposContratoAsync(string valueSelected = null)
        {
            try
            {
                return await _iTiposContratoCoreBusiness.SelectListAsync(valueSelected);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SelectList> SelectListAreasAsync(string valueSelected = null)
        {
            try
            {
                return await _iAreasCoreBusiness.SelectListAsync(valueSelected);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SelectList SelectListGrupoSanguineo(string valueSelected = null)
        {
            try
            {
                List<SelectListItem> grupoSanguineo = new List<SelectListItem>();
                grupoSanguineo.Add(new SelectListItem() { Text = "O negativo", Value = "O-" });
                grupoSanguineo.Add(new SelectListItem() { Text = "O positivo", Value = "O+" });
                grupoSanguineo.Add(new SelectListItem() { Text = "A negativo", Value = "A-" });
                grupoSanguineo.Add(new SelectListItem() { Text = "A positivo", Value = "A+" });
                grupoSanguineo.Add(new SelectListItem() { Text = "B negativo", Value = "B-" });
                grupoSanguineo.Add(new SelectListItem() { Text = "B positivo", Value = "B+" });
                grupoSanguineo.Add(new SelectListItem() { Text = "AB negativo", Value = "AB-" });
                grupoSanguineo.Add(new SelectListItem() { Text = "AB positivo", Value = "AB+" });

                return new SelectList(grupoSanguineo, "Value", "Text", valueSelected);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public string GuardarArchivoEmpleado(HttpFileCollectionBase files, ParamFilesDTO parametros)
        {
            try
            {
                return Archivos.GuardarArchivo(files, parametros);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long ObtenerTamanoDeArchivo(string nombreArchivo)
        {
            try
            {
                return Archivos.ObtenerTamanoDeArchivo(nombreArchivo);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string ObtenerRutaDeImagenEmpleado(string nombreImagen)
        {
            try
            {
                return Archivos.ObtenerRutaDeImagenEmpleado(nombreImagen);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string EliminarImagenEmpleado(string rutaImagen)
        {
            try
            {
                return Archivos.EliminarArchivo(rutaImagen);
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

        ~EmpleadosCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iCommonCoreBusiness != null)
                {
                    _iCommonCoreBusiness.Dispose();
                    _iCommonCoreBusiness = null;
                }

                if (_iCiudadesCoreBusiness != null)
                {
                    _iCiudadesCoreBusiness.Dispose();
                    _iCiudadesCoreBusiness = null;
                }

                if (_iTipoIdentificacionCoreBusiness != null)
                {
                    _iTipoIdentificacionCoreBusiness.Dispose();
                    _iTipoIdentificacionCoreBusiness = null;
                }

                if (_iEscolaridadesCoreBusiness != null)
                {
                    _iEscolaridadesCoreBusiness.Dispose();
                    _iEscolaridadesCoreBusiness = null;
                }

                if (_iCentrosDeTrabajoCoreBusiness != null)
                {
                    _iCentrosDeTrabajoCoreBusiness.Dispose();
                    _iCentrosDeTrabajoCoreBusiness = null;
                }

                if (_iEstadosCivilesCoreBusiness != null)
                {
                    _iEstadosCivilesCoreBusiness.Dispose();
                    _iEstadosCivilesCoreBusiness = null;
                }

                if (_iCargosCoreBusiness != null)
                {
                    _iCargosCoreBusiness.Dispose();
                    _iCargosCoreBusiness = null;
                }

                if (_iTurnosCoreBusiness != null)
                {
                    _iTurnosCoreBusiness.Dispose();
                    _iTurnosCoreBusiness = null;
                }

                if (_iFondosCoreBusiness != null)
                {
                    _iFondosCoreBusiness.Dispose();
                    _iFondosCoreBusiness = null;
                }

                if (_iEstratosCoreBusiness != null)
                {
                    _iEstratosCoreBusiness.Dispose();
                    _iEstratosCoreBusiness = null;
                }

                if (_iTiposContratoCoreBusiness != null)
                {
                    _iTiposContratoCoreBusiness.Dispose();
                    _iTiposContratoCoreBusiness = null;
                }

                if (_iAreasCoreBusiness != null)
                {
                    _iAreasCoreBusiness.Dispose();
                    _iAreasCoreBusiness = null;
                }

                if (_iProcesosCoreBusiness != null)
                {
                    _iProcesosCoreBusiness.Dispose();
                    _iProcesosCoreBusiness = null;
                }

                if (_iTiposFondoCoreBusiness == null)
                {
                    _iTiposFondoCoreBusiness.Dispose();
                    _iTiposFondoCoreBusiness = null;
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
