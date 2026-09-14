import React, { useState, useEffect, useRef } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import {
  LayoutDashboard,
  Users,
  Building2,
  ClipboardCheck,
  Settings,
  LogOut,
  Calendar,
  ChevronLeft,
  Pin,
  Lock,
  Clock,
  AlertTriangle
} from 'lucide-react';
import { supabase } from '../config/supabaseClient';
import { API_BASE_URL } from '../config/apiConfig';
import { performCompleteLogout } from '../utils/authUtils';

interface CrmSidebarProps {
  activeTab?: string;
}

export const CrmSidebar: React.FC<CrmSidebarProps> = ({ activeTab }) => {
  const [isPinned, setIsPinned] = useState<boolean>(true);
  const [isHovered, setIsHovered] = useState<boolean>(false);
  const [securityModal, setSecurityModal] = useState<{ title: string; message: string; isDeactivated?: boolean } | null>(null);
  const [redirectCountdown, setRedirectCountdown] = useState<number>(15);
  const eventSourceRef = useRef<EventSource | null>(null);

  const [userProfile, setUserProfile] = useState<{
    nombre: string;
    email: string;
    rol: string;
    modulos: string[];
    activo: boolean;
  }>(() => {
    try {
      const storedRaw = localStorage.getItem('sgi_user');
      if (storedRaw) {
        const u = JSON.parse(storedRaw);
        const isAdmin = u.rol === 'ADMIN_TI' || u.role === 'ADMIN_TI' || u.rol === 'ADMIN' || u.role === 'ADMIN' || u.email === 'admon@waloyogroup.com';
        const defaultModulos = isAdmin
          ? ['dashboard', 'clientes', 'agenda', 'consultor', 'usuarios', '*']
          : (u.modulos || ['dashboard', 'clientes', 'agenda', 'consultor']);

        return {
          nombre: u.nombre || u.email?.split('@')[0] || 'Usuario SGI',
          email: u.email || '',
          rol: isAdmin ? (u.rol || 'ADMIN_TI') : (u.rol || u.role || 'CONSULTOR'),
          modulos: defaultModulos,
          activo: u.activo ?? true
        };
      }
    } catch {}
    return {
      nombre: 'Usuario SGI',
      email: '',
      rol: 'CONSULTOR',
      modulos: ['dashboard', 'clientes', 'agenda', 'consultor'],
      activo: true
    };
  });

  const location = useLocation();
  const navigate = useNavigate();

  // Temporizador de auto-redirección a los 15 segundos al expirar sesión
  useEffect(() => {
    if (!securityModal) return;

    setRedirectCountdown(15);
    const interval = setInterval(() => {
      setRedirectCountdown((prev) => {
        if (prev <= 1) {
          clearInterval(interval);
          setSecurityModal(null);
          void performCompleteLogout().then(() => {
            navigate('/login', { replace: true });
          });
          return 0;
        }
        return prev - 1;
      });
    }, 1000);

    return () => clearInterval(interval);
  }, [securityModal, navigate]);

  // Monitoreo continuo de sesión cada 1 segundo (Soporta Modo Pruebas 10s y Límites Dinámicos)
  useEffect(() => {
    const timer = setInterval(() => {
      if (securityModal) return;

      const storedUserRaw = localStorage.getItem('sgi_user');
      if (!storedUserRaw) return;

      try {
        const storedUser = JSON.parse(storedUserRaw);
        if (!storedUser || !storedUser.loginTimestamp) return;

        const configuredLimit = parseFloat(localStorage.getItem('sgi_session_limit_hours') || '4');
        const MAX_SESSION_MS = Math.round(configuredLimit * 60 * 60 * 1000);
        const elapsed = Date.now() - storedUser.loginTimestamp;

        if (elapsed >= MAX_SESSION_MS) {
          const timeLabel = configuredLimit < 0.01 ? '10 segundos (Modo Pruebas)' : `${configuredLimit} hora(s)`;
          void performCompleteLogout();
          setSecurityModal({
            title: 'Sesión Expirada por Seguridad',
            message: `Su sesión de ${timeLabel} ha expirado por políticas de seguridad del sistema. Por favor ingrese sus credenciales nuevamente.`
          });
        }
      } catch (err) {
        console.error('Error evaluando expiración de sesión:', err);
      }
    }, 1000);

    return () => clearInterval(timer);
  }, [securityModal]);

  useEffect(() => {
    let eventSource: EventSource | null = null;
    let fallbackInterval: number | null = null;
    let reconnectTimer: number | null = null;
    let isComponentMounted = true;

    const clearTimers = () => {
      if (fallbackInterval) {
        window.clearInterval(fallbackInterval);
        fallbackInterval = null;
      }
      if (reconnectTimer) {
        window.clearTimeout(reconnectTimer);
        reconnectTimer = null;
      }
    };

    const syncUserProfile = async (): Promise<void> => {
      if (!isComponentMounted) return;

      try {
        const storedUserRaw = localStorage.getItem('sgi_user');
        const storedUser = storedUserRaw ? JSON.parse(storedUserRaw) : null;

        // Si no hay credenciales locales, forzar cierre y redirección a login
        if (!storedUser || !storedUser.email) {
          await performCompleteLogout();
          if (isComponentMounted) {
            navigate('/login', { replace: true });
          }
          return;
        }

        // Validar límite configurable de sesión
        const configuredLimit = parseFloat(localStorage.getItem('sgi_session_limit_hours') || '4');
        const MAX_SESSION_MS = Math.round(configuredLimit * 60 * 60 * 1000);

        if (storedUser.loginTimestamp) {
          const elapsed = Date.now() - storedUser.loginTimestamp;
          if (elapsed >= MAX_SESSION_MS) {
            const timeLabel = configuredLimit < 0.01 ? '10 segundos (Modo Pruebas)' : `${configuredLimit} hora(s)`;
            await performCompleteLogout();
            if (isComponentMounted) {
              setSecurityModal({
                title: 'Sesión Expirada por Seguridad',
                message: `Su sesión de ${timeLabel} ha expirado por políticas de seguridad del sistema. Por favor ingrese sus credenciales nuevamente.`
              });
            }
            return;
          }
        }

        // Obtener usuario/sesión con timeout protector de 1s para no retrasar el render inicial de la barra
        const userPromise = supabase.auth.getUser();
        const timeoutPromise = new Promise<{ data: { user: any } }>((resolve) =>
          setTimeout(() => resolve({ data: { user: null } }), 1000)
        );
        const { data } = await Promise.race([userPromise, timeoutPromise]);

        // Obtener token de sesión
        let accessToken: string | null = null;
        try {
          const sessionPromise = supabase.auth.getSession();
          const sessionTimeout = new Promise<{ data: { session: any } }>((resolve) =>
            setTimeout(() => resolve({ data: { session: null } }), 1000)
          );
          const sessionRes = await Promise.race([sessionPromise, sessionTimeout]);
          accessToken = sessionRes?.data?.session?.access_token ?? null;
        } catch {
          // Silencioso para evitar saturación de logs
        }

        const activeEmail = data?.user?.email || (storedUser ? storedUser.email : null);

        if (activeEmail && !eventSource && isComponentMounted) {
          // intentar conectar SSE
          try {
            const tokenParam = accessToken ? `&token=${encodeURIComponent(accessToken)}` : '';
            eventSource = new EventSource(`${API_BASE_URL}/usuarios/stream-estado?email=${encodeURIComponent(activeEmail)}${tokenParam}`);
            eventSourceRef.current = eventSource;

            eventSource.onopen = () => {
              clearTimers();
            };

            eventSource.onmessage = async (e) => {
              if (!isComponentMounted) return;
              try {
                const info = e.data ? JSON.parse(e.data) : null;
                if (!info) return;

                if (info.activo === false) {
                  await performCompleteLogout();
                  if (isComponentMounted) {
                    setSecurityModal({
                      title: 'Acceso Desactivado',
                      message: 'Su cuenta de asesor ha sido desactivada. Comuníquese con el administrador para restablecer su acceso.',
                      isDeactivated: true
                    });
                  }
                  return;
                }

                const isUserAdmin = info.rol === 'ADMIN_TI' || info.rol === 'ADMIN';
                const modulosList = isUserAdmin
                  ? ['dashboard', 'clientes', 'agenda', 'consultor', 'usuarios', '*']
                  : (info.modulosPermitidos
                      ? info.modulosPermitidos.split(',')
                      : ['dashboard', 'clientes', 'agenda', 'consultor']);

                setUserProfile({
                  nombre: info.nombreCompleto || activeEmail.split('@')[0],
                  email: activeEmail,
                  rol: info.rol || 'CONSULTOR',
                  modulos: modulosList,
                  activo: info.activo ?? true
                });
              } catch {
                // Parse ignorado
              }
            };

            let failCount = 0;
            eventSource.onerror = () => {
              // Cerrar el stream fallido de inmediato para evitar que el navegador reintente en loop
              try {
                if (eventSource) {
                  eventSource.close();
                  eventSource = null;
                }
                if (eventSourceRef.current) {
                  eventSourceRef.current.close();
                  eventSourceRef.current = null;
                }
              } catch {}

              clearTimers();
              failCount++;

              // Si falla repetidamente (microservicio apagado), pausar los reintentos (espera de 60s)
              const waitMs = failCount > 2 ? 60000 : 15000;

              if (isComponentMounted && document.visibilityState === 'visible') {
                reconnectTimer = window.setTimeout(() => {
                  if (isComponentMounted) {
                    void syncUserProfile();
                  }
                }, waitMs);
              }
            };
          } catch {
            clearTimers();
            if (isComponentMounted) {
              reconnectTimer = window.setTimeout(() => {
                if (isComponentMounted) void syncUserProfile();
              }, 30000);
            }
          }
        }
      } catch {
        clearTimers();
      }
    };

    // Iniciar conexión inicial
    void syncUserProfile();

    // Visibility API: cuando la pestaña vuelve a primer plano, verificar conexión
    const onVisibility = () => {
      if (document.visibilityState === 'visible' && isComponentMounted) {
        if (!eventSource) {
          clearTimers();
          void syncUserProfile();
        }
      } else {
        // Ahorrar memoria y recursos en background
        clearTimers();
        if (eventSource) {
          try {
            eventSource.close();
          } catch {}
          eventSource = null;
        }
        if (eventSourceRef.current) {
          try {
            eventSourceRef.current.close();
          } catch {}
          eventSourceRef.current = null;
        }
      }
    };
    document.addEventListener('visibilitychange', onVisibility);

    return () => {
      isComponentMounted = false;
      clearTimers();
      if (eventSource) {
        try {
          eventSource.close();
        } catch {}
        eventSource = null;
      }
      if (eventSourceRef.current) {
        try {
          eventSourceRef.current.close();
        } catch {}
        eventSourceRef.current = null;
      }
      document.removeEventListener('visibilitychange', onVisibility);
    };
  }, []);

  const isExpanded = isPinned || isHovered;

  const handleLogout = async () => {
    try {
      // 1. Matar en seco la conexión SSE activa para que no quede pendiente en el socket del navegador
      if (eventSourceRef.current) {
        try {
          eventSourceRef.current.close();
        } catch {}
        eventSourceRef.current = null;
      }

      // 2. Purgar credenciales locales
      await performCompleteLogout();
    } catch (error) {
      console.error('Error al cerrar sesión:', error);
    } finally {
      // 3. Redirección instantánea mediante React Router SPA (Cero cuelgues de red)
      navigate('/login', { replace: true });
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
    if (userProfile.rol === 'ADMIN_TI' || userProfile.rol === 'ADMIN' || userProfile.email === 'admon@waloyogroup.com') return true;
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

            {/* Contador visual de auto-redirección de seguridad */}
            <div className="flex items-center justify-between p-3 bg-amber-50 rounded-xl border border-amber-200/70 text-xs font-semibold text-amber-800">
              <span className="flex items-center gap-2">
                <Clock className="w-4 h-4 text-amber-600 animate-pulse" />
                Redirección automática por seguridad:
              </span>
              <span className="bg-amber-200 text-amber-900 px-2.5 py-0.5 rounded-full font-bold">
                {redirectCountdown}s
              </span>
            </div>

            <div className="pt-2">
              <button
                onClick={async () => {
                  setSecurityModal(null);
                  await performCompleteLogout();
                  navigate('/login', { replace: true });
                }}
                className="w-full py-3 bg-[#1E3A8A] text-white rounded-xl text-xs font-bold hover:bg-[#1E3A8A]/90 transition-all shadow-md cursor-pointer flex items-center justify-center gap-2"
              >
                <span>Reingresar al Sistema SGI</span>
                <span className="text-[11px] opacity-80 font-normal">({redirectCountdown}s)</span>
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
              title="Módulo Agenda"
              className={`flex items-center ${!isExpanded ? 'justify-center' : 'gap-3'} px-3.5 py-2.5 rounded-xl text-xs font-semibold transition-all ${
                isActive('/agenda') ? 'bg-[#055bb2] text-white shadow-sm' : 'text-[#d8e3fb]/80 hover:bg-white/10 hover:text-white'
              }`}
            >
              <Calendar className="w-4 h-4 shrink-0" />
              {isExpanded && <span>Módulo Agenda</span>}
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

        {isExpanded && (
          <div className="pt-2 text-center text-[10px] text-[#d8e3fb]/60 leading-tight">
            <span>Desarrollado por </span>
            <a
              href="https://waloyogroup.com/"
              target="_blank"
              rel="noopener noreferrer"
              className="text-white hover:text-[#a9c7ff] font-semibold underline underline-offset-2 transition-colors"
            >
              Waloyo Group
            </a>
          </div>
        )}
      </div>
    </aside>
  );
};

export default CrmSidebar;
