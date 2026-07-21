import React, { useState } from 'react';
import { CrmSidebar } from '../components/CrmSidebar';
import { ExternalLink, ShieldCheck, RefreshCw, Lock, AlertCircle } from 'lucide-react';

export const ConsultorView: React.FC = () => {
  const consultorUrl = 'https://consultor.gestionintegralsgi.com.co/Seguridad/Login';
  const [iframeKey, setIframeKey] = useState(0);
  const [showIframe, setShowIframe] = useState(true);

  const handleRefresh = () => {
    setIframeKey((prev) => prev + 1);
  };

  return (
    <div className="min-h-screen bg-[#f7f9fb] flex flex-col md:flex-row font-sans">
      <CrmSidebar />

      {/* Main Workspace Area */}
      <div className="flex-grow flex flex-col h-screen overflow-hidden">
        {/* Header Bar */}
        <header className="bg-white border-b border-[#c2c6d4]/40 px-6 py-3.5 flex items-center justify-between shadow-xs z-10 flex-wrap gap-3">
          <div className="flex items-center gap-3">
            <div className="p-2 bg-emerald-500/10 text-emerald-600 rounded-xl">
              <ShieldCheck className="w-5 h-5" />
            </div>
            <div>
              <h1 className="font-bold text-base font-headline text-[#191c1e] flex items-center gap-2">
                Consultor SGI — Módulo de Auditoría & Res. 0312
              </h1>
              <div className="flex items-center gap-2 text-[11px] text-[#545f73]">
                <Lock className="w-3 h-3 text-emerald-600" />
                <span className="font-mono text-[10px] bg-[#f2f4f6] px-2 py-0.5 rounded text-[#424752]">
                  {consultorUrl}
                </span>
              </div>
            </div>
          </div>

          <div className="flex items-center gap-2">
            <button
              onClick={() => setShowIframe(!showIframe)}
              className="px-3 py-1.5 rounded-xl border border-[#c2c6d4] text-xs font-semibold text-[#545f73] hover:bg-[#f2f4f6] transition-colors"
            >
              {showIframe ? 'Ver Acceso Directo' : 'Intentar Embebido'}
            </button>
            <button
              onClick={handleRefresh}
              className="p-2 rounded-xl border border-[#c2c6d4] text-[#545f73] hover:bg-[#f2f4f6] transition-colors"
              title="Recargar vista web"
            >
              <RefreshCw className="w-4 h-4" />
            </button>
            <a
              href={consultorUrl}
              target="_blank"
              rel="noopener noreferrer"
              className="inline-flex items-center gap-2 bg-[#055bb2] text-white px-4 py-2 rounded-xl text-xs font-bold hover:bg-[#3374cd] transition-all shadow-xs"
            >
              <ExternalLink className="w-4 h-4" />
              <span>Abrir Consultor SGI en Nueva Pestaña</span>
            </a>
          </div>
        </header>

        {/* Workspace Content */}
        <div className="flex-grow p-4 bg-[#eceef0] relative overflow-hidden flex flex-col">
          {/* Security Banner Alert */}
          <div className="bg-amber-50 border border-amber-200 text-amber-900 px-4 py-2.5 rounded-xl mb-3 flex items-center justify-between text-xs">
            <div className="flex items-center gap-2">
              <AlertCircle className="w-4 h-4 text-amber-600 flex-shrink-0" />
              <span>
                <b>Nota de Seguridad Navegador (X-Frame-Options):</b> Si el servidor bloquea la incrustación por políticas de origen cruzado (SAMEORIGIN), utiliza el botón azul para abrir la plataforma en pestaña externa.
              </span>
            </div>
            <a
              href={consultorUrl}
              target="_blank"
              rel="noopener noreferrer"
              className="underline font-bold text-[#055bb2] hover:text-[#3374cd] flex-shrink-0 ml-2"
            >
              Abrir Sistema Externo
            </a>
          </div>

          {showIframe ? (
            <iframe
              key={iframeKey}
              src={consultorUrl}
              title="Consultor SGI Web"
              className="w-full flex-grow border-0 rounded-2xl shadow-lg bg-white"
              allow="fullscreen; camera; microphone; geolocation; autoplay"
            />
          ) : (
            <div className="w-full flex-grow bg-white rounded-2xl shadow-lg border border-[#c2c6d4]/40 p-8 flex flex-col justify-center items-center text-center space-y-6">
              <div className="w-16 h-16 rounded-2xl bg-emerald-500/10 text-emerald-600 flex items-center justify-center">
                <ShieldCheck className="w-8 h-8" />
              </div>
              <div className="space-y-2 max-w-md">
                <h2 className="text-xl font-bold font-headline text-[#191c1e]">
                  Plataforma Consultor SGI
                </h2>
                <p className="text-xs text-[#545f73] leading-relaxed">
                  Acceso directo al portal de gestión de auditorías, listas de chequeo y evaluaciones diagnósticas Res. 0312 de Gestión Integral SGI.
                </p>
              </div>
              <a
                href={consultorUrl}
                target="_blank"
                rel="noopener noreferrer"
                className="inline-flex items-center gap-2.5 bg-[#055bb2] text-white px-8 py-3.5 rounded-xl font-bold text-sm hover:bg-[#3374cd] transition-all shadow-md"
              >
                <ExternalLink className="w-5 h-5" />
                <span>Ingresar a consultor.gestionintegralsgi.com.co</span>
              </a>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};
