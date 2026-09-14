using CoreBusiness;
using Models;
using Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Filters;

namespace Web.Controllers
{

    public class ClientesController : Controller
    {
        UsuariosCoreBusiness _usuariosCoreBusiness;
        ClientesCoreBusiness _clientesCoreBusiness;
        CiudadesCoreBusiness _ciudadesCoreBusiness;
        ContactosCoreBusiness _contactosCoreBusiness;
        ContratosCoreBusiness _contratosCoreBusiness;
        SistemasDeGestionCoreBusiness _sistemasDeGestionCoreBusiness;
        Contratos_UsuariosCoreBusiness _contratos_UsuariosCoreBusiness;
        Contratos_SistemasDeGestionCoreBusiness _contratos_SistemasDeGestionCoreBusiness;
        LogsExCoreBusiness _logsExCoreBusiness;
        ProcesosCoreBusiness _procesosCoreBusiness;

        [AuthorizeUser]
        public ActionResult Index()
        {
            return View();
        }

        #region Abrir vistas
        [HttpPost]
        public async Task<ActionResult> GetAllClientes()
        {
            try
            {
                _clientesCoreBusiness = new ClientesCoreBusiness();
                var model = await _clientesCoreBusiness.GetAllAsync();

                return PartialView("_GetAllClientes", model);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                ViewBag.Error = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return PartialView("_GetAllClientes");
            }
        }

        [HttpGet]
        public async Task<ActionResult> CrearCliente()
        {
            try
            {
                _ciudadesCoreBusiness = new CiudadesCoreBusiness();
                _usuariosCoreBusiness = new UsuariosCoreBusiness();
                _sistemasDeGestionCoreBusiness = new SistemasDeGestionCoreBusiness();
                _procesosCoreBusiness = new ProcesosCoreBusiness();

                var Ciudades = (from c in await _ciudadesCoreBusiness.GetAllAsync()
                                orderby c.StrCodigo
                                select new { Ciudad = c.IntCiudadID, Descripcion = c.StrCodigo + " - " + c.StrDescripcion });

                ViewBag.listaCiudades = new SelectList(Ciudades, "Ciudad", "Descripcion");


                var Usuarios = (from u in await _usuariosCoreBusiness.GetAllAsync()
                                where u.OpcEstado == true
                                orderby u.StrCodigo
                                select new { Usuario = u.IntUsuarioID, Descripcion = u.StrNombre });

                ViewBag.listaUsuarios = new SelectList(Usuarios, "Usuario", "Descripcion");


                var Sistemas = (from s in await _sistemasDeGestionCoreBusiness.GetAllAsync()
                                where s.OpcEstado == true
                                orderby s.StrCodigo
                                select new { Sistema = s.IntSistemaID, Descripcion = s.StrCodigo });

                ViewBag.listaSistemas = new SelectList(Sistemas, "Sistema", "Descripcion");

                var Procesos = (from p in await _procesosCoreBusiness.GetAllAsync()
                                where p.OpcEstado == true
                                orderby p.IntProcesoID
                                select new { Proceso = p.IntProcesoID, Descripcion = p.StrDescripcion });

                ViewBag.listaProcesos = new SelectList(Procesos, "Proceso", "Descripcion");


                return View("_CrearCliente", new Clientes());
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                string mensaje = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public async Task<ActionResult> GetAllContratos(string clienteID)
        {
            try
            {
                return PartialView("_GetAllContratos");
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                ViewBag.Error = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return PartialView("_GetAllContratos");
            }
        }

        [HttpPost]
        public async Task<string> GetAllContratosByEditar(string clienteID)
        {
            try
            {
                _contratosCoreBusiness = new ContratosCoreBusiness();
                _contratos_UsuariosCoreBusiness = new Contratos_UsuariosCoreBusiness();
                _contratos_SistemasDeGestionCoreBusiness = new Contratos_SistemasDeGestionCoreBusiness();

                //Html
                string newtr = string.Empty;
                string newtd = string.Empty;
                string tabla = string.Empty;
                string estado = string.Empty;

                //Contratos
                var model = _contratosCoreBusiness.GetAll();
                model = model.Where(x => x.IntClienteID == Convert.ToInt32(clienteID)).OrderBy(x => x.IntContratoID).ToList();

                if (model.Count() != 0)
                {
                    foreach (var item in model)
                    {
                        newtr = string.Empty;

                        newtr = "<tr class='item pb-0' data-id='" + item.IntContratoID + "'  data-numerocontrato = '" + item.IntNumeroContrato + "' >";
                        newtr = newtr + "<td style='text-align:center; display:none;'><span class='iContratoID'>" + item.IntContratoID + "</span></td>";
                        newtr = newtr + "<td class='font-weight-bold'><span data-contratobd='" + item.IntNumeroContrato + "' class='iNumeroContrato'>" + item.IntNumeroContrato + "</span></td>";
                        newtr = newtr + "<td class=''><span class='iFechaInicialContrato'>" + item.DatFechaInicial.ToString("yyyy-MM-dd") + "</span></td>";
                        newtr = newtr + "<td class=''><span class='iFechaFinalContrato'>" + item.DatFechaFinal.ToString("yyyy-MM-dd") + "</span></td>";
                        newtr = newtr + "<td class=''><span class='iHorasContrato'>" + item.IntHoras + "</span></td>";
                        newtr = newtr + "<td class=''><span class='iValorContrato'>" + item.IntValor + "</span></td>";


                        //Listar usuarios del contrato
                        var listaContratoUsuarios = _contratos_UsuariosCoreBusiness.GetAll();
                        listaContratoUsuarios = listaContratoUsuarios.Where(x => x.IntContratoID == item.IntContratoID).ToList();

                        if (listaContratoUsuarios.Count() != 0)
                        {
                            newtd = string.Empty;
                            foreach (var itemCU in listaContratoUsuarios)
                            {
                                newtd = newtd + "<span class ='iUsuarioID' data-contrato='" + item.IntNumeroContrato + "'>" + itemCU.IntUsuarioID + "</span>";
                            }
                            newtr = newtr + "<td class='' id='listaUsuariosID" + item.IntNumeroContrato + "' style='text-align:center; display:none;'>" + newtd + "</td>";

                            newtd = string.Empty;
                            foreach (var itemCU in listaContratoUsuarios)
                            {
                                newtd = newtd + "<span class='badge badge-primary mr-1'>" + itemCU.Usuarios.StrNombre + "</span>";
                            }
                            newtr = newtr + "<td class='' id='listaUsuarios" + item.IntNumeroContrato + "'>" + newtd + "</td>";
                        }
                        else
                        {
                            newtr = newtr + "<td class='' id='listaUsuariosID" + item.IntNumeroContrato + "' style='text-align:center; display:none;'></td>";
                            newtr = newtr + "<td class='' id='listaUsuarios" + item.IntNumeroContrato + "'></td>";
                        }

                        //Llistar Sistemas del contrato
                        var listaContratoSistemas = _contratos_SistemasDeGestionCoreBusiness.GetAll();
                        listaContratoSistemas = listaContratoSistemas.Where(x => x.IntContratoID == item.IntContratoID).ToList();


                        if (listaContratoSistemas.Count() != 0)
                        {
                            newtd = string.Empty;
                            foreach (var itemCS in listaContratoSistemas)
                            {
                                newtd = newtd + "<span class ='iSistemaID' data-contrato='" + item.IntNumeroContrato + "'>" + itemCS.IntSistemaID + "</span>";
                            }
                            newtr = newtr + "<td class='' id='listaSistemasID" + item.IntNumeroContrato + "' style='text-align:center; display:none;'>" + newtd + "</td>";


                            newtd = string.Empty;
                            foreach (var itemCS in listaContratoSistemas)
                            {
                                newtd = newtd + "<span class='badge badge-primary mr-1'>" + itemCS.SistemasDeGestion.StrCodigo + "</span>";
                            }
                            newtr = newtr + "<td class='' id='listaSistemas" + item.IntNumeroContrato + "'>" + newtd + "</td>";
                        }
                        else
                        {
                            newtr = newtr + "<td class='' id='listaSistemasID" + item.IntNumeroContrato + "' style='text-align:center; display:none;'></td>";
                            newtr = newtr + "<td class='' id='listaSistemas" + item.IntNumeroContrato + "'></td>";

                        }

                        newtr = newtr + "<td class=''><span class='iEmailContrato'>" + item.StrEmail + "</span></td>";

                        if (item.OpcEstado == true)
                        {
                            newtr = newtr + "<td class='text-center h5'><a href='#' class= 'iEstado' data-contratoid = '" + item.IntContratoID + "' data-estado = 'True'  onclick ='clientesCRUD.cambiarEstadoContrato(this)'> <span class='badge badge-success'>ACTIVO</span></a></td>";
                        }
                        else
                        {
                            newtr = newtr + "<td class='text-center h5'><a href='#' class= 'iEstado' data-contratoid = '" + item.IntContratoID + "' data-estado = 'False' onclick ='clientesCRUD.cambiarEstadoContrato(this)'> <span class='badge badge-danger'>INACTIVO</span></a></td>";
                        }

                        newtr = newtr + "<td class='text-right'><button type='button' class='btn bg-white pr-0 mr-1' data-numerocontrato='" + item.IntNumeroContrato + "' onclick='clientesCRUD.getContratoParaEditarAsync(this);'><i class='fa fa-edit fa-1x text-info'></i></button><button class='btn bg-white remove-item' data-contratoid='" + item.IntContratoID + "' onclick='clientesCRUD.identificarContratosDeleted(this)'><em class='fa fa-trash-alt fa-1x text-danger'></em> </button></td></tr>";

                        tabla = tabla + newtr;
                    }
                }
                else
                {
                    tabla = "0";
                }
                return tabla;

            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                ViewBag.ErrorContrato = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return string.Empty;
            }
        }

        [HttpPost]
        public async Task<string> GetAllContactosByEditar(string clienteID)
        {
            try
            {
                _contactosCoreBusiness = new ContactosCoreBusiness();

                //Html
                string newtr = string.Empty;
                string tabla = string.Empty;

                //Contratos
                var model = _contactosCoreBusiness.GetAll();
                model = model.Where(x => x.IntClienteID == Convert.ToInt32(clienteID)).OrderBy(x => x.IntContactoID).ToList();

                if (model.Count() != 0)
                {
                    foreach (var item in model)
                    {
                        newtr = string.Empty;

                        newtr = "<tr class='item' data-id='" + item.IntContactoID + "'>";

                        newtr = newtr + "<td style='text-align:center; display:none';><span class='iContactoID'>" + item.IntContactoID + "</span></td>";
                        newtr = newtr + "<td class=''><span class='iNombreContacto'>" + item.StrNombre + "</span></td>";
                        newtr = newtr + "<td class=''><span class='iCargoContacto'>" + item.StrCargo + "</span></td>";
                        newtr = newtr + "<td class=''><span class='iTelefonoContacto'>" + item.StrTelefonoFijo + "</span></td>";
                        newtr = newtr + "<td class=''><span class='iCelularContacto'>" + item.StrCelular + "</span></td>";
                        newtr = newtr + "<td class=''><span class='iEmailContacto'>" + item.StrEmail + "</span></td>";
                        newtr = newtr + "<td class='text-right '><button class='btn bg-white p-0 remove-item' data-contactoid='" + item.IntContactoID + "' onclick='clientesCRUD.identificarContactosDeleted(this)' ><em class='fa fa-trash-alt fa-1x text-danger'></em> </button></td></tr>";

                        tabla = tabla + newtr;
                    }
                }

                return tabla;

            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                ViewBag.Error = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return string.Empty;
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetAllContactos(string clienteID)
        {
            try
            {
                _contactosCoreBusiness = new ContactosCoreBusiness();
                var model = await _contactosCoreBusiness.GetAllAsync();
                model = model.Where(x => x.IntClienteID == Convert.ToInt32(clienteID)).ToList();

                return PartialView("_GetAllContactos", model);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                ViewBag.Error = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return PartialView("_GetAllContactos");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetContratoParaEditarAsync(Contratos modeloContrato, string modeloUsuarios, string modeloSistemas)
        {
            try
            {
                _usuariosCoreBusiness = new UsuariosCoreBusiness();
                _sistemasDeGestionCoreBusiness = new SistemasDeGestionCoreBusiness();

                var listaUsuarios = JsonConvert.DeserializeObject<List<Usuarios>>(modeloUsuarios);
                var listaSistemas = JsonConvert.DeserializeObject<List<SistemasDeGestion>>(modeloSistemas);

                var modelo = new ContratosDTO();

                modelo.IntNumeroContrato = modeloContrato.IntNumeroContrato;
                modelo.DatFechaInicial = modeloContrato.DatFechaInicial;
                modelo.DatFechaFinal = modeloContrato.DatFechaFinal;
                modelo.IntHoras = modeloContrato.IntHoras;
                modelo.IntValor = modeloContrato.IntValor;
                modelo.ContratosUsuariosDTO = new List<ContratosUsuariosDTO>();
                modelo.ContratosSistemasDTO = new List<ContratosSistemasDeGestionDTO>();

                foreach (var item in listaUsuarios)
                {
                    var usuarios = await _usuariosCoreBusiness.FindAsync(x => x.IntUsuarioID == item.IntUsuarioID);

                    var usuariosDTO = new ContratosUsuariosDTO();
                    usuariosDTO.IntUsuarioID = usuarios.IntUsuarioID;
                    usuariosDTO.NombreUsuario = usuarios.StrNombre;
                    modelo.ContratosUsuariosDTO.Add(usuariosDTO);
                }

                foreach (var item in listaSistemas)
                {
                    var sistemas = await _sistemasDeGestionCoreBusiness.FindAsync(x => x.IntSistemaID == item.IntSistemaID);

                    var sistemasDTO = new ContratosSistemasDeGestionDTO();
                    sistemasDTO.IntSistemaID = sistemas.IntSistemaID;
                    sistemasDTO.SistemaDescripcion = sistemas.StrDescripcion;
                    modelo.ContratosSistemasDTO.Add(sistemasDTO);
                }

                var Usuarios = (from u in await _usuariosCoreBusiness.GetAllAsync()
                                where u.OpcEstado == true
                                orderby u.StrCodigo
                                select new { Usuario = u.IntUsuarioID, Descripcion = u.StrNombre });

                ViewBag.listaUsuarios = new SelectList(Usuarios, "Usuario", "Descripcion");


                var Sistemas = (from s in await _sistemasDeGestionCoreBusiness.GetAllAsync()
                                where s.OpcEstado == true
                                orderby s.StrCodigo
                                select new { Sistema = s.IntSistemaID, Descripcion = s.StrCodigo });

                ViewBag.listaSistemas = new SelectList(Sistemas, "Sistema", "Descripcion");

                return View("_EditarContrato", modelo);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                ViewBag.Error = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return PartialView("_EditarContrato", new Clientes());

            }
        }

        [HttpPost]
        public async Task<ActionResult> GetClienteParaEditarAsync(int clienteID)
        {
            try
            {
                _contratosCoreBusiness = new ContratosCoreBusiness();
                _clientesCoreBusiness = new ClientesCoreBusiness();
                _ciudadesCoreBusiness = new CiudadesCoreBusiness();
                _usuariosCoreBusiness = new UsuariosCoreBusiness();
                _sistemasDeGestionCoreBusiness = new SistemasDeGestionCoreBusiness();
                _procesosCoreBusiness = new ProcesosCoreBusiness();


                var model = await _clientesCoreBusiness.FindAsync(x => x.IntClienteID == clienteID);

                var Ciudades = (from c in await _ciudadesCoreBusiness.GetAllAsync()
                                orderby c.StrCodigo
                                select new { Ciudad = c.IntCiudadID, Descripcion = c.StrCodigo + " - " + c.StrDescripcion });

                ViewBag.listaCiudades = new SelectList(Ciudades, "Ciudad", "Descripcion", model.IntCiudadID);


                var Usuarios = (from u in await _usuariosCoreBusiness.GetAllAsync()
                                where u.OpcEstado == true
                                orderby u.StrCodigo
                                select new { Usuario = u.IntUsuarioID, Descripcion = u.StrNombre });

                ViewBag.listaUsuarios = new SelectList(Usuarios, "Usuario", "Descripcion");


                var Sistemas = (from s in await _sistemasDeGestionCoreBusiness.GetAllAsync()
                                where s.OpcEstado == true
                                orderby s.StrCodigo
                                select new { Sistema = s.IntSistemaID, Descripcion = s.StrCodigo });

                ViewBag.listaSistemas = new SelectList(Sistemas, "Sistema", "Descripcion");

                var Procesos = (from p in await _procesosCoreBusiness.GetAllAsync()
                                where p.OpcEstado == true
                                orderby p.IntProcesoID
                                select new { Proceso = p.IntProcesoID, Descripcion = p.StrDescripcion });

                ViewBag.listaProcesos = new SelectList(Procesos, "Proceso", "Descripcion");

                var contratos = await _contratosCoreBusiness.GetAllAsync();
                contratos = contratos.Where(x => x.IntClienteID == model.IntClienteID).ToList();

                int numeroContratos = 0;
                if (contratos.Count() != 0)
                {
                    numeroContratos = (int)contratos.Max(x => x.IntNumeroContrato);
                }

                ViewBag.NumeroContratos = numeroContratos;

                return View("_EditarCliente", model);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                ViewBag.Error = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return PartialView("_EditarCliente", new Clientes());

            }
        }
        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearClienteAsync(Clientes modelo, string modeloContratos, string modeloUsuarios, string modeloSistemas, string modeloContactos, string modeloProcesos)
        {
            try
            {
                _clientesCoreBusiness = new ClientesCoreBusiness();
                _contratosCoreBusiness = new ContratosCoreBusiness();
                _contratos_UsuariosCoreBusiness = new Contratos_UsuariosCoreBusiness();
                _contratos_SistemasDeGestionCoreBusiness = new Contratos_SistemasDeGestionCoreBusiness();
                _contactosCoreBusiness = new ContactosCoreBusiness();

                List<Contratos> listaContratos = null;
                List<Contratos_Usuarios> listaContratosUsuarios = null;
                List<Contratos_SistemasDeGestion> listaContratosSistemas = null;
                List<Contactos> listaContactos = null;


                string retur = await _clientesCoreBusiness.CreateAsync(modelo);

                if (string.IsNullOrEmpty(retur))
                {
                    listaContratos = JsonConvert.DeserializeObject<List<Contratos>>(modeloContratos);
                    if (listaContratos.Count() != 0)
                    {
                        string returContratos = string.Empty;
                        string returUsuarios = string.Empty;
                        string returSistemas = string.Empty;

                        listaContratosUsuarios = JsonConvert.DeserializeObject<List<Contratos_Usuarios>>(modeloUsuarios);
                        listaContratosSistemas = JsonConvert.DeserializeObject<List<Contratos_SistemasDeGestion>>(modeloSistemas);

                        foreach (var item in listaContratos)
                        {
                            item.IntClienteID = modelo.IntClienteID;
                            await _contratosCoreBusiness.CreateAsync(item);

                            foreach (var cu in listaContratosUsuarios.Where(x => x.IntContratoID == item.IntNumeroContrato))
                            {
                                cu.IntContratoID = item.IntContratoID;
                                await _contratos_UsuariosCoreBusiness.CreateAsync(cu);
                            }

                            foreach (var cs in listaContratosSistemas.Where(x => x.IntContratoID == item.IntNumeroContrato))
                            {
                                cs.IntContratoID = item.IntContratoID;
                                await _contratos_SistemasDeGestionCoreBusiness.CreateAsync(cs);
                            }
                        }
                    }

                    listaContactos = JsonConvert.DeserializeObject<List<Contactos>>(modeloContactos);
                    if (listaContactos.Count() != 0)
                    {
                        foreach (var item in listaContactos)
                        {
                            item.IntClienteID = modelo.IntClienteID;
                            await _contactosCoreBusiness.CreateAsync(item);
                        }
                    }

                    return Json(new { msn = "success" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { error = retur }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                string mensaje = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateClienteAsync(Clientes modelo, string modeloContratos, string modeloUsuarios, string modeloSistemas, string modeloContactos, string modeloProcesos, string modificContrato, string modificContacto, string modeloContratosDeleted, string modeloContactosDeleted, string modeloProcesosDeleted)
        {
            try
            {
                _clientesCoreBusiness = new ClientesCoreBusiness();
                _contratosCoreBusiness = new ContratosCoreBusiness();
                _contratos_UsuariosCoreBusiness = new Contratos_UsuariosCoreBusiness();
                _contratos_SistemasDeGestionCoreBusiness = new Contratos_SistemasDeGestionCoreBusiness();
                _contactosCoreBusiness = new ContactosCoreBusiness();

                List<ContratosDTO> listaContratos = null;
                List<Contactos> listaContactos = null;
                List<Contratos_Usuarios> listaContratosUsuarios = null;
                List<Contratos_SistemasDeGestion> listaContratosSistemas = null;
                List<Contratos> listaContratosEliminados = null;
                List<Contactos> listaContactosEliminados = null;
                listaContratos = JsonConvert.DeserializeObject<List<ContratosDTO>>(modeloContratos);

                string retur = await _clientesCoreBusiness.UpdateAsync(modelo);

                if (string.IsNullOrEmpty(retur))
                {
                    //Eliminar contratos 
                    listaContratosEliminados = JsonConvert.DeserializeObject<List<Contratos>>(modeloContratosDeleted);

                    if (listaContratosEliminados.Count() != 0)
                    {
                        foreach (var item in listaContratosEliminados)
                        {
                            var contrato = await _contratosCoreBusiness.FindAsync(x => x.IntContratoID == item.IntContratoID);
                            await _contratosCoreBusiness.DeleteAsync(contrato);
                        }
                    }

                    //Eliminar contactos 
                    listaContactosEliminados = JsonConvert.DeserializeObject<List<Contactos>>(modeloContactosDeleted);

                    if (listaContactosEliminados.Count() != 0)
                    {
                        foreach (var item in listaContactosEliminados)
                        {
                            var contacto = await _contactosCoreBusiness.FindAsync(x => x.IntContactoID == item.IntContactoID);
                            await _contactosCoreBusiness.DeleteAsync(contacto);
                        }
                    }

                    //Si se modificó o agregó contrato
                    if (modificContrato == "True")
                    {
                        listaContratos = JsonConvert.DeserializeObject<List<ContratosDTO>>(modeloContratos);
                        if (listaContratos.Count() != 0)
                        {
                            string returContratos = string.Empty;
                            string returUsuarios = string.Empty;
                            string returSistemas = string.Empty;
                            Contratos newContrato = null;

                            listaContratosUsuarios = JsonConvert.DeserializeObject<List<Contratos_Usuarios>>(modeloUsuarios);
                            listaContratosSistemas = JsonConvert.DeserializeObject<List<Contratos_SistemasDeGestion>>(modeloSistemas);

                            //Crear contratos nuevos
                            foreach (var item in listaContratos)
                            {
                                if (item.IntContratoID == 0)
                                {
                                    newContrato = _contratosCoreBusiness.ConvertContratosFromDTO(item);
                                    await _contratosCoreBusiness.CreateAsync(newContrato);

                                    foreach (var cu in listaContratosUsuarios.Where(x => x.IntContratoID == item.IntNumeroContratoDB))
                                    {
                                        cu.IntContratoID = newContrato.IntContratoID;
                                        await _contratos_UsuariosCoreBusiness.CreateAsync(cu);
                                    }

                                    foreach (var cs in listaContratosSistemas.Where(x => x.IntContratoID == item.IntNumeroContratoDB))
                                    {
                                        cs.IntContratoID = newContrato.IntContratoID;
                                        await _contratos_SistemasDeGestionCoreBusiness.CreateAsync(cs);
                                    }
                                }
                            }

                            //Modificar contratos existentes, debe ser despues de crear los nuevos contratos
                            foreach (var item in listaContratos)
                            {
                                if (item.IntContratoID != 0)
                                {
                                    newContrato = _contratosCoreBusiness.ConvertContratosFromDTO(item);
                                    await _contratosCoreBusiness.UpdateAsync(newContrato);

                                    var usuariosOld = await _contratos_UsuariosCoreBusiness.GetAllAsync();
                                    usuariosOld = usuariosOld.Where(x => x.IntContratoID == item.IntContratoID).ToList();

                                    //Borrar usuarios del contrato
                                    foreach (var item2 in usuariosOld)
                                    {
                                        await _contratos_UsuariosCoreBusiness.DeleteAsync(item2);
                                    }

                                    var sistemasOld = await _contratos_SistemasDeGestionCoreBusiness.GetAllAsync();
                                    sistemasOld = sistemasOld.Where(x => x.IntContratoID == item.IntContratoID).ToList();

                                    //Borrar sistemas del contrato
                                    foreach (var item2 in sistemasOld)
                                    {
                                        await _contratos_SistemasDeGestionCoreBusiness.DeleteAsync(item2);
                                    }

                                    //Crear nuevamente los asesores modificados
                                    foreach (var cu in listaContratosUsuarios.Where(x => x.IntContratoID == item.IntNumeroContratoDB))
                                    {
                                        cu.IntContratoID = newContrato.IntContratoID;
                                        await _contratos_UsuariosCoreBusiness.CreateAsync(cu);
                                    }

                                    foreach (var cs in listaContratosSistemas.Where(x => x.IntContratoID == item.IntNumeroContratoDB))
                                    {
                                        cs.IntContratoID = newContrato.IntContratoID;
                                        await _contratos_SistemasDeGestionCoreBusiness.CreateAsync(cs);
                                    }

                                }
                            }
                        }
                    }
                    await _contratosCoreBusiness.ValidarNumeracionContratos(modelo.IntClienteID);

                    //Si se modificó o agregó contacto
                    if (modificContacto == "True")
                    {
                        listaContactos = JsonConvert.DeserializeObject<List<Contactos>>(modeloContactos);

                        if (listaContactos.Count() != 0)
                        {
                            //Crear contactos nuevos
                            foreach (var item in listaContactos)
                            {
                                if (item.IntContactoID == 0)
                                {
                                    await _contactosCoreBusiness.CreateAsync(item);
                                }
                            }
                        }
                    }

                    return Json(new { msn = "success" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { error = retur }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                string mensaje = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteClienteAsync(int clienteID)
        {
            try
            {
                _clientesCoreBusiness = new ClientesCoreBusiness();

                var Model = await _clientesCoreBusiness.FindAsync(x => x.IntClienteID == clienteID);

                await _clientesCoreBusiness.DeleteAsync(Model);

                return Json(new { msn = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                string mensaje = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public async Task<bool> CheckExists(string StrIdentificacion)
        {
            try
            {
                _clientesCoreBusiness = new ClientesCoreBusiness();

                return await _clientesCoreBusiness.ExistAsync(x => x.StrIdentificacion == StrIdentificacion);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}