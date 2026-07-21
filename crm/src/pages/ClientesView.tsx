import React, { useState, useEffect } from 'react';
import { Building2, Search, Plus, Mail, Phone, MapPin, CheckCircle, Edit, Trash2, X, ChevronLeft, ChevronRight, Zap, FileText, Users, Hash, Calendar, Globe, UserCheck } from 'lucide-react';
import { CrmSidebar } from '../components/CrmSidebar';

interface Cliente {
  id: string;
  nit: string;
  razonSocial: string;
  nombreComercial?: string;
  direccion?: string;
  telefono?: string;
  emailContacto?: string;
  personaContacto?: string;
  ciudad?: string;
  paginaWeb?: string;
  fechaIngreso?: string;
  activo: boolean;
}

interface Contrato {
  id?: string;
  clienteId?: string;
  numeroContrato: number;
  fechaInicio?: string;
  fechaFin?: string;
  horas: number;
  valor: number;
  asesores?: string;
  sistemas?: string;
  emailNotificacion?: string;
  activo?: boolean;
}

interface Contacto {
  id?: string;
  clienteId?: string;
  nombre: string;
  cargo: string;
  telefono?: string;
  celular?: string;
  email: string;
}

// Lista de Principales Ciudades DANE de Colombia
const CIUDADES_COLOMBIA = [
  '11001 - BOGOTÁ, D.C.',
  '05001 - MEDELLÍN',
  '08001 - BARRANQUILLA',
  '76001 - CALI',
  '68001 - BUCARAMANGA',
  '13001 - CARTAGENA',
  '17001 - MANIZALES',
  '66001 - PEREIRA',
  '63001 - ARMENIA',
  '54001 - CÚCUTA',
  '41001 - NEIVA',
  '73001 - IBAGUÉ',
  '50001 - VILLAVICENCIO',
  '52001 - PASTO',
  '23001 - MONTERÍA',
  '47001 - SANTA MARTA'
];

const ASESORES_SGI = [
  'María Elisa Arias',
  'Laura Natali Tenorio Arias',
  'Lesly Velez Alvarez',
  'Milena Valencia'
];

const SISTEMAS_SGI = [
  'SG-SST (Seguridad y Salud)',
  'SGC (ISO 9001)',
  'SGI (Sistema Integrado)',
  'PESV (Plan Estratégico de Seguridad Vial)'
];

// Helper to calculate DIAN verification digit (DV) for Colombia NITs
const calcularDV = (nit: string): string => {
  const cleanNit = nit.replace(/\D/g, '');
  if (!cleanNit || cleanNit.length === 0) return '';
  
  const vpri = [3, 7, 13, 17, 19, 23, 29, 37, 41, 43, 47, 53, 59, 67, 71];
  let z = 0;
  const len = cleanNit.length;
  
  for (let i = 0; i < len; i++) {
    z += parseInt(cleanNit.charAt(len - 1 - i), 10) * vpri[i];
  }
  
  const y = z % 11;
  return y > 1 ? (11 - y).toString() : y.toString();
};

// Format NIT with standard thousand points
const formatNit = (nit: string): string => {
  if (!nit) return '';
  const digits = nit.replace(/\D/g, '');
  if (digits.length >= 8) {
    return digits.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
  }
  return nit;
};

// Format COP currency
const formatCOP = (val: number): string => {
  if (!val) return '$0 COP';
  return '$ ' + val.toLocaleString('es-CO') + ' COP';
};

const ClientesView: React.FC = () => {
  const [clientes, setClientes] = useState<Cliente[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [isLive, setIsLive] = useState<boolean>(false);
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [filterActivo, setFilterActivo] = useState<string>('todos');

  // Pagination states (Default 5 items per page)
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [itemsPerPage, setItemsPerPage] = useState<number>(5);

  // Modals state & Active Tab inside Modal (general | contratos | contactos)
  const [showCreateModal, setShowCreateModal] = useState<boolean>(false);
  const [showEditModal, setShowEditModal] = useState<boolean>(false);
  const [modalTab, setModalTab] = useState<'general' | 'contratos' | 'contactos'>('general');
  const [selectedCliente, setSelectedCliente] = useState<Cliente | null>(null);

  // Real Contracts & Contacts from Spring Boot
  const [contratosCliente, setContratosCliente] = useState<Contrato[]>([]);
  const [contactosCliente, setContactosCliente] = useState<Contacto[]>([]);
  const [loadingContratos, setLoadingContratos] = useState<boolean>(false);
  const [loadingContactos, setLoadingContactos] = useState<boolean>(false);

  // Sub-forms for adding new contracts and contacts inside modal (Identical to legacy modal!)
  const [newContrato, setNewContrato] = useState<Contrato>({
    numeroContrato: 1,
    fechaInicio: '2026-01-01',
    fechaFin: '2026-12-31',
    horas: 16,
    valor: 1200000,
    asesores: 'María Elisa Arias',
    sistemas: 'SG-SST (Seguridad y Salud)',
    emailNotificacion: ''
  });

  const [newContacto, setNewContacto] = useState<Contacto>({
    nombre: '',
    cargo: '',
    telefono: '',
    celular: '',
    email: ''
  });

  // Form states (100% legacy parity fields)
  const [formData, setFormData] = useState({
    nit: '',
    dv: '',
    razonSocial: '',
    nombreComercial: '',
    direccion: '',
    telefono: '',
    emailContacto: '',
    ciudad: '11001 - BOGOTÁ, D.C.',
    paginaWeb: '',
    fechaIngreso: new Date().toISOString().split('T')[0],
    activo: true
  });

  // Initial Fetch & Reactive SSE Stream Connection
  useEffect(() => {
    const fetchInitialData = async () => {
      try {
        const response = await fetch('http://localhost:8084/api/clientes');
        if (response.ok) {
          const data = await response.json();
          setClientes(data);
        }
      } catch (error) {
        console.error('Error al conectar con API Spring Boot:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchInitialData();

    let eventSource: EventSource | null = null;
    try {
      eventSource = new EventSource('http://localhost:8084/api/clientes/stream');

      eventSource.addEventListener('clientes-update', (event: MessageEvent) => {
        try {
          const updatedClientes = JSON.parse(event.data);
          setClientes(updatedClientes);
          setIsLive(true);
        } catch (err) {
          console.error('Error al procesar flujo SSE:', err);
        }
      });

      eventSource.onopen = () => setIsLive(true);
      eventSource.onerror = () => setIsLive(false);
    } catch (e) {
      console.warn('Conexión reactiva SSE no disponible, usando modo estático:', e);
    }

    return () => {
      if (eventSource) {
        eventSource.close();
      }
    };
  }, []);

  // Fetch real contracts for a client
  const fetchContratosPorCliente = async (clienteId: string) => {
    setLoadingContratos(true);
    try {
      const res = await fetch(`http://localhost:8084/api/contratos/cliente/${clienteId}`);
      if (res.ok) {
        const data = await res.json();
        setContratosCliente(data);
      }
    } catch (e) {
      console.error('Error al cargar contratos:', e);
    } finally {
      setLoadingContratos(false);
    }
  };

  // Fetch real contacts for a client
  const fetchContactosPorCliente = async (clienteId: string) => {
    setLoadingContactos(true);
    try {
      const res = await fetch(`http://localhost:8084/api/contactos/cliente/${clienteId}`);
      if (res.ok) {
        const data = await res.json();
        setContactosCliente(data);
      }
    } catch (e) {
      console.error('Error al cargar contactos:', e);
    } finally {
      setLoadingContactos(false);
    }
  };

  // Recalculate DV on NIT change
  const handleNitChange = (val: string) => {
    const dvCalc = calcularDV(val);
    setFormData(prev => ({ ...prev, nit: val, dv: dvCalc }));
  };

  // Filter logic
  const filteredClientes = clientes.filter(c => {
    const matchesSearch = c.razonSocial.toLowerCase().includes(searchTerm.toLowerCase()) ||
                          c.nit.toLowerCase().includes(searchTerm.toLowerCase()) ||
                          (c.emailContacto && c.emailContacto.toLowerCase().includes(searchTerm.toLowerCase()));
    
    if (filterActivo === 'activos') return matchesSearch && c.activo;
    if (filterActivo === 'inactivos') return matchesSearch && !c.activo;
    return matchesSearch;
  });

  // Pagination calculation
  const totalItems = filteredClientes.length;
  const totalPages = Math.ceil(totalItems / itemsPerPage) || 1;
  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = filteredClientes.slice(indexOfFirstItem, indexOfLastItem);

  // Handlers
  const handlePageChange = (newPage: number) => {
    if (newPage >= 1 && newPage <= totalPages) {
      setCurrentPage(newPage);
    }
  };

  const handleOpenCreate = () => {
    setModalTab('general');
    setContratosCliente([]);
    setContactosCliente([]);
    setFormData({
      nit: '',
      dv: '',
      razonSocial: '',
      nombreComercial: '',
      direccion: '',
      telefono: '',
      emailContacto: '',
      ciudad: '11001 - BOGOTÁ, D.C.',
      paginaWeb: '',
      fechaIngreso: new Date().toISOString().split('T')[0],
      activo: true
    });
    setShowCreateModal(true);
  };

  const handleOpenEdit = (c: Cliente) => {
    setSelectedCliente(c);
    setModalTab('general');
    fetchContratosPorCliente(c.id);
    fetchContactosPorCliente(c.id);
    setFormData({
      nit: c.nit,
      dv: calcularDV(c.nit),
      razonSocial: c.razonSocial,
      nombreComercial: c.nombreComercial || '',
      direccion: c.direccion || '',
      telefono: c.telefono || '',
      emailContacto: c.emailContacto || '',
      ciudad: c.ciudad || '11001 - BOGOTÁ, D.C.',
      paginaWeb: c.paginaWeb || '',
      fechaIngreso: c.fechaIngreso || new Date().toISOString().split('T')[0],
      activo: c.activo
    });
    setShowEditModal(true);
  };

  // Save new contract for client
  const handleAddContratoSubmit = async (e: React.FormEvent, clienteId?: string) => {
    e.preventDefault();
    const targetId = clienteId || selectedCliente?.id;
    if (!targetId) return;

    try {
      const res = await fetch('http://localhost:8084/api/contratos', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ ...newContrato, clienteId: targetId })
      });
      if (res.ok) {
        fetchContratosPorCliente(targetId);
        setNewContrato({ numeroContrato: contratosCliente.length + 2, fechaInicio: '2026-01-01', fechaFin: '2026-12-31', horas: 16, valor: 1200000, asesores: 'María Elisa Arias', sistemas: 'SG-SST (Seguridad y Salud)', emailNotificacion: '' });
      }
    } catch (err) {
      console.error('Error al guardar contrato:', err);
    }
  };

  // Save new contact for client
  const handleAddContactoSubmit = async (e: React.FormEvent, clienteId?: string) => {
    e.preventDefault();
    const targetId = clienteId || selectedCliente?.id;
    if (!targetId) return;

    try {
      const res = await fetch('http://localhost:8084/api/contactos', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ ...newContacto, clienteId: targetId })
      });
      if (res.ok) {
        fetchContactosPorCliente(targetId);
        setNewContacto({ nombre: '', cargo: '', telefono: '', celular: '', email: '' });
      }
    } catch (err) {
      console.error('Error al guardar contacto:', err);
    }
  };

  const handleCreateSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const res = await fetch('http://localhost:8084/api/clientes', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData)
      });
      if (res.ok) {
        const createdClient = await res.json();
        
        for (const ctr of contratosCliente) {
          await fetch('http://localhost:8084/api/contratos', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ ...ctr, clienteId: createdClient.id })
          });
        }
        for (const cnt of contactosCliente) {
          await fetch('http://localhost:8084/api/contactos', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ ...cnt, clienteId: createdClient.id })
          });
        }

        setShowCreateModal(false);
      }
    } catch (err) {
      console.error('Error al crear cliente:', err);
    }
  };

  const handleEditSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedCliente) return;
    try {
      const res = await fetch(`http://localhost:8084/api/clientes/${selectedCliente.id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData)
      });
      if (res.ok) {
        setShowEditModal(false);
      }
    } catch (err) {
      console.error('Error al actualizar cliente:', err);
    }
  };

  const handleDelete = async (id: string, nombre: string) => {
    if (window.confirm(`¿Está seguro de cambiar el estado a INACTIVO para el cliente ${nombre}?`)) {
      try {
        await fetch(`http://localhost:8084/api/clientes/${id}`, { method: 'DELETE' });
      } catch (err) {
        console.error('Error al desactivar cliente:', err);
      }
    }
  };

  return (
    <div className="min-h-screen bg-[#f7f9fb] flex font-sans text-[#191c1e]">
      <CrmSidebar activeTab="clientes" />

      <main className="flex-1 overflow-y-auto p-6 md:p-10 w-full transition-all duration-300 ease-in-out">
        {/* Header Section (Stitch SGI Design Template System) */}
        <header className="flex flex-col md:flex-row md:items-center justify-between gap-4 mb-8">
          <div>
            <div className="flex items-center gap-2 text-xs font-semibold text-[#545f73] uppercase tracking-wider mb-1">
              <span>SGI Unified Core</span>
              <span>/</span>
              <span className="text-[#055bb2]">Directorio B2B</span>
            </div>
            <div className="flex items-center gap-3">
              <h1 className="text-3xl font-extrabold font-headline text-[#191c1e] tracking-tight">
                Clientes
              </h1>
              <span className={`inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-bold ${
                isLive ? 'bg-[#d1fae5] text-[#065f46] border border-emerald-200' : 'bg-amber-50 text-amber-800 border border-amber-200'
              }`}>
                <Zap className={`w-3.5 h-3.5 ${isLive ? 'text-[#059669] animate-pulse' : 'text-amber-600'}`} />
                {isLive ? 'Tiempo Real Reactivo (WebFlux Live Push)' : 'Sincronizado'}
              </span>
            </div>
            <p className="text-xs text-[#545f73] mt-1">
              Gestión de cartera, auditorías normadas y estado de cuentas de clientes.
            </p>
          </div>

          <div className="flex items-center gap-3">
            <button 
              onClick={handleOpenCreate}
              className="flex items-center gap-2 px-5 py-2.5 bg-[#055bb2] hover:bg-[#3374cd] text-white rounded-xl font-bold text-xs transition-all shadow-md cursor-pointer"
            >
              <Plus className="w-4 h-4" />
              Agregar Cliente
            </button>
          </div>
        </header>

        {/* Bento-style Advanced Filter Bar */}
        <div className="bg-white rounded-2xl p-5 shadow-[0_4px_12px_rgba(30,41,59,0.05)] border border-[#e0e3e5] mb-8 grid grid-cols-1 md:grid-cols-4 gap-4 items-end">
          <div className="md:col-span-2">
            <label className="block text-xs font-bold text-[#545f73] mb-1.5 uppercase tracking-wider">Buscar Cliente</label>
            <div className="relative">
              <Search className="w-4 h-4 text-[#727783] absolute left-3.5 top-1/2 -translate-y-1/2" />
              <input
                type="text"
                placeholder="Nombre, Identificación (NIT) o Email..."
                value={searchTerm}
                onChange={(e) => { setSearchTerm(e.target.value); setCurrentPage(1); }}
                className="w-full pl-10 pr-4 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl text-xs font-medium focus:border-[#055bb2] focus:ring-1 focus:ring-[#055bb2] focus:outline-none transition-shadow"
              />
            </div>
          </div>

          <div>
            <label className="block text-xs font-bold text-[#545f73] mb-1.5 uppercase tracking-wider">Estado</label>
            <select
              value={filterActivo}
              onChange={(e) => { setFilterActivo(e.target.value); setCurrentPage(1); }}
              className="w-full px-3.5 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl text-xs font-bold text-[#191c1e] focus:border-[#055bb2] focus:ring-1 focus:ring-[#055bb2] focus:outline-none cursor-pointer"
            >
              <option value="todos">Todos los Estados ({clientes.length})</option>
              <option value="activos">Solo Activos (47)</option>
              <option value="inactivos">Solo Inactivos (20)</option>
            </select>
          </div>

          <div>
            <label className="block text-xs font-bold text-[#545f73] mb-1.5 uppercase tracking-wider">Registros por Vista</label>
            <select
              value={itemsPerPage}
              onChange={(e) => { setItemsPerPage(Number(e.target.value)); setCurrentPage(1); }}
              className="w-full px-3.5 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl text-xs font-bold text-[#191c1e] focus:border-[#055bb2] focus:ring-1 focus:ring-[#055bb2] focus:outline-none cursor-pointer"
            >
              <option value={5}>5 registros por página</option>
              <option value={10}>10 registros por página</option>
              <option value={20}>20 registros por página</option>
              <option value={50}>50 registros por página</option>
              <option value={100}>100 registros por página</option>
            </select>
          </div>
        </div>

        {/* Data Table Card (Stitch SGI Design Template System) */}
        {loading ? (
          <div className="bg-white rounded-2xl border border-[#e0e3e5] p-12 text-center text-[#545f73] elevation-1">
            <div className="w-8 h-8 border-4 border-[#055bb2] border-t-transparent rounded-full animate-spin mx-auto mb-3" />
            <p className="font-bold text-xs">Cargando empresas clientes reales...</p>
          </div>
        ) : filteredClientes.length === 0 ? (
          <div className="bg-white rounded-2xl border border-[#e0e3e5] p-12 text-center text-[#545f73] elevation-1">
            <Building2 className="w-12 h-12 text-[#c2c6d4] mx-auto mb-3" />
            <p className="font-bold text-sm text-[#191c1e]">No se encontraron clientes</p>
          </div>
        ) : (
          <div className="bg-white rounded-2xl border border-[#e0e3e5] shadow-[0_4px_12px_rgba(30,41,59,0.05)] overflow-hidden flex flex-col">
            <div className="overflow-x-auto">
              <table className="w-full text-left border-collapse text-xs">
                <thead>
                  <tr className="bg-[#f2f4f6] border-b border-[#c2c6d4]/50 text-[#545f73] font-bold uppercase text-[11px] tracking-wider">
                    <th className="py-4 px-6">Identificación (NIT)</th>
                    <th className="py-4 px-6">Empresa / Razón Social</th>
                    <th className="py-4 px-6">Ubicación & Contacto</th>
                    <th className="py-4 px-6">Teléfono / Email</th>
                    <th className="py-4 px-6">Estado</th>
                    <th className="py-4 px-6 text-center">Acciones</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-[#e0e3e5]">
                  {currentItems.map((c) => (
                    <tr key={c.id} className="hover:bg-[#f7f9fb] transition-colors group">
                      <td className="py-4 px-6 font-mono text-[#191c1e] font-extrabold text-sm">
                        {formatNit(c.nit)}
                      </td>
                      <td className="py-4 px-6 font-bold text-[#191c1e]">
                        <div className="flex items-center gap-3">
                          <div className="w-9 h-9 rounded-xl bg-[#055bb2]/10 text-[#055bb2] flex items-center justify-center font-bold text-xs border border-[#055bb2]/20 shrink-0">
                            {c.razonSocial.substring(0, 2).toUpperCase()}
                          </div>
                          <div>
                            <div className="font-headline font-bold text-sm">{c.razonSocial}</div>
                            {c.nombreComercial && <div className="text-[11px] font-normal text-[#545f73]">{c.nombreComercial}</div>}
                          </div>
                        </div>
                      </td>
                      <td className="py-4 px-6 text-[#545f73]">
                        <div className="flex items-center gap-1.5">
                          <MapPin className="w-3.5 h-3.5 text-[#727783] shrink-0" />
                          <span className="truncate max-w-[200px]">{c.direccion || 'Bogotá, D.C.'}</span>
                        </div>
                      </td>
                      <td className="py-4 px-6 text-[#545f73]">
                        <div className="flex flex-col gap-1 text-[11px]">
                          {c.telefono && (
                            <span className="flex items-center gap-1.5">
                              <Phone className="w-3 h-3 text-[#727783]" /> {c.telefono}
                            </span>
                          )}
                          {c.emailContacto && (
                            <span className="flex items-center gap-1.5 text-[#055bb2] font-semibold">
                              <Mail className="w-3 h-3 text-[#727783]" /> {c.emailContacto}
                            </span>
                          )}
                        </div>
                      </td>
                      <td className="py-4 px-6">
                        <span className={`inline-flex items-center gap-1 px-3 py-1 rounded-full text-[10px] font-extrabold tracking-wider uppercase ${
                          c.activo ? 'bg-[#d1fae5] text-[#065f46]' : 'bg-[#fee2e2] text-[#991b1b]'
                        }`}>
                          <CheckCircle className="w-3 h-3" />
                          {c.activo ? 'ACTIVO' : 'INACTIVO'}
                        </span>
                      </td>
                      <td className="py-4 px-6 text-center">
                        <div className="flex items-center justify-center gap-2">
                          <button
                            onClick={() => handleOpenEdit(c)}
                            title="Editar Cliente, Contratos & Contactos"
                            className="p-2 text-[#727783] hover:text-[#055bb2] hover:bg-[#d6e3ff]/50 rounded-xl transition-colors cursor-pointer"
                          >
                            <Edit className="w-4 h-4" />
                          </button>
                          <button
                            onClick={() => handleDelete(c.id, c.razonSocial)}
                            title="Eliminar / Desactivar"
                            className="p-2 text-[#727783] hover:text-[#ba1a1a] hover:bg-[#ffdad6]/50 rounded-xl transition-colors cursor-pointer"
                          >
                            <Trash2 className="w-4 h-4" />
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            {/* Pagination Controls Footer */}
            <div className="p-4 bg-[#f2f4f6] border-t border-[#c2c6d4]/50 text-xs text-[#545f73] flex flex-col sm:flex-row justify-between items-center gap-4 font-semibold">
              <div>
                Mostrando <span className="font-extrabold text-[#191c1e]">{indexOfFirstItem + 1}</span> a <span className="font-extrabold text-[#191c1e]">{Math.min(indexOfLastItem, totalItems)}</span> de <span className="font-extrabold text-[#191c1e]">{totalItems}</span> clientes
              </div>

              <div className="flex items-center gap-2">
                <button
                  onClick={() => handlePageChange(currentPage - 1)}
                  disabled={currentPage === 1}
                  className="flex items-center gap-1 px-3.5 py-1.5 rounded-xl border border-[#c2c6d4] bg-white hover:bg-[#eceef0] disabled:opacity-40 disabled:cursor-not-allowed font-bold transition-colors cursor-pointer"
                >
                  <ChevronLeft className="w-4 h-4" /> Anterior
                </button>

                <span className="px-3.5 py-1.5 font-extrabold text-[#191c1e]">
                  Página {currentPage} de {totalPages}
                </span>

                <button
                  onClick={() => handlePageChange(currentPage + 1)}
                  disabled={currentPage === totalPages}
                  className="flex items-center gap-1 px-3.5 py-1.5 rounded-xl border border-[#c2c6d4] bg-white hover:bg-[#eceef0] disabled:opacity-40 disabled:cursor-not-allowed font-bold transition-colors cursor-pointer"
                >
                  Siguiente <ChevronRight className="w-4 h-4" />
                </button>
              </div>
            </div>
          </div>
        )}
      </main>

      {/* Modal Agregar Cliente (100% Paridad Fiel con la Vista _CrearCliente.cshtml Legada) */}
      {showCreateModal && (
        <div className="fixed inset-0 bg-[#0b1c30]/50 backdrop-blur-xs flex items-center justify-center p-4 z-50">
          <div className="bg-white rounded-2xl p-6 w-full max-w-4xl shadow-2xl border border-[#e0e3e5] space-y-5 max-h-[90vh] overflow-y-auto">
            <div className="flex justify-between items-center border-b border-[#e0e3e5] pb-3">
              <h3 className="font-bold text-lg font-headline text-[#191c1e] flex items-center gap-2">
                <Building2 className="w-5 h-5 text-[#055bb2]" />
                Agregar cliente
              </h3>
              <button onClick={() => setShowCreateModal(false)} className="text-[#727783] hover:text-[#191c1e] cursor-pointer">
                <X className="w-5 h-5" />
              </button>
            </div>

            {/* Pestañas Modal Agregar (Información general | Contratos | Contactos) */}
            <div className="flex border-b border-[#e0e3e5]">
              <button
                type="button"
                onClick={() => setModalTab('general')}
                className={`px-4 py-2.5 text-xs font-bold flex items-center gap-2 border-b-2 transition-colors cursor-pointer ${
                  modalTab === 'general' ? 'border-[#055bb2] text-[#055bb2]' : 'border-transparent text-[#545f73] hover:text-[#191c1e]'
                }`}
              >
                <FileText className="w-4 h-4" /> Información general
              </button>
              <button
                type="button"
                onClick={() => setModalTab('contratos')}
                className={`px-4 py-2.5 text-xs font-bold flex items-center gap-2 border-b-2 transition-colors cursor-pointer ${
                  modalTab === 'contratos' ? 'border-[#055bb2] text-[#055bb2]' : 'border-transparent text-[#545f73] hover:text-[#191c1e]'
                }`}
              >
                <Hash className="w-4 h-4" /> Contratos ({contratosCliente.length})
              </button>
              <button
                type="button"
                onClick={() => setModalTab('contactos')}
                className={`px-4 py-2.5 text-xs font-bold flex items-center gap-2 border-b-2 transition-colors cursor-pointer ${
                  modalTab === 'contactos' ? 'border-[#055bb2] text-[#055bb2]' : 'border-transparent text-[#545f73] hover:text-[#191c1e]'
                }`}
              >
                <Users className="w-4 h-4" /> Contactos ({contactosCliente.length})
              </button>
            </div>

            {/* Pestaña 1: Información General */}
            {modalTab === 'general' && (
              <form onSubmit={handleCreateSubmit} className="space-y-4 text-xs">
                <div className="text-[11px] text-[#545f73]">
                  Campos obligatorios <span className="text-red-500 font-bold">*</span>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-6 gap-3">
                  <div className="md:col-span-4">
                    <label className="block font-bold text-[#545f73] mb-1">Identificación <span className="text-red-500">*</span></label>
                    <div className="relative">
                      <input
                        type="text"
                        required
                        value={formData.nit}
                        onChange={(e) => handleNitChange(e.target.value)}
                        className="w-full pl-3.5 pr-10 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl focus:outline-none focus:border-[#055bb2] font-mono font-bold"
                        placeholder="Identificación del cliente"
                      />
                      <Building2 className="w-4 h-4 text-[#727783] absolute right-3 top-1/2 -translate-y-1/2" />
                    </div>
                  </div>

                  <div className="md:col-span-2">
                    <label className="block font-bold text-[#545f73] mb-1">Dígito de verificación <span className="text-red-500">*</span></label>
                    <div className="relative">
                      <input
                        type="text"
                        readOnly
                        value={formData.dv}
                        className="w-full pl-3.5 pr-10 py-2.5 bg-[#eceef0] border border-[#c2c6d4] rounded-xl text-center font-mono font-extrabold text-[#055bb2]"
                        placeholder="DV"
                      />
                      <UserCheck className="w-4 h-4 text-[#055bb2] absolute right-3 top-1/2 -translate-y-1/2" />
                    </div>
                  </div>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
                  <div>
                    <label className="block font-bold text-[#545f73] mb-1">Nombre / Razón Social <span className="text-red-500">*</span></label>
                    <input
                      type="text"
                      required
                      value={formData.razonSocial}
                      onChange={(e) => setFormData({ ...formData, razonSocial: e.target.value })}
                      className="w-full px-3.5 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl focus:outline-none focus:border-[#055bb2]"
                      placeholder="Nombre del cliente"
                    />
                  </div>
                  <div>
                    <label className="block font-bold text-[#545f73] mb-1">Nombre Comercial</label>
                    <input
                      type="text"
                      value={formData.nombreComercial}
                      onChange={(e) => setFormData({ ...formData, nombreComercial: e.target.value })}
                      className="w-full px-3.5 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl focus:outline-none focus:border-[#055bb2]"
                      placeholder="Nombre comercial"
                    />
                  </div>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
                  <div>
                    <label className="block font-bold text-[#545f73] mb-1">Teléfono <span className="text-red-500">*</span></label>
                    <div className="relative">
                      <input
                        type="text"
                        required
                        value={formData.telefono}
                        onChange={(e) => setFormData({ ...formData, telefono: e.target.value })}
                        className="w-full pl-3.5 pr-10 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl focus:outline-none focus:border-[#055bb2]"
                        placeholder="Teléfono del cliente"
                      />
                      <Phone className="w-4 h-4 text-[#727783] absolute right-3 top-1/2 -translate-y-1/2" />
                    </div>
                  </div>
                  <div>
                    <label className="block font-bold text-[#545f73] mb-1">Email <span className="text-red-500">*</span></label>
                    <div className="relative">
                      <input
                        type="email"
                        required
                        value={formData.emailContacto}
                        onChange={(e) => setFormData({ ...formData, emailContacto: e.target.value })}
                        className="w-full pl-3.5 pr-10 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl focus:outline-none focus:border-[#055bb2]"
                        placeholder="Email del cliente"
                      />
                      <Mail className="w-4 h-4 text-[#727783] absolute right-3 top-1/2 -translate-y-1/2" />
                    </div>
                  </div>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
                  <div>
                    <label className="block font-bold text-[#545f73] mb-1">Dirección</label>
                    <div className="relative">
                      <input
                        type="text"
                        value={formData.direccion}
                        onChange={(e) => setFormData({ ...formData, direccion: e.target.value })}
                        className="w-full pl-3.5 pr-10 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl focus:outline-none focus:border-[#055bb2]"
                        placeholder="Dirección del cliente"
                      />
                      <MapPin className="w-4 h-4 text-[#727783] absolute right-3 top-1/2 -translate-y-1/2" />
                    </div>
                  </div>
                  <div>
                    <label className="block font-bold text-[#545f73] mb-1">Ciudad <span className="text-red-500">*</span></label>
                    <select
                      value={formData.ciudad}
                      onChange={(e) => setFormData({ ...formData, ciudad: e.target.value })}
                      className="w-full px-3.5 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl focus:outline-none focus:border-[#055bb2] font-bold"
                    >
                      {CIUDADES_COLOMBIA.map((cd, idx) => (
                        <option key={idx} value={cd}>{cd}</option>
                      ))}
                    </select>
                  </div>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
                  <div>
                    <label className="block font-bold text-[#545f73] mb-1">Página Web</label>
                    <div className="relative">
                      <input
                        type="text"
                        value={formData.paginaWeb}
                        onChange={(e) => setFormData({ ...formData, paginaWeb: e.target.value })}
                        className="w-full pl-3.5 pr-10 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl focus:outline-none focus:border-[#055bb2]"
                        placeholder="Página web del cliente (ej: www.cliente.com)"
                      />
                      <Globe className="w-4 h-4 text-[#727783] absolute right-3 top-1/2 -translate-y-1/2" />
                    </div>
                  </div>
                  <div>
                    <label className="block font-bold text-[#545f73] mb-1">Fecha de Ingreso</label>
                    <div className="relative">
                      <input
                        type="date"
                        value={formData.fechaIngreso}
                        onChange={(e) => setFormData({ ...formData, fechaIngreso: e.target.value })}
                        className="w-full pl-3.5 pr-10 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl focus:outline-none focus:border-[#055bb2] font-bold"
                      />
                      <Calendar className="w-4 h-4 text-[#727783] absolute right-3 top-1/2 -translate-y-1/2 pointer-events-none" />
                    </div>
                  </div>
                </div>

                <div className="flex justify-end gap-3 pt-4 border-t border-[#e0e3e5]">
                  <button
                    type="button"
                    onClick={() => setShowCreateModal(false)}
                    className="px-5 py-2.5 rounded-xl border border-[#c2c6d4] text-[#545f73] hover:bg-[#eceef0] font-bold cursor-pointer"
                  >
                    Cancelar
                  </button>
                  <button
                    type="submit"
                    className="px-6 py-2.5 rounded-xl bg-[#055bb2] text-white font-bold hover:bg-[#3374cd] cursor-pointer shadow-md"
                  >
                    Guardar
                  </button>
                </div>
              </form>
            )}

            {/* Pestaña 2: Contratos */}
            {modalTab === 'contratos' && (
              <div className="space-y-4 text-xs">
                <div className="text-[11px] text-[#545f73]">
                  Campos obligatorios <span className="text-red-500 font-bold">*</span>
                </div>

                <div className="p-4 bg-[#d6e3ff]/30 border border-[#055bb2]/20 rounded-2xl space-y-3">
                  <h4 className="font-bold text-[#055bb2] font-headline">Agregar Contrato</h4>
                  <div className="grid grid-cols-1 md:grid-cols-4 gap-3">
                    <div>
                      <label className="block font-bold text-[#545f73] mb-1">Fecha Inicial <span className="text-red-500">*</span></label>
                      <input
                        type="date"
                        value={newContrato.fechaInicio}
                        onChange={(e) => setNewContrato({ ...newContrato, fechaInicio: e.target.value })}
                        className="w-full p-2.5 border border-[#c2c6d4] rounded-xl bg-white font-bold"
                      />
                    </div>
                    <div>
                      <label className="block font-bold text-[#545f73] mb-1">Fecha Final <span className="text-red-500">*</span></label>
                      <input
                        type="date"
                        value={newContrato.fechaFin}
                        onChange={(e) => setNewContrato({ ...newContrato, fechaFin: e.target.value })}
                        className="w-full p-2.5 border border-[#c2c6d4] rounded-xl bg-white font-bold"
                      />
                    </div>
                    <div>
                      <label className="block font-bold text-[#545f73] mb-1">Horas Mensuales <span className="text-red-500">*</span></label>
                      <input
                        type="number"
                        placeholder="Horas mensuales"
                        value={newContrato.horas}
                        onChange={(e) => setNewContrato({ ...newContrato, horas: Number(e.target.value) })}
                        className="w-full p-2.5 border border-[#c2c6d4] rounded-xl bg-white font-bold"
                      />
                    </div>
                    <div>
                      <label className="block font-bold text-[#545f73] mb-1">Valor Mensual COP <span className="text-red-500">*</span></label>
                      <input
                        type="number"
                        placeholder="Valor contrato"
                        value={newContrato.valor}
                        onChange={(e) => setNewContrato({ ...newContrato, valor: Number(e.target.value) })}
                        className="w-full p-2.5 border border-[#c2c6d4] rounded-xl bg-white font-bold"
                      />
                    </div>
                  </div>

                  <div className="grid grid-cols-1 md:grid-cols-3 gap-3 pt-2">
                    <div>
                      <label className="block font-bold text-[#545f73] mb-1">Asesor <span className="text-red-500">*</span></label>
                      <select
                        value={newContrato.asesores}
                        onChange={(e) => setNewContrato({ ...newContrato, asesores: e.target.value })}
                        className="w-full p-2.5 border border-[#c2c6d4] rounded-xl bg-white font-bold"
                      >
                        {ASESORES_SGI.map((as, idx) => (
                          <option key={idx} value={as}>{as}</option>
                        ))}
                      </select>
                    </div>

                    <div>
                      <label className="block font-bold text-[#545f73] mb-1">Sistema <span className="text-red-500">*</span></label>
                      <select
                        value={newContrato.sistemas}
                        onChange={(e) => setNewContrato({ ...newContrato, sistemas: e.target.value })}
                        className="w-full p-2.5 border border-[#c2c6d4] rounded-xl bg-white font-bold"
                      >
                        {SISTEMAS_SGI.map((sis, idx) => (
                          <option key={idx} value={sis}>{sis}</option>
                        ))}
                      </select>
                    </div>

                    <div className="flex items-end">
                      <button
                        type="button"
                        onClick={() => setContratosCliente([...contratosCliente, newContrato])}
                        className="w-full py-2.5 bg-[#055bb2] text-white font-bold rounded-xl hover:bg-[#3374cd] cursor-pointer shadow-md"
                      >
                        Agregar contrato
                      </button>
                    </div>
                  </div>
                </div>

                <div className="border border-[#e0e3e5] rounded-2xl overflow-hidden">
                  <table className="w-full text-left">
                    <thead className="bg-[#f2f4f6] text-[#545f73] font-bold uppercase text-[10px] border-b border-[#c2c6d4]/50">
                      <tr>
                        <th className="p-3">#</th>
                        <th className="p-3">Fecha inicial</th>
                        <th className="p-3">Fecha final</th>
                        <th className="p-3">Horas</th>
                        <th className="p-3">Valor</th>
                        <th className="p-3">Asesora</th>
                        <th className="p-3">Sistema</th>
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-[#e0e3e5]">
                      {contratosCliente.map((ctr, idx) => (
                        <tr key={idx} className="hover:bg-[#f7f9fb]">
                          <td className="p-3 font-bold text-[#055bb2]">{idx + 1}</td>
                          <td className="p-3 font-semibold">{ctr.fechaInicio || '2026-01-01'}</td>
                          <td className="p-3 font-semibold">{ctr.fechaFin || '2026-12-31'}</td>
                          <td className="p-3 font-bold">{ctr.horas} hrs</td>
                          <td className="p-3 font-extrabold text-[#065f46]">{formatCOP(ctr.valor)}</td>
                          <td className="p-3 text-[#545f73]">{ctr.asesores || 'María Elisa Arias'}</td>
                          <td className="p-3 text-[#055bb2] font-bold">{ctr.sistemas || 'SG-SST'}</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </div>
            )}

            {/* Pestaña 3: Contactos */}
            {modalTab === 'contactos' && (
              <div className="space-y-4 text-xs">
                <div className="text-[11px] text-[#545f73]">
                  Campos obligatorios <span className="text-red-500 font-bold">*</span>
                </div>

                <div className="p-4 bg-[#d6e3ff]/30 border border-[#055bb2]/20 rounded-2xl space-y-3">
                  <h4 className="font-bold text-[#055bb2] font-headline">Agregar Contacto</h4>
                  <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
                    <div>
                      <label className="block font-bold text-[#545f73] mb-1">Nombre <span className="text-red-500">*</span></label>
                      <input
                        type="text"
                        placeholder="Nombre del contacto"
                        value={newContacto.nombre}
                        onChange={(e) => setNewContacto({ ...newContacto, nombre: e.target.value })}
                        className="w-full p-2.5 border border-[#c2c6d4] rounded-xl bg-white font-bold"
                      />
                    </div>
                    <div>
                      <label className="block font-bold text-[#545f73] mb-1">Cargo <span className="text-red-500">*</span></label>
                      <input
                        type="text"
                        placeholder="Cargo del contacto"
                        value={newContacto.cargo}
                        onChange={(e) => setNewContacto({ ...newContacto, cargo: e.target.value })}
                        className="w-full p-2.5 border border-[#c2c6d4] rounded-xl bg-white font-bold"
                      />
                    </div>
                  </div>

                  <div className="grid grid-cols-1 md:grid-cols-3 gap-3">
                    <div>
                      <label className="block font-bold text-[#545f73] mb-1">Teléfono fijo</label>
                      <input
                        type="text"
                        placeholder="Teléfono fijo del contacto"
                        value={newContacto.telefono}
                        onChange={(e) => setNewContacto({ ...newContacto, telefono: e.target.value })}
                        className="w-full p-2.5 border border-[#c2c6d4] rounded-xl bg-white font-bold"
                      />
                    </div>
                    <div>
                      <label className="block font-bold text-[#545f73] mb-1">Celular <span className="text-red-500">*</span></label>
                      <input
                        type="text"
                        placeholder="Celular del contacto (ej: 310 1234567)"
                        value={newContacto.celular}
                        onChange={(e) => setNewContacto({ ...newContacto, celular: e.target.value })}
                        className="w-full p-2.5 border border-[#c2c6d4] rounded-xl bg-white font-bold"
                      />
                    </div>
                    <div>
                      <label className="block font-bold text-[#545f73] mb-1">Email <span className="text-red-500">*</span></label>
                      <input
                        type="email"
                        placeholder="Email del contacto"
                        value={newContacto.email}
                        onChange={(e) => setNewContacto({ ...newContacto, email: e.target.value })}
                        className="w-full p-2.5 border border-[#c2c6d4] rounded-xl bg-white font-bold"
                      />
                    </div>
                  </div>

                  <div className="flex justify-end pt-2">
                    <button
                      type="button"
                      onClick={() => setContactosCliente([...contactosCliente, newContacto])}
                      className="px-5 py-2.5 bg-[#055bb2] text-white font-bold rounded-xl hover:bg-[#3374cd] cursor-pointer shadow-md"
                    >
                      Agregar contacto
                    </button>
                  </div>
                </div>

                <div className="border border-[#e0e3e5] rounded-2xl overflow-hidden">
                  <table className="w-full text-left">
                    <thead className="bg-[#f2f4f6] text-[#545f73] font-bold uppercase text-[10px] border-b border-[#c2c6d4]/50">
                      <tr>
                        <th className="p-3">Nombre</th>
                        <th className="p-3">Cargo</th>
                        <th className="p-3">Teléfono</th>
                        <th className="p-3">Celular</th>
                        <th className="p-3">Email</th>
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-[#e0e3e5]">
                      {contactosCliente.map((cnt, idx) => (
                        <tr key={idx} className="hover:bg-[#f7f9fb]">
                          <td className="p-3 font-bold text-[#191c1e]">{cnt.nombre}</td>
                          <td className="p-3 text-[#545f73]">{cnt.cargo}</td>
                          <td className="p-3">{cnt.telefono || '-'}</td>
                          <td className="p-3 font-bold text-[#191c1e]">{cnt.celular || '-'}</td>
                          <td className="p-3 text-[#055bb2] font-semibold">{cnt.email}</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </div>
            )}
          </div>
        </div>
      )}

      {/* Modal Editar / Ver Cliente con Pestaña Contratos & Contactos Reales */}
      {showEditModal && selectedCliente && (
        <div className="fixed inset-0 bg-[#0b1c30]/50 backdrop-blur-xs flex items-center justify-center p-4 z-50">
          <div className="bg-white rounded-2xl p-6 w-full max-w-4xl shadow-2xl border border-[#e0e3e5] space-y-5 max-h-[90vh] overflow-y-auto">
            <div className="flex justify-between items-center border-b border-[#e0e3e5] pb-3">
              <h3 className="font-bold text-lg font-headline text-[#191c1e] flex items-center gap-2">
                <Edit className="w-5 h-5 text-[#055bb2]" />
                Cliente: <span className="text-[#055bb2]">{selectedCliente.razonSocial}</span>
              </h3>
              <button onClick={() => setShowEditModal(false)} className="text-[#727783] hover:text-[#191c1e] cursor-pointer">
                <X className="w-5 h-5" />
              </button>
            </div>

            {/* Pestañas Modal Editar */}
            <div className="flex border-b border-[#e0e3e5]">
              <button
                type="button"
                onClick={() => setModalTab('general')}
                className={`px-4 py-2.5 text-xs font-bold flex items-center gap-2 border-b-2 transition-colors cursor-pointer ${
                  modalTab === 'general' ? 'border-[#055bb2] text-[#055bb2]' : 'border-transparent text-[#545f73] hover:text-[#191c1e]'
                }`}
              >
                <FileText className="w-4 h-4" /> Información general
              </button>
              <button
                type="button"
                onClick={() => { setModalTab('contratos'); fetchContratosPorCliente(selectedCliente.id); }}
                className={`px-4 py-2.5 text-xs font-bold flex items-center gap-2 border-b-2 transition-colors cursor-pointer ${
                  modalTab === 'contratos' ? 'border-[#055bb2] text-[#055bb2]' : 'border-transparent text-[#545f73] hover:text-[#191c1e]'
                }`}
              >
                <Hash className="w-4 h-4" /> Contratos Reales ({contratosCliente.length})
              </button>
              <button
                type="button"
                onClick={() => { setModalTab('contactos'); fetchContactosPorCliente(selectedCliente.id); }}
                className={`px-4 py-2.5 text-xs font-bold flex items-center gap-2 border-b-2 transition-colors cursor-pointer ${
                  modalTab === 'contactos' ? 'border-[#055bb2] text-[#055bb2]' : 'border-transparent text-[#545f73] hover:text-[#191c1e]'
                }`}
              >
                <Users className="w-4 h-4" /> Contactos Reales ({contactosCliente.length})
              </button>
            </div>

            {modalTab === 'general' && (
              <form onSubmit={handleEditSubmit} className="space-y-4 text-xs">
                <div className="grid grid-cols-3 gap-3">
                  <div className="col-span-2">
                    <label className="block font-bold text-[#545f73] mb-1">Identificación (NIT) *</label>
                    <input
                      type="text"
                      required
                      value={formData.nit}
                      onChange={(e) => handleNitChange(e.target.value)}
                      className="w-full px-3.5 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl focus:outline-none focus:border-[#055bb2] font-mono font-bold"
                    />
                  </div>
                  <div>
                    <label className="block font-bold text-[#545f73] mb-1">Dígito (DV DIAN)</label>
                    <input
                      type="text"
                      readOnly
                      value={formData.dv}
                      className="w-full px-3.5 py-2.5 bg-[#eceef0] border border-[#c2c6d4] rounded-xl text-center font-mono font-extrabold text-[#055bb2]"
                    />
                  </div>
                </div>

                <div className="grid grid-cols-2 gap-3">
                  <div>
                    <label className="block font-bold text-[#545f73] mb-1">Razón Social *</label>
                    <input
                      type="text"
                      required
                      value={formData.razonSocial}
                      onChange={(e) => setFormData({ ...formData, razonSocial: e.target.value })}
                      className="w-full px-3.5 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl focus:outline-none focus:border-[#055bb2]"
                    />
                  </div>
                  <div>
                    <label className="block font-bold text-[#545f73] mb-1">Nombre Comercial</label>
                    <input
                      type="text"
                      value={formData.nombreComercial}
                      onChange={(e) => setFormData({ ...formData, nombreComercial: e.target.value })}
                      className="w-full px-3.5 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl focus:outline-none focus:border-[#055bb2]"
                    />
                  </div>
                </div>

                <div className="grid grid-cols-2 gap-3">
                  <div>
                    <label className="block font-bold text-[#545f73] mb-1">Teléfono</label>
                    <input
                      type="text"
                      value={formData.telefono}
                      onChange={(e) => setFormData({ ...formData, telefono: e.target.value })}
                      className="w-full px-3.5 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl focus:outline-none focus:border-[#055bb2]"
                    />
                  </div>
                  <div>
                    <label className="block font-bold text-[#545f73] mb-1">Correo Electrónico</label>
                    <input
                      type="email"
                      value={formData.emailContacto}
                      onChange={(e) => setFormData({ ...formData, emailContacto: e.target.value })}
                      className="w-full px-3.5 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl focus:outline-none focus:border-[#055bb2]"
                    />
                  </div>
                </div>

                <div className="grid grid-cols-2 gap-3">
                  <div>
                    <label className="block font-bold text-[#545f73] mb-1">Dirección</label>
                    <input
                      type="text"
                      value={formData.direccion}
                      onChange={(e) => setFormData({ ...formData, direccion: e.target.value })}
                      className="w-full px-3.5 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl focus:outline-none focus:border-[#055bb2]"
                    />
                  </div>
                  <div>
                    <label className="block font-bold text-[#545f73] mb-1">Estado</label>
                    <select
                      value={formData.activo ? 'activo' : 'inactivo'}
                      onChange={(e) => setFormData({ ...formData, activo: e.target.value === 'activo' })}
                      className="w-full px-3.5 py-2.5 bg-[#f7f9fb] border border-[#c2c6d4] rounded-xl focus:outline-none focus:border-[#055bb2] font-bold"
                    >
                      <option value="activo">ACTIVO</option>
                      <option value="inactivo">INACTIVO</option>
                    </select>
                  </div>
                </div>

                <div className="flex justify-end gap-3 pt-3 border-t border-[#e0e3e5]">
                  <button
                    type="button"
                    onClick={() => setShowEditModal(false)}
                    className="px-4 py-2.5 rounded-xl border border-[#c2c6d4] text-[#545f73] hover:bg-[#eceef0] font-bold cursor-pointer"
                  >
                    Cancelar
                  </button>
                  <button
                    type="submit"
                    className="px-5 py-2.5 rounded-xl bg-[#055bb2] text-white font-bold hover:bg-[#3374cd] cursor-pointer shadow-md"
                  >
                    Actualizar Cliente
                  </button>
                </div>
              </form>
            )}

            {/* Tab Contratos Reales en Modal Editar */}
            {modalTab === 'contratos' && (
              <div className="space-y-4 text-xs">
                <form onSubmit={(e) => handleAddContratoSubmit(e, selectedCliente.id)} className="p-4 bg-[#d6e3ff]/30 border border-[#055bb2]/20 rounded-2xl space-y-3">
                  <h4 className="font-bold text-[#055bb2] font-headline">Añadir Nuevo Contrato a {selectedCliente.razonSocial}</h4>
                  <div className="grid grid-cols-4 gap-2">
                    <input
                      type="number"
                      placeholder="N° Contrato"
                      value={newContrato.numeroContrato}
                      onChange={(e) => setNewContrato({ ...newContrato, numeroContrato: Number(e.target.value) })}
                      className="p-2.5 border border-[#c2c6d4] rounded-xl bg-white font-bold"
                      required
                    />
                    <input
                      type="number"
                      placeholder="Horas"
                      value={newContrato.horas}
                      onChange={(e) => setNewContrato({ ...newContrato, horas: Number(e.target.value) })}
                      className="p-2.5 border border-[#c2c6d4] rounded-xl bg-white font-bold"
                      required
                    />
                    <input
                      type="number"
                      placeholder="Valor COP"
                      value={newContrato.valor}
                      onChange={(e) => setNewContrato({ ...newContrato, valor: Number(e.target.value) })}
                      className="p-2.5 border border-[#c2c6d4] rounded-xl bg-white font-bold"
                      required
                    />
                    <button
                      type="submit"
                      className="px-3 py-2.5 bg-[#055bb2] text-white font-bold rounded-xl hover:bg-[#3374cd] cursor-pointer"
                    >
                      + Guardar Contrato
                    </button>
                  </div>
                </form>

                {loadingContratos ? (
                  <div className="p-8 text-center text-[#545f73]">Cargando contratos...</div>
                ) : contratosCliente.length === 0 ? (
                  <div className="p-8 text-center text-[#545f73] border border-dashed border-[#c2c6d4] rounded-2xl">
                    No hay contratos asociados registrados para este cliente.
                  </div>
                ) : (
                  <div className="border border-[#e0e3e5] rounded-2xl overflow-hidden shadow-xs">
                    <table className="w-full text-left">
                      <thead className="bg-[#f2f4f6] text-[#545f73] font-bold uppercase text-[10px] tracking-wider border-b border-[#c2c6d4]/50">
                        <tr>
                          <th className="p-3">N° Contrato</th>
                          <th className="p-3">Vigencia (Inicio - Fin)</th>
                          <th className="p-3">Horas</th>
                          <th className="p-3">Valor COP</th>
                          <th className="p-3">Estado</th>
                        </tr>
                      </thead>
                      <tbody className="divide-y divide-[#e0e3e5]">
                        {contratosCliente.map(ctr => (
                          <tr key={ctr.id} className="hover:bg-[#f7f9fb]">
                            <td className="p-3 font-bold text-[#055bb2] font-mono">
                              N° {ctr.numeroContrato}
                            </td>
                            <td className="p-3 text-[#191c1e] flex items-center gap-1.5 font-semibold">
                              <Calendar className="w-3.5 h-3.5 text-[#727783]" />
                              <span>{ctr.fechaInicio || '2021-04-01'} a {ctr.fechaFin || '2022-04-01'}</span>
                            </td>
                            <td className="p-3 font-bold text-[#191c1e]">
                              {ctr.horas} Horas
                            </td>
                            <td className="p-3 font-extrabold text-[#065f46]">
                              {formatCOP(ctr.valor)}
                            </td>
                            <td className="p-3">
                              <span className={`px-2.5 py-0.5 rounded-full text-[10px] font-extrabold ${
                                ctr.activo ? 'bg-[#d1fae5] text-[#065f46]' : 'bg-[#eceef0] text-[#545f73]'
                              }`}>
                                {ctr.activo ? 'ACTIVO / VIGENTE' : 'FINALIZADO'}
                              </span>
                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                )}
              </div>
            )}

            {/* Tab Contactos Reales en Modal Editar */}
            {modalTab === 'contactos' && (
              <div className="space-y-4 text-xs">
                <form onSubmit={(e) => handleAddContactoSubmit(e, selectedCliente.id)} className="p-4 bg-[#d6e3ff]/30 border border-[#055bb2]/20 rounded-2xl space-y-3">
                  <h4 className="font-bold text-[#055bb2] font-headline">Añadir Contacto Clave a {selectedCliente.razonSocial}</h4>
                  <div className="grid grid-cols-4 gap-2">
                    <input
                      type="text"
                      placeholder="Nombre Completo"
                      value={newContacto.nombre}
                      onChange={(e) => setNewContacto({ ...newContacto, nombre: e.target.value })}
                      className="p-2.5 border border-[#c2c6d4] rounded-xl bg-white font-bold"
                      required
                    />
                    <input
                      type="text"
                      placeholder="Cargo (ej: Director HSEQ)"
                      value={newContacto.cargo}
                      onChange={(e) => setNewContacto({ ...newContacto, cargo: e.target.value })}
                      className="p-2.5 border border-[#c2c6d4] rounded-xl bg-white font-bold"
                    />
                    <input
                      type="text"
                      placeholder="Teléfono / Celular"
                      value={newContacto.telefono}
                      onChange={(e) => setNewContacto({ ...newContacto, telefono: e.target.value })}
                      className="p-2.5 border border-[#c2c6d4] rounded-xl bg-white font-bold"
                    />
                    <button
                      type="submit"
                      className="px-3 py-2.5 bg-[#055bb2] text-white font-bold rounded-xl hover:bg-[#3374cd] cursor-pointer"
                    >
                      + Guardar Contacto
                    </button>
                  </div>
                </form>

                {loadingContactos ? (
                  <div className="p-8 text-center text-[#545f73]">Cargando contactos...</div>
                ) : contactosCliente.length === 0 ? (
                  <div className="p-8 text-center text-[#545f73] border border-dashed border-[#c2c6d4] rounded-2xl">
                    No hay personas de contacto asociadas a este cliente.
                  </div>
                ) : (
                  <div className="border border-[#e0e3e5] rounded-2xl overflow-hidden shadow-xs">
                    <table className="w-full text-left">
                      <thead className="bg-[#f2f4f6] text-[#545f73] font-bold uppercase text-[10px] tracking-wider border-b border-[#c2c6d4]/50">
                        <tr>
                          <th className="p-3">Nombre</th>
                          <th className="p-3">Cargo</th>
                          <th className="p-3">Teléfono / Celular</th>
                          <th className="p-3">Email</th>
                        </tr>
                      </thead>
                      <tbody className="divide-y divide-[#e0e3e5]">
                        {contactosCliente.map(cnt => (
                          <tr key={cnt.id} className="hover:bg-[#f7f9fb]">
                            <td className="p-3 font-bold text-[#191c1e]">{cnt.nombre}</td>
                            <td className="p-3 text-[#545f73] font-semibold">{cnt.cargo || 'Contacto Comercial'}</td>
                            <td className="p-3">{cnt.telefono || cnt.celular || '-'}</td>
                            <td className="p-3 text-[#055bb2] font-semibold">{cnt.email || selectedCliente.emailContacto || '-'}</td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                )}
              </div>
            )}
          </div>
        </div>
      )}
    </div>
  );
};

export default ClientesView;
