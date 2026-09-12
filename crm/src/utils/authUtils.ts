import { supabase } from '../config/supabaseClient';

/**
 * Cierre integral de sesión en SGI CRM:
 * 1. Elimina datos locales de usuario (sgi_user) en localStorage y sessionStorage.
 * 2. Purga tokens de autenticación de Supabase persistidos.
 * 3. Invalida la sesión en Supabase Auth a nivel global y local.
 */
export const performCompleteLogout = async (): Promise<void> => {
  try {
    // 1. Limpieza de llaves de sesión SGI
    localStorage.removeItem('sgi_user');
    sessionStorage.removeItem('sgi_user');

    // 2. Limpieza defensiva de tokens de Supabase en el almacenamiento del navegador
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

    // 3. Invalidación formal en el servidor de Supabase Auth
    try {
      await supabase.auth.signOut({ scope: 'global' });
    } catch {
      // Si el servidor falla o hay problemas de red, invalidar localmente
      await supabase.auth.signOut({ scope: 'local' }).catch(() => {});
    }
  } catch (error) {
    console.error('Error durante el cierre integral de sesión:', error);
  }
};
