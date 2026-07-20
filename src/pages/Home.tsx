import React from 'react';
import { Shield, CheckCircle2, Award, ArrowRight, Play, PhoneCall, Activity, LogIn, Target, Eye, HeartHandshake, Scale, Lock, Sparkles } from 'lucide-react';
import { Link } from 'react-router-dom';

export const Home: React.FC = () => {
  return (
    <div className="flex flex-col min-h-screen">
      {/* ============================================================ */}
      {/* 📱 DESKTOP HERO SECTION (md:flex)                            */}
      {/* ============================================================ */}
      <section className="hidden md:flex relative min-h-[82vh] items-center justify-center overflow-hidden bg-[#001b3e]" id="inicio">
        <div className="absolute inset-0 z-0">
          <img
            src="https://images.unsplash.com/photo-1522071820081-009f0129c71c?auto=format&fit=crop&w=1920&q=80"
            alt="Reunión corporativa de auditoría y consultoría SST"
            className="w-full h-full object-cover opacity-35"
          />
          <div className="absolute inset-0 bg-gradient-to-r from-[#001b3e] via-[#001b3e]/85 to-transparent" />
        </div>

        <div className="max-w-[1280px] mx-auto px-10 py-20 relative z-10 w-full text-white">
          <div className="max-w-3xl space-y-6">
            <div className="inline-flex items-center gap-2.5 bg-[#055bb2]/40 border border-[#a9c7ff]/30 text-[#d6e3ff] px-4 py-1.5 rounded-full text-xs font-semibold backdrop-blur-md">
              <img src="/logo.png" alt="SGI Logo" className="w-5 h-5 object-contain" />
              <span>Sistemas de Gestión Integrados • Res. 0312 & Dec. 1072</span>
            </div>

            <h1 className="text-5xl lg:text-6xl font-bold font-headline leading-tight tracking-tight">
              Servicios Profesionales en Sistemas de Gestión
            </h1>

            <p className="text-lg text-[#d8e3fb] font-sans leading-relaxed">
              Soluciones integrales en Seguridad y Salud en el Trabajo (SST), Calidad, Medio Ambiente y PESV para llevar su empresa al siguiente nivel de cumplimiento y resiliencia.
            </p>

            <div className="flex gap-4 pt-4">
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

      {/* ============================================================ */}
      {/* 📱 MOBILE HERO SECTION (md:hidden - Exact Plantilla Móvil)   */}
      {/* ============================================================ */}
      <section className="md:hidden relative bg-[#f2f4f6] px-4 py-12 text-center overflow-hidden flex flex-col justify-center items-center">
        <div className="relative z-10 space-y-5 max-w-md mx-auto">
          <div className="flex justify-center mb-2">
            <img src="/logo.png" alt="SGI Logo Mobile" className="w-16 h-16 object-contain" />
          </div>
          <h1 className="text-[36px] sm:text-[40px] leading-tight text-[#191c1e] font-bold font-headline">
            SU ASESOR... <br />
            <span className="text-[#055bb2]">SU ALIADO</span>
          </h1>

          <p className="text-base text-[#424752] leading-relaxed">
            Soluciones integrales para la gestión y crecimiento de su empresa. Experiencia y tecnología a su servicio.
          </p>

          <div className="pt-2 w-full">
            <button
              onClick={() => alert('Portal de Clientes SGI: Módulo en desarrollo.')}
              className="bg-[#055bb2] text-white font-semibold text-sm rounded-lg py-3 px-6 w-full flex items-center justify-center gap-2 shadow-md hover:bg-[#3374cd] transition-all active:scale-[0.98]"
            >
              <LogIn className="w-5 h-5" />
              Acceso Software/CRM
            </button>
          </div>
        </div>

        <div className="mt-8 w-full max-w-md rounded-xl overflow-hidden shadow-lg border border-[#c2c6d4]/40">
          <img
            src="https://images.unsplash.com/photo-1522071820081-009f0129c71c?auto=format&fit=crop&w=800&q=80"
            alt="Gestión Integral SGI Office"
            className="w-full h-auto object-cover aspect-[1.59]"
          />
        </div>
      </section>

      {/* ============================================================ */}
      {/* 📱 SECCIÓN NOSOTROS / MISIÓN Y VISIÓN                        */}
      {/* ============================================================ */}
      {/* Version Desktop (md:block) */}
      <section className="hidden md:block py-24 bg-[#f7f9fb]" id="nosotros">
        <div className="max-w-[1280px] mx-auto px-10">
          <div className="grid grid-cols-2 gap-16 items-center">
            <div className="space-y-6">
              <span className="text-[#055bb2] font-semibold text-xs uppercase tracking-widest bg-[#d6e3ff]/60 px-3 py-1 rounded-md">
                Sobre Nosotros
              </span>
              <h2 className="text-4xl font-bold font-headline text-[#191c1e]">
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

      {/* Version Mobile (md:hidden - Exact Bento Grid de la Plantilla Móvil) */}
      <section className="md:hidden px-4 py-10 bg-[#f7f9fb] space-y-5" id="nosotros-mobile">
        {/* Mission Card Mobile */}
        <div className="bg-white rounded-xl p-6 shadow-md relative overflow-hidden border border-[#e0e3e5]">
          <div className="absolute top-0 left-0 w-1.5 h-full bg-[#055bb2]" />
          <div className="flex items-center space-x-3 mb-3 border-b border-[#e6e8ea] pb-3">
            <div className="bg-[#055bb2]/15 p-2.5 rounded-full text-[#055bb2]">
              <Target className="w-6 h-6" />
            </div>
            <h2 className="text-xl font-bold font-headline text-[#191c1e]">Misión</h2>
          </div>
          <p className="text-sm text-[#424752] leading-relaxed">
            Brindar asesoría y consultoría integral a las organizaciones, apoyándolas en el cumplimiento de sus objetivos estratégicos mediante la implementación de soluciones tecnológicas y metodologías innovadoras.
          </p>
        </div>

        {/* Vision Card Mobile */}
        <div className="bg-white rounded-xl p-6 shadow-md relative overflow-hidden border border-[#e0e3e5]">
          <div className="absolute top-0 left-0 w-1.5 h-full bg-[#545f73]" />
          <div className="flex items-center space-x-3 mb-3 border-b border-[#e6e8ea] pb-3">
            <div className="bg-[#545f73]/15 p-2.5 rounded-full text-[#545f73]">
              <Eye className="w-6 h-6" />
            </div>
            <h2 className="text-xl font-bold font-headline text-[#191c1e]">Visión</h2>
          </div>
          <p className="text-sm text-[#424752] leading-relaxed">
            Ser reconocidos para el 2027 como una firma líder a nivel nacional en la prestación de servicios de consultoría y gestión empresarial, destacando por nuestra innovación, confiabilidad y el valor agregado entregado a nuestros clientes.
          </p>
        </div>
      </section>

      {/* ============================================================ */}
      {/* 🌟 SECCIÓN VALORES CORPORATIVOS (Con valores.png)             */}
      {/* ============================================================ */}
      <section className="py-20 bg-white border-y border-[#c2c6d4]/40" id="valores">
        <div className="max-w-[1280px] mx-auto px-4 md:px-10 space-y-12">
          <div className="text-center max-w-2xl mx-auto space-y-3">
            <span className="text-[#055bb2] font-semibold text-xs uppercase tracking-widest bg-[#d6e3ff]/60 px-3 py-1 rounded-md">
              Cultura & Principios
            </span>
            <h2 className="text-3xl md:text-4xl font-bold font-headline text-[#191c1e]">
              Nuestros Valores Corporativos
            </h2>
            <p className="text-sm md:text-base text-[#424752]">
              Fundamentamos cada proyecto de asesoría en principios éticos sólidos y en una relación integral de confianza y valor con nuestros clientes.
            </p>
          </div>

          {/* Banner Visual / Infografía de Valores */}
          <div className="max-w-4xl mx-auto rounded-2xl overflow-hidden shadow-lg border border-[#c2c6d4]/50 bg-[#f7f9fb] p-4 md:p-8 flex flex-col md:flex-row items-center gap-8">
            <div className="w-full md:w-1/2 flex justify-center">
              <img
                src="/valores.png"
                alt="Valores Corporativos Gestión Integral SGI"
                className="w-full h-auto max-h-[320px] object-contain rounded-xl shadow-xs"
              />
            </div>
            <div className="w-full md:w-1/2 space-y-4">
              <div className="inline-flex items-center gap-2 text-[#055bb2] font-bold text-sm">
                <Sparkles className="w-5 h-5" />
                <span>Nuestra Filosofía de Trabajo</span>
              </div>
              <h3 className="text-xl font-bold font-headline text-[#191c1e]">
                Confianza, Valor y Seguridad
              </h3>
              <p className="text-xs md:text-sm text-[#424752] leading-relaxed">
                Nuestros profesionales están altamente calificados, formados y comprometidos con ofrecer un servicio eficiente, oportuno, personalizado y confidencial que se ajuste a las particularidades de cada empresa.
              </p>
            </div>
          </div>

          {/* Cards Bento Grid de los 4 Valores */}
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
            {/* Valor 1: Ética Empresarial */}
            <div className="bg-[#f7f9fb] p-6 rounded-xl border border-[#e0e3e5] elevation-1 flex flex-col space-y-3">
              <div className="w-12 h-12 rounded-lg bg-[#055bb2]/10 text-[#055bb2] flex items-center justify-center">
                <Scale className="w-6 h-6" />
              </div>
              <h4 className="font-bold text-base text-[#191c1e] font-headline">Ética Empresarial</h4>
              <p className="text-xs text-[#424752] leading-relaxed">
                Prevalencia absoluta de la transparencia, rectitud y cumplimiento legal en cada diagnóstico, auditoría y consultoría.
              </p>
            </div>

            {/* Valor 2: Calidad Humana */}
            <div className="bg-[#f7f9fb] p-6 rounded-xl border border-[#e0e3e5] elevation-1 flex flex-col space-y-3">
              <div className="w-12 h-12 rounded-lg bg-[#16A34A]/10 text-[#16A34A] flex items-center justify-center">
                <HeartHandshake className="w-6 h-6" />
              </div>
              <h4 className="font-bold text-base text-[#191c1e] font-headline">Calidad Humana</h4>
              <p className="text-xs text-[#424752] leading-relaxed">
                Desarrollo sostenible enfocado primordialmente en la salud, la seguridad y la dignidad de cada colaborador.
              </p>
            </div>

            {/* Valor 3: Confidencialidad */}
            <div className="bg-[#f7f9fb] p-6 rounded-xl border border-[#e0e3e5] elevation-1 flex flex-col space-y-3">
              <div className="w-12 h-12 rounded-lg bg-[#545f73]/10 text-[#545f73] flex items-center justify-center">
                <Lock className="w-6 h-6" />
              </div>
              <h4 className="font-bold text-base text-[#191c1e] font-headline">Confidencialidad</h4>
              <p className="text-xs text-[#424752] leading-relaxed">
                Tratamiento seguro y reservado de toda la información técnica, documental y operativa de las organizaciones aliadas.
              </p>
            </div>

            {/* Valor 4: Eficiencia & Oportunidad */}
            <div className="bg-[#f7f9fb] p-6 rounded-xl border border-[#e0e3e5] elevation-1 flex flex-col space-y-3">
              <div className="w-12 h-12 rounded-lg bg-[#055bb2]/10 text-[#055bb2] flex items-center justify-center">
                <Sparkles className="w-6 h-6" />
              </div>
              <h4 className="font-bold text-base text-[#191c1e] font-headline">Eficiencia Oportuna</h4>
              <p className="text-xs text-[#424752] leading-relaxed">
                Acompañamiento ágil y soluciones a la medida que garantizan resultados tangibles y cierres efectivos de compromisos.
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* ============================================================ */}
      {/* 📱 ESPECIALIDADES PRINCIPALES (Bento Grid)                    */}
      {/* ============================================================ */}
      <section className="py-16 md:py-20 bg-[#eceef0]" id="servicios">
        <div className="max-w-[1280px] mx-auto px-4 md:px-10 space-y-10">
          <div className="text-center max-w-2xl mx-auto space-y-4">
            <span className="text-[#055bb2] font-semibold text-xs uppercase tracking-widest">
              Líneas de Consultoría
            </span>
            <h2 className="text-2xl md:text-4xl font-bold font-headline text-[#191c1e]">
              Nuestras Especialidades B2B
            </h2>
            <p className="text-sm md:text-base text-[#424752]">
              Soluciones diseñadas para mitigar riesgos ocupacionales, garantizar la calidad y optimizar la gestión documental normada en Colombia.
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-6 md:gap-8">
            {/* Card 1: SG-SST */}
            <div className="bg-white rounded-xl p-6 md:p-8 border border-[#c2c6d4]/40 elevation-1 bento-hover flex flex-col justify-between space-y-6">
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
            <div className="bg-white rounded-xl p-6 md:p-8 border border-[#c2c6d4]/40 elevation-1 bento-hover flex flex-col justify-between space-y-6">
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
            <div className="bg-white rounded-xl p-6 md:p-8 border border-[#c2c6d4]/40 elevation-1 bento-hover flex flex-col justify-between space-y-6">
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

      {/* ============================================================ */}
      {/* 📱 INDICADORES DE CONFIANZA / STATS (Plantilla Móvil)        */}
      {/* ============================================================ */}
      <section className="py-8 md:py-12 bg-white border-y border-[#c2c6d4]/40">
        <div className="max-w-[1280px] mx-auto px-4 md:px-10 grid grid-cols-2 md:grid-cols-4 gap-4 md:gap-6 text-center">
          <div className="flex flex-col items-center p-3">
            <span className="text-[#055bb2] font-headline text-3xl md:text-4xl font-bold mb-1">10+</span>
            <span className="text-xs md:text-sm font-medium text-[#424752]">Años de Experiencia</span>
          </div>
          <div className="flex flex-col items-center p-3 border-l border-[#c2c6d4]/30">
            <span className="text-[#055bb2] font-headline text-3xl md:text-4xl font-bold mb-1">100%</span>
            <span className="text-xs md:text-sm font-medium text-[#424752]">Compromiso</span>
          </div>
          <div className="flex flex-col items-center p-3 border-l border-[#c2c6d4]/30">
            <span className="text-[#055bb2] font-headline text-3xl md:text-4xl font-bold mb-1">24/7</span>
            <span className="text-xs md:text-sm font-medium text-[#424752]">Acompañamiento Técnico</span>
          </div>
          <div className="flex flex-col items-center p-3 border-l border-[#c2c6d4]/30">
            <span className="text-[#055bb2] font-headline text-3xl md:text-4xl font-bold mb-1">ISO</span>
            <span className="text-xs md:text-sm font-medium text-[#424752]">Auditoría Certificada</span>
          </div>
        </div>
      </section>

      {/* ============================================================ */}
      {/* 📱 CTA FINAL / CONTACTO                                      */}
      {/* ============================================================ */}
      <section className="py-16 md:py-20 bg-[#f7f9fb] pb-24 md:pb-20" id="contacto">
        <div className="max-w-[1280px] mx-auto px-4 md:px-10">
          <div className="bg-[#055bb2] rounded-2xl p-6 sm:p-8 md:p-14 text-white flex flex-col md:flex-row justify-between items-center gap-6 md:gap-8 shadow-xl">
            <div className="space-y-3 md:space-y-4 max-w-2xl text-center md:text-left">
              <h2 className="text-2xl md:text-4xl font-bold font-headline">
                ¿Listo para elevar el estándar de su Sistema de Gestión?
              </h2>
              <p className="text-xs md:text-base text-[#d6e3ff]">
                Contáctenos hoy mismo para agendar una evaluación diagnóstica con nuestras asesoras expertas.
              </p>
            </div>
            <a
              href="https://wa.me/573112490072?text=Hola%20SGI%2C%20quiero%20solicitar%20un%20diagnostico%20gratuito"
              target="_blank"
              rel="noopener noreferrer"
              className="bg-[#16A34A] text-white px-6 sm:px-8 py-3.5 sm:py-4 rounded-xl font-bold text-sm hover:bg-[#15803D] transition-colors shadow-lg flex-shrink-0 w-full md:w-auto text-center"
            >
              Contactar por WhatsApp
            </a>
          </div>
        </div>
      </section>
    </div>
  );
};
