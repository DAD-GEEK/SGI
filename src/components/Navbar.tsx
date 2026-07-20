import React, { useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { Menu, X, Shield, MessageSquare } from 'lucide-react';

export const Navbar: React.FC = () => {
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
  const location = useLocation();

  const isActive = (path: string) => location.pathname === path;

  return (
    <header className="bg-white/95 dark:bg-[#111827]/95 backdrop-blur-md sticky top-0 z-50 border-b border-[#c2c6d4]/40 shadow-xs transition-all duration-300">
      <div className="max-w-[1280px] mx-auto px-4 md:px-10 py-3.5 flex justify-between items-center">
        {/* Brand Logo & Name */}
        <Link to="/" className="flex items-center gap-3 group">
          <div className="w-10 h-10 rounded-full bg-[#055bb2]/10 border border-[#055bb2]/30 flex items-center justify-center text-[#055bb2] group-hover:scale-105 transition-transform">
            <Shield className="w-5 h-5" />
          </div>
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
    </header>
  );
};
