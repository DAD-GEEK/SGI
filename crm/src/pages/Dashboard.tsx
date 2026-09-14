import React, { useState, useRef, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { CrmSidebar } from '../components/CrmSidebar';
import {
  Users,
  ClipboardCheck,
  Bell,
  User,
  TrendingUp,
  AlertTriangle,
  CheckCircle2,
  Clock,
  Calendar,
  Filter,
  ShieldCheck,
  X
} from 'lucide-react';
import { API_BASE_URL } from '../config/apiConfig';

interface AgendaDashboardItem {
  id: string;
  titulo: string;
  cliente: string;
  descripcion?: string;
  fecha: string;
  hora: string;
  duracion: string;
  asesorNombre: string;
  asesorEmail?: string;
  tipoEvento?: string;
  estado?: string;
}

interface DashboardKpis {
  clientesAsignados?: number;
  horasEjecutadasMes: number;
  horasContratadasMes: number;
  porcentajeEjecucionHoras: number;
  compromisosPendientes: number;
  compromisosVencidos: number;
  auditoriasEnCurso: number;
  auditoriasPendientesFirma: number;
  planesAccionPendientes: number;
  diagnosticosEnProceso: number;
}

interface AuditoriaItem {
  id: string;
  codigo: string;
  cliente: string;
  norma: string;
  estado: string;
  auditor: string;
  fechaInicio: string;
  fechaFin: string;
  firmada: boolean;
}

export const Dashboard: React.FC = () => {
  const [showNotifications, setShowNotifications] = useState(false);
  const popoverRef = useRef<HTMLDivElement>(null);
  const [agendaEventos, setAgendaEventos] = useState<AgendaDashboardItem[]>([]);
  const [loadingAgenda, setLoadingAgenda] = useState<boolean>(true);
  const [kpis, setKpis] = useState<DashboardKpis | null>(null);
  const [loadingKpis, setLoadingKpis] = useState<boolean>(true);
  const [auditoriasRecientes, setAuditoriasRecientes] = useState<AuditoriaItem[]>([]);
  const [loadingAuditorias, setLoadingAuditorias] = useState<boolean>(true);
  const [filtroAgenda, setFiltroAgenda] = useState<'todas' | 'mias'>('todas');
  const [userName, setUserName] = useState<string>(() => {
    try {
      const stored = localStorage.getItem('sgi_user');
      if (stored) {
        const u = JSON.parse(stored);
        if (u.nombre && typeof u.nombre === 'string' && u.nombre.trim().length > 0) {
          return u.nombre.trim();
        }
        if (u.email) {
          const prefix = u.email.split('@')[0];
          return prefix.charAt(0).toUpperCase() + prefix.slice(1);
        }
      }
    } catch {}
    return 'Consultor';
  });
  const [totalClientes, setTotalClientes] = useState<number | null>(null);
  const [clientesActivos, setClientesActivos] = useState<number | null>(null);
  const [loadingClientes, setLoadingClientes] = useState<boolean>(true);
  const [isUserAdmin, setIsUserAdmin] = useState<boolean>(() => {
    try {
      const stored = localStorage.getItem('sgi_user');
      if (stored) {
        const u = JSON.parse(stored);
        return u.rol === 'ADMIN_TI' || u.role === 'ADMIN_TI' || u.rol === 'ADMIN' || u.role === 'ADMIN' || u.email === 'admon@waloyogroup.com';
      }
    } catch {}
    return false;
  });

  useEffect(() => {
    const handleUserUpdate = () => {
      try {
        const stored = localStorage.getItem('sgi_user');
        if (stored) {
          const u = JSON.parse(stored);
          if (u.nombre && typeof u.nombre === 'string' && u.nombre.trim().length > 0) {
            setUserName(u.nombre.trim());
          } else if (u.email) {
            const prefix = u.email.split('@')[0];
            setUserName(prefix.charAt(0).toUpperCase() + prefix.slice(1));
          }
          setIsUserAdmin(u.rol === 'ADMIN_TI' || u.role === 'ADMIN_TI' || u.rol === 'ADMIN' || u.role === 'ADMIN' || u.email === 'admon@waloyogroup.com');
        }
      } catch {}
    };

    window.addEventListener('sgi_user_changed', handleUserUpdate);
    window.addEventListener('storage', handleUserUpdate);
    return () => {
      window.removeEventListener('sgi_user_changed', handleUserUpdate);
      window.removeEventListener('storage', handleUserUpdate);
    };
  }, []);

  const [notifications, setNotifications] = useState<any[]>(() => {
    try {
      const stored = localStorage.getItem('sgi_notifications');
      if (stored) return JSON.parse(stored);
    } catch {}
    return [];
  });

  const unreadCount = notifications.filter((n) => !n.read).length;

  const markAllAsRead = () => {
    const updated = notifications.map((n) => ({ ...n, read: true }));
    setNotifications(updated);
    localStorage.setItem('sgi_notifications', JSON.stringify(updated));
  };

  const removeNotification = (id: string, e: React.MouseEvent) => {
    e.stopPropagation();
    const updated = notifications.filter((n) => n.id !== id);
    setNotifications(updated);
    localStorage.setItem('sgi_notifications', JSON.stringify(updated));
  };

  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (popoverRef.current && !popoverRef.current.contains(e.target as Node)) {
        setShowNotifications(false);
      }
    };
    if (showNotifications) {
      document.addEventListener('mousedown', handleClickOutside);
    }
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, [showNotifications]);

  useEffect(() => {
    const reloadNotifs = () => {
      try {
        const stored = localStorage.getItem('sgi_notifications');
        if (stored) setNotifications(JSON.parse(stored));
      } catch {}
    };
    window.addEventListener('sgi_notifications_changed', reloadNotifs);
    window.addEventListener('storage', reloadNotifs);
    return () => {
      window.removeEventListener('sgi_notifications_changed', reloadNotifs);
      window.removeEventListener('storage', reloadNotifs);
    };
  }, []);

  useEffect(() => {
    const fetchAgenda = async () => {
      try {
        setLoadingAgenda(true);
        const storedRaw = localStorage.getItem('sgi_user');
        const user = storedRaw ? JSON.parse(storedRaw) : null;
        const email = user?.email || '';
        const soloMias = isUserAdmin ? (filtroAgenda === 'mias') : true;
        const res = await fetch(`${API_BASE_URL}/agenda/dashboard?email=${encodeURIComponent(email)}&limit=5&soloMias=${soloMias}`);
        if (res.ok) {
          const data = await res.json();
          setAgendaEventos(Array.isArray(data) ? data : []);
        }
      } catch (err) {
        console.error('Error cargando asesorías de agenda:', err);
      } finally {
        setLoadingAgenda(false);
      }
    };
    void fetchAgenda();
  }, [filtroAgenda, isUserAdmin]);

  useEffect(() => {
    const fetchClientes = async () => {
      try {
        setLoadingClientes(true);
        const res = await fetch(`${API_BASE_URL}/clientes`);
        if (res.ok) {
          const list = await res.json();
          if (Array.isArray(list)) {
            setTotalClientes(list.length);
            const activos = list.filter((c: any) => c.activo === true || c.activo === 1 || c.activo === 'true').length;
            setClientesActivos(activos);
          }
        }
      } catch (err) {
        console.error('Error cargando métricas de clientes en Dashboard:', err);
      } finally {
        setLoadingClientes(false);
      }
    };
    void fetchClientes();
  }, []);

  useEffect(() => {
    const fetchMetrics = async () => {
      try {
        setLoadingKpis(true);
        setLoadingAuditorias(true);
        const storedRaw = localStorage.getItem('sgi_user');
        const user = storedRaw ? JSON.parse(storedRaw) : null;
        const email = user?.email || '';
        const soloMias = isUserAdmin ? (filtroAgenda === 'mias') : true;

        const [kpiRes, audRes] = await Promise.all([
          fetch(`${API_BASE_URL}/dashboard/kpis?email=${encodeURIComponent(email)}&soloMias=${soloMias}`),
          fetch(`${API_BASE_URL}/dashboard/auditorias?email=${encodeURIComponent(email)}&limit=6&soloMias=${soloMias}`)
        ]);

        if (kpiRes.ok) {
          const kpiData = await kpiRes.json();
          setKpis(kpiData);
        }
        if (audRes.ok) {
          const audData = await audRes.json();
          setAuditoriasRecientes(Array.isArray(audData) ? audData : []);
        }
      } catch (err) {
        console.error('Error cargando metricas operativas en Dashboard:', err);
      } finally {
        setLoadingKpis(false);
        setLoadingAuditorias(false);
      }
    };
    void fetchMetrics();
  }, [filtroAgenda, isUserAdmin]);


  return (
    <div className="h-screen bg-[#f7f9fb] flex flex-col md:flex-row font-sans overflow-hidden">
      {/* Shared CrmSidebar */}
      <CrmSidebar />

      {/* Main Workspace Area */}
      <div className="flex-grow flex flex-col overflow-y-auto w-full transition-all duration-300 ease-in-out">
        {/* Top Header */}
        <header className="bg-white border-b border-[#c2c6d4]/40 px-6 py-3.5 flex items-center justify-between sticky top-0 z-30 elevation-1">
          <div className="flex items-center gap-3">
            <h2 className="text-base font-bold font-headline text-[#191c1e]">
              Panel de Control
            </h2>
            <span className="hidden sm:inline-block text-[11px] font-semibold text-[#055bb2] bg-[#055bb2]/10 px-2.5 py-0.5 rounded-full">
              SGI Software
            </span>
            <span className="hidden sm:inline-block text-slate-300">|</span>
            <span className="text-xs sm:text-sm font-medium text-[#424752]">
              ¡Hola, <strong className="font-semibold text-[#191c1e]">{userName}</strong>! Bienvenido(a)
            </span>
          </div>

          <div className="flex items-center gap-3">
            {/* Notification Center Popover */}
            <div className="relative" ref={popoverRef}>
              <button
                onClick={() => setShowNotifications(!showNotifications)}
                className="relative p-2 rounded-xl text-[#545f73] hover:bg-[#f2f4f6] transition-colors cursor-pointer"
                title="Notificaciones del sistema"
              >
                <Bell className="w-5 h-5" />
                {unreadCount > 0 && (
                  <span className="absolute top-1.5 right-1.5 w-2 h-2 rounded-full bg-red-500 ring-2 ring-white animate-pulse" />
                )}
              </button>

              {showNotifications && (
                <div className="absolute right-0 mt-2 w-80 sm:w-96 bg-white rounded-2xl shadow-2xl border border-[#c2c6d4]/60 z-50 overflow-hidden">
                  <div className="p-4 bg-[#055bb2] text-white flex items-center justify-between">
                    <div className="flex items-center gap-2">
                      <Bell className="w-4 h-4" />
                      <span className="font-bold text-sm">Notificaciones</span>
                      {unreadCount > 0 && (
                        <span className="bg-white/20 text-white text-[10px] font-bold px-2 py-0.5 rounded-full">
                          {unreadCount} nuevas
                        </span>
                      )}
                    </div>
                    {unreadCount > 0 && (
                      <button
                        onClick={markAllAsRead}
                        className="text-[11px] underline text-sky-100 hover:text-white cursor-pointer transition-colors"
                      >
                        Marcar leídas
                      </button>
                    )}
                  </div>

                  <div className="max-h-80 overflow-y-auto divide-y divide-[#eceef0]">
                    {notifications.length === 0 ? (
                      <div className="p-6 text-center text-[#727783] text-xs">
                        No hay notificaciones pendientes.
                      </div>
                    ) : (
                      notifications.map((n) => (
                        <div
                          key={n.id}
                          className={`p-3.5 transition-colors flex gap-3 items-start ${
                            n.read ? 'bg-white hover:bg-slate-50' : 'bg-sky-50/50 hover:bg-sky-50'
                          }`}
                        >
                          <div className="p-2 rounded-xl bg-emerald-500/10 text-emerald-600 shrink-0 mt-0.5">
                            <ShieldCheck className="w-4 h-4" />
                          </div>
                          <div className="flex-grow space-y-1">
                            <div className="flex items-center justify-between gap-2">
                              <p className="text-xs font-bold text-[#191c1e] leading-snug">
                                {n.title}
                              </p>
                              <div className="flex items-center gap-1.5 shrink-0">
                                <span className="text-[10px] text-[#727783] font-mono">
                                  {n.time}
                                </span>
                                <button
                                  onClick={(e) => removeNotification(n.id, e)}
                                  className="p-1 rounded-md text-[#727783] hover:text-red-600 hover:bg-red-50 transition-colors cursor-pointer"
                                  title="Quitar notificación"
                                >
                                  <X className="w-3.5 h-3.5" />
                                </button>
                              </div>
                            </div>
                            <p className="text-[11px] text-[#424752] leading-relaxed">
                              {n.description}
                            </p>
                          </div>
                        </div>
                      ))
                    )}
                  </div>
                </div>
              )}
            </div>
            <Link
              to="/perfil"
              className="p-2 rounded-xl text-[#545f73] hover:bg-[#f2f4f6] transition-colors"
              title="Mi Perfil"
            >
              <User className="w-5 h-5" />
            </Link>
          </div>
        </header>

        {/* Dashboard Content — Adaptable Fluid Layout */}
        <main className="w-full max-w-[1720px] mx-auto px-4 sm:px-6 lg:px-8 py-4 sm:py-5 space-y-4 sm:space-y-5 flex-grow flex flex-col transition-all duration-300">
          {/* KPI Cards Grid */}
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-3.5 xl:gap-4.5">
            {/* KPI 1 */}
            <Link to="/clientes" className="bg-white p-4 sm:p-4.5 xl:p-5 rounded-2xl border border-[#c2c6d4]/40 elevation-1 space-y-2.5 hover:border-[#055bb2]/60 hover:shadow-md transition-all group flex flex-col justify-between">
              <div className="flex justify-between items-center">
                <span className="text-xs font-semibold text-[#545f73] group-hover:text-[#055bb2] transition-colors">
                  {isUserAdmin ? 'Clientes Activos' : 'Mis Clientes Asignados'}
                </span>
                <div className="p-1.5 sm:p-2 bg-[#055bb2]/10 rounded-xl text-[#055bb2]">
                  <Users className="w-4 h-4 sm:w-5 sm:h-5" />
                </div>
              </div>
              <div className="flex items-baseline gap-2">
                <span className="text-2xl sm:text-3xl font-bold font-headline text-[#191c1e]">
                  {isUserAdmin ? (
                    loadingClientes ? (
                      <span className="text-lg text-[#727783] animate-pulse">...</span>
                    ) : (
                      clientesActivos ?? 0
                    )
                  ) : (
                    loadingKpis ? (
                      <span className="text-lg text-[#727783] animate-pulse">...</span>
                    ) : (
                      kpis?.clientesAsignados ?? 0
                    )
                  )}
                </span>
                <span className="text-xs font-semibold text-emerald-600 inline-flex items-center" title="Directorio actualizado">
                  <TrendingUp className="w-4 h-4" />
                </span>
              </div>
              <p className="text-[11px] sm:text-xs text-[#727783] truncate">
                {isUserAdmin
                  ? (totalClientes !== null ? `${clientesActivos ?? 0} activos de ${totalClientes} registrados` : 'Directorio corporativo B2B')
                  : (kpis ? `${kpis.clientesAsignados ?? 0} cuentas a su cargo en contratos` : 'Empresas asignadas')}
              </p>
            </Link>

            {/* KPI 2 */}
            <Link to="/consultor" className="bg-white p-4 sm:p-4.5 xl:p-5 rounded-2xl border border-[#c2c6d4]/40 elevation-1 space-y-2.5 hover:border-[#055bb2]/60 hover:shadow-md transition-all group flex flex-col justify-between">
              <div className="flex justify-between items-center">
                <span className="text-xs font-semibold text-[#545f73] group-hover:text-[#055bb2] transition-colors">Auditorías en Curso</span>
                <div className="p-1.5 sm:p-2 bg-[#3374cd]/10 rounded-xl text-[#055bb2]">
                  <ClipboardCheck className="w-4 h-4 sm:w-5 sm:h-5" />
                </div>
              </div>
              <div className="flex items-baseline gap-2">
                <span className="text-2xl sm:text-3xl font-bold font-headline text-[#191c1e]">
                  {loadingKpis ? (
                    <span className="text-lg text-[#727783] animate-pulse">...</span>
                  ) : (
                    kpis?.auditoriasEnCurso ?? 0
                  )}
                </span>
                <span className="text-xs font-medium text-[#424752]">SG-SST & ISO</span>
              </div>
              <p className="text-[11px] sm:text-xs text-[#727783] truncate">
                {kpis
                  ? `${kpis.auditoriasPendientesFirma} pendientes de firma informe`
                  : 'Auditorías internas y de certificación'}
              </p>
            </Link>

            {/* KPI 3 */}
            <Link to="/agenda" className="bg-white p-4 sm:p-4.5 xl:p-5 rounded-2xl border border-[#c2c6d4]/40 elevation-1 space-y-2.5 hover:border-[#055bb2]/60 hover:shadow-md transition-all group flex flex-col justify-between">
              <div className="flex justify-between items-center">
                <span className="text-xs font-semibold text-[#545f73] group-hover:text-[#055bb2] transition-colors">Ejecución del Mes</span>
                <div className="p-1.5 sm:p-2 bg-emerald-500/10 rounded-xl text-emerald-600">
                  <CheckCircle2 className="w-4 h-4 sm:w-5 sm:h-5" />
                </div>
              </div>
              <div className="flex items-baseline gap-2">
                <span className="text-2xl sm:text-3xl font-bold font-headline text-[#191c1e]">
                  {loadingKpis ? (
                    <span className="text-lg text-[#727783] animate-pulse">...</span>
                  ) : (
                    `${kpis?.porcentajeEjecucionHoras ?? 0}%`
                  )}
                </span>
                <span className="text-xs font-semibold text-emerald-600">
                  {kpis ? `${kpis.horasEjecutadasMes}h ejecutadas` : ''}
                </span>
              </div>
              <p className="text-[11px] sm:text-xs text-[#727783] truncate">
                {kpis
                  ? `${kpis.horasEjecutadasMes}h de ${kpis.horasContratadasMes}h contratadas`
                  : 'Horas de asesoría'}
              </p>
            </Link>

            {/* KPI 4 */}
            <Link to="/agenda" className="bg-white p-4 sm:p-4.5 xl:p-5 rounded-2xl border border-[#c2c6d4]/40 elevation-1 space-y-2.5 hover:border-[#055bb2]/60 hover:shadow-md transition-all group flex flex-col justify-between">
              <div className="flex justify-between items-center">
                <span className="text-xs font-semibold text-[#545f73] group-hover:text-[#055bb2] transition-colors">Compromisos de Actas</span>
                <div className="p-1.5 sm:p-2 bg-amber-500/10 rounded-xl text-amber-600">
                  <AlertTriangle className="w-4 h-4 sm:w-5 sm:h-5" />
                </div>
              </div>
              <div className="flex items-baseline gap-2">
                <span className="text-2xl sm:text-3xl font-bold font-headline text-[#191c1e]">
                  {loadingKpis ? (
                    <span className="text-lg text-[#727783] animate-pulse">...</span>
                  ) : (
                    kpis?.compromisosPendientes ?? 0
                  )}
                </span>
                <span className={`text-xs font-semibold ${kpis && kpis.compromisosVencidos > 0 ? 'text-rose-600' : 'text-emerald-600'}`}>
                  {kpis && kpis.compromisosVencidos > 0 ? `${kpis.compromisosVencidos} vencidos` : 'Al día'}
                </span>
              </div>
              <p className="text-[11px] sm:text-xs text-[#727783] truncate">
                {kpis
                  ? `${kpis.planesAccionPendientes} planes de acción en auditorías`
                  : 'Compromisos pactados en visitas'}
              </p>
            </Link>
          </div>

          {/* Main Table & Widgets (12-Column Fluid Responsive Grid) */}
          <div className="grid grid-cols-1 lg:grid-cols-12 gap-4 xl:gap-5 flex-grow items-stretch">
            {/* Table Section (8 Cols in XL, 7 Cols in LG) */}
            <div className="lg:col-span-7 xl:col-span-8 bg-white rounded-2xl border border-[#c2c6d4]/40 elevation-1 flex flex-col justify-between overflow-hidden">
              <div>
                <div className="px-4 sm:px-5 py-3 sm:py-3.5 border-b border-[#e0e3e5] flex justify-between items-center bg-white">
                  <div>
                    <h3 className="font-bold text-sm sm:text-base font-headline text-[#191c1e]">
                      Auditorías y Acompañamientos Recientes
                    </h3>
                    <p className="text-[11px] sm:text-xs text-[#727783]">Consultor SGI y estado normativo</p>
                  </div>
                  <Link
                    to="/consultor"
                    className="inline-flex items-center gap-1.5 text-xs font-semibold text-[#055bb2] hover:bg-[#d6e3ff]/40 px-2.5 sm:px-3 py-1 sm:py-1.5 rounded-lg transition-colors"
                  >
                    <Filter className="w-3.5 h-3.5" />
                    <span>Ver Todas</span>
                  </Link>
                </div>

                <div className="overflow-x-auto">
                  <table className="w-full text-left text-xs">
                    <thead className="bg-[#f8fafc] text-[#545f73] font-semibold border-b border-[#e0e3e5]">
                      <tr>
                        <th className="px-3.5 sm:px-4 py-2 sm:py-2.5 text-[11px] font-semibold">Cliente / Empresa</th>
                        <th className="px-3.5 sm:px-4 py-2 sm:py-2.5 text-[11px] font-semibold">Norma / Código</th>
                        <th className="px-3.5 sm:px-4 py-2 sm:py-2.5 text-[11px] font-semibold">Estado</th>
                        <th className="px-3.5 sm:px-4 py-2 sm:py-2.5 text-[11px] font-semibold">Fecha</th>
                        <th className="px-3.5 sm:px-4 py-2 sm:py-2.5 text-[11px] font-semibold text-right">Acción</th>
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-[#e0e3e5] text-[#191c1e]">
                      {loadingAuditorias ? (
                        <tr>
                          <td colSpan={5} className="px-4 py-6 text-center text-xs text-[#727783] animate-pulse">
                            Cargando auditorías de Consultor SGI...
                          </td>
                        </tr>
                      ) : auditoriasRecientes.length === 0 ? (
                        <tr>
                          <td colSpan={5} className="px-4 py-6 text-center text-xs text-[#727783]">
                            No hay auditorías registradas recientemente.
                          </td>
                        </tr>
                      ) : (
                        auditoriasRecientes.map((aud) => (
                          <tr key={aud.id} className="hover:bg-[#f8fafc] transition-colors">
                            <td className="px-3.5 sm:px-4 py-2 sm:py-2.5">
                              <div className="font-semibold text-xs text-[#191c1e] truncate max-w-[160px] sm:max-w-[200px] xl:max-w-[280px]" title={aud.cliente}>
                                {aud.cliente}
                              </div>
                              <div className="text-[10px] sm:text-[11px] text-[#727783] truncate max-w-[160px] sm:max-w-[200px]">Auditor: {aud.auditor}</div>
                            </td>
                            <td className="px-3.5 sm:px-4 py-2 sm:py-2.5">
                              <div className="font-medium text-xs text-[#424752] truncate max-w-[120px] sm:max-w-[180px]">{aud.norma}</div>
                              <div className="text-[10px] sm:text-[11px] text-[#727783] font-mono">{aud.codigo}</div>
                            </td>
                            <td className="px-3.5 sm:px-4 py-2 sm:py-2.5 whitespace-nowrap">
                              <span
                                className={`inline-flex items-center px-2 py-0.5 rounded-full text-[10px] sm:text-[11px] font-semibold ${
                                  aud.estado === 'Cerrada'
                                    ? 'bg-emerald-100 text-emerald-800'
                                    : aud.estado === 'Pendiente Firma'
                                    ? 'bg-amber-100 text-amber-800'
                                    : 'bg-blue-100 text-blue-800'
                                }`}
                              >
                                {aud.estado}
                              </span>
                            </td>
                            <td className="px-3.5 sm:px-4 py-2 sm:py-2.5 font-mono text-[11px] sm:text-xs text-[#545f73] whitespace-nowrap">
                              {aud.fechaInicio || 'Por programar'}
                            </td>
                            <td className="px-3.5 sm:px-4 py-2 sm:py-2.5 text-right whitespace-nowrap">
                              <Link to="/consultor" className="text-[#055bb2] font-semibold text-[11px] sm:text-xs hover:underline">
                                Ver Auditoría
                              </Link>
                            </td>
                          </tr>
                        ))
                      )}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>

            {/* Agenda & Reminders Widget (4 Cols in XL, 5 Cols in LG) */}
            <div className="lg:col-span-5 xl:col-span-4 bg-white rounded-2xl border border-[#c2c6d4]/40 elevation-1 p-4 sm:p-5 space-y-3 sm:space-y-4 flex flex-col justify-between">
              <div>
                <div className="flex flex-col sm:flex-row sm:items-center justify-between border-b border-[#e0e3e5] pb-2.5 gap-2">
                  <div className="flex items-center gap-2 flex-wrap">
                    <h3 className="font-bold text-sm sm:text-base font-headline text-[#191c1e] flex items-center gap-2">
                      <Calendar className="w-4 h-4 text-[#055bb2]" />
                      Próximas Asesorías
                    </h3>
                    {isUserAdmin && (
                      <div className="inline-flex items-center bg-[#f2f4f6] p-0.5 rounded-lg text-[10px] font-bold ml-1">
                        <button
                          onClick={() => setFiltroAgenda('todas')}
                          className={`px-2 py-0.5 rounded-md transition-all cursor-pointer ${
                            filtroAgenda === 'todas'
                              ? 'bg-[#055bb2] text-white shadow-xs'
                              : 'text-[#545f73] hover:text-[#191c1e]'
                          }`}
                          title="Ver asesorías de todo el equipo"
                        >
                          Todas
                        </button>
                        <button
                          onClick={() => setFiltroAgenda('mias')}
                          className={`px-2 py-0.5 rounded-md transition-all cursor-pointer ${
                            filtroAgenda === 'mias'
                              ? 'bg-[#055bb2] text-white shadow-xs'
                              : 'text-[#545f73] hover:text-[#191c1e]'
                          }`}
                          title="Ver únicamente mis asesorías programadas"
                        >
                          Mis Citas
                        </button>
                      </div>
                    )}
                  </div>
                  <Link
                    to="/agenda"
                    className="text-xs text-[#055bb2] font-bold hover:underline shrink-0"
                    title="Abrir Módulo Agenda completo"
                  >
                    Ver Todo
                  </Link>
                </div>

                <div className="space-y-2 pt-2.5">
                  {loadingAgenda ? (
                    <div className="space-y-2 py-1">
                      <div className="h-12 bg-slate-100/80 rounded-xl animate-pulse" />
                      <div className="h-12 bg-slate-100/80 rounded-xl animate-pulse" />
                    </div>
                  ) : agendaEventos.length === 0 ? (
                    <div className="p-5 text-center text-[#727783] text-xs">
                      No hay asesorías próximas programadas en este momento.
                    </div>
                  ) : (
                    agendaEventos.map((evento) => {
                      const nombrePrincipal = evento.cliente && evento.cliente !== 'Sin Cliente Asignado'
                        ? evento.cliente
                        : (evento.titulo || 'Asesoría SGI');
                      const subtitulo = evento.titulo && evento.titulo.trim().toLowerCase() !== nombrePrincipal.trim().toLowerCase()
                        ? evento.titulo
                        : null;

                      return (
                        <Link
                          to="/agenda"
                          key={evento.id}
                          className="block px-3 py-2 sm:py-2.5 bg-[#f8fafc] hover:bg-sky-50/70 hover:border-[#055bb2]/40 transition-all rounded-xl border border-[#e0e3e5] space-y-1 group cursor-pointer text-inherit no-underline"
                          title="Abrir cita en Módulo Agenda"
                        >
                          <div className="flex justify-between items-center text-xs font-bold text-[#191c1e] gap-2">
                            <span className="truncate group-hover:text-[#055bb2] transition-colors" title={nombrePrincipal}>
                              {nombrePrincipal}
                            </span>
                            <span className="text-[10px] sm:text-[11px] text-[#055bb2] font-semibold shrink-0 bg-sky-100/80 px-1.5 py-0.5 rounded">
                              {evento.hora}
                            </span>
                          </div>

                          {subtitulo && (
                            <p className="text-[10px] sm:text-[11px] text-[#545f73] truncate" title={subtitulo}>
                              {subtitulo}
                            </p>
                          )}

                          <div className="flex items-center justify-between gap-1 text-[10px] sm:text-[11px] text-[#727783]">
                            <div className="flex items-center gap-1.5 truncate">
                              <Clock className="w-3 h-3 text-[#055bb2] shrink-0" />
                              <span>{evento.fecha} • {evento.duracion}</span>
                            </div>
                            {evento.asesorNombre && (
                              <span
                                className="inline-flex items-center gap-1 text-[10px] sm:text-[11px] bg-slate-100 px-1.5 py-0.5 rounded text-slate-700 font-medium truncate max-w-[120px]"
                                title={`Asesor: ${evento.asesorNombre}`}
                              >
                                <User className="w-3 h-3 text-slate-500 shrink-0" />
                                <span className="truncate">{evento.asesorNombre}</span>
                              </span>
                            )}
                          </div>
                        </Link>
                      );
                    })
                  )}
                </div>
              </div>
            </div>
          </div>
        </main>
      </div>
    </div>
  );
};
