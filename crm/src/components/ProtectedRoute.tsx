import React, { useEffect, useState, type ReactNode } from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { supabase } from '../config/supabaseClient';
import { performCompleteLogout } from '../utils/authUtils';

interface ProtectedRouteProps {
  children: ReactNode;
}

export const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children }) => {
  const location = useLocation();
  const [isChecking, setIsChecking] = useState(true);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [pendingPasswordChange, setPendingPasswordChange] = useState(false);

  useEffect(() => {
    let isMounted = true;

    const verifySession = async () => {
      try {
        const storedUserRaw = localStorage.getItem('sgi_user');
        if (!storedUserRaw) {
          if (isMounted) {
            setIsAuthenticated(false);
            setPendingPasswordChange(false);
            setIsChecking(false);
          }
          return;
        }

        const storedUser = JSON.parse(storedUserRaw);
        if (!storedUser || !storedUser.email) {
          await performCompleteLogout();
          if (isMounted) {
            setIsAuthenticated(false);
            setPendingPasswordChange(false);
            setIsChecking(false);
          }
          return;
        }

        // Validación de tiempo límite de expiración configurado
        const configuredLimit = parseFloat(localStorage.getItem('sgi_session_limit_hours') || '4');
        const MAX_SESSION_MS = Math.round(configuredLimit * 60 * 60 * 1000);

        if (storedUser.loginTimestamp && (Date.now() - storedUser.loginTimestamp >= MAX_SESSION_MS)) {
          await performCompleteLogout();
          if (isMounted) {
            setIsAuthenticated(false);
            setPendingPasswordChange(false);
            setIsChecking(false);
          }
          return;
        }

        const mustChange = Boolean(storedUser.mustChangePassword);

        // Si existe un registro local válido, confirmar estado
        if (isMounted) {
          setIsAuthenticated(true);
          setPendingPasswordChange(mustChange);
          setIsChecking(false);
        }
      } catch (err) {
        console.error('Error al validar sesión en ProtectedRoute:', err);
        await performCompleteLogout();
        if (isMounted) {
          setIsAuthenticated(false);
          setPendingPasswordChange(false);
          setIsChecking(false);
        }
      }
    };

    void verifySession();

    // Suscripción reactiva a eventos de Supabase Auth
    const { data: authListener } = supabase.auth.onAuthStateChange((event) => {
      if (event === 'SIGNED_OUT') {
        void performCompleteLogout();
        if (isMounted) {
          setIsAuthenticated(false);
          setIsChecking(false);
        }
      }
    });

    // Escudo contra Back-Forward Cache (bfcache) del navegador
    const handlePageShow = (event: PageTransitionEvent) => {
      if (event.persisted) {
        void verifySession();
      }
    };
    window.addEventListener('pageshow', handlePageShow);

    return () => {
      isMounted = false;
      authListener?.subscription?.unsubscribe();
      window.removeEventListener('pageshow', handlePageShow);
    };
  }, [location.pathname]);

  if (isChecking) {
    return (
      <div className="min-h-screen bg-[#f7f9fb] flex items-center justify-center">
        <div className="w-8 h-8 border-3 border-[#055bb2] border-t-transparent rounded-full animate-spin" />
      </div>
    );
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" replace state={{ from: location }} />;
  }

  // Si tiene cambio de clave pendiente y trata de acceder a otra ruta, forzar a /cambiar-password
  if (pendingPasswordChange && location.pathname !== '/cambiar-password') {
    return <Navigate to="/cambiar-password" replace />;
  }

  // Si NO tiene cambio de clave pendiente y se encuentra en /cambiar-password, redirigir al Dashboard
  if (!pendingPasswordChange && location.pathname === '/cambiar-password') {
    return <Navigate to="/dashboard" replace />;
  }

  return <>{children}</>;
};
