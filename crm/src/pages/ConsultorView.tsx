import React, { useState } from 'react';
import { CrmSidebar } from '../components/CrmSidebar';
import { ExternalLink, ShieldCheck, RefreshCw, Lock } from 'lucide-react';

export const ConsultorView: React.FC = () => {
  const [iframeKey, setIframeKey] = useState(0);

  const storedUserRaw = localStorage.getItem('sgi_user');
  const storedUser = storedUserRaw ? JSON.parse(storedUserRaw) : {};
  const userEmail = storedUser.email || '';

  const consultorUrl = userEmail
    ? `https://consultor.gestionintegralsgi.com.co/Seguridad/SSO?email=${encodeURIComponent(userEmail)}`
    : 'https://consultor.gestionintegralsgi.com.co/Seguridad/Login';

  const handleRefresh = () => {
    setIframeKey((prev) => prev + 1);
  };

  return (
    <div className="min-h-screen bg-[#f7f9fb] flex flex-col md:flex-row font-sans">
      <CrmSidebar />

      {/* Main Workspace Area */}
      <div className="flex-grow flex flex-col h-screen overflow-hidden">
        {/* Header Bar Compacto */}
        <header className="bg-white border-b border-[#c2c6d4]/40 px-4 py-2 flex items-center justify-between shadow-xs z-10 flex-wrap gap-2 text-xs">
          <div className="flex items-center gap-2">
            <div className="p-1.5 bg-emerald-500/10 text-emerald-600 rounded-lg">
              <ShieldCheck className="w-4 h-4" />
            </div>
            <span className="font-bold font-headline text-[#191c1e] text-sm">
              Consultor SGI
            </span>
            <div className="hidden md:flex items-center gap-1.5 font-mono text-[10px] bg-[#f2f4f6] px-2 py-0.5 rounded text-[#424752]">
              <Lock className="w-2.5 h-2.5 text-emerald-600" />
              <span>consultor.gestionintegralsgi.com.co</span>
            </div>
          </div>

          <div className="flex items-center gap-2">
            <button
              onClick={handleRefresh}
              className="p-1.5 rounded-lg border border-[#c2c6d4] text-[#545f73] hover:bg-[#f2f4f6] transition-colors cursor-pointer"
              title="Recargar vista web"
            >
              <RefreshCw className="w-3.5 h-3.5" />
            </button>
            <a
              href={consultorUrl}
              target="_blank"
              rel="noopener noreferrer"
              className="inline-flex items-center gap-1.5 bg-[#055bb2] text-white px-3 py-1.5 rounded-lg text-xs font-semibold hover:bg-[#3374cd] transition-all shadow-xs"
              title="Abrir en ventana completa"
            >
              <ExternalLink className="w-3.5 h-3.5" />
              <span>Abrir</span>
            </a>
          </div>
        </header>

        {/* Workspace Content al 100% de espacio */}
        <div className="flex-grow p-1 bg-[#eceef0] relative overflow-hidden flex flex-col">
          <iframe
            key={iframeKey}
            src={consultorUrl}
            title="Consultor SGI Web"
            className="w-full flex-grow border-0 rounded-2xl shadow-lg bg-white"
            allow="fullscreen; camera; microphone; geolocation; autoplay"
          />
        </div>
      </div>
    </div>
  );
};
