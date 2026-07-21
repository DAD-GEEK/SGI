import React, { useState, useEffect } from 'react';
import { Users, Search, Plus, ShieldCheck, Mail, CheckCircle, RefreshCw } from 'lucide-react';
import { CrmSidebar } from '../components/CrmSidebar';

interface Usuario {
  id: string;
  documento: string;
  nombreCompleto: string;
  email: string;
  telefonoMovil?: string;
  rol: string;
  activo: boolean;
}

const UsuariosView: React.FC = () => {
  const [usuarios, setUsuarios] = useState<Usuario[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [searchTerm, setSearchTerm] = useState<string>('');

  const fetchUsuarios = async () => {
    setLoading(true);
    try {
      const response = await fetch('http://localhost:8084/api/usuarios');
      if (response.ok) {
        const data = await response.json();
        setUsuarios(data);
      } else {
        console.warn('API Spring Boot no lista aún usuarios.');
      }
    } catch (error) {
      console.error('Error al conectar con API Spring Boot:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsuarios();
  }, []);

  const filteredUsuarios = usuarios.filter(u =>
    u.nombreCompleto.toLowerCase().includes(searchTerm.toLowerCase()) ||
    u.documento.toLowerCase().includes(searchTerm.toLowerCase()) ||
    u.email.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="min-h-screen bg-[#F8FAFC] flex font-sans text-slate-800">
      <CrmSidebar activeTab="usuarios" />

      <main className="flex-1 overflow-y-auto p-8">
        <header className="flex flex-col md:flex-row md:items-center justify-between gap-4 mb-8">
          <div>
            <div className="flex items-center gap-2 text-sm text-slate-500 mb-1">
              <span>SGI Core</span>
              <span>/</span>
              <span className="text-[#1E3A8A] font-semibold">Equipo Técnico</span>
            </div>
            <h1 className="text-2xl font-bold text-slate-900 flex items-center gap-2">
              <Users className="w-7 h-7 text-[#1E3A8A]" />
              Asesores & Consultores (26 Migrados)
            </h1>
          </div>

          <div className="flex items-center gap-3">
            <button 
              onClick={fetchUsuarios}
              className="flex items-center gap-2 px-3.5 py-2 bg-white border border-slate-200 text-slate-700 rounded-lg hover:bg-slate-50 font-medium transition-colors shadow-sm"
            >
              <RefreshCw className={`w-4 h-4 ${loading ? 'animate-spin' : ''}`} />
              Sincronizar API
            </button>
            <button className="flex items-center gap-2 px-4 py-2 bg-[#1E3A8A] text-white rounded-lg hover:bg-[#1E3A8A]/90 font-semibold transition-colors shadow-sm">
              <Plus className="w-4 h-4" />
              Nuevo Asesor SGI
            </button>
          </div>
        </header>

        {/* Search Bar */}
        <div className="bg-white p-4 rounded-xl shadow-sm border border-slate-200/80 mb-6">
          <div className="relative w-full">
            <Search className="w-4 h-4 text-slate-400 absolute left-3.5 top-1/2 -translate-y-1/2" />
            <input
              type="text"
              placeholder="Buscar por documento, nombre de asesor o correo..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="w-full pl-10 pr-4 py-2 bg-slate-50 border border-slate-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-[#1E3A8A]/20 text-sm"
            />
          </div>
        </div>

        {/* Content Table */}
        {loading ? (
          <div className="bg-white rounded-xl border border-slate-200 p-12 text-center text-slate-500">
            <RefreshCw className="w-8 h-8 text-[#1E3A8A] animate-spin mx-auto mb-3" />
            <p className="font-medium">Cargando equipo de asesores desde Spring Boot API (localhost:8084)...</p>
          </div>
        ) : filteredUsuarios.length === 0 ? (
          <div className="bg-white rounded-xl border border-slate-200 p-12 text-center text-slate-500">
            <Users className="w-12 h-12 text-slate-300 mx-auto mb-3" />
            <p className="font-semibold text-slate-700">No se encontraron usuarios</p>
          </div>
        ) : (
          <div className="bg-white rounded-xl border border-slate-200/80 shadow-sm overflow-hidden">
            <div className="overflow-x-auto">
              <table className="w-full text-left border-collapse text-sm">
                <thead>
                  <tr className="bg-slate-50 border-b border-slate-200 text-slate-600 font-semibold uppercase text-[11px] tracking-wider">
                    <th className="py-3.5 px-4">Asesor / Nombre Completo</th>
                    <th className="py-3.5 px-4">Documento ID</th>
                    <th className="py-3.5 px-4">Rol en Sistema</th>
                    <th className="py-3.5 px-4">Contacto</th>
                    <th className="py-3.5 px-4">Estado</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {filteredUsuarios.map((u) => (
                    <tr key={u.id} className="hover:bg-slate-50/80 transition-colors">
                      <td className="py-3.5 px-4 font-semibold text-slate-900">
                        <div className="flex items-center gap-3">
                          <div className="w-9 h-9 rounded-full bg-slate-100 border border-slate-200 text-[#1E3A8A] flex items-center justify-center font-bold text-sm">
                            {u.nombreCompleto.substring(0, 2).toUpperCase()}
                          </div>
                          <div>
                            <div>{u.nombreCompleto}</div>
                            <div className="text-xs font-normal text-slate-500">{u.email}</div>
                          </div>
                        </div>
                      </td>
                      <td className="py-3.5 px-4 font-mono text-slate-700">
                        {u.documento}
                      </td>
                      <td className="py-3.5 px-4">
                        <span className="inline-flex items-center gap-1.5 px-2.5 py-1 rounded-md bg-blue-50 text-[#1E3A8A] text-xs font-semibold border border-blue-200/60">
                          <ShieldCheck className="w-3.5 h-3.5 text-[#1E3A8A]" />
                          {u.rol}
                        </span>
                      </td>
                      <td className="py-3.5 px-4 text-slate-600">
                        <div className="flex flex-col gap-0.5 text-xs">
                          <span className="flex items-center gap-1">
                            <Mail className="w-3 h-3 text-slate-400" /> {u.email}
                          </span>
                        </div>
                      </td>
                      <td className="py-3.5 px-4">
                        <span className="inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full text-xs font-medium bg-emerald-50 text-emerald-700 border border-emerald-200">
                          <CheckCircle className="w-3 h-3" />
                          Activo
                        </span>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        )}
      </main>
    </div>
  );
};

export default UsuariosView;
