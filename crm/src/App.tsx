import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { Login } from './pages/Login';
import { Dashboard } from './pages/Dashboard';
import { Profile } from './pages/Profile';
import { ConsultorView } from './pages/ConsultorView';
import { AgendaView } from './pages/AgendaView';
import ClientesView from './pages/ClientesView';
import UsuariosView from './pages/UsuariosView';

export const App: React.FC = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Navigate to="/dashboard" replace />} />
        <Route path="/login" element={<Login />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/consultor" element={<ConsultorView />} />
        <Route path="/agenda" element={<AgendaView />} />
        <Route path="/clientes" element={<ClientesView />} />
        <Route path="/usuarios" element={<UsuariosView />} />
        <Route path="/perfil" element={<Profile />} />
        <Route path="*" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </BrowserRouter>
  );
};
