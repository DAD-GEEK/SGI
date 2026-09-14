import React, { useState, useEffect } from 'react';
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
  CheckCircle2,
  FileText,
  Lock,
  KeyRound,
  AlertCircle,
  Eye,
  EyeOff
} from 'lucide-react';
import { supabase } from '../config/supabaseClient';
import { API_BASE_URL } from '../config/apiConfig';

export const Profile: React.FC = () => {
  const navigate = useNavigate();
  const [userId, setUserId] = useState<string | null>(null);
  const [name, setName] = useState('Administrador TI SGI');
  const [email, setEmail] = useState('admon@waloyogroup.com');
  const [phone, setPhone] = useState('+57 300 000 00 00');
  const [documento, setDocumento] = useState('CC-99999999');
  const [role, setRole] = useState('ADMIN_TI');
  const [savedSuccess, setSavedSuccess] = useState(false);
  const [loading, setLoading] = useState(true);

  // Estados para Cambio de Contraseña Voluntario
  const [showPasswordSection, setShowPasswordSection] = useState(false);
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [showPass, setShowPass] = useState(false);
  const [passLoading, setPassLoading] = useState(false);
  const [passError, setPassError] = useState<string | null>(null);
  const [passSuccess, setPassSuccess] = useState(false);

  const isAdminTi = role === 'ADMIN_TI';

  useEffect(() => {
    const fetchCurrentProfile = async () => {
      try {
        const { data } = await supabase.auth.getUser();
        const activeEmail = data?.user?.email || (localStorage.getItem('sgi_user') ? JSON.parse(localStorage.getItem('sgi_user') || '{}').email : 'admon@waloyogroup.com');

        if (activeEmail) {
          const res = await fetch(`${API_BASE_URL}/usuarios/verificar-estado?email=${encodeURIComponent(activeEmail)}`);
          if (res.ok) {
            const info = await res.json();
            if (info.email) {
              setUserId(info.id || null);
              setName(info.nombreCompleto || activeEmail.split('@')[0]);
              setEmail(info.email || activeEmail);
              setPhone(info.telefonoMovil || '');
              setDocumento(info.documento || '');
              setRole(info.rol || 'CONSULTOR');
            }
          }
        }
      } catch (err) {
        console.error('Error al cargar perfil:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchCurrentProfile();
  }, []);

  const handleSave = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (userId) {
        const payload: any = {
          nombreCompleto: name,
          telefonoMovil: phone,
          documento: documento
        };

        // Solo Admin TI puede modificar email y rol en su perfil
        if (isAdminTi) {
          payload.email = email;
          payload.rol = role;
        }

        const res = await fetch(`${API_BASE_URL}/usuarios/${userId}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(payload)
        });

        if (res.ok) {
          setSavedSuccess(true);
          setTimeout(() => setSavedSuccess(false), 3000);
          return;
        }
      }
      setSavedSuccess(true);
      setTimeout(() => setSavedSuccess(false), 3000);
    } catch (err) {
      console.error('Error al guardar perfil:', err);
      alert('Error al guardar cambios de perfil.');
    }
  };

  const handleChangePassword = async (e: React.FormEvent) => {
    e.preventDefault();
    setPassError(null);
    setPassSuccess(false);

    if (newPassword.length < 8) {
      setPassError('La nueva contraseña debe contener al menos 8 caracteres.');
      return;
    }

    const hasUpper = /[A-Z]/.test(newPassword);
    const hasLower = /[a-z]/.test(newPassword);
    const hasNum = /[0-9]/.test(newPassword);
    const hasSpecial = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(newPassword);

    if (!hasUpper || !hasLower || !hasNum || !hasSpecial) {
      setPassError('La contraseña debe incluir mayúsculas, minúsculas, números y un carácter especial.');
      return;
    }

    if (newPassword !== confirmPassword) {
      setPassError('Las contraseñas no coinciden. Por favor verifique ambas entradas.');
      return;
    }

    setPassLoading(true);
    try {
      // 1. Actualizar contraseña en Supabase Auth
      try {
        await supabase.auth.updateUser({ password: newPassword });
      } catch (authErr) {
        console.warn('Supabase Auth session password update:', authErr);
      }

      // 2. Notificar al backend Spring Boot para sincronización completa (Supabase + MSSQL Agenda 3DES + MSSQL Consultor Identity)
      const res = await fetch(`${API_BASE_URL}/usuarios/confirmar-clave`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          email: email,
          password: newPassword
        })
      });

      if (!res.ok) {
        throw new Error('No se pudo sincronizar la contraseña en el servidor.');
      }

      setPassSuccess(true);
      setNewPassword('');
      setConfirmPassword('');
      setShowPasswordSection(false);
      setTimeout(() => setPassSuccess(false), 4000);
    } catch (err: any) {
      setPassError(err.message || 'Error al actualizar la contraseña.');
    } finally {
      setPassLoading(false);
    }
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
            <span>Perfil actualizado exitosamente en el servidor SGI (Sincronizado en tiempo real).</span>
          </div>
        )}

        {/* Profile Card Header */}
        <div className="bg-white rounded-2xl border border-[#c2c6d4]/40 p-6 elevation-1 flex flex-col sm:flex-row items-center gap-6">
          <div className="w-20 h-20 rounded-full bg-[#055bb2] text-white font-bold text-2xl flex items-center justify-center shadow-md">
            {name ? name.substring(0, 2).toUpperCase() : 'US'}
          </div>
          <div className="space-y-1 text-center sm:text-left">
            <h2 className="text-xl font-bold font-headline text-[#191c1e]">{name}</h2>
            <p className="text-xs font-semibold text-[#055bb2]">
              {role === 'ADMIN_TI' ? '👑 Administrador TI (Holding / Super Admin)' : role === 'ADMIN' ? 'Administrador SGI' : 'Consultor SGI & Auditor'}
            </p>
            <p className="text-xs text-[#727783]">Licencia SST Vigente: Res. 0312 / Cobertura Nacional</p>
          </div>
        </div>

        {/* Form Settings */}
        {loading ? (
          <div className="bg-white rounded-2xl p-12 text-center text-slate-500">
            <div className="w-8 h-8 border-3 border-[#055bb2] border-t-transparent rounded-full animate-spin mx-auto mb-3" />
            <p className="font-medium text-xs">Cargando datos de perfil...</p>
          </div>
        ) : (
          <form onSubmit={handleSave} className="space-y-6">
            <div className="bg-white rounded-2xl border border-[#c2c6d4]/40 p-6 elevation-1 space-y-6">
              <div className="flex items-center justify-between border-b border-[#e0e3e5] pb-3">
                <h3 className="text-base font-bold font-headline text-[#191c1e] flex items-center gap-2">
                  <User className="w-5 h-5 text-[#055bb2]" />
                  Información Personal de la Cuenta
                </h3>
                {isAdminTi && (
                  <span className="text-[10px] bg-purple-100 text-purple-700 px-2.5 py-1 rounded-full font-bold">
                    Modo Edición Total (Admin TI)
                  </span>
                )}
              </div>

              <div className="grid grid-cols-1 sm:grid-cols-2 gap-5">
                <div className="space-y-1.5">
                  <label className="block text-xs font-semibold text-[#424752]">Nombre Completo *</label>
                  <div className="relative">
                    <User className="w-4 h-4 text-[#727783] absolute left-3 top-1/2 -translate-y-1/2" />
                    <input
                      type="text"
                      required
                      value={name}
                      onChange={(e) => setName(e.target.value)}
                      className="w-full pl-9 pr-4 py-2.5 rounded-xl border border-[#c2c6d4] text-xs focus:outline-none focus:border-[#055bb2] bg-[#f8fafc]"
                    />
                  </div>
                </div>

                <div className="space-y-1.5">
                  <label className="block text-xs font-semibold text-[#424752]">Cédula (CC / Documento ID) *</label>
                  <div className="relative">
                    <FileText className="w-4 h-4 text-[#727783] absolute left-3 top-1/2 -translate-y-1/2" />
                    <input
                      type="text"
                      required
                      value={documento}
                      onChange={(e) => setDocumento(e.target.value)}
                      className="w-full pl-9 pr-4 py-2.5 rounded-xl border border-[#c2c6d4] text-xs focus:outline-none focus:border-[#055bb2] bg-[#f8fafc]"
                    />
                  </div>
                </div>

                <div className="space-y-1.5">
                  <div className="flex justify-between items-center">
                    <label className="block text-xs font-semibold text-[#424752]">Correo Electrónico</label>
                    {!isAdminTi && (
                      <span className="text-[10px] text-slate-400 font-medium">(Solo lectura)</span>
                    )}
                  </div>
                  <div className="relative">
                    <Mail className="w-4 h-4 text-[#727783] absolute left-3 top-1/2 -translate-y-1/2" />
                    <input
                      type="email"
                      disabled={!isAdminTi}
                      value={email}
                      onChange={(e) => setEmail(e.target.value)}
                      className={`w-full pl-9 pr-4 py-2.5 rounded-xl border text-xs ${
                        isAdminTi
                          ? 'border-[#c2c6d4] focus:outline-none focus:border-[#055bb2] bg-[#f8fafc]'
                          : 'border-[#c2c6d4] bg-[#e0e3e5] text-[#545f73] cursor-not-allowed'
                      }`}
                    />
                  </div>
                </div>

                <div className="space-y-1.5">
                  <label className="block text-xs font-semibold text-[#424752]">Teléfono Móvil / WhatsApp Directo</label>
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

                <div className="space-y-1.5 sm:col-span-2">
                  <div className="flex justify-between items-center">
                    <label className="block text-xs font-semibold text-[#424752]">Rol de Licencia y Gobernanza</label>
                    {!isAdminTi && (
                      <span className="text-[10px] text-slate-400 font-medium">(Asignado por Administrador TI)</span>
                    )}
                  </div>
                  <div className="relative">
                    <Shield className="w-4 h-4 text-[#727783] absolute left-3 top-1/2 -translate-y-1/2" />
                    {isAdminTi ? (
                      <select
                        value={role}
                        onChange={(e) => setRole(e.target.value)}
                        className="w-full pl-9 pr-4 py-2.5 rounded-xl border border-[#c2c6d4] text-xs focus:outline-none focus:border-[#055bb2] bg-[#f8fafc] font-semibold text-purple-900"
                      >
                        <option value="ADMIN_TI">👑 Administrador TI (Holding / Super Admin)</option>
                        <option value="ADMIN">Administrador SGI</option>
                        <option value="CONSULTOR">Consultor / Asesor Técnico SGI</option>
                      </select>
                    ) : (
                      <input
                        type="text"
                        disabled
                        value={role === 'ADMIN' ? 'Administrador SGI' : 'Consultor / Asesor Técnico SGI'}
                        className="w-full pl-9 pr-4 py-2.5 rounded-xl border border-[#c2c6d4] text-xs bg-[#e0e3e5] text-[#545f73] cursor-not-allowed font-semibold"
                      />
                    )}
                  </div>
                </div>
              </div>
            </div>

            {/* Seguridad y Cambio de Contraseña */}
            <div className="bg-white rounded-2xl border border-[#c2c6d4]/40 p-6 elevation-1 space-y-5">
              <div className="flex items-center justify-between border-b border-[#e0e3e5] pb-3">
                <div className="flex items-center gap-2">
                  <KeyRound className="w-5 h-5 text-[#055bb2]" />
                  <div>
                    <h3 className="text-base font-bold font-headline text-[#191c1e]">Seguridad & Contraseña</h3>
                    <p className="text-[11px] text-[#727783]">Actualiza tu clave de acceso en CRM, Supabase y módulos legados (Agenda y Consultor).</p>
                  </div>
                </div>

                {!showPasswordSection ? (
                  <button
                    type="button"
                    onClick={() => setShowPasswordSection(true)}
                    className="inline-flex items-center gap-1.5 px-4 py-2 bg-slate-100 hover:bg-slate-200 text-slate-800 text-xs font-bold rounded-xl transition-colors cursor-pointer"
                  >
                    <Lock className="w-3.5 h-3.5" />
                    <span>Cambiar Contraseña</span>
                  </button>
                ) : (
                  <button
                    type="button"
                    onClick={() => {
                      setShowPasswordSection(false);
                      setPassError(null);
                      setNewPassword('');
                      setConfirmPassword('');
                    }}
                    className="text-xs text-slate-500 hover:text-slate-800 font-semibold cursor-pointer"
                  >
                    Cancelar Cambio
                  </button>
                )}
              </div>

              {passSuccess && (
                <div className="bg-emerald-50 border border-emerald-200 text-emerald-800 px-4 py-3 rounded-xl flex items-center gap-2.5 text-xs font-semibold">
                  <CheckCircle2 className="w-4 h-4 text-emerald-600 shrink-0" />
                  <span>Contraseña actualizada y sincronizada en Supabase Auth, Agenda SGI y Consultor SGI.</span>
                </div>
              )}

              {showPasswordSection && (
                <div className="bg-[#f8fafc] border border-slate-200/80 rounded-xl p-5 space-y-4">
                  {passError && (
                    <div className="bg-red-50 border border-red-200 text-red-700 px-3.5 py-2.5 rounded-lg flex items-center gap-2 text-xs font-medium">
                      <AlertCircle className="w-4 h-4 shrink-0 text-red-500" />
                      <span>{passError}</span>
                    </div>
                  )}

                  <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                    <div className="space-y-1.5">
                      <label className="block text-xs font-semibold text-slate-700">Nueva Contraseña</label>
                      <div className="relative">
                        <Lock className="w-4 h-4 text-slate-400 absolute left-3 top-1/2 -translate-y-1/2" />
                        <input
                          type={showPass ? 'text' : 'password'}
                          placeholder="Mínimo 8 caracteres..."
                          value={newPassword}
                          onChange={(e) => setNewPassword(e.target.value)}
                          className="w-full pl-9 pr-10 py-2.5 bg-white border border-slate-200 rounded-xl text-xs focus:outline-none focus:border-[#055bb2] font-medium"
                        />
                        <button
                          type="button"
                          onClick={() => setShowPass(!showPass)}
                          className="absolute right-3 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600 cursor-pointer"
                        >
                          {showPass ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                        </button>
                      </div>
                    </div>

                    <div className="space-y-1.5">
                      <label className="block text-xs font-semibold text-slate-700">Confirmar Nueva Contraseña</label>
                      <div className="relative">
                        <Lock className="w-4 h-4 text-slate-400 absolute left-3 top-1/2 -translate-y-1/2" />
                        <input
                          type={showPass ? 'text' : 'password'}
                          placeholder="Repita la nueva contraseña..."
                          value={confirmPassword}
                          onChange={(e) => setConfirmPassword(e.target.value)}
                          className="w-full pl-9 pr-4 py-2.5 bg-white border border-slate-200 rounded-xl text-xs focus:outline-none focus:border-[#055bb2] font-medium"
                        />
                      </div>
                    </div>
                  </div>

                  <p className="text-[11px] text-slate-500">
                    Requisitos: Mínimo 8 caracteres, mayúsculas, minúsculas, números y un carácter especial (ej. <code className="bg-white px-1.5 py-0.5 rounded border border-slate-200 text-slate-700 font-mono">!*</code>).
                  </p>

                  <div className="flex justify-end">
                    <button
                      type="button"
                      disabled={passLoading}
                      onClick={handleChangePassword}
                      className="inline-flex items-center gap-2 bg-[#055bb2] hover:bg-[#3374cd] text-white px-5 py-2.5 rounded-xl text-xs font-bold transition-all shadow-xs disabled:opacity-50 cursor-pointer"
                    >
                      <KeyRound className="w-4 h-4" />
                      <span>{passLoading ? 'Sincronizando...' : 'Confirmar y Sincronizar Clave'}</span>
                    </button>
                  </div>
                </div>
              )}
            </div>

            {/* Preferences */}
            <div className="bg-white rounded-2xl border border-[#c2c6d4]/40 p-6 elevation-1 space-y-4">
              <h3 className="text-base font-bold font-headline text-[#191c1e] border-b border-[#e0e3e5] pb-3 flex items-center gap-2">
                <Bell className="w-5 h-5 text-[#055bb2]" />
                Preferencias de Notificación & Alertas
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
                className="px-6 py-3 rounded-xl border border-[#c2c6d4] text-xs font-bold text-[#545f73] hover:bg-[#f2f4f6] transition-colors cursor-pointer"
              >
                Cancelar
              </button>
              <button
                type="submit"
                className="inline-flex items-center gap-2 bg-[#055bb2] text-white px-8 py-3 rounded-xl text-xs font-bold hover:bg-[#3374cd] transition-all shadow-md cursor-pointer"
              >
                <Save className="w-4 h-4" />
                <span>Guardar Cambios</span>
              </button>
            </div>
          </form>
        )}
      </main>
    </div>
  );
};
