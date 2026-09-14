import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import {
  User,
  Mail,
  Phone,
  Shield,
  Save,
  ArrowLeft,
  LayoutDashboard,
  CheckCircle2,
  FileText,
  Lock,
  KeyRound,
  AlertCircle,
  Eye,
  EyeOff,
  Sparkles,
  RefreshCw
} from 'lucide-react';
import { CrmSidebar } from '../components/CrmSidebar';
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
  const [isSaving, setIsSaving] = useState(false);
  const [loading, setLoading] = useState(true);

  // Estados para Cambio de Contraseña Voluntario
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
        const activeEmail =
          data?.user?.email ||
          (localStorage.getItem('sgi_user')
            ? JSON.parse(localStorage.getItem('sgi_user') || '{}').email
            : 'admon@waloyogroup.com');

        if (activeEmail) {
          const res = await fetch(
            `${API_BASE_URL}/usuarios/verificar-estado?email=${encodeURIComponent(activeEmail)}`
          );
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
    setIsSaving(true);
    try {
      if (userId) {
        const payload: any = {
          nombreCompleto: name.trim(),
          telefonoMovil: phone.trim(),
          documento: documento.trim()
        };

        if (isAdminTi) {
          payload.email = email.trim();
          payload.rol = role;
        }

        const res = await fetch(`${API_BASE_URL}/usuarios/${userId}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(payload)
        });

        if (!res.ok) {
          throw new Error('Error al actualizar en el servidor SGI');
        }
      }

      // Actualizar inmediatamente localStorage para reactividad instantánea en toda la SPA
      try {
        const storedRaw = localStorage.getItem('sgi_user');
        const current = storedRaw ? JSON.parse(storedRaw) : {};
        const updated = {
          ...current,
          nombre: name.trim(),
          email: isAdminTi ? email.trim() : current.email || email.trim(),
          rol: isAdminTi ? role : current.rol || role
        };
        localStorage.setItem('sgi_user', JSON.stringify(updated));

        // Registrar notificación en el centro de notificaciones de la campanita
        const rawNotifs = localStorage.getItem('sgi_notifications');
        const currentNotifs = rawNotifs ? JSON.parse(rawNotifs) : [];
        const newNotif = {
          id: `notif-profile-${Date.now()}`,
          title: 'Perfil Actualizado',
          description: `Los datos del perfil de ${name.trim()} se han sincronizado exitosamente.`,
          time: 'Ahora',
          read: false
        };
        localStorage.setItem(
          'sgi_notifications',
          JSON.stringify([newNotif, ...currentNotifs].slice(0, 30))
        );
        window.dispatchEvent(new Event('sgi_notifications_changed'));
      } catch {}

      // Despachar eventos nativos de reactividad inmediata
      window.dispatchEvent(new Event('sgi_user_changed'));
      window.dispatchEvent(new Event('storage'));

      setSavedSuccess(true);

      // Redirigir al dashboard tras breve momento para apreciar la confirmación
      setTimeout(() => {
        navigate('/dashboard');
      }, 1000);
    } catch (err) {
      console.error('Error al guardar perfil:', err);
      alert('Ocurrió un error al guardar los cambios de perfil.');
      setIsSaving(false);
    }
  };

  const handleChangePassword = async (e: React.FormEvent) => {
    e.preventDefault();
    setPassError(null);
    setPassSuccess(false);

    if (newPassword.length < 8) {
      setPassError('La nueva contraseña debe tener al menos 8 caracteres.');
      return;
    }

    const hasUpper = /[A-Z]/.test(newPassword);
    const hasLower = /[a-z]/.test(newPassword);
    const hasNum = /[0-9]/.test(newPassword);
    const hasSpecial = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(newPassword);

    if (!hasUpper || !hasLower || !hasNum || !hasSpecial) {
      setPassError('Debe incluir mayúsculas, minúsculas, números y un carácter especial.');
      return;
    }

    if (newPassword !== confirmPassword) {
      setPassError('Las contraseñas no coinciden. Por favor verifique ambas entradas.');
      return;
    }

    setPassLoading(true);
    try {
      try {
        await supabase.auth.updateUser({ password: newPassword });
      } catch (authErr) {
        console.warn('Supabase Auth session password update:', authErr);
      }

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
      setTimeout(() => setPassSuccess(false), 4000);
    } catch (err: any) {
      setPassError(err.message || 'Error al actualizar la contraseña.');
    } finally {
      setPassLoading(false);
    }
  };

  const getRoleBadge = () => {
    if (role === 'ADMIN_TI') {
      return (
        <span className="inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-bold bg-purple-100 text-purple-800 border border-purple-200 shadow-xs">
          <Sparkles className="w-3.5 h-3.5 text-purple-600" />
          Administrador TI
        </span>
      );
    }
    if (role === 'ADMIN') {
      return (
        <span className="inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-bold bg-sky-100 text-sky-800 border border-sky-200 shadow-xs">
          <Shield className="w-3.5 h-3.5 text-sky-600" />
          Administrador SGI
        </span>
      );
    }
    return (
      <span className="inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-bold bg-emerald-100 text-emerald-800 border border-emerald-200 shadow-xs">
        <User className="w-3.5 h-3.5 text-emerald-600" />
        Consultor & Auditor
      </span>
    );
  };

  return (
    <div className="h-screen bg-[#f7f9fb] flex flex-col md:flex-row font-sans overflow-hidden">
      {/* Barra Lateral Corporativa SGI */}
      <CrmSidebar activeTab="perfil" />

      {/* Área Principal de Trabajo */}
      <div className="flex-grow flex flex-col overflow-y-auto w-full transition-all duration-300 ease-in-out">
        {/* Encabezado Superior Compacto */}
        <header className="bg-white border-b border-[#c2c6d4]/40 px-5 py-3 flex items-center justify-between sticky top-0 z-30 elevation-1">
          <div className="flex items-center gap-3">
            <Link
              to="/dashboard"
              className="p-1.5 rounded-xl text-[#545f73] hover:bg-[#f2f4f6] hover:text-[#191c1e] transition-colors"
              title="Volver al Panel de Control"
            >
              <ArrowLeft className="w-5 h-5" />
            </Link>
            <div className="flex items-center gap-2">
              <h2 className="text-base font-bold font-headline text-[#191c1e]">
                Perfil & Cuenta
              </h2>
              <span className="hidden sm:inline-block text-[11px] font-semibold text-[#055bb2] bg-[#055bb2]/10 px-2 py-0.5 rounded-full">
                SGI Software
              </span>
            </div>
          </div>

          <div className="flex items-center gap-3">
            <Link
              to="/dashboard"
              className="inline-flex items-center gap-1.5 bg-[#f2f4f6] text-[#191c1e] hover:bg-slate-200 px-3 py-1.5 rounded-xl text-xs font-semibold transition-all"
            >
              <LayoutDashboard className="w-4 h-4 text-[#055bb2]" />
              <span className="hidden sm:inline">Volver al Dashboard</span>
            </Link>
          </div>
        </header>

        {/* Notificación Push Flotante de Éxito en Sistema */}
        {savedSuccess && (
          <div className="fixed top-4 right-4 z-50 animate-in fade-in slide-in-from-top-4 duration-300 shadow-2xl bg-white border border-emerald-300 rounded-2xl p-3.5 flex items-start gap-3 max-w-sm">
            <div className="w-9 h-9 rounded-xl bg-emerald-500 text-white flex items-center justify-center shrink-0 shadow-md">
              <CheckCircle2 className="w-5 h-5" />
            </div>
            <div className="space-y-0.5 flex-grow">
              <div className="flex items-center justify-between">
                <h4 className="text-xs font-bold text-slate-900">¡Perfil Guardado con Éxito!</h4>
                <span className="bg-emerald-100 text-emerald-800 text-[9px] font-bold px-1.5 py-0.5 rounded-full">
                  En Vivo
                </span>
              </div>
              <p className="text-[11px] text-slate-600">
                Sincronizado en todo el CRM. Regresando al Dashboard...
              </p>
              <div className="w-full bg-slate-100 h-1 rounded-full overflow-hidden mt-1.5">
                <div className="bg-emerald-500 h-full w-full animate-pulse" />
              </div>
            </div>
          </div>
        )}

        {/* Contenedor de Contenido Horizontal Fluidamente Adaptable */}
        <main className="w-full max-w-[1720px] mx-auto px-4 sm:px-6 lg:px-8 py-5 space-y-5 flex-grow">
          {/* Tarjeta Resumen Horizontal de Usuario (Adaptable Móvil / Escritorio) */}
          <div className="bg-white rounded-2xl border border-[#c2c6d4]/40 px-4 sm:px-5 py-3.5 shadow-xs flex flex-col sm:flex-row items-start sm:items-center justify-between gap-3.5 sm:gap-4">
            <div className="flex items-center gap-3.5 w-full sm:w-auto">
              <div className="relative shrink-0">
                <div className="w-12 h-12 sm:w-13 sm:h-13 rounded-2xl bg-gradient-to-br from-[#055bb2] to-[#0b1c30] text-white font-bold text-lg sm:text-xl flex items-center justify-center shadow-md">
                  {name ? name.substring(0, 2).toUpperCase() : 'US'}
                </div>
                <span
                  className="absolute -bottom-0.5 -right-0.5 w-3.5 h-3.5 sm:w-4 sm:h-4 bg-emerald-500 border-2 border-white rounded-full flex items-center justify-center"
                  title="Sesión Activa"
                >
                  <span className="w-1 h-1 bg-white rounded-full animate-ping" />
                </span>
              </div>

              <div className="min-w-0 flex-grow">
                <h1 className="text-base sm:text-lg font-bold font-headline text-[#191c1e] tracking-tight leading-snug truncate">
                  {name}
                </h1>
                <div className="flex flex-wrap items-center gap-x-2.5 sm:gap-x-3 gap-y-0.5 text-xs text-[#545f73]">
                  <span className="truncate max-w-[200px] sm:max-w-none">{email}</span>
                  {documento && (
                    <>
                      <span className="hidden sm:inline">•</span>
                      <span className="font-mono text-[#727783]">ID: {documento}</span>
                    </>
                  )}
                  {phone && (
                    <>
                      <span className="hidden sm:inline">•</span>
                      <span>Móvil: {phone}</span>
                    </>
                  )}
                </div>
              </div>
            </div>

            <div className="self-start sm:self-auto shrink-0">{getRoleBadge()}</div>
          </div>

          {/* Formulario de Doble Columna Horizontal (Aprovechamiento de Espacio) */}
          {loading ? (
            <div className="bg-white rounded-2xl p-10 text-center text-slate-500 border border-[#c2c6d4]/40">
              <div className="w-7 h-7 border-2 border-[#055bb2] border-t-transparent rounded-full animate-spin mx-auto mb-2" />
              <p className="font-medium text-xs">Cargando datos de cuenta...</p>
            </div>
          ) : (
            <div className="grid grid-cols-1 lg:grid-cols-12 gap-4 items-start">
              {/* Columna Izquierda: Información Personal (7 Cols) */}
              <form
                onSubmit={handleSave}
                className="lg:col-span-7 bg-white rounded-2xl border border-[#c2c6d4]/40 p-5 shadow-xs space-y-4"
              >
                <div className="flex items-center justify-between border-b border-[#e0e3e5] pb-2.5">
                  <h3 className="text-sm font-bold font-headline text-[#191c1e] flex items-center gap-2">
                    <User className="w-4 h-4 text-[#055bb2]" />
                    <span>Información Personal de la Cuenta</span>
                  </h3>
                  {isAdminTi && (
                    <span className="text-[10px] bg-purple-100 text-purple-700 px-2 py-0.5 rounded-full font-bold">
                      Admin TI
                    </span>
                  )}
                </div>

                <div className="grid grid-cols-1 sm:grid-cols-2 gap-3.5">
                  <div className="space-y-1">
                    <label className="block text-[11px] font-semibold text-[#424752]">
                      Nombre Completo *
                    </label>
                    <div className="relative">
                      <User className="w-3.5 h-3.5 text-[#727783] absolute left-3 top-1/2 -translate-y-1/2" />
                      <input
                        type="text"
                        required
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        placeholder="Ej. Carlos Mendoza"
                        className="w-full pl-9 pr-3 py-2.5 sm:py-2 min-h-[40px] sm:min-h-[38px] rounded-xl border border-[#c2c6d4] text-xs focus:outline-none focus:ring-2 focus:ring-[#055bb2]/20 focus:border-[#055bb2] bg-[#f8fafc] font-medium"
                      />
                    </div>
                  </div>

                  <div className="space-y-1">
                    <label className="block text-[11px] font-semibold text-[#424752]">
                      Cédula / Documento de Identidad *
                    </label>
                    <div className="relative">
                      <FileText className="w-3.5 h-3.5 text-[#727783] absolute left-3 top-1/2 -translate-y-1/2" />
                      <input
                        type="text"
                        required
                        value={documento}
                        onChange={(e) => setDocumento(e.target.value)}
                        placeholder="Ej. CC-10203040"
                        className="w-full pl-9 pr-3 py-2.5 sm:py-2 min-h-[40px] sm:min-h-[38px] rounded-xl border border-[#c2c6d4] text-xs focus:outline-none focus:ring-2 focus:ring-[#055bb2]/20 focus:border-[#055bb2] bg-[#f8fafc] font-medium font-mono"
                      />
                    </div>
                  </div>

                  <div className="space-y-1">
                    <div className="flex justify-between items-center">
                      <label className="block text-[11px] font-semibold text-[#424752]">
                        Correo Electrónico
                      </label>
                      {!isAdminTi && (
                        <span className="text-[9px] text-slate-400 font-medium">(Solo lectura)</span>
                      )}
                    </div>
                    <div className="relative">
                      <Mail className="w-3.5 h-3.5 text-[#727783] absolute left-3 top-1/2 -translate-y-1/2" />
                      <input
                        type="email"
                        disabled={!isAdminTi}
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        className={`w-full pl-9 pr-3 py-2.5 sm:py-2 min-h-[40px] sm:min-h-[38px] rounded-xl border text-xs font-medium ${
                          isAdminTi
                            ? 'border-[#c2c6d4] focus:outline-none focus:ring-2 focus:ring-[#055bb2]/20 focus:border-[#055bb2] bg-[#f8fafc]'
                            : 'border-[#c2c6d4] bg-[#e0e3e5] text-[#545f73] cursor-not-allowed'
                        }`}
                      />
                    </div>
                  </div>

                  <div className="space-y-1">
                    <label className="block text-[11px] font-semibold text-[#424752]">
                      Teléfono Móvil / WhatsApp
                    </label>
                    <div className="relative">
                      <Phone className="w-3.5 h-3.5 text-[#727783] absolute left-3 top-1/2 -translate-y-1/2" />
                      <input
                        type="text"
                        value={phone}
                        onChange={(e) => setPhone(e.target.value)}
                        placeholder="Ej. +57 310 123 4567"
                        className="w-full pl-9 pr-3 py-2.5 sm:py-2 min-h-[40px] sm:min-h-[38px] rounded-xl border border-[#c2c6d4] text-xs focus:outline-none focus:ring-2 focus:ring-[#055bb2]/20 focus:border-[#055bb2] bg-[#f8fafc] font-medium"
                      />
                    </div>
                  </div>
                </div>

                <div className="flex flex-col-reverse sm:flex-row sm:items-center sm:justify-end gap-2.5 sm:gap-3 pt-3 border-t border-slate-100">
                  <button
                    type="button"
                    onClick={() => navigate('/dashboard')}
                    className="w-full sm:w-auto px-5 py-2.5 sm:py-2 rounded-xl border border-[#c2c6d4] text-xs font-bold text-[#545f73] hover:bg-[#f2f4f6] transition-colors cursor-pointer text-center"
                  >
                    Cancelar
                  </button>
                  <button
                    type="submit"
                    disabled={isSaving}
                    className="w-full sm:w-auto inline-flex items-center justify-center gap-2 bg-[#055bb2] text-white px-6 py-2.5 sm:py-2 rounded-xl text-xs font-bold hover:bg-[#3374cd] transition-all shadow-sm cursor-pointer disabled:opacity-50"
                  >
                    {isSaving ? (
                      <>
                        <RefreshCw className="w-3.5 h-3.5 animate-spin" />
                        <span>Guardando...</span>
                      </>
                    ) : (
                      <>
                        <Save className="w-3.5 h-3.5" />
                        <span>Guardar Cambios</span>
                      </>
                    )}
                  </button>
                </div>
              </form>

              {/* Columna Derecha: Seguridad & Contraseña (5 Cols) */}
              <div className="lg:col-span-5 bg-white rounded-2xl border border-[#c2c6d4]/40 p-5 shadow-xs space-y-3.5">
                <div className="flex items-center justify-between border-b border-[#e0e3e5] pb-2.5">
                  <h3 className="text-sm font-bold font-headline text-[#191c1e] flex items-center gap-2">
                    <KeyRound className="w-4 h-4 text-[#055bb2]" />
                    <span>Seguridad & Contraseña</span>
                  </h3>
                  <span className="text-[10px] text-slate-400 font-medium">Supabase + SQL Server</span>
                </div>

                {passSuccess && (
                  <div className="bg-emerald-50 border border-emerald-200 text-emerald-800 px-3 py-2 rounded-xl flex items-center gap-2 text-xs font-semibold">
                    <CheckCircle2 className="w-4 h-4 text-emerald-600 shrink-0" />
                    <span>Contraseña sincronizada en todos los módulos.</span>
                  </div>
                )}

                {passError && (
                  <div className="bg-red-50 border border-red-200 text-red-700 px-3 py-2 rounded-xl flex items-center gap-2 text-xs font-medium">
                    <AlertCircle className="w-4 h-4 shrink-0 text-red-500" />
                    <span>{passError}</span>
                  </div>
                )}

                <div className="space-y-3">
                  <div className="space-y-1">
                    <label className="block text-[11px] font-semibold text-[#424752]">
                      Nueva Contraseña
                    </label>
                    <div className="relative">
                      <Lock className="w-3.5 h-3.5 text-slate-400 absolute left-3 top-1/2 -translate-y-1/2" />
                      <input
                        type={showPass ? 'text' : 'password'}
                        placeholder="Mínimo 8 caracteres..."
                        value={newPassword}
                        onChange={(e) => setNewPassword(e.target.value)}
                        className="w-full pl-9 pr-9 py-2.5 sm:py-2 min-h-[40px] sm:min-h-[38px] bg-[#f8fafc] border border-slate-200 rounded-xl text-xs focus:outline-none focus:border-[#055bb2] font-medium"
                      />
                      <button
                        type="button"
                        onClick={() => setShowPass(!showPass)}
                        className="absolute right-3 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600 cursor-pointer"
                      >
                        {showPass ? <EyeOff className="w-3.5 h-3.5" /> : <Eye className="w-3.5 h-3.5" />}
                      </button>
                    </div>
                  </div>

                  <div className="space-y-1">
                    <label className="block text-[11px] font-semibold text-[#424752]">
                      Confirmar Contraseña
                    </label>
                    <div className="relative">
                      <Lock className="w-3.5 h-3.5 text-slate-400 absolute left-3 top-1/2 -translate-y-1/2" />
                      <input
                        type={showPass ? 'text' : 'password'}
                        placeholder="Repita la nueva contraseña..."
                        value={confirmPassword}
                        onChange={(e) => setConfirmPassword(e.target.value)}
                        className="w-full pl-9 pr-3 py-2.5 sm:py-2 min-h-[40px] sm:min-h-[38px] bg-[#f8fafc] border border-slate-200 rounded-xl text-xs focus:outline-none focus:border-[#055bb2] font-medium"
                      />
                    </div>
                  </div>

                  <p className="text-[10px] text-slate-500 leading-tight">
                    Mínimo 8 caracteres con mayúsculas, minúsculas, números y símbolo (ej.{' '}
                    <code className="bg-slate-100 px-1 py-0.5 rounded text-slate-700 font-mono">!*@#</code>).
                  </p>

                  <div className="pt-1 flex justify-end">
                    <button
                      type="button"
                      disabled={passLoading || !newPassword}
                      onClick={handleChangePassword}
                      className="w-full inline-flex items-center justify-center gap-2 bg-slate-900 hover:bg-black text-white py-2.5 sm:py-2 rounded-xl text-xs font-bold transition-all shadow-sm disabled:opacity-40 cursor-pointer min-h-[40px] sm:min-h-[38px]"
                    >
                      <KeyRound className="w-3.5 h-3.5" />
                      <span>{passLoading ? 'Sincronizando...' : 'Actualizar Contraseña'}</span>
                    </button>
                  </div>
                </div>
              </div>
            </div>
          )}
        </main>
      </div>
    </div>
  );
};

export default Profile;
