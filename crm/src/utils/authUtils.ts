import { supabase } from '../config/supabaseClient';

/**
 * Cierre integral de sesión en SGI CRM:
 * 1. Purga síncrona e inmediata de credenciales en localStorage y sessionStorage.
 * 2. Invalidación en Supabase Auth en segundo plano con scope local sin colgar la UI.
 */
export const performCompleteLogout = async (): Promise<void> => {
  try {
    // 1. Limpieza síncrona e instantánea de datos de usuario en memoria y almacenamiento local
    localStorage.removeItem('sgi_user');
    sessionStorage.removeItem('sgi_user');

    // 2. Limpieza defensiva de todos los tokens de Supabase
    try {
      const storageKeys = Object.keys(localStorage);
      for (const key of storageKeys) {
        if (key.startsWith('sb-') && key.endsWith('-auth-token')) {
          localStorage.removeItem(key);
        }
      }

      const sessionKeys = Object.keys(sessionStorage);
      for (const key of sessionKeys) {
        if (key.startsWith('sb-') && key.endsWith('-auth-token')) {
          sessionStorage.removeItem(key);
        }
      }
    } catch {
      // Ignorar errores de acceso al almacenamiento
    }

    // 3. Invalidación no bloqueante en Supabase Auth aislada completamente
    try {
      setTimeout(() => {
        void supabase.auth.signOut({ scope: 'local' }).catch(() => {});
      }, 0);
    } catch {
      // Ignorado
    }
  } catch (error) {
    console.error('Error durante el cierre integral de sesión:', error);
  }
};

