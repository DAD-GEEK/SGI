// SGI CRM & Client Portal - Enterprise Release
import React, { useEffect } from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { Login } from './pages/Login';
import { Dashboard } from './pages/Dashboard';
import { Profile } from './pages/Profile';
import { ConsultorView } from './pages/ConsultorView';
import { AgendaView } from './pages/AgendaView';
import ClientesView from './pages/ClientesView';
import UsuariosView from './pages/UsuariosView';
import { ChangePassword } from './pages/ChangePassword';
import { ProtectedRoute } from './components/ProtectedRoute';

export const App: React.FC = () => {
  useEffect(() => {
    if (import.meta.env.PROD) {
      const hostname = window.location.hostname;
      if (hostname.endsWith('.web.app') || hostname.endsWith('.firebaseapp.com')) {
        const canonicalTarget = 'https://crm.gestionintegralsgi.com.co' + window.location.pathname + window.location.search + window.location.hash;
        window.location.replace(canonicalTarget);
      }
    }
  }, []);

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Navigate to="/dashboard" replace />} />
        <Route path="/login" element={<Login />} />
        <Route
          path="/cambiar-password"
          element={
            <ProtectedRoute>
              <ChangePassword />
            </ProtectedRoute>
          }
        />
        <Route
          path="/dashboard"
          element={
            <ProtectedRoute>
              <Dashboard />
            </ProtectedRoute>
          }
        />
        <Route
          path="/consultor"
          element={
            <ProtectedRoute>
              <ConsultorView />
            </ProtectedRoute>
          }
        />
        <Route
          path="/agenda"
          element={
            <ProtectedRoute>
              <AgendaView />
            </ProtectedRoute>
          }
        />
        <Route
          path="/clientes"
          element={
            <ProtectedRoute>
              <ClientesView />
            </ProtectedRoute>
          }
        />
        <Route
          path="/usuarios"
          element={
            <ProtectedRoute>
              <UsuariosView />
            </ProtectedRoute>
          }
        />
        <Route
          path="/perfil"
          element={
            <ProtectedRoute>
              <Profile />
            </ProtectedRoute>
          }
        />
        <Route path="*" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </BrowserRouter>
  );
};
