import React, { useState, useEffect } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import {
  LayoutDashboard,
  Users,
  Building2,
  ClipboardCheck,
  Settings,
  LogOut,
  Calendar,
  ShieldCheck,
  ChevronLeft,
  Pin,
  Lock,
  AlertTriangle
} from 'lucide-react';
import { supabase } from '../config/supabaseClient';
import { API_BASE_URL } from '../config/apiConfig';

interface CrmSidebarProps {
  activeTab?: string;
}

export const CrmSidebar: React.FC<CrmSidebarProps> = ({ activeTab }) => {
  const [isPinned, setIsPinned] = useState<boolean>(true);
  const [isHovered, setIsHovered] = useState<boolean>(false);
  const [securityModal, setSecurityModal] = useState<{ title: string; message: string; isDeactivated?: boolean } | null>(null);

  const [userProfile, setUserProfile] = useState<{
    nombre: string;
    email: string;
    rol: string;
    modulos: string[];
    activo: boolean;
  }>({
    nombre: 'Dra. Amanda Durango',
    email: 'admin@gestionintegralsgi.com.co',
    rol: 'ADMIN',
    modulos: ['dashboard', 'clientes', 'agenda', 'consultor', 'usuarios'],
    activo: true
  });

  const location = useLocation();
  const navigate = useNavigate();

  useEffect(() => {
    let eventSource: EventSource | null = null;
    let fallbackInterval: number | null = null;
    let reconnectTimer: number | null = null;

    const syncUserProfile = async (): Promise<void> => {
      try {
        const storedUserRaw = localStorage.getItem('sgi_user');
        let storedUser = storedUserRaw ? JSON.parse(storedUserRaw) : null;

        // Auto-inicializar sgi_user y loginTimestamp si falta
        if (!storedUser || !storedUser.loginTimestamp) {
          storedUser = storedUser || {};
          storedUser.loginTimestamp = Date.now();
          localStorage.setItem('sgi_user', JSON.stringify(storedUser));
        }

        // Validar límite configurable de sesión (Defecto: 4 horas)
        const configuredLimit = parseFloat(localStorage.getItem('sgi_session_limit_hours') || '4');
        const MAX_SESSION_MS = Math.round(configuredLimit * 60 * 60 * 1000);

        if (storedUser && storedUser.loginTimestamp) {
          const elapsed = Date.now() - storedUser.loginTimestamp;
          if (elapsed > MAX_SESSION_MS) {
            const timeLabel = configuredLimit < 0.01 ? '10 segundos (Modo Pruebas)' : `${configuredLimit} hora(s)`;
            localStorage.removeItem('sgi_user');
            await supabase.auth.signOut();
            setSecurityModal({
              title: 'Sesión Expirada por Seguridad',
              message: `Su sesión de ${timeLabel} ha expirado por políticas de seguridad del sistema. Por favor ingrese sus credenciales nuevamente.`
            });
            return;
          }
        }

        const { data } = await supabase.auth.getUser();
        // Obtener token de sesión
        let accessToken: string | null = null;
        try {
          const sessionRes = await supabase.auth.getSession();
          accessToken = sessionRes?.data?.session?.access_token ?? null;
        } catch (error) {
          // Log error de sesión pero continuar
          console.warn('No se pudo obtener token de sesión:', error instanceof Error ? error.message : 'Error desconocido');
        }

        const activeEmail = data?.user?.email || (storedUser ? storedUser.email : null);

        if (activeEmail && !eventSource) {
          // intentar conectar SSE
          try {
            const tokenParam = accessToken ? `&token=${encodeURIComponent(accessToken)}` : '';
            eventSource = new EventSource(`${API_BASE_URL}/usuarios/stream-estado?email=${encodeURIComponent(activeEmail)}${tokenParam}`);

            eventSource.onopen = () => {
              // limpiar fallback
              if (fallbackInterval) { window.clearInterval(fallbackInterval); fallbackInterval = null; }
            };

            eventSource.onmessage = async (e) => {
              try {
                const info = e.data ? JSON.parse(e.data) : null;
                if (!info) return;

                if (info.activo === false) {
                  localStorage.removeItem('sgi_user');
                  await supabase.auth.signOut();
                  setSecurityModal({
                    title: 'Acceso Desactivado',
                    message: 'Su cuenta de asesor ha sido desactivada. Comuníquese con el administrador para restablecer su acceso.',
                    isDeactivated: true
                  });
                  return;
                }

                const modulosList = info.modulosPermitidos
                  ? info.modulosPermitidos.split(',')
                  : ['dashboard', 'clientes', 'agenda', 'consultor'];

                setUserProfile({
                  nombre: info.nombreCompleto || activeEmail.split('@')[0],
                  email: activeEmail,
                  rol: info.rol || 'CONSULTOR',
                  modulos: modulosList,
                  activo: info.activo ?? true
                });
              } catch (err) {
                console.warn('SSE parse error:', err);
              }
            };

            eventSource.onerror = () => {
              // al primer error, cerramos y activamos fallback
              try {
                if (eventSource) {
                  eventSource.close();
                  eventSource = null;
                }
              } catch (closeError) {
                console.warn('Error al cerrar EventSource:', closeError instanceof Error ? closeError.message : 'Error desconocido');
              }
              if (!fallbackInterval) fallbackInterval = window.setInterval(() => { void syncUserProfile(); }, 60000);
              // reconexión simple
              if (reconnectTimer) window.clearTimeout(reconnectTimer);
              reconnectTimer = window.setTimeout(() => { void syncUserProfile(); }, 5000);
            };
          } catch (error) {
            // SSE no disponible, usar fallback polling
            console.warn('Error al crear EventSource:', error instanceof Error ? error.message : 'Error desconocido');
            if (!fallbackInterval) {
              fallbackInterval = window.setInterval(() => { void syncUserProfile(); }, 60000);
            }
          }
        }
      } catch (e) {
        console.warn('Fallback perfil sidebar:', e);
        // fallback si ocurre error
        if (!fallbackInterval) fallbackInterval = window.setInterval(() => { void syncUserProfile(); }, 60000);
      }
    };

    // iniciar la primera carga / conexión
    void syncUserProfile();

    // Visibility API: cuando la pestaña vuelve visible, re-intentar conexión
    const onVisibility = () => {
      if (document.visibilityState === 'visible') {
        void syncUserProfile();
      } else {
        // ahorrar recursos si está en background
        try { if (eventSource) { eventSource.close(); eventSource = null; } } catch(e){}
      }
    };
    document.addEventListener('visibilitychange', onVisibility);

    return () => {
      try {
        if (eventSource) {
          eventSource.close();
        }
      } catch (error) {
        console.warn('Error al cerrar EventSource en cleanup:', error instanceof Error ? error.message : 'Error desconocido');
      }
      if (fallbackInterval) window.clearInterval(fallbackInterval);
      if (reconnectTimer) window.clearTimeout(reconnectTimer);
      document.removeEventListener('visibilitychange', onVisibility);
    };
  }, []);

  const isExpanded = isPinned || isHovered;

  const handleLogout = async () => {
    try {
      localStorage.removeItem('sgi_user');
      await supabase.auth.signOut();
      navigate('/login');
    } catch (error) {
      console.error('Error al cerrar sesión:', error);
      navigate('/login');
    }
  };

  const isActive = (path: string) => {
    if (activeTab && path.includes(activeTab)) return true;
    return location.pathname === path;
  };

  const getRolLabel = (): string => {
    switch (userProfile.rol) {
      case 'ADMIN_TI':
        return '👑 Admin TI';
      case 'ADMIN':
        return 'Admin SGI';
      default:
        return 'Consultor SGI';
    }
  };

  
  const canAccessModule = (modulo: string): boolean => {
    if (userProfile.rol === 'ADMIN_TI' || userProfile.rol === 'ADMIN') return true;
    return userProfile.modulos.includes(modulo);
  };

  return (
    <aside
      onMouseEnter={() => setIsHovered(true)}
      onMouseLeave={() => setIsHovered(false)}
      className={`relative h-screen bg-[#0b1c30] text-white flex flex-col justify-between shadow-2xl transition-all duration-300 z-40 select-none ${
        isExpanded ? 'w-64' : 'w-20'
      }`}
    >
      {/* Modal Corporativo de Seguridad del Sistema */}
      {securityModal && (
        <div className="fixed inset-0 bg-slate-900/80 backdrop-blur-md flex items-center justify-center p-4 z-9999">
          <div className="bg-white rounded-2xl p-6 w-full max-w-md shadow-2xl border border-slate-200 space-y-4 text-slate-800 animate-in fade-in zoom-in-95 duration-200">
            <div className="flex items-center gap-3">
              <div className={`w-12 h-12 rounded-2xl flex items-center justify-center shrink-0 ${securityModal.isDeactivated ? 'bg-red-100 text-red-600' : 'bg-amber-100 text-amber-600'}`}>
                {securityModal.isDeactivated ? <AlertTriangle className="w-6 h-6" /> : <Lock className="w-6 h-6" />}
              </div>
              <div>
                <h3 className="text-base font-bold text-slate-900">{securityModal.title}</h3>
                <p className="text-xs text-slate-500 font-semibold">Gobernanza & Seguridad SGI</p>
              </div>
            </div>
            <p className="text-sm text-slate-600 leading-relaxed bg-slate-50 p-4 rounded-xl border border-slate-200 font-medium">
              {securityModal.message}
            </p>
            <div className="pt-2">
              <button
                onClick={() => {
                  setSecurityModal(null);
                  navigate('/login');
                }}
                className="w-full py-3 bg-[#1E3A8A] text-white rounded-xl text-xs font-bold hover:bg-[#1E3A8A]/90 transition-all shadow-md cursor-pointer"
              >
                Reingresar al Sistema SGI
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Header & Logo */}
      <div>
        <div className="p-4 flex items-center justify-between border-b border-white/10">
          <div className="flex items-center gap-3 overflow-hidden">
            <div className="w-10 h-10 rounded-xl bg-white/10 flex items-center justify-center p-1 shrink-0">
              <img src="/logo-limpio.png" alt="SGI Logo" className="w-full h-full object-contain" />
            </div>
            {isExpanded && (
              <div className="flex flex-col">
                <span className="font-bold text-sm tracking-wide text-white font-headline">SGI CRM</span>
                <span className="text-[10px] text-[#a9c7ff] font-semibold">Gestión Integral SST</span>
              </div>
            )}
          </div>
          <button
            onClick={() => setIsPinned(!isPinned)}
            className={`p-1.5 rounded-lg text-white/70 hover:text-white hover:bg-white/10 transition-colors ${
              !isExpanded && 'hidden'
            }`}
            title={isPinned ? 'Desanclar barra lateral' : 'Anclar barra lateral'}
          >
            {isPinned ? <Pin className="w-4 h-4 text-sky-400 fill-sky-400" /> : <ChevronLeft className="w-4 h-4" />}
          </button>
        </div>

        {/* Navigation Items (RBAC Dynamic Filtering) */}
        <nav className="p-3 space-y-1.5">
          {canAccessModule('dashboard') && (
            <Link
              to="/dashboard"
              title="Dashboard General"
              className={`flex items-center ${!isExpanded ? 'justify-center' : 'gap-3'} px-3.5 py-2.5 rounded-xl text-xs font-semibold transition-all ${
                isActive('/dashboard') ? 'bg-[#055bb2] text-white shadow-sm' : 'text-[#d8e3fb]/80 hover:bg-white/10 hover:text-white'
              }`}
            >
              <LayoutDashboard className="w-4 h-4 shrink-0" />
              {isExpanded && <span>Dashboard General</span>}
            </Link>
          )}

          {canAccessModule('clientes') && (
            <Link
              to="/clientes"
              title="Gestión de Clientes"
              className={`flex items-center ${!isExpanded ? 'justify-center' : 'gap-3'} px-3.5 py-2.5 rounded-xl text-xs font-semibold transition-all ${
                isActive('/clientes') ? 'bg-[#055bb2] text-white shadow-sm' : 'text-[#d8e3fb]/80 hover:bg-white/10 hover:text-white'
              }`}
            >
              <Building2 className="w-4 h-4 shrink-0" />
              {isExpanded && <span>Gestión de Clientes</span>}
            </Link>
          )}

          {canAccessModule('agenda') && (
            <Link
              to="/agenda"
              title="Agenda de Citas"
              className={`flex items-center ${!isExpanded ? 'justify-center' : 'gap-3'} px-3.5 py-2.5 rounded-xl text-xs font-semibold transition-all ${
                isActive('/agenda') ? 'bg-[#055bb2] text-white shadow-sm' : 'text-[#d8e3fb]/80 hover:bg-white/10 hover:text-white'
              }`}
            >
              <Calendar className="w-4 h-4 shrink-0" />
              {isExpanded && <span>Agenda SGI & Citas</span>}
            </Link>
          )}

          {canAccessModule('consultor') && (
            <Link
              to="/consultor"
              title="Módulo Consultor (SG-SST/PESV)"
              className={`flex items-center ${!isExpanded ? 'justify-center' : 'gap-3'} px-3.5 py-2.5 rounded-xl text-xs font-semibold transition-all ${
                isActive('/consultor') ? 'bg-[#055bb2] text-white shadow-sm' : 'text-[#d8e3fb]/80 hover:bg-white/10 hover:text-white'
              }`}
            >
              <ClipboardCheck className="w-4 h-4 shrink-0" />
              {isExpanded && <span>Módulo Consultor</span>}
            </Link>
          )}

          {canAccessModule('usuarios') && (
            <Link
              to="/usuarios"
              title="Asesores, Equipos & Seguridad"
              className={`flex items-center ${!isExpanded ? 'justify-center' : 'gap-3'} px-3.5 py-2.5 rounded-xl text-xs font-semibold transition-all ${
                isActive('/usuarios') ? 'bg-[#055bb2] text-white shadow-sm' : 'text-[#d8e3fb]/80 hover:bg-white/10 hover:text-white'
              }`}
            >
              <Users className="w-4 h-4 shrink-0" />
              {isExpanded && <span>Asesores & Seguridad</span>}
            </Link>
          )}

          <a
            href="https://app.gestionintegralsgi.com.co"
            target="_blank"
            rel="noopener noreferrer"
            title="Plataforma SG-SST Externa"
            className={`flex items-center ${!isExpanded ? 'justify-center' : 'gap-3'} px-3.5 py-2.5 rounded-xl text-xs font-semibold text-[#d8e3fb]/80 hover:bg-white/10 hover:text-white transition-all`}
          >
            <ShieldCheck className="w-4 h-4 shrink-0 text-sky-400" />
            {isExpanded && <span>Plataforma SGI Ext.</span>}
          </a>

          <Link
            to="/perfil"
            title="Perfil & Preferencias"
            className={`flex items-center ${!isExpanded ? 'justify-center' : 'gap-3'} px-3.5 py-2.5 rounded-xl text-xs font-semibold transition-all ${
              isActive('/perfil') ? 'bg-[#055bb2] text-white shadow-sm' : 'text-[#d8e3fb]/80 hover:bg-white/10 hover:text-white'
            }`}
          >
            <Settings className="w-4 h-4 shrink-0" />
            {isExpanded && <span>Perfil & Preferencias</span>}
          </Link>
        </nav>
      </div>

      {/* User Card & Logout */}
      <div className="p-4 border-t border-white/10 space-y-3">
        <div className={`flex items-center ${!isExpanded ? 'justify-center' : 'gap-3'} px-2`}>
          <div className="w-9 h-9 rounded-full bg-[#055bb2] text-white flex items-center justify-center font-bold text-xs shrink-0">
            {userProfile.nombre.substring(0, 2).toUpperCase()}
          </div>
          {isExpanded && (
            <div className="flex flex-col overflow-hidden transition-opacity">
              <span className="text-xs font-bold text-white truncate">{userProfile.nombre}</span>
               <span className="text-[10px] text-[#a9c7ff] truncate">
                 {getRolLabel()}
               </span>
            </div>
          )}
        </div>
        <button
          onClick={handleLogout}
          title="Cerrar Sesión"
          className="w-full flex items-center justify-center gap-2 py-2 rounded-lg bg-white/5 hover:bg-red-500/20 text-red-300 hover:text-red-200 text-xs font-semibold transition-colors cursor-pointer"
        >
          <LogOut className="w-3.5 h-3.5 shrink-0" />
          {isExpanded && <span>Cerrar Sesión</span>}
        </button>
      </div>
    </aside>
  );
};

export default CrmSidebar;
