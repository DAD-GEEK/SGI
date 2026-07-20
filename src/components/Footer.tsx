import React from 'react';
import { Phone, Mail, MapPin } from 'lucide-react';
import { Link } from 'react-router-dom';

const WhatsAppIcon: React.FC<{ className?: string }> = ({ className = "w-4 h-4" }) => (
  <svg className={className} viewBox="0 0 24 24" fill="currentColor">
    <path d="M.057 24l1.687-6.163c-1.041-1.804-1.588-3.849-1.587-5.946.003-6.556 5.338-11.891 11.893-11.891 3.181.001 6.167 1.24 8.413 3.488 2.245 2.248 3.481 5.236 3.48 8.414-.003 6.557-5.338 11.892-11.893 11.892-1.99-.001-3.951-.5-5.688-1.448l-6.305 1.654zm6.597-3.807c1.676.995 3.276 1.591 5.392 1.592 5.448 0 9.886-4.434 9.889-9.885.002-5.462-4.415-9.89-9.881-9.892-5.452 0-9.887 4.434-9.889 9.884-.001 2.225.651 3.891 1.746 5.634l-.999 3.648 3.742-.981zm11.387-5.464c-.074-.124-.272-.198-.57-.347-.297-.149-1.758-.868-2.031-.967-.272-.099-.47-.149-.669.149-.198.297-.768.967-.941 1.165-.173.198-.347.223-.644.074-.297-.149-1.255-.462-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.297-.347.446-.521.151-.172.2-.296.3-.495.099-.198.05-.372-.025-.521-.075-.148-.669-1.611-.916-2.206-.242-.579-.487-.501-.669-.51l-.57-.01c-.198 0-.52.074-.792.372s-1.04 1.016-1.04 2.479 1.065 2.876 1.213 3.074c.149.198 2.095 3.2 5.076 4.487.709.306 1.263.489 1.694.626.712.226 1.36.194 1.872.118.571-.085 1.758-.719 2.006-1.413.248-.695.248-1.29.173-1.414z"/>
  </svg>
);

export const Footer: React.FC = () => {
  return (
    <footer className="bg-[#3c475a] text-[#fdfcff] mt-auto w-full">
      <div className="max-w-[1280px] mx-auto px-4 md:px-10 py-16 grid grid-cols-1 md:grid-cols-4 gap-10">
        {/* Col 1: Brand */}
        <div className="space-y-4 col-span-1">
          <div className="flex items-center gap-3">
            <img
              src="/logo.png"
              alt="Gestión Integral SGI Logo Footer"
              className="w-10 h-10 object-contain"
            />
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
            <WhatsAppIcon className="w-4 h-4 fill-current" />
            Contactar por WhatsApp
          </a>
        </div>
      </div>

      {/* Bottom Legal bar con enlace a Waloyo Group */}
      <div className="border-t border-white/10 py-6 pb-24 md:pb-6 text-center text-xs text-[#d8e3fb]/70 px-4 flex flex-col md:flex-row justify-between items-center max-w-[1280px] mx-auto gap-2">
        <p>© 2026 Gestión Integral SGI S.A.S. Todos los derechos reservados.</p>
        <p className="text-[11px] opacity-80">
          Desarrollado por{' '}
          <a
            href="https://waloyogroup.com"
            target="_blank"
            rel="noopener noreferrer"
            className="font-semibold text-white hover:text-[#a9c7ff] underline underline-offset-2 transition-colors"
          >
            Waloyo Group
          </a>{' '}
          — <i>Tecnología resiliente. Operación continua.</i>
        </p>
      </div>
    </footer>
  );
};
