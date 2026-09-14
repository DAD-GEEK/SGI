import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Shield, Lock, Mail, ArrowRight, AlertCircle, HelpCircle } from 'lucide-react';
import { supabase } from '../config/supabaseClient';
import { API_BASE_URL } from '../config/apiConfig';

export const Login: React.FC = () => {
  const navigate = useNavigate();
  const [email, setEmail] = useState(() => {
    return localStorage.getItem('sgi_remembered_email') || 'admon@waloyogroup.com';
  });
  const [password, setPassword] = useState('');
  const [remember, setRemember] = useState(true);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const checkActiveSession = () => {
      const storedUserRaw = localStorage.getItem('sgi_user');
      if (storedUserRaw) {
        try {
          const storedUser = JSON.parse(storedUserRaw);
          const configuredLimit = parseFloat(localStorage.getItem('sgi_session_limit_hours') || '4');
          const MAX_SESSION_MS = Math.round(configuredLimit * 60 * 60 * 1000);
          if (
            storedUser.email &&
            storedUser.loginTimestamp &&
            Date.now() - storedUser.loginTimestamp < MAX_SESSION_MS
          ) {
            if (storedUser.mustChangePassword) {
              navigate('/cambiar-password', { replace: true });
            } else {
              navigate('/dashboard', { replace: true });
            }
          }
        } catch {}
      }
    };
    checkActiveSession();
  }, [navigate]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    // Validaciones programáticas corporativas (Cero popups genéricos de navegador)
    if (!email || !email.trim()) {
      setError('Por favor ingrese su correo electrónico corporativo.');
      return;
    }
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email.trim())) {
      setError('El formato del correo electrónico es inválido. Ejemplo: asesor@gestionintegralsgi.com.co');
      return;
    }
    if (!password || !password.trim()) {
      setError('Por favor ingrese su contraseña de acceso al sistema SGI.');
      return;
    }

    setLoading(true);

    if (remember) {
      localStorage.setItem('sgi_remembered_email', email);
    } else {
      localStorage.removeItem('sgi_remembered_email');
    }

    try {
      // 1. Verificar primero en la API Spring Boot si el usuario está ACTIVO o INACTIVO (timeout de 1.5s para no congelar la UI)
      try {
        const checkRes = await fetch(`${API_BASE_URL}/usuarios/verificar-estado?email=${encodeURIComponent(email)}`, {
          signal: AbortSignal.timeout(1500)
        });
        if (checkRes.ok) {
          const userStatus = await checkRes.json();
          if (userStatus.activo === false) {
            throw new Error('Su cuenta de asesor ha sido desactivada. Por favor comuníquese con el Administrador para restablecer su acceso.');
          }
          if (userStatus.mustChangePassword) {
            // Guardar sesión previa para flujo de cambio de clave
            localStorage.setItem('sgi_user', JSON.stringify({
              email,
              mustChangePassword: true,
              loginTimestamp: Date.now()
            }));
            navigate('/cambiar-password', { replace: true });
            return;
          }
        }
      } catch (err: any) {
        if (err.message && err.message.includes('desactivada')) {
          throw err;
        }
        console.warn('Backend API estado check fallback (continuando login):', err);
      }

      // 2. Intentar inicio de sesión con Supabase Auth (Email + Password) con timeout holgado de 10s
      const authPromise = supabase.auth.signInWithPassword({
        email,
        password
      });
      const timeoutPromise = new Promise<{ data: any; error: any }>((_, reject) =>
        setTimeout(() => reject(new Error('SUPABASE_TIMEOUT')), 10000)
      );

      let data: any = null;
      let authError: any = null;

      try {
        const res = await Promise.race([authPromise, timeoutPromise]);
        data = res.data;
        authError = res.error;
      } catch (err: any) {
        if (err.message === 'SUPABASE_TIMEOUT') {
          authError = { message: 'El servicio de autenticación tardó en responder. Por favor intente nuevamente.' };
        } else {
          authError = err;
        }
      }

      if (authError) {
        throw new Error(
          authError.message === 'Invalid login credentials'
            ? 'Correo o contraseña incorrectos. Verifique sus credenciales registradas.'
            : (authError.message || 'Error de autenticación en el sistema.')
        );
      }

      const user = data.user;
      if (!user) throw new Error('No se pudo obtener el perfil de usuario.');

      // 3. Consultar y asociar el perfil institucional (rol, modulos, nombre) del usuario
      let userRole = 'CONSULTOR';
      let userNombre = user.email?.split('@')[0] || 'Usuario SGI';
      let userModulos = ['dashboard', 'clientes', 'agenda', 'consultor'];

      let userMustChange = false;

      try {
        const profileRes = await fetch(`${API_BASE_URL}/usuarios/verificar-estado?email=${encodeURIComponent(user.email || email)}`, {
          signal: AbortSignal.timeout(1500)
        });
        if (profileRes.ok) {
          const profileData = await profileRes.json();
          if (profileData.activo === false) {
            throw new Error('Su cuenta de asesor ha sido desactivada. Por favor comuníquese con el Administrador para restablecer su acceso.');
          }
          if (profileData.rol) userRole = profileData.rol;
          if (profileData.nombreCompleto) userNombre = profileData.nombreCompleto;
          if (profileData.modulosPermitidos) {
            userModulos = profileData.modulosPermitidos.split(',');
          }
          if (profileData.mustChangePassword) {
            userMustChange = true;
          }
        }
      } catch (e: any) {
        if (e.message && e.message.includes('desactivada')) {
          throw e;
        }
        console.warn('No se pudo precargar perfil en login:', e);
      }

      // Si debe cambiar clave obligatoriamente, redirigir a cambio de contraseña
      if (userMustChange) {
        localStorage.setItem('sgi_user', JSON.stringify({
          email: user.email,
          id: user.id,
          nombre: userNombre,
          rol: userRole,
          modulos: userModulos,
          mustChangePassword: true,
          loginTimestamp: Date.now()
        }));
        navigate('/cambiar-password', { replace: true });
        return;
      }

      // Si es ADMIN_TI o ADMIN, garantizar acceso universal a todos los módulos existentes y futuros
      if (userRole === 'ADMIN_TI' || userRole === 'ADMIN') {
        userModulos = ['dashboard', 'clientes', 'agenda', 'consultor', 'usuarios', '*'];
      }

      // 4. Auto-sincronizar contraseñas con MSSQL y generar notificación informativa
      try {
        const syncRes = await fetch(`${API_BASE_URL}/usuarios/auto-sincronizar-password`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ email: user.email || email, password }),
          signal: AbortSignal.timeout(2500)
        });
        if (syncRes.ok) {
          const syncData = await syncRes.json();
          const storedNotifs = JSON.parse(localStorage.getItem('sgi_notifications') || '[]');
          const remainingNotifs = storedNotifs.filter((n: any) => !n.id.startsWith('pwd-sync-'));

          if (syncData && syncData.updated === true) {
            const syncNotif = {
              id: `pwd-sync-${Date.now()}`,
              title: 'Contraseñas actualizadas en aplicativos',
              description: 'El sistema detectó que tu clave en Agenda y Consultor SGI estaba desactualizada y la sincronizó automáticamente con tu cuenta del CRM.',
              time: new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
              date: 'Hoy',
              read: false,
              type: 'sync'
            };
            localStorage.setItem('sgi_notifications', JSON.stringify([syncNotif, ...remainingNotifs]));
          } else if (syncData && syncData.success === true) {
            const syncNotif = {
              id: `pwd-sync-${Date.now()}`,
              title: 'Credenciales sincronizadas en aplicativos',
              description: 'Tus accesos para Agenda y Consultor SGI se encuentran verificados y al día con tu sesión del CRM.',
              time: new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
              date: 'Hoy',
              read: false,
              type: 'sync'
            };
            localStorage.setItem('sgi_notifications', JSON.stringify([syncNotif, ...remainingNotifs]));
          }
          window.dispatchEvent(new CustomEvent('sgi_notifications_changed'));
        }
      } catch (syncErr) {
        console.warn('Auto-sync password note:', syncErr);
      }


      // 6. Guardar sesión con timestamp y redirigir al Dashboard
      localStorage.setItem('sgi_user', JSON.stringify({
        email: user.email,
        id: user.id,
        nombre: userNombre,
        rol: userRole,
        modulos: userModulos,
        mustChangePassword: false,
        loginTimestamp: Date.now()
      }));
      navigate('/dashboard', { replace: true });

    } catch (err: any) {
      setError(err.message || 'Error al iniciar sesión en la plataforma.');
    } finally {
      setLoading(false);
    }
  };

  const handleForgotPassword = (e: React.MouseEvent) => {
    e.preventDefault();
    setError('Para restablecer su clave corporativa, solicite a un Administrador TI la generación de credenciales temporales.');
  };

  return (
    <div className="min-h-[100dvh] w-full bg-[#f7f9fb] flex flex-col justify-between items-center px-4 py-3 sm:py-4 md:py-6 relative overflow-x-hidden font-sans">
      <div className="absolute inset-0 bg-gradient-to-br from-[#055bb2]/5 via-transparent to-[#3c475a]/5 pointer-events-none" />

      {/* Tarjeta de Login Centrada Verticalmente sin Desbordamiento */}
      <div className="w-full max-w-md bg-white rounded-2xl p-5 sm:p-7 md:p-8 shadow-xl border border-[#c2c6d4]/40 relative z-10 space-y-4 sm:space-y-5 my-auto">
        <div className="text-center space-y-1.5 sm:space-y-2">
          <div className="flex justify-center mb-1.5 sm:mb-2">
            <img
              src="/logo-limpio.png"
              alt="Gestión Integral SGI Logo"
              className="w-12 h-12 sm:w-14 sm:h-14 object-contain"
            />
          </div>
          <h1 className="text-xl sm:text-2xl font-bold font-headline text-[#191c1e] tracking-tight">
            Gestión Integral SGI
          </h1>
          <p className="text-[11px] sm:text-xs uppercase tracking-wider font-semibold text-[#055bb2]">
            Portal de Software
          </p>
        </div>

        {/* Notificación de Error o Validación del Sistema SGI */}
        {error && (
          <div className="p-3.5 bg-red-50 border border-red-200 text-red-700 rounded-xl text-xs flex items-center gap-3 leading-relaxed shadow-xs animate-in fade-in duration-200">
            <AlertCircle className="w-4 h-4 shrink-0 text-red-600" />
            <span className="font-medium">{error}</span>
          </div>
        )}

        <form onSubmit={handleSubmit} noValidate className="space-y-3.5 sm:space-y-4">
          <div className="space-y-1">
            <label className="block text-xs font-semibold text-[#424752]">
              Correo Electrónico Corporativo *
            </label>
            <div className="relative">
              <Mail className="w-4 h-4 text-[#727783] absolute left-3.5 top-1/2 -translate-y-1/2" />
              <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="usuario@empresa.com"
                className="w-full pl-10 pr-4 py-2.5 rounded-xl border border-[#c2c6d4] text-xs sm:text-sm focus:outline-none focus:border-[#055bb2] focus:ring-2 focus:ring-[#055bb2]/20 transition-all bg-[#f8fafc]"
              />
            </div>
          </div>

          <div className="space-y-1">
            <div className="flex justify-between items-center">
              <label className="block text-xs font-semibold text-[#424752]">
                Contraseña *
              </label>
              <button
                type="button"
                onClick={handleForgotPassword}
                className="text-[11px] sm:text-xs font-semibold text-[#055bb2] hover:underline cursor-pointer flex items-center gap-1"
              >
                <HelpCircle className="w-3.5 h-3.5" />
                <span>¿Olvidó su contraseña?</span>
              </button>
            </div>
            <div className="relative">
              <Lock className="w-4 h-4 text-[#727783] absolute left-3.5 top-1/2 -translate-y-1/2" />
              <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="••••••••••••"
                className="w-full pl-10 pr-4 py-2.5 rounded-xl border border-[#c2c6d4] text-xs sm:text-sm focus:outline-none focus:border-[#055bb2] focus:ring-2 focus:ring-[#055bb2]/20 transition-all bg-[#f8fafc]"
              />
            </div>
          </div>

          <div className="flex items-center justify-between pt-0.5">
            <label className="flex items-center gap-2 cursor-pointer select-none text-xs text-[#424752] font-medium">
              <input
                type="checkbox"
                checked={remember}
                onChange={(e) => setRemember(e.target.checked)}
                className="w-3.5 h-3.5 rounded text-[#055bb2] border-[#c2c6d4] focus:ring-[#055bb2]"
              />
              <span>Recordar esta sesión</span>
            </label>

            {/* Tooltip con explicación de la opción Recordar */}
            <div className="relative group flex items-center">
              <button
                type="button"
                tabIndex={0}
                aria-label="Información sobre la opción recordar sesión"
                className="text-[#727783] hover:text-[#055bb2] transition-colors p-1 rounded-full hover:bg-black/5 cursor-help"
              >
                <HelpCircle className="w-3.5 h-3.5" />
              </button>
              <div className="absolute right-0 bottom-full mb-2 hidden group-hover:block group-focus-within:block w-64 p-3 bg-[#191c1e] text-white text-[11px] leading-relaxed rounded-xl shadow-xl z-30 pointer-events-none border border-white/10">
                <p className="font-bold text-[#a9c7ff] mb-1">¿Qué hace esta opción?</p>
                <p className="text-gray-300">
                  Guarda su correo corporativo en este navegador para que no tenga que escribirlo de nuevo en sus próximos inicios de sesión. Por seguridad, su contraseña jamás es almacenada.
                </p>
                <div className="absolute top-full right-2 border-4 border-transparent border-t-[#191c1e]" />
              </div>
            </div>
          </div>

          <button
            type="submit"
            disabled={loading}
            className="w-full py-2.5 sm:py-3 px-4 bg-[#055bb2] hover:bg-[#3374cd] text-white font-bold text-xs sm:text-sm rounded-xl shadow-md transition-all flex items-center justify-center gap-2 disabled:opacity-50 cursor-pointer active:scale-[0.99]"
          >
            {loading ? (
              <div className="w-4 h-4 border-2 border-white border-t-transparent rounded-full animate-spin" />
            ) : (
              <>
                <span>Iniciar Sesión</span>
                <ArrowRight className="w-4 h-4" />
              </>
            )}
          </button>
        </form>

        <div className="pt-3 border-t border-[#e0e3e5] text-center">
          <div className="flex items-center justify-center gap-1.5 text-[11px] sm:text-xs text-[#727783]">
            <Shield className="w-3.5 h-3.5 text-[#055bb2]" />
            <span>Conexión Cifrada de Alta Seguridad</span>
          </div>
        </div>
      </div>

      {/* Footer Corporativo con enlace a Waloyo Group perfectamente adaptado */}
      <footer className="w-full text-center text-[10px] sm:text-xs text-[#727783] relative z-10 shrink-0 pt-2 pb-1 space-y-0.5">
        <p>© {new Date().getFullYear()} Gestión Integral SGI S.A.S. Todos los derechos reservados.</p>
        <p className="text-[10px] sm:text-[11px] text-[#545f73]">
          Desarrollado por{' '}
          <a
            href="https://waloyogroup.com/"
            target="_blank"
            rel="noopener noreferrer"
            className="font-bold text-[#055bb2] hover:underline underline-offset-2 transition-colors"
          >
            Waloyo Group
          </a>{' '}
          — <span className="italic">Tecnología resiliente. Operación continua.</span>
        </p>
      </footer>
    </div>
  );
};
