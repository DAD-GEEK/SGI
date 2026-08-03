import React, { useState, useEffect } from 'react';
import { Users, Search, Plus, UserCheck, Edit, Power, X, AlertTriangle, CheckCircle2, KeyRound, Shield, Clock, Settings, Trash2, ChevronLeft, ChevronRight } from 'lucide-react';
import { CrmSidebar } from '../components/CrmSidebar';
import { API_BASE_URL } from '../config/apiConfig';

interface Usuario {
  id: string;
  documento: string;
  nombreCompleto: string;
  email: string;
  telefonoMovil?: string;
  rol: string;
  activo: boolean;
  modulosPermitidos?: string;
}

const UsuariosView: React.FC = () => {
  const [usuarios, setUsuarios] = useState<Usuario[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [searchTerm, setSearchTerm] = useState<string>('');
  
  // Modals & Settings
  const [showCreateModal, setShowCreateModal] = useState<boolean>(false);
  const [showEditModal, setShowEditModal] = useState<boolean>(false);
  const [usuarioToToggle, setUsuarioToToggle] = useState<Usuario | null>(null);
  const [usuarioToDelete, setUsuarioToDelete] = useState<Usuario | null>(null);
  const [usuarioToResend, setUsuarioToResend] = useState<Usuario | null>(null);

  // Configuración de Sesión del Sistema (Solo Admin TI)
  const [sessionLimitHours, setSessionLimitHours] = useState<number>(() => {
    return parseFloat(localStorage.getItem('sgi_session_limit_hours') || '4');
  });
  const [savedSettingsSuccess, setSavedSettingsSuccess] = useState<boolean>(false);

  const handleSaveSystemSettings = (e: React.FormEvent) => {
    e.preventDefault();
    localStorage.setItem('sgi_session_limit_hours', sessionLimitHours.toString());

    // Reiniciar timestamp de prueba para que los 10s arranquen desde el momento exacto en que se guarda
    const storedUserRaw = localStorage.getItem('sgi_user');
    const storedUser = storedUserRaw ? JSON.parse(storedUserRaw) : {};
    storedUser.loginTimestamp = Date.now();
    localStorage.setItem('sgi_user', JSON.stringify(storedUser));

    setSavedSettingsSuccess(true);
    const label = sessionLimitHours < 0.01 ? '10 Segundos (Modo Pruebas)' : `${sessionLimitHours} horas`;
    showToast(`Configuración guardada: Sesiones expiran automáticamente a las ${label}.`);
    setTimeout(() => setSavedSettingsSuccess(false), 3000);
  };

  // Notification Toast System
  const [toastMessage, setToastMessage] = useState<{ type: 'success' | 'error'; text: string } | null>(null);

  const showToast = (text: string, type: 'success' | 'error' = 'success') => {
    setToastMessage({ text, type });
    setTimeout(() => setToastMessage(null), 4000);
  };

  // Form State
  const [usuarioForm, setUsuarioForm] = useState({
    id: '',
    nombreCompleto: '',
    email: '',
    tipoDocumento: 'CC',
    documento: '',
    pais: 'Colombia (+57)',
    telefonoMovil: '',
    rol: 'CONSULTOR',
    activo: true,
    modulos: {
      dashboard: true,
      clientes: true,
      agenda: true,
      consultor: true,
      usuarios: false
    }
  });

  const isEmailAlreadyRegistered = usuarios.some(
    (u) => u.email.toLowerCase() === usuarioForm.email.toLowerCase() && u.id !== usuarioForm.id
  );

  const fetchUsuarios = async () => {
    try {
      const response = await fetch(`${API_BASE_URL}/usuarios`);
      if (response.ok) {
        const data = await response.json();
        setUsuarios(data);
      }
    } catch (error) {
      console.error('Error al cargar asesores:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsuarios();
    const interval = setInterval(fetchUsuarios, 10000);
    return () => clearInterval(interval);
  }, []);

  const openCreateModal = () => {
    setUsuarioForm({
      id: '',
      nombreCompleto: '',
      email: '',
      tipoDocumento: 'CC',
      documento: '',
      pais: 'Colombia (+57)',
      telefonoMovil: '',
      rol: 'CONSULTOR',
      activo: true,
      modulos: { dashboard: true, clientes: true, agenda: true, consultor: true, usuarios: false }
    });
    setShowCreateModal(true);
  };

  const openEditModal = (u: any) => {
    const modulosList = u.modulosPermitidos ? u.modulosPermitidos.split(',') : ['dashboard', 'clientes', 'agenda'];

    setUsuarioForm({
      id: u.id,
      nombreCompleto: u.nombreCompleto || '',
      email: u.email || '',
      tipoDocumento: u.tipoDocumento || 'CC',
      documento: u.documento || '',
      pais: u.pais || 'Colombia (+57)',
      telefonoMovil: u.telefonoMovil || '',
      rol: u.rol || 'CONSULTOR',
      activo: u.activo ?? true,
      modulos: {
        dashboard: modulosList.includes('dashboard'),
        clientes: modulosList.includes('clientes'),
        agenda: modulosList.includes('agenda'),
        consultor: modulosList.includes('consultor'),
        usuarios: modulosList.includes('usuarios')
      }
    });
    setShowEditModal(true);
  };

  const validateUsuarioForm = () => {
    // 1. Validación de Nombre (Letras y espacios, mínimo 3 caracteres)
    if (!usuarioForm.nombreCompleto || usuarioForm.nombreCompleto.trim().length < 3) {
      showToast('El nombre debe contener al menos 3 caracteres.', 'error');
      return false;
    }
    const nameRegex = /^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$/;
    if (!nameRegex.test(usuarioForm.nombreCompleto.trim())) {
      showToast('El nombre solo debe contener letras y espacios (sin números ni símbolos).', 'error');
      return false;
    }

    // 2. Validación de Correo Electrónico (Estructura RFC Válida)
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!usuarioForm.email || !emailRegex.test(usuarioForm.email.trim())) {
      showToast('Ingrese un correo electrónico corporativo válido (ej: usuario@empresa.com).', 'error');
      return false;
    }

    // 3. Validación de Documento ID (Alfanumérico entre 5 y 20 caracteres)
    if (!usuarioForm.documento || usuarioForm.documento.trim().length < 5 || usuarioForm.documento.trim().length > 20) {
      showToast('El número de documento debe contener entre 5 y 20 caracteres.', 'error');
      return false;
    }

    // 4. Validación de Teléfono (Numérico de 7 a 15 dígitos si se ingresa)
    if (usuarioForm.telefonoMovil && usuarioForm.telefonoMovil.trim().length > 0) {
      const phoneRegex = /^[0-9\s\+\-\(\)]{7,20}$/;
      if (!phoneRegex.test(usuarioForm.telefonoMovil.trim())) {
        showToast('El número telefónico solo debe contener números (mínimo 7 dígitos).', 'error');
        return false;
      }
    }

    // 5. Validación de Módulos (Al menos 1 módulo activo)
    const hasModule = Object.values(usuarioForm.modulos).some(Boolean);
    if (!hasModule) {
      showToast('Debe seleccionar al menos un módulo habilitado para el colaborador.', 'error');
      return false;
    }

    return true;
  };

  const handleRegisterSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!validateUsuarioForm()) return;

    try {
      const selectedModulos = Object.entries(usuarioForm.modulos)
        .filter(([_, enabled]) => enabled)
        .map(([key]) => key)
        .join(',');

      const res = await fetch(`${API_BASE_URL}/usuarios/registrar`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          nombreCompleto: usuarioForm.nombreCompleto.trim(),
          email: usuarioForm.email.trim(),
          tipoDocumento: usuarioForm.tipoDocumento,
          documento: usuarioForm.documento.trim(),
          pais: usuarioForm.pais,
          telefonoMovil: usuarioForm.telefonoMovil ? usuarioForm.telefonoMovil.trim() : '',
          rol: usuarioForm.rol,
          modulosPermitidos: selectedModulos
        })
      });

      if (res.ok) {
        const msg = isEmailAlreadyRegistered
          ? `Credenciales temporales re-enviadas al asesor existente (${usuarioForm.email}).`
          : `Usuario ${usuarioForm.nombreCompleto} registrado exitosamente con clave temporal.`;
        showToast(msg);
        setShowCreateModal(false);
        fetchUsuarios();
      }
    } catch (err) {
      console.error('Error al registrar usuario:', err);
      showToast('Error al conectar con la API de registro.', 'error');
    }
  };

  const handleEditSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!usuarioForm.id) return;

    try {
      const selectedModulos = Object.entries(usuarioForm.modulos)
        .filter(([_, enabled]) => enabled)
        .map(([key]) => key)
        .join(',');

      const res = await fetch(`${API_BASE_URL}/usuarios/${usuarioForm.id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          nombreCompleto: usuarioForm.nombreCompleto.trim(),
          email: usuarioForm.email.trim(),
          tipoDocumento: usuarioForm.tipoDocumento,
          documento: usuarioForm.documento.trim(),
          pais: usuarioForm.pais,
          telefonoMovil: usuarioForm.telefonoMovil ? usuarioForm.telefonoMovil.trim() : '',
          rol: usuarioForm.rol,
          activo: usuarioForm.activo,
          modulosPermitidos: selectedModulos
        })
      });

      if (res.ok) {
        showToast(`Usuario ${usuarioForm.nombreCompleto} actualizado exitosamente.`);
        setShowEditModal(false);
        fetchUsuarios();
      }
    } catch (err) {
      console.error('Error al actualizar usuario:', err);
      showToast('Error al actualizar el usuario.', 'error');
    }
  };

  const handleResendCredentials = async (u: Usuario) => {
    try {
      let res = u.id ? await fetch(`${API_BASE_URL}/usuarios/${u.id}/reenviar-credenciales`, { method: 'POST' }) : null;
      if (!res || !res.ok) {
        res = await fetch(`${API_BASE_URL}/usuarios/reenviar-credenciales-email?email=${encodeURIComponent(u.email)}`, { method: 'POST' });
      }

      if (res.ok) {
        showToast(`Nueva clave temporal generada exitosamente para ${u.email}. En su próximo ingreso deberá cambiarla.`);
        setUsuarioToResend(null);
        fetchUsuarios();
      } else {
        const errorData = await res.json().catch(() => ({}));
        showToast(errorData.error || 'No se pudo reenviar la clave temporal.', 'error');
      }
    } catch (err) {
      console.error('Error al reenviar credenciales:', err);
      showToast('Error de conexión al reenviar credenciales temporales.', 'error');
    }
  };

  const confirmToggleEstado = async () => {
    if (!usuarioToToggle) return;
    const nuevoEstado = !usuarioToToggle.activo;

    try {
      const res = await fetch(`${API_BASE_URL}/usuarios/${usuarioToToggle.id}/alternar-estado`, {
        method: 'PUT'
      });
      if (res.ok) {
        showToast(`Estado del asesor ${usuarioToToggle.nombreCompleto} cambiado a ${nuevoEstado ? 'ACTIVO' : 'INACTIVO'}.`);
        setUsuarioToToggle(null);
        fetchUsuarios();
      }
    } catch (err) {
      console.error('Error al alternar estado:', err);
      showToast('Error al alternar el estado del asesor.', 'error');
    }
  };

  const confirmDeleteUsuario = async () => {
    if (!usuarioToDelete) return;

    try {
      const res = await fetch(`${API_BASE_URL}/usuarios/${usuarioToDelete.id}`, {
        method: 'DELETE'
      });
      if (res.ok) {
        showToast(`Usuario ${usuarioToDelete.nombreCompleto} eliminado permanentemente de la base de datos.`);
        setUsuarioToDelete(null);
        fetchUsuarios();
      }
    } catch (err) {
      console.error('Error al eliminar usuario:', err);
      showToast('Error al eliminar definitivamente el usuario.', 'error');
    }
  };

  const [currentPage, setCurrentPage] = useState<number>(1);
  const itemsPerPage = 5;

  const filteredUsuarios = usuarios.filter(u =>
    (u.nombreCompleto || '').toLowerCase().includes(searchTerm.toLowerCase()) ||
    (u.documento || '').toLowerCase().includes(searchTerm.toLowerCase()) ||
    (u.email || '').toLowerCase().includes(searchTerm.toLowerCase())
  );

  const totalPages = Math.ceil(filteredUsuarios.length / itemsPerPage) || 1;
  const currentUsuarios = filteredUsuarios.slice((currentPage - 1) * itemsPerPage, currentPage * itemsPerPage);

  const handlePageChange = (newPage: number) => {
    if (newPage >= 1 && newPage <= totalPages) {
      setCurrentPage(newPage);
    }
  };

  return (
    <div className="min-h-screen bg-[#F8FAFC] flex font-sans text-slate-800 relative">
      <CrmSidebar activeTab="usuarios" />

      <main className="flex-1 overflow-y-auto p-8 space-y-6">
        {/* Toast Notificación del Sistema */}
        {toastMessage && (
          <div className={`fixed top-5 right-5 z-50 px-4 py-3 rounded-xl shadow-xl border flex items-center gap-3 animate-in fade-in slide-in-from-top duration-300 ${
            toastMessage.type === 'success' ? 'bg-emerald-900 text-emerald-100 border-emerald-700' : 'bg-red-900 text-red-100 border-red-700'
          }`}>
            {toastMessage.type === 'success' ? <CheckCircle2 className="w-5 h-5 text-emerald-400" /> : <AlertTriangle className="w-5 h-5 text-red-400" />}
            <span className="text-xs font-semibold">{toastMessage.text}</span>
          </div>
        )}

        <header className="flex flex-col md:flex-row md:items-center justify-between gap-4">
          <div>
            <div className="flex items-center gap-2 text-sm text-slate-500 mb-1">
              <span>SGI Core</span>
              <span>/</span>
              <span className="text-[#1E3A8A] font-semibold">Gobernanza & Seguridad</span>
            </div>
            <h1 className="text-2xl font-bold text-slate-900 flex items-center gap-2">
              <Users className="w-7 h-7 text-[#1E3A8A]" />
              Asesores, Equipos & Seguridad
            </h1>
          </div>

          <div className="flex items-center gap-3">
            <button 
              onClick={openCreateModal}
              className="flex items-center gap-2 px-4 py-2 bg-[#1E3A8A] text-white rounded-lg hover:bg-[#1E3A8A]/90 font-semibold transition-colors shadow-sm cursor-pointer"
            >
              <Plus className="w-4 h-4" />
              Nuevo Asesor SGI
            </button>
          </div>
        </header>

        {/* Panel de Configuración de Gobernanza de Sesiones del Sistema (Exclusivo Admin TI) */}
        <div className="bg-white rounded-2xl border border-blue-100 p-5 shadow-sm space-y-4">
          <div className="flex items-center justify-between border-b border-slate-100 pb-3">
            <div className="flex items-center gap-2.5">
              <div className="p-2 bg-blue-50 text-[#1E3A8A] rounded-xl">
                <Settings className="w-5 h-5" />
              </div>
              <div>
                <h3 className="text-sm font-bold text-slate-900 flex items-center gap-2">
                  Configuración Global de Seguridad & Expiración de Sesión
                  <span className="bg-purple-100 text-purple-700 text-[10px] px-2 py-0.5 rounded-full font-bold">Admin TI</span>
                </h3>
                <p className="text-xs text-slate-500">Parámetro global aplicable a todos los usuarios del sistema CRM SGI</p>
              </div>
            </div>
          </div>

          <form onSubmit={handleSaveSystemSettings} className="flex flex-col md:flex-row md:items-center justify-between gap-4 text-xs">
            <div className="flex items-center gap-3">
              <Clock className="w-4 h-4 text-slate-400 shrink-0" />
              <label className="font-semibold text-slate-700">Duración máxima de validez de sesión/tokens (en horas):</label>
              <select
                value={sessionLimitHours}
                onChange={(e) => setSessionLimitHours(parseFloat(e.target.value))}
                className="px-3 py-1.5 border border-slate-200 rounded-lg bg-slate-50 font-bold text-slate-900 focus:outline-none focus:border-[#1E3A8A]"
              >
                <option value={0.00277778}>⏱️ 10 Segundos (Modo Pruebas)</option>
                <option value={1}>1 hora (Alta Seguridad)</option>
                <option value={2}>2 horas</option>
                <option value={4}>4 horas (Recomendado)</option>
                <option value={8}>8 horas (Jornada completa)</option>
                <option value={12}>12 horas</option>
                <option value={24}>24 horas</option>
              </select>
            </div>

            <button
              type="submit"
              className="px-4 py-2 bg-slate-900 text-white rounded-lg font-semibold hover:bg-slate-800 transition-colors shadow-xs flex items-center gap-2 shrink-0 cursor-pointer"
            >
              <Shield className="w-3.5 h-3.5 text-emerald-400" />
              <span>Guardar Política de Sesión</span>
            </button>
          </form>

          {savedSettingsSuccess && (
            <p className="text-xs text-emerald-600 font-semibold flex items-center gap-1.5 pt-1">
              <CheckCircle2 className="w-4 h-4" />
              Política de caducidad de sesión guardada. Aplica para todos los accesos.
            </p>
          )}
        </div>

        {/* Search Bar */}
        <div className="bg-white p-4 rounded-xl shadow-sm border border-slate-200/80">
          <div className="relative w-full">
            <Search className="w-4 h-4 text-slate-400 absolute left-3.5 top-1/2 -translate-y-1/2" />
            <input
              type="text"
              placeholder="Buscar por Cédula (CC), nombre de asesor o correo corporativo..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="w-full pl-10 pr-4 py-2 bg-slate-50 border border-slate-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-[#1E3A8A]/20 text-sm"
            />
          </div>
        </div>

        {/* Content Table */}
        {loading ? (
          <div className="bg-white rounded-xl border border-slate-200 p-12 text-center text-slate-500">
            <div className="w-8 h-8 border-3 border-[#1E3A8A] border-t-transparent rounded-full animate-spin mx-auto mb-3" />
            <p className="font-medium">Cargando equipo de asesores en tiempo real...</p>
          </div>
        ) : filteredUsuarios.length === 0 ? (
          <div className="bg-white rounded-xl border border-slate-200 p-12 text-center text-slate-500">
            <Users className="w-12 h-12 text-slate-300 mx-auto mb-3" />
            <p className="font-semibold text-slate-700">No se encontraron usuarios</p>
          </div>
        ) : (
          <div className="bg-white rounded-xl border border-slate-200/80 shadow-sm overflow-hidden">
            <div className="overflow-x-auto">
              <table className="w-full text-left border-collapse text-sm">
                <thead>
                  <tr className="bg-slate-50 border-b border-slate-200 text-slate-600 font-semibold uppercase text-[11px] tracking-wider">
                    <th className="py-3.5 px-4">Asesor / Nombre Completo</th>
                    <th className="py-3.5 px-4">Cédula (CC / Documento ID)</th>
                    <th className="py-3.5 px-4">Rol de Sistema</th>
                    <th className="py-3.5 px-4">Contacto</th>
                    <th className="py-3.5 px-4">Estado</th>
                    <th className="py-3.5 px-4 text-right">Acciones</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {currentUsuarios.map((u) => (
                    <tr key={u.id} className="hover:bg-slate-50/80 transition-colors">
                      <td className="py-3.5 px-4 font-semibold text-slate-900 flex items-center gap-2">
                        <div className="w-8 h-8 bg-blue-50 text-[#1E3A8A] rounded-full flex items-center justify-center font-bold text-xs shrink-0">
                          {u.nombreCompleto ? u.nombreCompleto.substring(0, 2).toUpperCase() : 'US'}
                        </div>
                        <div>
                          <div>{u.nombreCompleto}</div>
                          {(u.email === 'admin@gestionintegralsgi.com.co' || u.email === 'admon@waloyogroup.com') && (
                            <span className="text-[10px] text-purple-700 font-bold bg-purple-50 px-1.5 py-0.5 rounded">Cuenta Matriz TI</span>
                          )}
                        </div>
                      </td>
                      <td className="py-3.5 px-4 font-mono font-semibold text-slate-700">{u.documento || 'CC-00000000'}</td>
                      <td className="py-3.5 px-4">
                        <span className={`px-2.5 py-1 rounded-full text-xs font-semibold ${
                          u.rol === 'ADMIN_TI' ? 'bg-purple-100 text-purple-800 border border-purple-200' :
                          u.rol === 'ADMIN' ? 'bg-blue-100 text-blue-800' : 'bg-slate-100 text-slate-700'
                        }`}>
                          {u.rol === 'ADMIN_TI' ? '👑 Admin TI (Holding)' : u.rol === 'ADMIN' ? 'Admin SGI' : 'Consultor'}
                        </span>
                      </td>
                      <td className="py-3.5 px-4 text-slate-600">
                        <div>{u.email}</div>
                        {u.telefonoMovil && <div className="text-xs text-slate-400">{u.telefonoMovil}</div>}
                      </td>
                      <td className="py-3.5 px-4">
                        <span className={`px-2.5 py-0.5 rounded-full text-xs font-semibold ${u.activo ? 'bg-emerald-100 text-emerald-700 border border-emerald-200' : 'bg-slate-100 text-slate-500 border border-slate-200'}`}>
                          {u.activo ? 'Activo' : 'Inactivo'}
                        </span>
                      </td>
                      <td className="py-3.5 px-4 text-right">
                        <div className="flex items-center justify-end gap-1.5">
                          <button
                            onClick={() => setUsuarioToResend(u)}
                            title="Reenviar Clave Temporal"
                            className="p-1.5 text-blue-600 hover:bg-blue-50 rounded-lg transition-colors cursor-pointer"
                          >
                            <KeyRound className="w-4 h-4" />
                          </button>
                          <button
                            onClick={() => openEditModal(u)}
                            title="Editar Asesor y Permisos"
                            className="p-1.5 text-slate-600 hover:text-[#1E3A8A] hover:bg-slate-100 rounded-lg transition-colors cursor-pointer"
                          >
                            <Edit className="w-4 h-4" />
                          </button>
                          <button
                            onClick={() => setUsuarioToToggle(u)}
                            title={u.activo ? 'Desactivar Acceso' : 'Activar Acceso'}
                            className={`p-1.5 rounded-lg transition-colors cursor-pointer ${u.activo ? 'text-amber-600 hover:bg-amber-50' : 'text-emerald-600 hover:bg-emerald-50'}`}
                          >
                            <Power className="w-4 h-4" />
                          </button>
                          <button
                            onClick={() => setUsuarioToDelete(u)}
                            title="Eliminar Definitivamente del Sistema"
                            className="p-1.5 text-red-500 hover:text-red-700 hover:bg-red-50 rounded-lg transition-colors cursor-pointer"
                          >
                            <Trash2 className="w-4 h-4" />
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            {/* Pie de Paginación (Máximo 5 registros por página) */}
            <div className="px-6 py-4 bg-slate-50 border-t border-slate-200 flex flex-col sm:flex-row items-center justify-between gap-4 text-xs">
              <span className="text-slate-500 font-medium">
                Mostrando <strong className="text-slate-900">{filteredUsuarios.length === 0 ? 0 : (currentPage - 1) * itemsPerPage + 1}</strong> a <strong className="text-slate-900">{Math.min(currentPage * itemsPerPage, filteredUsuarios.length)}</strong> de <strong className="text-slate-900">{filteredUsuarios.length}</strong> asesores
              </span>

              <div className="flex items-center gap-2">
                <button
                  onClick={() => handlePageChange(currentPage - 1)}
                  disabled={currentPage === 1}
                  className="px-3 py-1.5 bg-white border border-slate-200 rounded-lg font-semibold text-slate-700 hover:bg-slate-100 disabled:opacity-40 disabled:cursor-not-allowed flex items-center gap-1 shadow-2xs cursor-pointer"
                >
                  <ChevronLeft className="w-4 h-4" />
                  <span>Anterior</span>
                </button>

                <div className="flex items-center gap-1 px-2 font-bold text-slate-700">
                  <span>Página {currentPage} de {totalPages}</span>
                </div>

                <button
                  onClick={() => handlePageChange(currentPage + 1)}
                  disabled={currentPage === totalPages}
                  className="px-3 py-1.5 bg-white border border-slate-200 rounded-lg font-semibold text-slate-700 hover:bg-slate-100 disabled:opacity-40 disabled:cursor-not-allowed flex items-center gap-1 shadow-2xs cursor-pointer"
                >
                  <span>Siguiente</span>
                  <ChevronRight className="w-4 h-4" />
                </button>
              </div>
            </div>
          </div>
        )}

        {/* Modal Reenviar Credenciales Temporales */}
        {usuarioToResend && (
          <div className="fixed inset-0 bg-slate-900/50 backdrop-blur-xs flex items-center justify-center p-4 z-50">
            <div className="bg-white rounded-2xl p-6 w-full max-w-md shadow-2xl border border-slate-200 space-y-4 animate-in fade-in zoom-in-95 duration-200">
              <div className="flex items-center gap-3">
                <div className="w-11 h-11 rounded-full bg-blue-100 text-blue-600 flex items-center justify-center">
                  <KeyRound className="w-6 h-6" />
                </div>
                <div>
                  <h3 className="text-base font-bold text-slate-900">Reenviar Clave Temporal</h3>
                  <p className="text-xs text-slate-500">Gestión de Accesos SGI</p>
                </div>
              </div>

              <p className="text-sm text-slate-700 leading-relaxed">
                Se generará una nueva contraseña temporal para el asesor <strong className="text-slate-900">{usuarioToResend.nombreCompleto}</strong> (`{usuarioToResend.email}`). Se le exigirá cambiar la contraseña en su próximo inicio de sesión.
              </p>

              <div className="flex justify-end gap-3 pt-3 border-t border-slate-100">
                <button
                  type="button"
                  onClick={() => setUsuarioToResend(null)}
                  className="px-4 py-2 border border-slate-200 text-slate-600 rounded-lg text-sm font-semibold hover:bg-slate-50 cursor-pointer"
                >
                  Cancelar
                </button>
                <button
                  type="button"
                  onClick={() => handleResendCredentials(usuarioToResend)}
                  className="px-5 py-2 bg-[#1E3A8A] text-white rounded-lg text-sm font-semibold hover:bg-[#1E3A8A]/90 shadow-sm cursor-pointer"
                >
                  Reenviar Credenciales
                </button>
              </div>
            </div>
          </div>
        )}

        {/* Modal Confirmación de Cambio de Estado */}
        {usuarioToToggle && (
          <div className="fixed inset-0 bg-slate-900/50 backdrop-blur-xs flex items-center justify-center p-4 z-50">
            <div className="bg-white rounded-2xl p-6 w-full max-w-md shadow-2xl border border-slate-200 space-y-4 animate-in fade-in zoom-in-95 duration-200">
              <div className="flex items-center gap-3">
                <div className={`w-11 h-11 rounded-full flex items-center justify-center ${usuarioToToggle.activo ? 'bg-amber-100 text-amber-600' : 'bg-emerald-100 text-emerald-600'}`}>
                  <AlertTriangle className="w-6 h-6" />
                </div>
                <div>
                  <h3 className="text-base font-bold text-slate-900">
                    {usuarioToToggle.activo ? 'Desactivar Acceso de Asesor' : 'Activar Acceso de Asesor'}
                  </h3>
                  <p className="text-xs text-slate-500">Confirmación de seguridad de plataforma SGI</p>
                </div>
              </div>

              <p className="text-sm text-slate-700 leading-relaxed">
                ¿Está seguro de cambiar el estado del asesor <strong className="text-slate-900">{usuarioToToggle.nombreCompleto}</strong> a <strong className={usuarioToToggle.activo ? 'text-amber-600' : 'text-emerald-600'}>{usuarioToToggle.activo ? 'INACTIVO' : 'ACTIVO'}</strong>?
              </p>

              <div className="flex justify-end gap-3 pt-3 border-t border-slate-100">
                <button
                  type="button"
                  onClick={() => setUsuarioToToggle(null)}
                  className="px-4 py-2 border border-slate-200 text-slate-600 rounded-lg text-sm font-semibold hover:bg-slate-50 cursor-pointer"
                >
                  Cancelar
                </button>
                <button
                  type="button"
                  onClick={confirmToggleEstado}
                  className={`px-5 py-2 text-white rounded-lg text-sm font-semibold shadow-sm cursor-pointer ${
                    usuarioToToggle.activo ? 'bg-amber-600 hover:bg-amber-700' : 'bg-emerald-600 hover:bg-emerald-700'
                  }`}
                >
                  Confirmar {usuarioToToggle.activo ? 'Desactivación' : 'Activación'}
                </button>
              </div>
            </div>
          </div>
        )}

        {/* Modal Confirmación de Eliminación Definitiva */}
        {usuarioToDelete && (
          <div className="fixed inset-0 bg-slate-900/50 backdrop-blur-xs flex items-center justify-center p-4 z-50">
            <div className="bg-white rounded-2xl p-6 w-full max-w-md shadow-2xl border border-red-100 space-y-4 animate-in fade-in zoom-in-95 duration-200">
              <div className="flex items-center gap-3">
                <div className="w-11 h-11 rounded-full bg-red-100 text-red-600 flex items-center justify-center">
                  <Trash2 className="w-6 h-6" />
                </div>
                <div>
                  <h3 className="text-base font-bold text-slate-900">Eliminar Usuario Definitivamente</h3>
                  <p className="text-xs text-red-500 font-semibold">Acción Irreversible</p>
                </div>
              </div>

              <p className="text-sm text-slate-700 leading-relaxed">
                ¿Está seguro de eliminar de forma permanente al usuario <strong className="text-slate-900">{usuarioToDelete.nombreCompleto}</strong> (`{usuarioToDelete.email}`)? Esta acción borra completamente el registro de la base de datos.
              </p>

              <div className="flex justify-end gap-3 pt-3 border-t border-slate-100">
                <button
                  type="button"
                  onClick={() => setUsuarioToDelete(null)}
                  className="px-4 py-2 border border-slate-200 text-slate-600 rounded-lg text-sm font-semibold hover:bg-slate-50 cursor-pointer"
                >
                  Cancelar
                </button>
                <button
                  type="button"
                  onClick={confirmDeleteUsuario}
                  className="px-5 py-2 bg-red-600 text-white rounded-lg text-sm font-semibold hover:bg-red-700 shadow-sm cursor-pointer"
                >
                  Eliminar Definitivamente
                </button>
              </div>
            </div>
          </div>
        )}

        {/* Modal Crear Usuario */}
        {showCreateModal && (
          <div className="fixed inset-0 bg-slate-900/50 backdrop-blur-xs flex items-center justify-center p-4 z-50">
            <div className="bg-white rounded-2xl p-6 w-full max-w-lg shadow-2xl border border-slate-200 space-y-5">
              <div className="flex justify-between items-center border-b border-slate-100 pb-3">
                <h3 className="text-lg font-bold text-slate-900 flex items-center gap-2">
                  <UserCheck className="w-5 h-5 text-[#1E3A8A]" />
                  Registrar Colaborador SGI
                </h3>
                <button onClick={() => setShowCreateModal(false)} className="text-slate-400 hover:text-slate-600 font-bold cursor-pointer">
                  <X className="w-5 h-5" />
                </button>
              </div>

              {isEmailAlreadyRegistered && (
                <div className="p-3 bg-blue-50 border border-blue-200 rounded-xl text-blue-800 text-xs flex items-center gap-2">
                  <KeyRound className="w-4 h-4 text-blue-600 shrink-0" />
                  <span>Este correo ya pertenece a un asesor registrado. Se actualizarán sus datos y se le reenviará una nueva clave temporal.</span>
                </div>
              )}

              <form noValidate onSubmit={handleRegisterSubmit} className="space-y-4 text-sm">
                <div>
                  <label className="block font-semibold text-slate-700 mb-1">Nombre Completo del Colaborador *</label>
                  <input
                    type="text"
                    value={usuarioForm.nombreCompleto}
                    onChange={(e) => setUsuarioForm({ ...usuarioForm, nombreCompleto: e.target.value })}
                    placeholder="Ej: Carlos Mario Restrepo"
                    className="w-full px-3.5 py-2 border border-slate-200 rounded-lg bg-slate-50 focus:outline-none focus:border-[#1E3A8A]"
                  />
                </div>

                <div className="grid grid-cols-2 gap-3">
                  <div>
                    <label className="block font-semibold text-slate-700 mb-1">Tipo de Documento *</label>
                    <select
                      value={usuarioForm.tipoDocumento}
                      onChange={(e) => setUsuarioForm({ ...usuarioForm, tipoDocumento: e.target.value })}
                      className="w-full px-3.5 py-2 border border-slate-200 rounded-lg bg-slate-50 focus:outline-none focus:border-[#1E3A8A]"
                    >
                      <option value="CC">Cédula de Ciudadanía (CC)</option>
                      <option value="CE">Cédula de Extranjería (CE)</option>
                      <option value="PP">Pasaporte (PP)</option>
                      <option value="NIT">Número Identificación Tributaria (NIT)</option>
                      <option value="PPT">Permiso Protección Temporal (PPT)</option>
                    </select>
                  </div>
                  <div>
                    <label className="block font-semibold text-slate-700 mb-1">Número de Documento *</label>
                    <input
                      type="text"
                      value={usuarioForm.documento}
                      onChange={(e) => setUsuarioForm({ ...usuarioForm, documento: e.target.value })}
                      placeholder="Ej: 1020304050"
                      className="w-full px-3.5 py-2 border border-slate-200 rounded-lg bg-slate-50 focus:outline-none focus:border-[#1E3A8A]"
                    />
                  </div>
                </div>

                <div>
                  <label className="block font-semibold text-slate-700 mb-1">Correo Electrónico Corporativo *</label>
                  <input
                    type="email"
                    value={usuarioForm.email}
                    onChange={(e) => setUsuarioForm({ ...usuarioForm, email: e.target.value })}
                    placeholder="usuario@empresa.com"
                    className="w-full px-3.5 py-2 border border-slate-200 rounded-lg bg-slate-50 focus:outline-none focus:border-[#1E3A8A]"
                  />
                </div>

                <div className="grid grid-cols-2 gap-3">
                  <div>
                    <label className="block font-semibold text-slate-700 mb-1">País de Origen / Indicativo</label>
                    <select
                      value={usuarioForm.pais}
                      onChange={(e) => setUsuarioForm({ ...usuarioForm, pais: e.target.value })}
                      className="w-full px-3.5 py-2 border border-slate-200 rounded-lg bg-slate-50 focus:outline-none focus:border-[#1E3A8A]"
                    >
                      <option value="Colombia (+57)">🇨🇴 Colombia (+57)</option>
                      <option value="México (+52)">🇲🇽 México (+52)</option>
                      <option value="Perú (+51)">🇵🇪 Perú (+51)</option>
                      <option value="Ecuador (+593)">🇪🇨 Ecuador (+593)</option>
                      <option value="Chile (+56)">🇨🇱 Chile (+56)</option>
                      <option value="Panamá (+507)">🇵🇦 Panamá (+507)</option>
                      <option value="Estados Unidos (+1)">🇺🇸 Estados Unidos (+1)</option>
                      <option value="España (+34)">🇪🇸 España (+34)</option>
                    </select>
                  </div>
                  <div>
                    <label className="block font-semibold text-slate-700 mb-1">Teléfono Móvil / WhatsApp</label>
                    <input
                      type="text"
                      value={usuarioForm.telefonoMovil}
                      onChange={(e) => setUsuarioForm({ ...usuarioForm, telefonoMovil: e.target.value })}
                      placeholder="Ej: 3112490072"
                      className="w-full px-3.5 py-2 border border-slate-200 rounded-lg bg-slate-50 focus:outline-none focus:border-[#1E3A8A]"
                    />
                  </div>
                </div>

                <div>
                  <label className="block font-semibold text-slate-700 mb-1">Rol en Sistema</label>
                  <select
                    value={usuarioForm.rol}
                    onChange={(e) => setUsuarioForm({ ...usuarioForm, rol: e.target.value })}
                    className="w-full px-3.5 py-2 border border-slate-200 rounded-lg bg-slate-50 focus:outline-none focus:border-[#1E3A8A]"
                  >
                    <option value="CONSULTOR">Consultor / Asesor</option>
                    <option value="ADMIN">Administrador SGI</option>
                    <option value="ADMIN_TI">👑 Administrador TI (Holding / Super Admin)</option>
                  </select>
                </div>

                <div>
                  <label className="block font-semibold text-slate-700 mb-2">Módulos Habilitados</label>
                  <div className="grid grid-cols-2 gap-2 bg-slate-50 p-3 rounded-lg border border-slate-200 text-xs">
                    {Object.entries({
                      dashboard: '📊 Dashboard General',
                      clientes: '🏢 Gestión de Clientes',
                      agenda: '📅 Agenda de Citas',
                      consultor: '💼 Módulo Consultor (SG-SST/PESV)',
                      usuarios: '👥 Administración de Usuarios'
                    }).map(([key, label]) => (
                      <label key={key} className="flex items-center gap-2 cursor-pointer font-medium text-slate-700">
                        <input
                          type="checkbox"
                          checked={(usuarioForm.modulos as any)[key]}
                          onChange={(e) => setUsuarioForm({
                            ...usuarioForm,
                            modulos: { ...usuarioForm.modulos, [key]: e.target.checked }
                          })}
                          className="rounded text-[#1E3A8A]"
                        />
                        <span>{label}</span>
                      </label>
                    ))}
                  </div>
                </div>

                <div className="flex justify-end gap-3 pt-3 border-t border-slate-100">
                  <button
                    type="button"
                    onClick={() => setShowCreateModal(false)}
                    className="px-4 py-2 border border-slate-200 text-slate-600 rounded-lg font-semibold hover:bg-slate-50 cursor-pointer"
                  >
                    Cancelar
                  </button>
                  <button
                    type="submit"
                    className="px-5 py-2 bg-[#1E3A8A] text-white rounded-lg font-semibold hover:bg-[#1E3A8A]/90 shadow-sm cursor-pointer"
                  >
                    {isEmailAlreadyRegistered ? 'Actualizar & Reenviar Clave Temporal' : 'Enviar Invitación & Clave Temporal'}
                  </button>
                </div>
              </form>
            </div>
          </div>
        )}

        {/* Modal Editar Usuario */}
        {showEditModal && (
          <div className="fixed inset-0 bg-slate-900/50 backdrop-blur-xs flex items-center justify-center p-4 z-50">
            <div className="bg-white rounded-2xl p-6 w-full max-w-lg shadow-2xl border border-slate-200 space-y-5">
              <div className="flex justify-between items-center border-b border-slate-100 pb-3">
                <h3 className="text-lg font-bold text-slate-900 flex items-center gap-2">
                  <Edit className="w-5 h-5 text-[#1E3A8A]" />
                  Editar Asesor / Permisos
                </h3>
                <button onClick={() => setShowEditModal(false)} className="text-slate-400 hover:text-slate-600 font-bold cursor-pointer">
                  <X className="w-5 h-5" />
                </button>
              </div>

              <form noValidate onSubmit={handleEditSubmit} className="space-y-4 text-sm">
                <div>
                  <label className="block font-semibold text-slate-700 mb-1">Nombre Completo *</label>
                  <input
                    type="text"
                    value={usuarioForm.nombreCompleto}
                    onChange={(e) => setUsuarioForm({ ...usuarioForm, nombreCompleto: e.target.value })}
                    className="w-full px-3.5 py-2 border border-slate-200 rounded-lg bg-slate-50 focus:outline-none focus:border-[#1E3A8A]"
                  />
                </div>

                <div className="grid grid-cols-2 gap-3">
                  <div>
                    <label className="block font-semibold text-slate-700 mb-1">Tipo de Documento *</label>
                    <select
                      value={usuarioForm.tipoDocumento}
                      onChange={(e) => setUsuarioForm({ ...usuarioForm, tipoDocumento: e.target.value })}
                      className="w-full px-3.5 py-2 border border-slate-200 rounded-lg bg-slate-50 focus:outline-none focus:border-[#1E3A8A]"
                    >
                      <option value="CC">Cédula de Ciudadanía (CC)</option>
                      <option value="CE">Cédula de Extranjería (CE)</option>
                      <option value="PP">Pasaporte (PP)</option>
                      <option value="NIT">Número Identificación Tributaria (NIT)</option>
                      <option value="PPT">Permiso Protección Temporal (PPT)</option>
                    </select>
                  </div>
                  <div>
                    <label className="block font-semibold text-slate-700 mb-1">Número de Documento *</label>
                    <input
                      type="text"
                      value={usuarioForm.documento}
                      onChange={(e) => setUsuarioForm({ ...usuarioForm, documento: e.target.value })}
                      className="w-full px-3.5 py-2 border border-slate-200 rounded-lg bg-slate-50 focus:outline-none focus:border-[#1E3A8A]"
                    />
                  </div>
                </div>

                <div>
                  <label className="block font-semibold text-slate-700 mb-1">Correo Electrónico *</label>
                  <input
                    type="email"
                    value={usuarioForm.email}
                    onChange={(e) => setUsuarioForm({ ...usuarioForm, email: e.target.value })}
                    className="w-full px-3.5 py-2 border border-slate-200 rounded-lg bg-slate-50 focus:outline-none focus:border-[#1E3A8A]"
                  />
                </div>

                <div className="grid grid-cols-2 gap-3">
                  <div>
                    <label className="block font-semibold text-slate-700 mb-1">País / Indicativo</label>
                    <select
                      value={usuarioForm.pais}
                      onChange={(e) => setUsuarioForm({ ...usuarioForm, pais: e.target.value })}
                      className="w-full px-3.5 py-2 border border-slate-200 rounded-lg bg-slate-50 focus:outline-none focus:border-[#1E3A8A]"
                    >
                      <option value="Colombia (+57)">🇨🇴 Colombia (+57)</option>
                      <option value="México (+52)">🇲🇽 México (+52)</option>
                      <option value="Perú (+51)">🇵🇪 Perú (+51)</option>
                      <option value="Ecuador (+593)">🇪🇨 Ecuador (+593)</option>
                      <option value="Chile (+56)">🇨🇱 Chile (+56)</option>
                      <option value="Panamá (+507)">🇵🇦 Panamá (+507)</option>
                      <option value="Estados Unidos (+1)">🇺🇸 Estados Unidos (+1)</option>
                      <option value="España (+34)">🇪🇸 España (+34)</option>
                    </select>
                  </div>
                  <div>
                    <label className="block font-semibold text-slate-700 mb-1">Teléfono Móvil</label>
                    <input
                      type="text"
                      value={usuarioForm.telefonoMovil}
                      onChange={(e) => setUsuarioForm({ ...usuarioForm, telefonoMovil: e.target.value })}
                      className="w-full px-3.5 py-2 border border-slate-200 rounded-lg bg-slate-50 focus:outline-none focus:border-[#1E3A8A]"
                    />
                  </div>
                </div>

                <div>
                  <label className="block font-semibold text-slate-700 mb-1">Estado de Acceso</label>
                  <select
                    value={usuarioForm.activo ? 'true' : 'false'}
                    onChange={(e) => setUsuarioForm({ ...usuarioForm, activo: e.target.value === 'true' })}
                    className="w-full px-3.5 py-2 border border-slate-200 rounded-lg bg-slate-50 focus:outline-none focus:border-[#1E3A8A]"
                  >
                    <option value="true">Activo (Acceso Permitido)</option>
                    <option value="false">Inactivo (Acceso Bloqueado)</option>
                  </select>
                </div>

                <div>
                  <label className="block font-semibold text-slate-700 mb-2">Módulos Habilitados (RBAC)</label>
                  <div className="grid grid-cols-2 gap-2 bg-slate-50 p-3 rounded-lg border border-slate-200 text-xs">
                    {Object.entries({
                      dashboard: '📊 Dashboard General',
                      clientes: '🏢 Gestión de Clientes',
                      agenda: '📅 Agenda de Citas',
                      consultor: '💼 Módulo Consultor (SG-SST/PESV)',
                      usuarios: '👥 Administración de Usuarios'
                    }).map(([key, label]) => (
                      <label key={key} className="flex items-center gap-2 cursor-pointer font-medium text-slate-700">
                        <input
                          type="checkbox"
                          checked={(usuarioForm.modulos as any)[key]}
                          onChange={(e) => setUsuarioForm({
                            ...usuarioForm,
                            modulos: { ...usuarioForm.modulos, [key]: e.target.checked }
                          })}
                          className="rounded text-[#1E3A8A]"
                        />
                        <span>{label}</span>
                      </label>
                    ))}
                  </div>
                </div>

                <div className="flex justify-between items-center pt-3 border-t border-slate-100">
                  <button
                    type="button"
                    onClick={() => {
                      setShowEditModal(false);
                      setUsuarioToResend({
                        id: usuarioForm.id,
                        nombreCompleto: usuarioForm.nombreCompleto,
                        email: usuarioForm.email,
                        documento: usuarioForm.documento,
                        rol: usuarioForm.rol,
                        activo: usuarioForm.activo
                      });
                    }}
                    className="px-3.5 py-2 text-xs bg-blue-50 text-blue-700 border border-blue-200 rounded-lg font-semibold hover:bg-blue-100 flex items-center gap-1.5 cursor-pointer"
                  >
                    <KeyRound className="w-3.5 h-3.5" />
                    Reenviar Clave Temporal
                  </button>

                  <div className="flex gap-2">
                    <button
                      type="button"
                      onClick={() => setShowEditModal(false)}
                      className="px-4 py-2 border border-slate-200 text-slate-600 rounded-lg font-semibold hover:bg-slate-50 cursor-pointer"
                    >
                      Cancelar
                    </button>
                    <button
                      type="submit"
                      className="px-5 py-2 bg-[#1E3A8A] text-white rounded-lg font-semibold hover:bg-[#1E3A8A]/90 shadow-sm cursor-pointer"
                    >
                      Guardar Cambios
                    </button>
                  </div>
                </div>
              </form>
            </div>
          </div>
        )}
      </main>
    </div>
  );
};

export default UsuariosView;
