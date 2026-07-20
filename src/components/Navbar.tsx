import React, { useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { Menu, X, MessageSquare } from 'lucide-react';

export const Navbar: React.FC = () => {
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
  const location = useLocation();

  const isActive = (path: string) => location.pathname === path;

  return (
    <header className="bg-white/95 dark:bg-[#111827]/95 backdrop-blur-md sticky top-0 z-50 border-b border-[#c2c6d4]/40 shadow-xs transition-all duration-300">
      <div className="max-w-[1280px] mx-auto px-4 md:px-10 py-3.5 flex justify-between items-center">
        {/* Brand Logo & Name */}
        <Link to="/" className="flex items-center gap-3 group">
          <img
            src="/logo-limpio.png"
            alt="Gestión Integral SGI Logo"
            className="w-10 h-10 md:w-11 md:h-11 object-contain group-hover:scale-105 transition-transform"
          />
          <div className="flex flex-col">
            <span className="text-lg md:text-xl font-bold font-headline text-[#055bb2] leading-tight">
              Gestión Integral SGI
            </span>
            <span className="text-[11px] font-semibold text-[#545f73] uppercase tracking-wider">
              SU ASESOR… SU ALIADO
            </span>
          </div>
        </Link>

        {/* Desktop Navigation */}
        <nav className="hidden md:flex items-center gap-2">
          <Link
            to="/"
            className={`px-4 py-2 rounded-md font-medium text-sm transition-colors ${
              isActive('/')
                ? 'text-[#055bb2] font-bold border-b-2 border-[#055bb2] bg-[#f2f4f6]'
                : 'text-[#424752] hover:text-[#055bb2] hover:bg-[#f2f4f6]'
            }`}
          >
            Inicio
          </Link>
          <Link
            to="/servicios"
            className={`px-4 py-2 rounded-md font-medium text-sm transition-colors ${
              isActive('/servicios')
                ? 'text-[#055bb2] font-bold border-b-2 border-[#055bb2] bg-[#f2f4f6]'
                : 'text-[#424752] hover:text-[#055bb2] hover:bg-[#f2f4f6]'
            }`}
          >
            Servicios
          </Link>
          <Link
            to="/pesv"
            className={`px-4 py-2 rounded-md font-medium text-sm transition-colors ${
              isActive('/pesv')
                ? 'text-[#055bb2] font-bold border-b-2 border-[#055bb2] bg-[#f2f4f6]'
                : 'text-[#424752] hover:text-[#055bb2] hover:bg-[#f2f4f6]'
            }`}
          >
            PESV
          </Link>
          <a
            href="#contacto"
            className="px-4 py-2 rounded-md font-medium text-sm text-[#424752] hover:text-[#055bb2] hover:bg-[#f2f4f6] transition-colors"
          >
            Contacto
          </a>
        </nav>

        {/* Desktop CTAs */}
        <div className="hidden md:flex items-center gap-3">
          <a
            href="https://wa.me/573112490072?text=Hola%2C%20quisiera%20cotizar%20servicios%20SGI"
            target="_blank"
            rel="noopener noreferrer"
            className="flex items-center gap-2 bg-[#16A34A] text-white px-4 py-2 rounded-lg font-semibold text-xs hover:bg-[#15803D] transition-colors shadow-sm"
          >
            <MessageSquare className="w-4 h-4" />
            WhatsApp
          </a>
          <button
            onClick={() => alert('Portal de Clientes SGI: Módulo en desarrollo.')}
            className="bg-[#055bb2] text-white px-5 py-2.5 rounded-lg font-semibold text-xs hover:bg-[#3374cd] transition-colors shadow-xs"
          >
            Acceso Software/CRM
          </button>
        </div>

        {/* Mobile Toggle Button */}
        <button
          onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
          className="md:hidden p-2 text-[#055bb2] rounded-lg hover:bg-[#f2f4f6] transition-colors min-w-[44px] min-h-[44px] flex items-center justify-center"
          aria-label="Abrir menú"
        >
          {mobileMenuOpen ? <X className="w-6 h-6" /> : <Menu className="w-6 h-6" />}
        </button>
      </div>

      {/* Mobile Drawer Menu */}
      {mobileMenuOpen && (
        <div className="md:hidden bg-white border-b border-[#c2c6d4] px-4 py-6 space-y-4 shadow-lg animate-in slide-in-from-top duration-200">
          <nav className="flex flex-col gap-2">
            <Link
              to="/"
              onClick={() => setMobileMenuOpen(false)}
              className={`px-4 py-3 rounded-lg font-semibold text-base transition-colors ${
                isActive('/') ? 'bg-[#055bb2] text-white' : 'text-[#191c1e] hover:bg-[#f2f4f6]'
              }`}
            >
              Inicio
            </Link>
            <Link
              to="/servicios"
              onClick={() => setMobileMenuOpen(false)}
              className={`px-4 py-3 rounded-lg font-semibold text-base transition-colors ${
                isActive('/servicios') ? 'bg-[#055bb2] text-white' : 'text-[#191c1e] hover:bg-[#f2f4f6]'
              }`}
            >
              Servicios & Especialidades
            </Link>
            <Link
              to="/pesv"
              onClick={() => setMobileMenuOpen(false)}
              className={`px-4 py-3 rounded-lg font-semibold text-base transition-colors ${
                isActive('/pesv') ? 'bg-[#055bb2] text-white' : 'text-[#191c1e] hover:bg-[#f2f4f6]'
              }`}
            >
              Plan Estratégico de Seguridad Vial (PESV)
            </Link>
            <a
              href="#contacto"
              onClick={() => setMobileMenuOpen(false)}
              className="px-4 py-3 rounded-lg font-semibold text-base text-[#191c1e] hover:bg-[#f2f4f6] transition-colors"
            >
              Contacto Directo
            </a>
          </nav>
          <div className="pt-4 border-t border-[#e0e3e5] flex flex-col gap-3">
            <a
              href="https://wa.me/573112490072?text=Hola%2C%20necesito%20asesoria%20en%20SG-SST"
              target="_blank"
              rel="noopener noreferrer"
              className="w-full flex items-center justify-center gap-2 bg-[#16A34A] text-white py-3 rounded-lg font-semibold text-sm shadow-sm"
            >
              <MessageSquare className="w-5 h-5" />
              WhatsApp Consultoría
            </a>
            <button
              onClick={() => {
                setMobileMenuOpen(false);
                alert('Portal de Clientes SGI: Módulo en desarrollo.');
              }}
              className="w-full bg-[#055bb2] text-white py-3 rounded-lg font-semibold text-sm shadow-xs"
            >
              Acceso Software / CRM
            </button>
          </div>
        </div>
      )}

      {/* Bottom Navigation Bar (Exclusiva para Móviles < 768px con soporte para Safe Area / OS Navigation Bar) */}
      <nav className="md:hidden fixed bottom-0 left-0 right-0 bg-white/95 backdrop-blur-md border-t border-[#c2c6d4]/80 shadow-[0_-4px_16px_rgba(30,41,59,0.08)] z-50 pt-1.5 pb-safe px-4 flex justify-around items-center">
        <Link
          to="/"
          className={`flex flex-col items-center justify-center min-w-[56px] min-h-[44px] py-1 transition-colors ${
            isActive('/') ? 'text-[#055bb2] font-bold' : 'text-[#545f73] hover:text-[#055bb2]'
          }`}
        >
          <span className="material-symbols-outlined text-[22px]" style={{ fontVariationSettings: isActive('/') ? "'FILL' 1" : "'FILL' 0" }}>
            home
          </span>
          <span className="text-[10px] font-medium leading-none mt-1">Inicio</span>
        </Link>

        <Link
          to="/servicios"
          className={`flex flex-col items-center justify-center min-w-[56px] min-h-[44px] py-1 transition-colors ${
            isActive('/servicios') ? 'text-[#055bb2] font-bold' : 'text-[#545f73] hover:text-[#055bb2]'
          }`}
        >
          <span className="material-symbols-outlined text-[22px]" style={{ fontVariationSettings: isActive('/servicios') ? "'FILL' 1" : "'FILL' 0" }}>
            grid_view
          </span>
          <span className="text-[10px] font-medium leading-none mt-1">Servicios</span>
        </Link>

        <Link
          to="/pesv"
          className={`flex flex-col items-center justify-center min-w-[56px] min-h-[44px] py-1 transition-colors ${
            isActive('/pesv') ? 'text-[#055bb2] font-bold' : 'text-[#545f73] hover:text-[#055bb2]'
          }`}
        >
          <span className="material-symbols-outlined text-[22px]" style={{ fontVariationSettings: isActive('/pesv') ? "'FILL' 1" : "'FILL' 0" }}>
            directions_car
          </span>
          <span className="text-[10px] font-medium leading-none mt-1">PESV</span>
        </Link>

        <a
          href="https://wa.me/573112490072?text=Hola%2C%20quisiera%20cotizar"
          target="_blank"
          rel="noopener noreferrer"
          className="flex flex-col items-center justify-center min-w-[56px] min-h-[44px] py-1 text-[#16A34A] hover:text-[#15803D] transition-colors"
        >
          <svg className="w-5 h-5 fill-current" viewBox="0 0 24 24">
            <path d="M.057 24l1.687-6.163c-1.041-1.804-1.588-3.849-1.587-5.946.003-6.556 5.338-11.891 11.893-11.891 3.181.001 6.167 1.24 8.413 3.488 2.245 2.248 3.481 5.236 3.48 8.414-.003 6.557-5.338 11.892-11.893 11.892-1.99-.001-3.951-.5-5.688-1.448l-6.305 1.654zm6.597-3.807c1.676.995 3.276 1.591 5.392 1.592 5.448 0 9.886-4.434 9.889-9.885.002-5.462-4.415-9.89-9.881-9.892-5.452 0-9.887 4.434-9.889 9.884-.001 2.225.651 3.891 1.746 5.634l-.999 3.648 3.742-.981zm11.387-5.464c-.074-.124-.272-.198-.57-.347-.297-.149-1.758-.868-2.031-.967-.272-.099-.47-.149-.669.149-.198.297-.768.967-.941 1.165-.173.198-.347.223-.644.074-.297-.149-1.255-.462-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.297-.347.446-.521.151-.172.2-.296.3-.495.099-.198.05-.372-.025-.521-.075-.148-.669-1.611-.916-2.206-.242-.579-.487-.501-.669-.51l-.57-.01c-.198 0-.52.074-.792.372s-1.04 1.016-1.04 2.479 1.065 2.876 1.213 3.074c.149.198 2.095 3.2 5.076 4.487.709.306 1.263.489 1.694.626.712.226 1.36.194 1.872.118.571-.085 1.758-.719 2.006-1.413.248-.695.248-1.29.173-1.414z"/>
          </svg>
          <span className="text-[10px] font-medium leading-none mt-1">WhatsApp</span>
        </a>
      </nav>
    </header>
  );
};
