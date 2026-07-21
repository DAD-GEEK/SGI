import React, { useState } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import {
  LayoutDashboard,
  Users,
  Building2,
  ClipboardCheck,
  FileSpreadsheet,
  Settings,
  LogOut,
  Calendar,
  ShieldCheck,
  ChevronLeft,
  Pin
} from 'lucide-react';

interface CrmSidebarProps {
  activeTab?: string;
}

export const CrmSidebar: React.FC<CrmSidebarProps> = ({ activeTab }) => {
  const [isPinned, setIsPinned] = useState<boolean>(true);
  const [isHovered, setIsHovered] = useState<boolean>(false);
  const location = useLocation();
  const navigate = useNavigate();

  const isActive = (path: string, tabName?: string) => {
    if (activeTab && tabName) return activeTab === tabName;
    return location.pathname === path;
  };

  // Determine if sidebar is currently expanded (either pinned or temporarily hovered)
  const isExpanded = isPinned || isHovered;

  const togglePin = () => {
    setIsPinned(!isPinned);
  };

  return (
    <aside
      onMouseEnter={() => setIsHovered(true)}
      onMouseLeave={() => setIsHovered(false)}
      className={`bg-[#0b1c30] text-white flex-shrink-0 flex flex-col justify-between border-r border-[#3c475a] min-h-screen transition-all duration-300 ease-in-out z-40 ${
        isExpanded ? 'w-full md:w-64' : 'w-20'
      }`}
    >
      <div className="p-4 space-y-6">
        {/* Header: Logo, Brand & Pin / Expand Button */}
        <div className="flex items-center justify-between pb-2 border-b border-white/10">
          <Link to="/dashboard" className="flex items-center gap-3 group overflow-hidden">
            <img
              src="/logo-limpio.png"
              alt="SGI Logo"
              className="w-9 h-9 object-contain shrink-0 group-hover:scale-105 transition-transform"
            />
            {isExpanded && (
              <div className="flex flex-col whitespace-nowrap transition-opacity duration-200">
                <span className="font-bold text-base font-headline text-white leading-tight">
                  SGI CRM B2B
                </span>
                <span className="text-[10px] text-[#a9c7ff] uppercase tracking-wider font-semibold">
                  Gestión & Auditoría
                </span>
              </div>
            )}
          </Link>

          {/* Toggle / Pin Button */}
          <button
            onClick={togglePin}
            title={isPinned ? 'Fijar colapsado' : 'Fijar expandido'}
            className="p-1.5 rounded-lg text-slate-300 hover:text-white hover:bg-white/10 transition-colors cursor-pointer"
          >
            {isPinned ? <ChevronLeft className="w-5 h-5" /> : <Pin className="w-4 h-4 text-amber-400" />}
          </button>
        </div>

        {/* Navigation Links */}
        <nav className="space-y-1.5">
          {isExpanded && (
            <span className="block px-3 text-[10px] font-bold text-[#a9c7ff]/70 uppercase tracking-widest mb-2 transition-opacity">
              Navegación Principal
            </span>
          )}

          <Link
            to="/dashboard"
            title="Dashboard Principal"
            className={`flex items-center ${!isExpanded ? 'justify-center' : 'gap-3'} px-3.5 py-2.5 rounded-xl text-xs font-bold transition-all ${
              isActive('/dashboard')
                ? 'bg-[#055bb2] text-white shadow-sm'
                : 'text-[#d8e3fb]/80 hover:bg-white/10 hover:text-white'
            }`}
          >
            <LayoutDashboard className="w-4 h-4 shrink-0" />
            {isExpanded && <span>Dashboard Principal</span>}
          </Link>

          {/* Consultor SGI */}
          <Link
            to="/consultor"
            title="Consultor SGI"
            className={`flex items-center ${!isExpanded ? 'justify-center' : 'justify-between'} px-3.5 py-2.5 rounded-xl text-xs font-bold transition-all ${
              isActive('/consultor')
                ? 'bg-[#055bb2] text-white shadow-sm'
                : 'text-[#d8e3fb]/80 hover:bg-white/10 hover:text-white'
            }`}
          >
            <div className="flex items-center gap-3">
              <ShieldCheck className="w-4 h-4 text-[#10B981] shrink-0" />
              {isExpanded && <span>Consultor SGI</span>}
            </div>
            {isExpanded && (
              <span className="text-[9px] font-extrabold uppercase px-1.5 py-0.5 rounded bg-emerald-500/20 text-emerald-300">
                Web
              </span>
            )}
          </Link>

          {/* Agenda SGI */}
          <Link
            to="/agenda"
            title="Agenda SGI"
            className={`flex items-center ${!isExpanded ? 'justify-center' : 'justify-between'} px-3.5 py-2.5 rounded-xl text-xs font-bold transition-all ${
              isActive('/agenda')
                ? 'bg-[#055bb2] text-white shadow-sm'
                : 'text-[#d8e3fb]/80 hover:bg-white/10 hover:text-white'
            }`}
          >
            <div className="flex items-center gap-3">
              <Calendar className="w-4 h-4 text-[#38bdf8] shrink-0" />
              {isExpanded && <span>Agenda SGI</span>}
            </div>
            {isExpanded && (
              <span className="text-[9px] font-extrabold uppercase px-1.5 py-0.5 rounded bg-sky-500/20 text-sky-300">
                Web
              </span>
            )}
          </Link>

          {isExpanded && (
            <span className="block px-3 text-[10px] font-bold text-[#a9c7ff]/70 uppercase tracking-widest pt-4 mb-2 transition-opacity">
              Módulos B2B (Fase 1)
            </span>
          )}

          <Link
            to="/clientes"
            title="Clientes"
            className={`flex items-center ${!isExpanded ? 'justify-center' : 'gap-3'} px-3.5 py-2.5 rounded-xl text-xs font-semibold transition-all ${
              isActive('/clientes', 'clientes')
                ? 'bg-[#055bb2] text-white shadow-sm'
                : 'text-[#d8e3fb]/80 hover:bg-white/10 hover:text-white'
            }`}
          >
            <Building2 className="w-4 h-4 text-amber-400 shrink-0" />
            {isExpanded && <span>Clientes</span>}
          </Link>

          <Link
            to="/usuarios"
            title="Asesores & Equipo"
            className={`flex items-center ${!isExpanded ? 'justify-center' : 'gap-3'} px-3.5 py-2.5 rounded-xl text-xs font-semibold transition-all ${
              isActive('/usuarios', 'usuarios')
                ? 'bg-[#055bb2] text-white shadow-sm'
                : 'text-[#d8e3fb]/80 hover:bg-white/10 hover:text-white'
            }`}
          >
            <Users className="w-4 h-4 text-indigo-300 shrink-0" />
            {isExpanded && <span>Asesores & Equipo</span>}
          </Link>

          <a
            href="#auditorias"
            title="Auditorías & PHVA"
            onClick={(e) => e.preventDefault()}
            className={`flex items-center ${!isExpanded ? 'justify-center' : 'gap-3'} px-3.5 py-2.5 rounded-xl text-xs font-semibold text-[#d8e3fb]/80 hover:bg-white/10 hover:text-white transition-colors opacity-60`}
          >
            <ClipboardCheck className="w-4 h-4 shrink-0" />
            {isExpanded && <span>Auditorías & PHVA</span>}
          </a>

          <a
            href="#compromisos"
            title="Actas de Compromisos"
            onClick={(e) => e.preventDefault()}
            className={`flex items-center ${!isExpanded ? 'justify-center' : 'gap-3'} px-3.5 py-2.5 rounded-xl text-xs font-semibold text-[#d8e3fb]/80 hover:bg-white/10 hover:text-white transition-colors opacity-60`}
          >
            <FileSpreadsheet className="w-4 h-4 shrink-0" />
            {isExpanded && <span>Actas de Compromisos</span>}
          </a>

          <Link
            to="/perfil"
            title="Perfil & Preferencias"
            className={`flex items-center ${!isExpanded ? 'justify-center' : 'gap-3'} px-3.5 py-2.5 rounded-xl text-xs font-semibold transition-all ${
              isActive('/perfil')
                ? 'bg-[#055bb2] text-white shadow-sm'
                : 'text-[#d8e3fb]/80 hover:bg-white/10 hover:text-white'
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
            AD
          </div>
          {isExpanded && (
            <div className="flex flex-col overflow-hidden transition-opacity">
              <span className="text-xs font-bold text-white truncate">Dra. Amanda Durango</span>
              <span className="text-[10px] text-[#a9c7ff] truncate">Consultora Senior SGI</span>
            </div>
          )}
        </div>
        <button
          onClick={() => navigate('/login')}
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
