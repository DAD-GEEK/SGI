import React from 'react';
import { Shield, CheckCircle2, Award, ArrowRight, Play, PhoneCall, Activity } from 'lucide-react';
import { Link } from 'react-router-dom';

export const Home: React.FC = () => {
  return (
    <div className="flex flex-col min-h-screen">
      {/* Hero Section */}
      <section className="relative min-h-[82vh] flex items-center justify-center overflow-hidden bg-[#001b3e]" id="inicio">
        <div className="absolute inset-0 z-0">
          <img
            src="https://images.unsplash.com/photo-1522071820081-009f0129c71c?auto=format&fit=crop&w=1920&q=80"
            alt="Reunión corporativa de auditoría y consultoría SST"
            className="w-full h-full object-cover opacity-35"
          />
          <div className="absolute inset-0 bg-gradient-to-r from-[#001b3e] via-[#001b3e]/85 to-transparent" />
        </div>

        <div className="max-w-[1280px] mx-auto px-4 md:px-10 py-20 relative z-10 w-full text-white">
          <div className="max-w-3xl space-y-6">
            <div className="inline-flex items-center gap-2 bg-[#055bb2]/40 border border-[#a9c7ff]/30 text-[#d6e3ff] px-4 py-1.5 rounded-full text-xs font-semibold backdrop-blur-md">
              <Shield className="w-4 h-4 text-[#a9c7ff]" />
              <span>Sistemas de Gestión Integrados • Res. 0312 & Dec. 1072</span>
            </div>

            <h1 className="text-3xl md:text-5xl lg:text-6xl font-bold font-headline leading-tight tracking-tight">
              Servicios Profesionales en Sistemas de Gestión
            </h1>

            <p className="text-base md:text-lg text-[#d8e3fb] font-sans leading-relaxed">
              Soluciones integrales en Seguridad y Salud en el Trabajo (SST), Calidad, Medio Ambiente y PESV para llevar su empresa al siguiente nivel de cumplimiento y resiliencia.
            </p>

            <div className="flex flex-col sm:flex-row gap-4 pt-4">
              <Link
                to="/servicios"
                className="inline-flex items-center justify-center gap-2 bg-[#055bb2] text-white px-8 py-3.5 rounded-lg font-semibold text-sm hover:bg-[#3374cd] transition-all shadow-lg hover:shadow-xl"
              >
                Conocer Servicios
                <ArrowRight className="w-4 h-4" />
              </Link>
              <a
                href="https://wa.me/573112490072?text=Hola%2C%20quisiera%20agendar%20una%20consultoria"
                target="_blank"
                rel="noopener noreferrer"
                className="inline-flex items-center justify-center gap-2 border border-white/40 bg-white/10 backdrop-blur-md text-white px-8 py-3.5 rounded-lg font-semibold text-sm hover:bg-white/20 transition-all"
              >
                <PhoneCall className="w-4 h-4" />
                Agendar Consultoría
              </a>
            </div>
          </div>
        </div>
      </section>

      {/* Seccion Nosotros / Manifiesto */}
      <section className="py-20 md:py-28 bg-[#f7f9fb]" id="nosotros">
        <div className="max-w-[1280px] mx-auto px-4 md:px-10">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-12 lg:gap-16 items-center">
            <div className="space-y-6">
              <span className="text-[#055bb2] font-semibold text-xs uppercase tracking-widest bg-[#d6e3ff]/60 px-3 py-1 rounded-md">
                Sobre Nosotros
              </span>
              <h2 className="text-3xl md:text-4xl font-bold font-headline text-[#191c1e]">
                ¿Quiénes Somos?
              </h2>
              <h3 className="text-lg font-semibold text-[#055bb2]">
                Gestión Integral SGI S.A.S. — SU ASESOR… SU ALIADO
              </h3>

              <div className="space-y-6 text-[#424752]">
                <div className="bg-white p-6 rounded-xl border border-[#c2c6d4]/40 elevation-1">
                  <h4 className="font-bold text-base text-[#191c1e] mb-2 flex items-center gap-2">
                    <CheckCircle2 className="w-5 h-5 text-[#055bb2]" />
                    Misión
                  </h4>
                  <p className="text-sm leading-relaxed">
                    Promover ambientes de trabajo seguros, generar valor constante y acompañar activamente en el mejoramiento continuo de los procesos en las organizaciones clientes a través de consultorías técnicas especializadas y soluciones digitales.
                  </p>
                </div>

                <div className="bg-white p-6 rounded-xl border border-[#c2c6d4]/40 elevation-1">
                  <h4 className="font-bold text-base text-[#191c1e] mb-2 flex items-center gap-2">
                    <Award className="w-5 h-5 text-[#055bb2]" />
                    Visión (2027)
                  </h4>
                  <p className="text-sm leading-relaxed">
                    Ser reconocidos nacionalmente como la firma aliada líder en el diseño, implementación y auditoría de Sistemas de Gestión Integrados, destacándonos por la innovación en software y el rigor legal.
                  </p>
                </div>
              </div>
            </div>

            {/* Video / Visual Asset Card */}
            <div className="relative group">
              <div className="absolute inset-0 bg-[#055bb2]/10 rounded-2xl transform translate-x-4 translate-y-4 -z-10" />
              <div className="bg-white p-3 rounded-2xl shadow-xl border border-[#c2c6d4]/40 overflow-hidden">
                <img
                  src="https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?auto=format&fit=crop&w=800&q=80"
                  alt="Equipo consultor SGI"
                  className="w-full h-auto rounded-xl object-cover aspect-[4/3] group-hover:scale-105 transition-transform duration-500"
                />
                <div className="absolute inset-0 flex items-center justify-center">
                  <a
                    href="https://wa.me/573112490072"
                    target="_blank"
                    rel="noopener noreferrer"
                    className="bg-[#055bb2]/90 text-white w-16 h-16 rounded-full flex items-center justify-center shadow-xl hover:scale-110 transition-transform"
                    aria-label="Contactar al equipo SGI"
                  >
                    <Play className="w-6 h-6 fill-current ml-1" />
                  </a>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Especialidades Principales */}
      <section className="py-20 bg-[#eceef0]" id="servicios">
        <div className="max-w-[1280px] mx-auto px-4 md:px-10 space-y-12">
          <div className="text-center max-w-2xl mx-auto space-y-4">
            <span className="text-[#055bb2] font-semibold text-xs uppercase tracking-widest">
              Líneas de Consultoría
            </span>
            <h2 className="text-3xl md:text-4xl font-bold font-headline text-[#191c1e]">
              Nuestras Especialidades B2B
            </h2>
            <p className="text-sm md:text-base text-[#424752]">
              Soluciones diseñadas para mitigar riesgos ocupacionales, garantizar la calidad y optimizar la gestión documental normada en Colombia.
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            {/* Card 1: SG-SST */}
            <div className="bg-white rounded-xl p-8 border border-[#c2c6d4]/40 elevation-1 bento-hover flex flex-col justify-between space-y-6">
              <div className="space-y-4">
                <div className="w-14 h-14 rounded-full bg-[#d5e0f8] text-[#055bb2] flex items-center justify-center">
                  <Shield className="w-7 h-7" />
                </div>
                <h3 className="text-xl font-bold font-headline text-[#191c1e]">
                  SG-SST (Seguridad y Salud)
                </h3>
                <p className="text-sm text-[#424752] leading-relaxed">
                  Diseño, implementación y administración del Sistema de Gestión de Seguridad y Salud en el Trabajo bajo el Decreto 1072/2015 y la Res. 0312/2019.
                </p>
              </div>
              <Link
                to="/servicios"
                className="inline-flex items-center gap-2 text-sm font-bold text-[#055bb2] hover:underline pt-2"
              >
                Ver detalle SG-SST <ArrowRight className="w-4 h-4" />
              </Link>
            </div>

            {/* Card 2: SGC */}
            <div className="bg-white rounded-xl p-8 border border-[#c2c6d4]/40 elevation-1 bento-hover flex flex-col justify-between space-y-6">
              <div className="space-y-4">
                <div className="w-14 h-14 rounded-full bg-[#d6e3ff] text-[#055bb2] flex items-center justify-center">
                  <Award className="w-7 h-7" />
                </div>
                <h3 className="text-xl font-bold font-headline text-[#191c1e]">
                  SGC (Gestión de Calidad)
                </h3>
                <p className="text-sm text-[#424752] leading-relaxed">
                  Estructuración de Sistemas de Gestión de Calidad bajo la norma ISO 9001:2015, orientados a la optimización de procesos y satisfacción del cliente.
                </p>
              </div>
              <Link
                to="/servicios"
                className="inline-flex items-center gap-2 text-sm font-bold text-[#055bb2] hover:underline pt-2"
              >
                Ver detalle Calidad <ArrowRight className="w-4 h-4" />
              </Link>
            </div>

            {/* Card 3: PESV */}
            <div className="bg-white rounded-xl p-8 border border-[#c2c6d4]/40 elevation-1 bento-hover flex flex-col justify-between space-y-6">
              <div className="space-y-4">
                <div className="w-14 h-14 rounded-full bg-[#d3e4fe] text-[#055bb2] flex items-center justify-center">
                  <Activity className="w-7 h-7" />
                </div>
                <h3 className="text-xl font-bold font-headline text-[#191c1e]">
                  PESV (Seguridad Vial)
                </h3>
                <p className="text-sm text-[#424752] leading-relaxed">
                  Elaboración y seguimiento del Plan Estratégico de Seguridad Vial bajo la Ley 2251/2022 y Res. 40595 para flotas corporativas y conductores.
                </p>
              </div>
              <Link
                to="/pesv"
                className="inline-flex items-center gap-2 text-sm font-bold text-[#055bb2] hover:underline pt-2"
              >
                Ver detalle PESV <ArrowRight className="w-4 h-4" />
              </Link>
            </div>
          </div>
        </div>
      </section>

      {/* CTA / Contact Section */}
      <section className="py-20 bg-white" id="contacto">
        <div className="max-w-[1280px] mx-auto px-4 md:px-10">
          <div className="bg-[#055bb2] rounded-2xl p-8 md:p-14 text-white flex flex-col md:flex-row justify-between items-center gap-8 shadow-xl">
            <div className="space-y-4 max-w-2xl text-center md:text-left">
              <h2 className="text-2xl md:text-4xl font-bold font-headline">
                ¿Listo para elevar el estándar de su Sistema de Gestión?
              </h2>
              <p className="text-sm md:text-base text-[#d6e3ff]">
                Contáctenos hoy mismo para agendar una evaluación diagnóstica con nuestras asesoras expertas.
              </p>
            </div>
            <a
              href="https://wa.me/573112490072?text=Hola%20SGI%2C%20quiero%20solicitar%20un%20diagnostico%20gratuito"
              target="_blank"
              rel="noopener noreferrer"
              className="bg-[#16A34A] text-white px-8 py-4 rounded-xl font-bold text-sm hover:bg-[#15803D] transition-colors shadow-lg flex-shrink-0"
            >
              Contactar por WhatsApp
            </a>
          </div>
        </div>
      </section>
    </div>
  );
};
