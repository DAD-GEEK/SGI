import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { ShieldCheck, Lock, ArrowRight, CheckCircle2, AlertCircle, Eye, EyeOff, Check, X } from 'lucide-react';
import { supabase } from '../config/supabaseClient';
import { API_BASE_URL } from '../config/apiConfig';

export const ChangePassword: React.FC = () => {
  const navigate = useNavigate();
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [showNewPassword, setShowNewPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);

  // Evaluador de Fortaleza de Contraseña OWASP
  const hasMinLength = newPassword.length >= 8;
  const hasUpperCase = /[A-Z]/.test(newPassword);
  const hasLowerCase = /[a-z]/.test(newPassword);
  const hasNumber = /[0-9]/.test(newPassword);
  const hasSpecial = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(newPassword);

  const passedRulesCount = [hasMinLength, hasUpperCase && hasLowerCase, hasNumber, hasSpecial].filter(Boolean).length;

  const getStrengthLabel = () => {
    if (newPassword.length === 0) return { label: 'Sin ingresar', color: 'bg-slate-200', text: 'text-slate-400', percent: '0%' };
    if (passedRulesCount <= 1) return { label: 'Débil', color: 'bg-red-500', text: 'text-red-600', percent: '25%' };
    if (passedRulesCount === 2) return { label: 'Aceptable', color: 'bg-amber-500', text: 'text-amber-600', percent: '50%' };
    if (passedRulesCount === 3) return { label: 'Fuerte', color: 'bg-blue-600', text: 'text-blue-600', percent: '75%' };
    return { label: 'Excelente / Blindada', color: 'bg-emerald-600', text: 'text-emerald-600', percent: '100%' };
  };

  const strength = getStrengthLabel();

  const handlePreventCopyPaste = (e: React.ClipboardEvent) => {
    e.preventDefault();
    setError('Por seguridad de la plataforma, no está permitido copiar ni pegar en la confirmación. Escriba la clave manualmente.');
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!hasMinLength) {
      setError('La contraseña debe contener al menos 8 caracteres.');
      return;
    }

    if (passedRulesCount < 3) {
      setError('Por seguridad, la contraseña debe incluir letras mayúsculas/minúsculas, números y un carácter especial.');
      return;
    }

    if (newPassword !== confirmPassword) {
      setError('Las contraseñas no coinciden. Por favor verifique ambas entradas.');
      return;
    }

    setLoading(true);
    try {
      const storedUserRaw = localStorage.getItem('sgi_user');
      const storedUser = storedUserRaw ? JSON.parse(storedUserRaw) : {};
      const activeEmail = storedUser.email || (await supabase.auth.getUser()).data.user?.email;

      // 1. Intentar actualizar contraseña en Supabase Auth si hay sesión activa de Supabase
      try {
        await supabase.auth.updateUser({ password: newPassword });
      } catch (authErr) {
        console.warn('Supabase Auth session update fallback:', authErr);
      }

      // 2. Notificar obligatoriamente al backend Spring Boot PostgreSQL que la clave fue confirmada
      if (activeEmail) {
        await fetch(`${API_BASE_URL}/usuarios/confirmar-clave`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ email: activeEmail })
        });
      }

      // 3. Actualizar estado local y redirigir
      storedUser.mustChangePassword = false;
      localStorage.setItem('sgi_user', JSON.stringify(storedUser));

      setSuccess(true);
      setTimeout(() => {
        navigate('/dashboard');
      }, 1500);

    } catch (err: any) {
      setError(err.message || 'Error al actualizar la contraseña en el servidor.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-[#f7f9fb] flex flex-col justify-center items-center px-4 py-12 relative overflow-hidden font-sans">
      <div className="absolute inset-0 bg-gradient-to-br from-[#055bb2]/5 via-transparent to-[#3c475a]/5 pointer-events-none" />

      <div className="w-full max-w-md bg-white rounded-2xl p-8 md:p-10 shadow-xl border border-[#c2c6d4]/40 relative z-10 space-y-6">
        <div className="text-center space-y-3">
          <div className="flex justify-center mb-3">
            <div className="w-14 h-14 bg-[#055bb2]/10 rounded-2xl flex items-center justify-center text-[#055bb2]">
              <ShieldCheck className="w-8 h-8" />
            </div>
          </div>
          <h1 className="text-2xl font-bold font-headline text-[#191c1e]">
            Cambio de Contraseña Obligatorio
          </h1>
          <p className="text-xs text-[#545f73] leading-relaxed">
            Ha ingresado con una clave temporal. Por seguridad de la plataforma SGI, asigné su contraseña definitiva.
          </p>
        </div>

        {error && (
          <div className="p-3.5 bg-red-50 border border-red-200 text-red-700 rounded-xl text-xs flex items-center gap-3 animate-in fade-in duration-200">
            <AlertCircle className="w-5 h-5 shrink-0 text-red-600" />
            <span className="font-semibold">{error}</span>
          </div>
        )}

        {success && (
          <div className="p-3.5 bg-emerald-50 border border-emerald-200 text-emerald-800 rounded-xl text-xs flex items-center gap-3 animate-in fade-in duration-200">
            <CheckCircle2 className="w-5 h-5 shrink-0 text-emerald-600" />
            <span className="font-semibold">Contraseña actualizada exitosamente. Redirigiendo al Dashboard...</span>
          </div>
        )}

        <form onSubmit={handleSubmit} noValidate className="space-y-5">
          {/* Nueva Contraseña */}
          <div className="space-y-1.5">
            <label className="block text-xs font-semibold text-[#424752]">
              Nueva Contraseña Definitiva *
            </label>
            <div className="relative">
              <Lock className="w-5 h-5 text-[#727783] absolute left-3.5 top-1/2 -translate-y-1/2" />
              <input
                type={showNewPassword ? 'text' : 'password'}
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
                placeholder="Ingrese clave segura"
                autoComplete="new-password"
                className="w-full pl-11 pr-11 py-3 rounded-xl border border-[#c2c6d4] text-sm focus:outline-none focus:border-[#055bb2] focus:ring-2 focus:ring-[#055bb2]/20 transition-all bg-[#f8fafc]"
              />
              <button
                type="button"
                onClick={() => setShowNewPassword(!showNewPassword)}
                className="absolute right-3.5 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600 cursor-pointer p-1"
                title={showNewPassword ? 'Ocultar contraseña' : 'Ver contraseña'}
              >
                {showNewPassword ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
              </button>
            </div>

            {/* Medidor de Fortaleza Visual */}
            {newPassword.length > 0 && (
              <div className="space-y-2 pt-1 animate-in fade-in duration-200">
                <div className="flex justify-between items-center text-[11px] font-bold">
                  <span className="text-slate-500">Nivel de Fortaleza:</span>
                  <span className={strength.text}>{strength.label}</span>
                </div>
                <div className="w-full h-1.5 bg-slate-100 rounded-full overflow-hidden">
                  <div
                    className={`h-full transition-all duration-300 ${strength.color}`}
                    style={{ width: strength.percent }}
                  />
                </div>

                {/* Lista de Requisitos de Seguridad */}
                <div className="grid grid-cols-2 gap-1.5 text-[11px] pt-1 font-medium text-slate-600">
                  <div className={`flex items-center gap-1 ${hasMinLength ? 'text-emerald-700 font-bold' : 'text-slate-400'}`}>
                    {hasMinLength ? <Check className="w-3 h-3 text-emerald-600" /> : <X className="w-3 h-3" />}
                    <span>Mínimo 8 caracteres</span>
                  </div>
                  <div className={`flex items-center gap-1 ${hasUpperCase && hasLowerCase ? 'text-emerald-700 font-bold' : 'text-slate-400'}`}>
                    {hasUpperCase && hasLowerCase ? <Check className="w-3 h-3 text-emerald-600" /> : <X className="w-3 h-3" />}
                    <span>Mayúsculas y minúsculas</span>
                  </div>
                  <div className={`flex items-center gap-1 ${hasNumber ? 'text-emerald-700 font-bold' : 'text-slate-400'}`}>
                    {hasNumber ? <Check className="w-3 h-3 text-emerald-600" /> : <X className="w-3 h-3" />}
                    <span>Al menos 1 número</span>
                  </div>
                  <div className={`flex items-center gap-1 ${hasSpecial ? 'text-emerald-700 font-bold' : 'text-slate-400'}`}>
                    {hasSpecial ? <Check className="w-3 h-3 text-emerald-600" /> : <X className="w-3 h-3" />}
                    <span>Símbolo (!@#$...)</span>
                  </div>
                </div>
              </div>
            )}
          </div>

          {/* Confirmar Nueva Contraseña con Bloqueo de Copy-Paste */}
          <div className="space-y-1.5">
            <div className="flex justify-between items-center">
              <label className="block text-xs font-semibold text-[#424752]">
                Confirmar Nueva Contraseña *
              </label>
              <span className="text-[10px] text-slate-400 font-medium">(Solo escritura manual)</span>
            </div>
            <div className="relative">
              <Lock className="w-5 h-5 text-[#727783] absolute left-3.5 top-1/2 -translate-y-1/2" />
              <input
                type={showConfirmPassword ? 'text' : 'password'}
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                onCopy={handlePreventCopyPaste}
                onPaste={handlePreventCopyPaste}
                onCut={handlePreventCopyPaste}
                onContextMenu={(e) => e.preventDefault()}
                placeholder="Repita exactamente su contraseña"
                autoComplete="off"
                className="w-full pl-11 pr-11 py-3 rounded-xl border border-[#c2c6d4] text-sm focus:outline-none focus:border-[#055bb2] focus:ring-2 focus:ring-[#055bb2]/20 transition-all bg-[#f8fafc]"
              />
              <button
                type="button"
                onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                className="absolute right-3.5 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600 cursor-pointer p-1"
                title={showConfirmPassword ? 'Ocultar contraseña' : 'Ver contraseña'}
              >
                {showConfirmPassword ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
              </button>
            </div>
          </div>

          <button
            type="submit"
            disabled={loading || success}
            className="w-full bg-[#055bb2] text-white py-3.5 rounded-xl font-bold text-sm hover:bg-[#3374cd] transition-all shadow-md flex items-center justify-center gap-2 active:scale-[0.99] disabled:opacity-50 cursor-pointer"
          >
            {loading ? (
              <div className="w-5 h-5 border-2 border-white border-t-transparent rounded-full animate-spin" />
            ) : (
              <>
                <span>Establecer Contraseña Definitiva</span>
                <ArrowRight className="w-4 h-4" />
              </>
            )}
          </button>
        </form>
      </div>
    </div>
  );
};
