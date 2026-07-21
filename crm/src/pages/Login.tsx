import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Shield, Lock, Mail, ArrowRight } from 'lucide-react';

export const Login: React.FC = () => {
  const navigate = useNavigate();
  const [email, setEmail] = useState('asesor@gestionintegralsgi.com.co');
  const [password, setPassword] = useState('••••••••••••');
  const [remember, setRemember] = useState(true);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    // In unauthenticated demo, navigate directly to dashboard
    navigate('/dashboard');
  };

  return (
    <div className="min-h-screen bg-[#f7f9fb] flex flex-col justify-center items-center px-4 py-12 relative overflow-hidden">
      {/* Subtle Background Pattern */}
      <div className="absolute inset-0 bg-gradient-to-br from-[#055bb2]/5 via-transparent to-[#3c475a]/5 pointer-events-none" />

      <div className="w-full max-w-md bg-white rounded-2xl p-8 md:p-10 shadow-xl border border-[#c2c6d4]/40 relative z-10 space-y-8">
        {/* Brand Header */}
        <div className="text-center space-y-3">
          <div className="flex justify-center mb-3">
            <img
              src="/logo-limpio.png"
              alt="Gestión Integral SGI Logo"
              className="w-16 h-16 object-contain"
            />
          </div>
          <h1 className="text-2xl font-bold font-headline text-[#191c1e]">
            Gestión Integral SGI
          </h1>
          <p className="text-xs uppercase tracking-wider font-semibold text-[#055bb2]">
            Portal de Software & CRM B2B
          </p>
        </div>

        {/* Login Form */}
        <form onSubmit={handleSubmit} className="space-y-5">
          <div className="space-y-1.5">
            <label className="block text-xs font-semibold text-[#424752]">
              Correo Electrónico Corporativo
            </label>
            <div className="relative">
              <Mail className="w-5 h-5 text-[#727783] absolute left-3.5 top-1/2 -translate-y-1/2" />
              <input
                type="email"
                required
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="usuario@empresa.com"
                className="w-full pl-11 pr-4 py-3 rounded-xl border border-[#c2c6d4] text-sm focus:outline-none focus:border-[#055bb2] focus:ring-2 focus:ring-[#055bb2]/20 transition-all bg-[#f8fafc]"
              />
            </div>
          </div>

          <div className="space-y-1.5">
            <div className="flex justify-between items-center">
              <label className="block text-xs font-semibold text-[#424752]">
                Contraseña
              </label>
              <a
                href="#recuperar"
                onClick={(e) => {
                  e.preventDefault();
                  alert('Instrucciones enviadas a su correo corporativo.');
                }}
                className="text-xs font-semibold text-[#055bb2] hover:underline"
              >
                ¿Olvidó su contraseña?
              </a>
            </div>
            <div className="relative">
              <Lock className="w-5 h-5 text-[#727783] absolute left-3.5 top-1/2 -translate-y-1/2" />
              <input
                type="password"
                required
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="••••••••••••"
                className="w-full pl-11 pr-4 py-3 rounded-xl border border-[#c2c6d4] text-sm focus:outline-none focus:border-[#055bb2] focus:ring-2 focus:ring-[#055bb2]/20 transition-all bg-[#f8fafc]"
              />
            </div>
          </div>

          <div className="flex items-center justify-between pt-1">
            <label className="flex items-center gap-2 cursor-pointer text-xs font-medium text-[#424752]">
              <input
                type="checkbox"
                checked={remember}
                onChange={(e) => setRemember(e.target.checked)}
                className="w-4 h-4 rounded text-[#055bb2] focus:ring-[#055bb2]"
              />
              Recordar esta sesión
            </label>
          </div>

          <button
            type="submit"
            className="w-full bg-[#055bb2] text-white py-3.5 rounded-xl font-bold text-sm hover:bg-[#3374cd] transition-all shadow-md flex items-center justify-center gap-2 active:scale-[0.99]"
          >
            <span>Iniciar Sesión en CRM</span>
            <ArrowRight className="w-4 h-4" />
          </button>
        </form>

        {/* Security Footer */}
        <div className="pt-4 border-t border-[#e0e3e5] text-center space-y-2">
          <div className="inline-flex items-center gap-1.5 text-[11px] font-semibold text-[#545f73]">
            <Shield className="w-3.5 h-3.5 text-[#16A34A]" />
            <span>Acceso Seguro Encriptado SSL / OAuth 2.0</span>
          </div>
          <p className="text-[10px] text-[#727783]">
            © 2026 Gestión Integral SGI S.A.S. Plataforma privada.
          </p>
        </div>
      </div>
    </div>
  );
};
