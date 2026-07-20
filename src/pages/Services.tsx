import React from 'react';
import { Shield, CheckCircle, Award, Car, Layers, FileSpreadsheet, ArrowRight, MessageSquare } from 'lucide-react';
import { Link } from 'react-router-dom';

export const Services: React.FC = () => {
  return (
    <div className="flex flex-col min-h-screen bg-[#f7f9fb]">
      {/* Hero Section */}
      <section className="py-20 lg:py-28 bg-[#f2f4f6] border-b border-[#c2c6d4]/40">
        <div className="max-w-[1280px] mx-auto px-4 md:px-10 flex flex-col lg:flex-row items-center gap-12">
          <div className="flex-1 space-y-6">
            <span className="bg-[#d6e3ff] text-[#001b3e] text-xs font-semibold px-3 py-1 rounded-full uppercase tracking-wider">
              Soluciones Corporativas
            </span>
            <h1 className="text-3xl lg:text-5xl font-bold font-headline text-[#191c1e] leading-tight">
              Elevando el estándar operativo de su empresa.
            </h1>
            <p className="text-base lg:text-lg text-[#424752] leading-relaxed">
              Proveemos consultoría especializada y herramientas tecnológicas avanzadas para la implementación, mantenimiento y auditoría de sistemas de gestión, garantizando cumplimiento legal y eficiencia.
            </p>
            <div className="pt-2">
              <a
                href="https://wa.me/573112490072?text=Hola%2C%20quisiera%20agendar%20una%20consultoria%20especializada"
                target="_blank"
                rel="noopener noreferrer"
                className="inline-flex items-center gap-2 bg-[#055bb2] text-white px-7 py-3 rounded-lg font-semibold text-sm hover:bg-[#3374cd] transition-colors shadow-xs"
              >
                Agendar Consultoría
                <ArrowRight className="w-4 h-4" />
              </a>
            </div>
          </div>
          <div className="flex-1 w-full rounded-2xl overflow-hidden elevation-2 border border-[#c2c6d4]/40 aspect-[4/3] relative">
            <img
              src="https://images.unsplash.com/photo-1600880292203-757bb62b4baf?auto=format&fit=crop&w=1000&q=80"
              alt="Consultores SGI trabajando en reunión corporativa"
              className="w-full h-full object-cover"
            />
          </div>
        </div>
      </section>

      {/* Services Bento Grid */}
      <section className="py-20">
        <div className="max-w-[1280px] mx-auto px-4 md:px-10 space-y-12">
          <div className="max-w-3xl space-y-4">
            <h2 className="text-3xl font-bold font-headline text-[#191c1e]">
              Nuestras Especialidades
            </h2>
            <p className="text-base text-[#424752]">
              Soluciones integrales diseñadas para mitigar riesgos, asegurar la calidad y optimizar la gestión documental a través de nuestro acompañamiento personalizado.
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
            {/* Card 1: SG-SST */}
            <div className="bg-white rounded-2xl p-8 border border-[#e0e3e5] elevation-1 bento-hover flex flex-col justify-between space-y-6">
              <div className="space-y-4">
                <div className="w-14 h-14 rounded-full bg-[#d5e0f8] text-[#055bb2] flex items-center justify-center">
                  <Shield className="w-7 h-7" />
                </div>
                <h3 className="text-xl font-bold font-headline text-[#191c1e]">
                  SG-SST (Seguridad y Salud)
                </h3>
                <p className="text-sm text-[#424752] leading-relaxed">
                  Diseño, implementación y administración del Sistema de Gestión de Seguridad y Salud en el Trabajo, garantizando el cumplimiento normativo (Decreto 1072/2015 y Res. 0312/2019) y el bienestar corporativo.
                </p>
              </div>
              <ul className="space-y-2 text-xs text-[#545f73]">
                <li className="flex items-center gap-2">
                  <CheckCircle className="w-4 h-4 text-[#16A34A]" />
                  <span>Diseño y Plan de Trabajo Anual</span>
                </li>
                <li className="flex items-center gap-2">
                  <CheckCircle className="w-4 h-4 text-[#16A34A]" />
                  <span>Conformación de COPASST y Comité</span>
                </li>
              </ul>
            </div>

            {/* Card 2: SGC */}
            <div className="bg-white rounded-2xl p-8 border border-[#e0e3e5] elevation-1 bento-hover flex flex-col justify-between space-y-6">
              <div className="space-y-4">
                <div className="w-14 h-14 rounded-full bg-[#d6e3ff] text-[#055bb2] flex items-center justify-center">
                  <Award className="w-7 h-7" />
                </div>
                <h3 className="text-xl font-bold font-headline text-[#191c1e]">
                  SGC (Gestión de Calidad)
                </h3>
                <p className="text-sm text-[#424752] leading-relaxed">
                  Estructuración de Sistemas de Gestión de Calidad bajo estándares ISO 9001:2015, enfocados en la mejora continua de procesos y satisfacción del cliente.
                </p>
              </div>
              <ul className="space-y-2 text-xs text-[#545f73]">
                <li className="flex items-center gap-2">
                  <CheckCircle className="w-4 h-4 text-[#16A34A]" />
                  <span>Mapas de Procesos & Indicadores</span>
                </li>
                <li className="flex items-center gap-2">
                  <CheckCircle className="w-4 h-4 text-[#16A34A]" />
                  <span>Gestión de Riesgos e Insumos</span>
                </li>
              </ul>
            </div>

            {/* Card 3: PESV */}
            <div className="bg-white rounded-2xl p-8 border border-[#e0e3e5] elevation-1 bento-hover flex flex-col justify-between space-y-6">
              <div className="space-y-4">
                <div className="w-14 h-14 rounded-full bg-[#d3e4fe] text-[#055bb2] flex items-center justify-center">
                  <Car className="w-7 h-7" />
                </div>
                <h3 className="text-xl font-bold font-headline text-[#191c1e]">
                  PESV (Seguridad Vial)
                </h3>
                <p className="text-sm text-[#424752] leading-relaxed">
                  Elaboración y seguimiento del Plan Estratégico de Seguridad Vial para flotas corporativas (Res. 40595 / Ley 2251), mitigando riesgos de tránsito.
                </p>
              </div>
              <Link
                to="/pesv"
                className="inline-flex items-center gap-2 text-xs font-bold text-[#055bb2] hover:underline"
              >
                Ver módulo PESV <ArrowRight className="w-4 h-4" />
              </Link>
            </div>

            {/* Card 4: SGI Spans 2 cols */}
            <div className="bg-white rounded-2xl p-8 border border-[#e0e3e5] elevation-1 bento-hover flex flex-col lg:flex-row gap-8 lg:col-span-2 items-start lg:items-center">
              <div className="w-16 h-16 rounded-full bg-[#545f73] text-white flex-shrink-0 flex items-center justify-center">
                <Layers className="w-8 h-8" />
              </div>
              <div className="space-y-3">
                <h3 className="text-2xl font-bold font-headline text-[#191c1e]">
                  Sistemas de Gestión Integrados (SGI)
                </h3>
                <p className="text-sm text-[#424752] leading-relaxed">
                  Articulación sinérgica de normativas de calidad (ISO 9001), medio ambiente (ISO 14001) y seguridad laboral (ISO 45001 / SG-SST) en una única estructura operativa optimizada, reduciendo redundancias y costos administrativos.
                </p>
              </div>
            </div>

            {/* Card 5: Auditorias */}
            <div className="bg-white rounded-2xl p-8 border border-[#e0e3e5] elevation-1 bento-hover flex flex-col justify-between space-y-6">
              <div className="space-y-4">
                <div className="w-14 h-14 rounded-full bg-[#eceef0] text-[#055bb2] flex items-center justify-center">
                  <FileSpreadsheet className="w-7 h-7" />
                </div>
                <h3 className="text-xl font-bold font-headline text-[#191c1e]">
                  Auditorías Internas
                </h3>
                <p className="text-sm text-[#424752] leading-relaxed">
                  Evaluaciones independientes de 1ª y 2ª parte para verificar el cumplimiento antes de visitas de entes certificadores en Colombia.
                </p>
              </div>
              <a
                href="https://wa.me/573112490072?text=Hola%2C%20necesito%20cotizar%20una%20auditoria%20interna"
                target="_blank"
                rel="noopener noreferrer"
                className="inline-flex items-center gap-2 text-xs font-bold text-[#055bb2] hover:underline"
              >
                Cotizar Auditoría <ArrowRight className="w-4 h-4" />
              </a>
            </div>
          </div>
        </div>
      </section>

      {/* CTA Final */}
      <section className="py-16 bg-[#055bb2] text-white">
        <div className="max-w-[1280px] mx-auto px-4 md:px-10 text-center space-y-6">
          <h2 className="text-3xl font-bold font-headline">
            ¿Necesita soporte en la implementación de sus Sistemas de Gestión?
          </h2>
          <p className="text-sm md:text-base text-[#d6e3ff] max-w-xl mx-auto">
            Nuestros consultores están listos para guiar a su empresa en el cumplimiento normativo integral.
          </p>
          <a
            href="https://wa.me/573112490072?text=Hola%2C%20quisiera%20solicitar%20asesoria%20en%20servicios%20SGI"
            target="_blank"
            rel="noopener noreferrer"
            className="inline-flex items-center gap-2 bg-[#16A34A] text-white px-8 py-3.5 rounded-xl font-bold text-sm hover:bg-[#15803D] transition-colors shadow-lg"
          >
            <MessageSquare className="w-5 h-5" />
            Contactar por WhatsApp
          </a>
        </div>
      </section>
    </div>
  );
};
