import React from 'react';
import { Car, MessageSquare, CheckCircle } from 'lucide-react';
import { WhatsAppIcon } from '../components/WhatsAppIcon';

export const PESV: React.FC = () => {
  return (
    <div className="flex flex-col min-h-screen bg-[#f7f9fb]">
      {/* Hero Section */}
      <section className="py-20 lg:py-28 bg-[#001b3e] text-white relative overflow-hidden">
        <div className="absolute inset-0 z-0 opacity-20">
          <img
            src="https://images.unsplash.com/photo-1549399542-7e3f8b79c341?auto=format&fit=crop&w=1920&q=80"
            alt="Flotas y conductores corporativos en carretera"
            className="w-full h-full object-cover"
          />
        </div>
        <div className="max-w-[1280px] mx-auto px-4 md:px-10 relative z-10 space-y-6">
          <div className="inline-flex items-center gap-2 bg-[#055bb2]/50 border border-[#a9c7ff]/30 text-[#d6e3ff] px-4 py-1.5 rounded-full text-xs font-semibold">
            <Car className="w-4 h-4 text-[#a9c7ff]" />
            <span>Ley 2251/2022 & Res. 40595 de 2022</span>
          </div>

          <h1 className="text-3xl md:text-5xl font-bold font-headline leading-tight max-w-3xl">
            Plan Estratégico de Seguridad Vial (PESV)
          </h1>

          <p className="text-base md:text-lg text-[#d8e3fb] max-w-2xl leading-relaxed">
            Diseño, estructuración, seguimiento y articulación del PESV con el SG-SST de su organización, garantizando la protección de sus conductores y el cumplimiento de la normatividad de transporte en Colombia.
          </p>

          <div className="pt-4">
            <a
              href="https://wa.me/573112490072?text=Hola%2C%20necesito%20asesoria%20para%20el%20PESV%20de%20mi%20empresa"
              target="_blank"
              rel="noopener noreferrer"
              className="inline-flex items-center gap-2 bg-[#055bb2] text-white px-8 py-3.5 rounded-lg font-semibold text-sm hover:bg-[#3374cd] transition-colors shadow-lg"
            >
              <MessageSquare className="w-4 h-4" />
              Solicitar Cotización PESV
            </a>
          </div>
        </div>
      </section>

      {/* Pilares del PESV */}
      <section className="py-20">
        <div className="max-w-[1280px] mx-auto px-4 md:px-10 space-y-12">
          <div className="text-center max-w-2xl mx-auto space-y-4">
            <span className="text-[#055bb2] font-semibold text-xs uppercase tracking-widest">
              Estructura Normativa
            </span>
            <h2 className="text-3xl font-bold font-headline text-[#191c1e]">
              Fases de Implementación del PESV
            </h2>
            <p className="text-sm md:text-base text-[#424752]">
              Acompañamos a su empresa en los 24 pasos del PESV articulados bajo la Metodología PHVA.
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
            {/* Pilar 1 */}
            <div className="bg-white p-6 rounded-xl border border-[#e0e3e5] elevation-1 space-y-4">
              <div className="w-12 h-12 rounded-lg bg-[#d6e3ff] text-[#055bb2] flex items-center justify-center font-bold font-headline text-lg">
                01
              </div>
              <h3 className="text-lg font-bold font-headline text-[#191c1e]">
                Planificación
              </h3>
              <p className="text-xs text-[#424752] leading-relaxed">
                Diagnóstico de riesgos viales, caracterización de la flota y conformación del Comité de Seguridad Vial.
              </p>
            </div>

            {/* Pilar 2 */}
            <div className="bg-white p-6 rounded-xl border border-[#e0e3e5] elevation-1 space-y-4">
              <div className="w-12 h-12 rounded-lg bg-[#d5e0f8] text-[#055bb2] flex items-center justify-center font-bold font-headline text-lg">
                02
              </div>
              <h3 className="text-lg font-bold font-headline text-[#191c1e]">
                Implementación
              </h3>
              <p className="text-xs text-[#424752] leading-relaxed">
                Capacitación continua de conductores, selección de personal de conducción e inspecciones preoperacionales.
              </p>
            </div>

            {/* Pilar 3 */}
            <div className="bg-white p-6 rounded-xl border border-[#e0e3e5] elevation-1 space-y-4">
              <div className="w-12 h-12 rounded-lg bg-[#d3e4fe] text-[#055bb2] flex items-center justify-center font-bold font-headline text-lg">
                03
              </div>
              <h3 className="text-lg font-bold font-headline text-[#191c1e]">
                Seguimiento
              </h3>
              <p className="text-xs text-[#424752] leading-relaxed">
                Monitoreo de indicadores de siniestralidad, mantenimiento preventivo de vehículos y auditorías viales.
              </p>
            </div>

            {/* Pilar 4 */}
            <div className="bg-white p-6 rounded-xl border border-[#e0e3e5] elevation-1 space-y-4">
              <div className="w-12 h-12 rounded-lg bg-[#eceef0] text-[#055bb2] flex items-center justify-center font-bold font-headline text-lg">
                04
              </div>
              <h3 className="text-lg font-bold font-headline text-[#191c1e]">
                Mejora Continua
              </h3>
              <p className="text-xs text-[#424752] leading-relaxed">
                Investigación de siniestros viales y planes de acción correctiva y preventiva (CAPA).
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* Beneficios */}
      <section className="py-20 bg-[#f2f4f6]">
        <div className="max-w-[1280px] mx-auto px-4 md:px-10 grid grid-cols-1 md:grid-cols-2 gap-12 items-center">
          <div className="space-y-6">
            <h2 className="text-3xl font-bold font-headline text-[#191c1e]">
              ¿Por qué integrar el PESV con Gestión Integral SGI?
            </h2>
            <div className="space-y-4">
              <div className="flex gap-4 items-start">
                <CheckCircle className="w-6 h-6 text-[#16A34A] flex-shrink-0 mt-1" />
                <div>
                  <h4 className="font-bold text-base text-[#191c1e]">Articulación 100% con SG-SST</h4>
                  <p className="text-xs text-[#424752]">Evita duplicar procesos de auditoría y gestión documental entre la seguridad laboral y la vial.</p>
                </div>
              </div>

              <div className="flex gap-4 items-start">
                <CheckCircle className="w-6 h-6 text-[#16A34A] flex-shrink-0 mt-1" />
                <div>
                  <h4 className="font-bold text-base text-[#191c1e]">Mitigación de Sanciones de Transporte</h4>
                  <p className="text-xs text-[#424752]">Garantiza el cumplimiento ante la Superintendencia de Transporte y el Ministerio de Trabajo.</p>
                </div>
              </div>

              <div className="flex gap-4 items-start">
                <CheckCircle className="w-6 h-6 text-[#16A34A] flex-shrink-0 mt-1" />
                <div>
                  <h4 className="font-bold text-base text-[#191c1e]">Reducción de Siniestralidad</h4>
                  <p className="text-xs text-[#424752]">Protege la vida de sus colaboradores y disminuye costos operacionales por accidentes de tránsito.</p>
                </div>
              </div>
            </div>
          </div>

          <div className="bg-white p-8 rounded-2xl border border-[#e0e3e5] elevation-2 space-y-6">
            <h3 className="text-xl font-bold font-headline text-[#191c1e]">
              Cotizar Plan PESV para su Organización
            </h3>
            <p className="text-xs text-[#424752]">
              Escríbanos indicando el número de vehículos o conductores de su flota para enviar una propuesta comercial inmediata.
            </p>
            <a
              href="https://wa.me/573112490072?text=Hola%2C%20quisiera%20cotizar%20el%20PESV%20para%20nuestra%20flota"
              target="_blank"
              rel="noopener noreferrer"
              className="w-full inline-flex items-center justify-center gap-2 bg-[#16A34A] text-white py-3.5 rounded-xl font-bold text-sm hover:bg-[#15803D] transition-colors shadow-md"
            >
              <WhatsAppIcon className="w-5 h-5 fill-current" />
              Solicitar Cotización por WhatsApp
            </a>
          </div>
        </div>
      </section>
    </div>
  );
};
