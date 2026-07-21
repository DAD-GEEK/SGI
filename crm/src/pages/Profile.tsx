import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import {
  User,
  Mail,
  Phone,
  Shield,
  Bell,
  Save,
  ArrowLeft,
  LayoutDashboard,
  CheckCircle2
} from 'lucide-react';

export const Profile: React.FC = () => {
  const navigate = useNavigate();
  const [name, setName] = useState('Dra. Amanda Durango');
  const [email, setEmail] = useState('asesorias@gestionintegralsgi.com.co');
  const [phone, setPhone] = useState('+57 311 249 00 72');
  const [role] = useState('Consultora Senior SGI & Auditora Líder');
  const [savedSuccess, setSavedSuccess] = useState(false);

  const handleSave = (e: React.FormEvent) => {
    e.preventDefault();
    setSavedSuccess(true);
    setTimeout(() => setSavedSuccess(false), 3000);
  };

  return (
    <div className="min-h-screen bg-[#f7f9fb] flex flex-col font-sans">
      {/* Header */}
      <header className="bg-[#0b1c30] text-white px-6 py-4 flex items-center justify-between shadow-md">
        <div className="flex items-center gap-4">
          <Link
            to="/dashboard"
            className="p-2 rounded-xl bg-white/10 hover:bg-white/20 transition-colors text-white"
          >
            <ArrowLeft className="w-5 h-5" />
          </Link>
          <div className="flex items-center gap-3">
            <img src="/logo-limpio.png" alt="SGI Logo" className="w-8 h-8 object-contain" />
            <h1 className="font-bold text-lg font-headline">SGI CRM — Perfil & Preferencias</h1>
          </div>
        </div>
        <Link
          to="/dashboard"
          className="inline-flex items-center gap-2 bg-[#055bb2] text-white px-4 py-2 rounded-xl text-xs font-bold hover:bg-[#3374cd] transition-all"
        >
          <LayoutDashboard className="w-4 h-4" />
          <span>Volver al Dashboard</span>
        </Link>
      </header>

      {/* Main Profile Content */}
      <main className="p-6 max-w-4xl mx-auto w-full space-y-8 flex-grow">
        {savedSuccess && (
          <div className="bg-emerald-50 border border-emerald-200 text-emerald-800 px-4 py-3 rounded-xl flex items-center gap-3 text-xs font-semibold shadow-xs">
            <CheckCircle2 className="w-5 h-5 text-emerald-600" />
            <span>Perfil y preferencias guardadas exitosamente en el servidor SGI.</span>
          </div>
        )}

        {/* Profile Card Header */}
        <div className="bg-white rounded-2xl border border-[#c2c6d4]/40 p-6 elevation-1 flex flex-col sm:flex-row items-center gap-6">
          <div className="w-20 h-20 rounded-full bg-[#055bb2] text-white font-bold text-2xl flex items-center justify-center shadow-md">
            AD
          </div>
          <div className="space-y-1 text-center sm:text-left">
            <h2 className="text-xl font-bold font-headline text-[#191c1e]">{name}</h2>
            <p className="text-xs font-semibold text-[#055bb2]">{role}</p>
            <p className="text-xs text-[#727783]">Licencia SST Vigente: Res. 0312 / Cobertura Nacional</p>
          </div>
        </div>

        {/* Form Settings */}
        <form onSubmit={handleSave} className="space-y-6">
          <div className="bg-white rounded-2xl border border-[#c2c6d4]/40 p-6 elevation-1 space-y-6">
            <h3 className="text-base font-bold font-headline text-[#191c1e] border-b border-[#e0e3e5] pb-3 flex items-center gap-2">
              <User className="w-5 h-5 text-[#055bb2]" />
              Información de la Cuenta
            </h3>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-5">
              <div className="space-y-1.5">
                <label className="block text-xs font-semibold text-[#424752]">Nombre Completo</label>
                <div className="relative">
                  <User className="w-4 h-4 text-[#727783] absolute left-3 top-1/2 -translate-y-1/2" />
                  <input
                    type="text"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    className="w-full pl-9 pr-4 py-2.5 rounded-xl border border-[#c2c6d4] text-xs focus:outline-none focus:border-[#055bb2] bg-[#f8fafc]"
                  />
                </div>
              </div>

              <div className="space-y-1.5">
                <label className="block text-xs font-semibold text-[#424752]">Correo Electrónico</label>
                <div className="relative">
                  <Mail className="w-4 h-4 text-[#727783] absolute left-3 top-1/2 -translate-y-1/2" />
                  <input
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    className="w-full pl-9 pr-4 py-2.5 rounded-xl border border-[#c2c6d4] text-xs focus:outline-none focus:border-[#055bb2] bg-[#f8fafc]"
                  />
                </div>
              </div>

              <div className="space-y-1.5">
                <label className="block text-xs font-semibold text-[#424752]">Teléfono / WhatsApp Directo</label>
                <div className="relative">
                  <Phone className="w-4 h-4 text-[#727783] absolute left-3 top-1/2 -translate-y-1/2" />
                  <input
                    type="text"
                    value={phone}
                    onChange={(e) => setPhone(e.target.value)}
                    className="w-full pl-9 pr-4 py-2.5 rounded-xl border border-[#c2c6d4] text-xs focus:outline-none focus:border-[#055bb2] bg-[#f8fafc]"
                  />
                </div>
              </div>

              <div className="space-y-1.5">
                <label className="block text-xs font-semibold text-[#424752]">Rol de Licencia</label>
                <div className="relative">
                  <Shield className="w-4 h-4 text-[#727783] absolute left-3 top-1/2 -translate-y-1/2" />
                  <input
                    type="text"
                    disabled
                    value={role}
                    className="w-full pl-9 pr-4 py-2.5 rounded-xl border border-[#c2c6d4] text-xs bg-[#e0e3e5] text-[#545f73] cursor-not-allowed"
                  />
                </div>
              </div>
            </div>
          </div>

          {/* Preferences */}
          <div className="bg-white rounded-2xl border border-[#c2c6d4]/40 p-6 elevation-1 space-y-4">
            <h3 className="text-base font-bold font-headline text-[#191c1e] border-b border-[#e0e3e5] pb-3 flex items-center gap-2">
              <Bell className="w-5 h-5 text-[#055bb2]" />
              Preferencias de Notificación
            </h3>

            <div className="space-y-3 text-xs text-[#424752]">
              <label className="flex items-center justify-between cursor-pointer p-2 hover:bg-[#f8fafc] rounded-xl transition-colors">
                <span>Notificaciones de vencimiento de compromisos por WhatsApp</span>
                <input type="checkbox" defaultChecked className="w-4 h-4 rounded text-[#055bb2]" />
              </label>

              <label className="flex items-center justify-between cursor-pointer p-2 hover:bg-[#f8fafc] rounded-xl transition-colors">
                <span>Resumen semanal de avance PHVA por correo electrónico</span>
                <input type="checkbox" defaultChecked className="w-4 h-4 rounded text-[#055bb2]" />
              </label>
            </div>
          </div>

          <div className="flex justify-end gap-4">
            <button
              type="button"
              onClick={() => navigate('/dashboard')}
              className="px-6 py-3 rounded-xl border border-[#c2c6d4] text-xs font-bold text-[#545f73] hover:bg-[#f2f4f6] transition-colors"
            >
              Cancelar
            </button>
            <button
              type="submit"
              className="inline-flex items-center gap-2 bg-[#055bb2] text-white px-8 py-3 rounded-xl text-xs font-bold hover:bg-[#3374cd] transition-all shadow-md"
            >
              <Save className="w-4 h-4" />
              <span>Guardar Cambios</span>
            </button>
          </div>
        </form>
      </main>
    </div>
  );
};
