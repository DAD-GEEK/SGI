import React from 'react';
import { Shield, Phone, Mail, MapPin, MessageSquare } from 'lucide-react';
import { Link } from 'react-router-dom';

export const Footer: React.FC = () => {
  return (
    <footer className="bg-[#3c475a] text-[#fdfcff] mt-auto w-full">
      <div className="max-w-[1280px] mx-auto px-4 md:px-10 py-16 grid grid-cols-1 md:grid-cols-4 gap-10">
        {/* Col 1: Brand */}
        <div className="space-y-4 col-span-1">
          <div className="flex items-center gap-3">
            <div className="w-10 h-10 rounded-full bg-white/10 border border-white/20 flex items-center justify-center text-white">
              <Shield className="w-5 h-5" />
            </div>
            <span className="font-headline font-bold text-lg text-white">
              Gestión Integral SGI S.A.S
            </span>
          </div>
          <p className="text-xs text-[#d8e3fb] leading-relaxed opacity-90">
            Firma consultora especializada en la implementación, administración y auditoría de Sistemas de Gestión Integrados (SG-SST, ISO 9001/14001/45001 y PESV) en Colombia.
          </p>
          <div className="space-y-2 text-xs text-[#d8e3fb]">
            <div className="flex items-center gap-2">
              <Phone className="w-4 h-4 text-[#a9c7ff]" />
              <span>+57 311 249 00 72 / +57 300 805 74 89</span>
            </div>
            <div className="flex items-center gap-2">
              <Mail className="w-4 h-4 text-[#a9c7ff]" />
              <span>asesorias@gestionintegralsgi.com.co</span>
            </div>
            <div className="flex items-center gap-2">
              <MapPin className="w-4 h-4 text-[#a9c7ff]" />
              <span>Medellín & Cobertura Nacional</span>
            </div>
          </div>
        </div>

        {/* Col 2: Enlaces Rápidos */}
        <div className="space-y-4">
          <h4 className="font-headline font-bold text-base text-white border-b border-white/10 pb-2">
            Navegación
          </h4>
          <ul className="space-y-2.5 text-xs text-[#d8e3fb]">
            <li>
              <Link to="/" className="hover:text-white transition-colors">
                Inicio / Portal Comercial
              </Link>
            </li>
            <li>
              <Link to="/servicios" className="hover:text-white transition-colors">
                Nuestras Especialidades SGI
              </Link>
            </li>
            <li>
              <Link to="/pesv" className="hover:text-white transition-colors">
                Plan Estratégico de Seguridad Vial (PESV)
              </Link>
            </li>
            <li>
              <a href="#contacto" className="hover:text-white transition-colors">
                Contacto & Asesorías
              </a>
            </li>
          </ul>
        </div>

        {/* Col 3: Servicios */}
        <div className="space-y-4">
          <h4 className="font-headline font-bold text-base text-white border-b border-white/10 pb-2">
            Líneas de Servicio
          </h4>
          <ul className="space-y-2.5 text-xs text-[#d8e3fb]">
            <li>SG-SST (Decreto 1072 / Res. 0312)</li>
            <li>ISO 9001:2015 (Calidad)</li>
            <li>ISO 14001:2015 (Medio Ambiente)</li>
            <li>ISO 45001:2018 (Salud Ocupacional)</li>
            <li>Auditorías de 1ª, 2ª y 3ª Parte</li>
          </ul>
        </div>

        {/* Col 4: Atención Rápida */}
        <div className="space-y-4">
          <h4 className="font-headline font-bold text-base text-white border-b border-white/10 pb-2">
            Atención Inmediata
          </h4>
          <p className="text-xs text-[#d8e3fb] opacity-90">
            Escríbenos directamente para agendar una valoración o cotizar el diseño y mantenimiento de tu Sistema de Gestión.
          </p>
          <a
            href="https://wa.me/573112490072?text=Hola%20SGI%2C%20necesito%20informacion%20de%20consultoria"
            target="_blank"
            rel="noopener noreferrer"
            className="inline-flex items-center gap-2 bg-[#16A34A] text-white px-4 py-2.5 rounded-lg text-xs font-semibold hover:bg-[#15803D] transition-colors shadow-sm"
          >
            <MessageSquare className="w-4 h-4" />
            Contactar por WhatsApp
          </a>
        </div>
      </div>

      {/* Bottom Legal bar */}
      <div className="border-t border-white/10 py-6 pb-24 md:pb-6 text-center text-xs text-[#d8e3fb]/70 px-4 flex flex-col md:flex-row justify-between items-center max-w-[1280px] mx-auto gap-2">
        <p>© 2026 Gestión Integral SGI S.A.S. Todos los derechos reservados.</p>
        <p className="text-[11px] opacity-80">
          Desarrollado por <span className="font-semibold text-white">Waloyo Group</span> — <i>Tecnología resiliente. Operación continua.</i>
        </p>
      </div>
    </footer>
  );
};
