import React from 'react';
import { Link } from 'react-router-dom';
import { CrmSidebar } from '../components/CrmSidebar';
import {
  Users,
  ClipboardCheck,
  Bell,
  Search,
  User,
  TrendingUp,
  AlertTriangle,
  CheckCircle2,
  Clock,
  Calendar,
  Filter,
  Plus
} from 'lucide-react';

export const Dashboard: React.FC = () => {
  return (
    <div className="min-h-screen bg-[#f7f9fb] flex flex-col md:flex-row font-sans">
      {/* Shared CrmSidebar */}
      <CrmSidebar />

      {/* Main Workspace Area */}
      <div className="flex-grow flex flex-col overflow-y-auto w-full transition-all duration-300 ease-in-out">
        {/* Top Header */}
        <header className="bg-white border-b border-[#c2c6d4]/40 px-6 py-4 flex items-center justify-between sticky top-0 z-30 elevation-1">
          <div className="flex items-center gap-4 w-full max-w-md">
            <div className="relative w-full">
              <Search className="w-4 h-4 text-[#727783] absolute left-3 top-1/2 -translate-y-1/2" />
              <input
                type="text"
                placeholder="Buscar cliente, NIT, código de auditoría..."
                className="w-full pl-9 pr-4 py-2 rounded-xl border border-[#c2c6d4]/60 text-xs focus:outline-none focus:border-[#055bb2] bg-[#f8fafc]"
              />
            </div>
          </div>

          <div className="flex items-center gap-3">
            <button className="relative p-2 rounded-xl text-[#545f73] hover:bg-[#f2f4f6] transition-colors">
              <Bell className="w-5 h-5" />
              <span className="absolute top-1.5 right-1.5 w-2 h-2 rounded-full bg-red-500" />
            </button>
            <Link
              to="/perfil"
              className="p-2 rounded-xl text-[#545f73] hover:bg-[#f2f4f6] transition-colors"
            >
              <User className="w-5 h-5" />
            </Link>
            <button
              onClick={() => alert('Crear nueva auditoría / compromiso')}
              className="hidden sm:flex items-center gap-2 bg-[#055bb2] text-white px-4 py-2 rounded-xl text-xs font-bold hover:bg-[#3374cd] transition-all shadow-xs"
            >
              <Plus className="w-4 h-4" />
              <span>Nueva Auditoría</span>
            </button>
          </div>
        </header>

        {/* Dashboard Content */}
        <main className="p-6 space-y-8 max-w-7xl w-full mx-auto">
          {/* Welcome Banner */}
          <div className="flex flex-col sm:flex-row justify-between sm:items-center gap-4 bg-[#055bb2] rounded-2xl p-6 text-white shadow-lg">
            <div className="space-y-1">
              <h1 className="text-xl font-bold font-headline">
                Bienvenido al Panel de Control SGI
              </h1>
              <p className="text-xs text-[#d6e3ff]">
                Resumen ejecutivo del estado de Sistemas de Gestión Integrados de sus clientes.
              </p>
            </div>
            <div className="flex items-center gap-3">
              <span className="text-xs font-medium bg-white/10 px-3 py-1.5 rounded-lg backdrop-blur-md">
                Última sincronización: Hoy, 17:45
              </span>
            </div>
          </div>

          {/* KPI Cards */}
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-5">
            {/* KPI 1 */}
            <Link to="/clientes" className="bg-white p-5 rounded-2xl border border-[#c2c6d4]/40 elevation-1 space-y-3 hover:border-[#055bb2]/60 hover:shadow-md transition-all group block">
              <div className="flex justify-between items-center">
                <span className="text-xs font-semibold text-[#545f73] group-hover:text-[#055bb2] transition-colors">Clientes Activos</span>
                <div className="p-2 bg-[#055bb2]/10 rounded-xl text-[#055bb2]">
                  <Users className="w-5 h-5" />
                </div>
              </div>
              <div className="flex items-baseline gap-2">
                <span className="text-3xl font-bold font-headline text-[#191c1e]">67</span>
                <span className="text-xs font-semibold text-emerald-600 inline-flex items-center gap-0.5">
                  <TrendingUp className="w-3.5 h-3.5" /> Reales B2B
                </span>
              </div>
              <p className="text-[11px] text-[#727783]">Ver directorio B2B en PostgreSQL 15</p>
            </Link>

            {/* KPI 2 */}
            <div className="bg-white p-5 rounded-2xl border border-[#c2c6d4]/40 elevation-1 space-y-3">
              <div className="flex justify-between items-center">
                <span className="text-xs font-semibold text-[#545f73]">Auditorías en Curso</span>
                <div className="p-2 bg-[#3374cd]/10 rounded-xl text-[#055bb2]">
                  <ClipboardCheck className="w-5 h-5" />
                </div>
              </div>
              <div className="flex items-baseline gap-2">
                <span className="text-3xl font-bold font-headline text-[#191c1e]">8</span>
                <span className="text-xs font-medium text-[#424752]">SG-SST & ISO</span>
              </div>
              <p className="text-[11px] text-[#727783]">4 con fecha de cierre esta semana</p>
            </div>

            {/* KPI 3 */}
            <div className="bg-white p-5 rounded-2xl border border-[#c2c6d4]/40 elevation-1 space-y-3">
              <div className="flex justify-between items-center">
                <span className="text-xs font-semibold text-[#545f73]">Cumplimiento Promedio</span>
                <div className="p-2 bg-emerald-500/10 rounded-xl text-emerald-600">
                  <CheckCircle2 className="w-5 h-5" />
                </div>
              </div>
              <div className="flex items-baseline gap-2">
                <span className="text-3xl font-bold font-headline text-[#191c1e]">94.2%</span>
                <span className="text-xs font-semibold text-emerald-600">+3.1% vs 2025</span>
              </div>
              <p className="text-[11px] text-[#727783]">Estándares mínimos Res. 0312</p>
            </div>

            {/* KPI 4 */}
            <div className="bg-white p-5 rounded-2xl border border-[#c2c6d4]/40 elevation-1 space-y-3">
              <div className="flex justify-between items-center">
                <span className="text-xs font-semibold text-[#545f73]">Pendientes Críticos</span>
                <div className="p-2 bg-amber-500/10 rounded-xl text-amber-600">
                  <AlertTriangle className="w-5 h-5" />
                </div>
              </div>
              <div className="flex items-baseline gap-2">
                <span className="text-3xl font-bold font-headline text-[#191c1e]">3</span>
                <span className="text-xs font-semibold text-amber-600">Acción Requerida</span>
              </div>
              <p className="text-[11px] text-[#727783]">Compromisos con vencimiento cercano</p>
            </div>
          </div>

          {/* Main Table & Widgets */}
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            {/* Table Section (Col 2) */}
            <div className="lg:col-span-2 bg-white rounded-2xl border border-[#c2c6d4]/40 elevation-1 overflow-hidden space-y-4">
              <div className="p-5 border-b border-[#e0e3e5] flex justify-between items-center">
                <div>
                  <h3 className="font-bold text-base font-headline text-[#191c1e]">
                    Auditorías y Acompañamientos Recientes
                  </h3>
                  <p className="text-xs text-[#727783]">Estado PHVA y entregables normados</p>
                </div>
                <button className="inline-flex items-center gap-1.5 text-xs font-semibold text-[#055bb2] hover:bg-[#d6e3ff]/40 px-3 py-1.5 rounded-lg transition-colors">
                  <Filter className="w-3.5 h-3.5" />
                  Filtrar
                </button>
              </div>

              <div className="overflow-x-auto">
                <table className="w-full text-left text-xs">
                  <thead className="bg-[#f8fafc] text-[#545f73] font-semibold border-b border-[#e0e3e5]">
                    <tr>
                      <th className="px-5 py-3">Cliente / Empresa</th>
                      <th className="px-5 py-3">Norma</th>
                      <th className="px-5 py-3">Estado PHVA</th>
                      <th className="px-5 py-3">% Avance</th>
                      <th className="px-5 py-3 text-right">Acción</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-[#e0e3e5] text-[#191c1e]">
                    <tr className="hover:bg-[#f8fafc] transition-colors">
                      <td className="px-5 py-4 font-semibold">Transportes del Norte S.A.</td>
                      <td className="px-5 py-4 text-[#545f73]">PESV (Ley 2251)</td>
                      <td className="px-5 py-4">
                        <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-[11px] font-semibold bg-blue-100 text-blue-800">
                          Hacer (Implementación)
                        </span>
                      </td>
                      <td className="px-5 py-4 font-bold text-[#055bb2]">88%</td>
                      <td className="px-5 py-4 text-right">
                        <Link to="/consultor" className="text-[#055bb2] font-semibold hover:underline">Ver Informe</Link>
                      </td>
                    </tr>

                    <tr className="hover:bg-[#f8fafc] transition-colors">
                      <td className="px-5 py-4 font-semibold">Constructora Andina B2B</td>
                      <td className="px-5 py-4 text-[#545f73]">SG-SST (Res. 0312)</td>
                      <td className="px-5 py-4">
                        <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-[11px] font-semibold bg-emerald-100 text-emerald-800">
                          Verificar (Auditoría)
                        </span>
                      </td>
                      <td className="px-5 py-4 font-bold text-emerald-600">96%</td>
                      <td className="px-5 py-4 text-right">
                        <Link to="/consultor" className="text-[#055bb2] font-semibold hover:underline">Ver Informe</Link>
                      </td>
                    </tr>

                    <tr className="hover:bg-[#f8fafc] transition-colors">
                      <td className="px-5 py-4 font-semibold">Industrias Químicas S.A.S.</td>
                      <td className="px-5 py-4 text-[#545f73]">ISO 9001:2015</td>
                      <td className="px-5 py-4">
                        <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-[11px] font-semibold bg-amber-100 text-amber-800">
                          Planear (Diagnóstico)
                        </span>
                      </td>
                      <td className="px-5 py-4 font-bold text-amber-600">45%</td>
                      <td className="px-5 py-4 text-right">
                        <Link to="/consultor" className="text-[#055bb2] font-semibold hover:underline">Ver Informe</Link>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>

            {/* Agenda & Reminders Widget (Col 1) */}
            <div className="bg-white rounded-2xl border border-[#c2c6d4]/40 elevation-1 p-5 space-y-4">
              <div className="flex items-center justify-between border-b border-[#e0e3e5] pb-3">
                <h3 className="font-bold text-base font-headline text-[#191c1e] flex items-center gap-2">
                  <Calendar className="w-4 h-4 text-[#055bb2]" />
                  Próximas Asesorías
                </h3>
                <Link to="/agenda" className="text-[11px] text-[#055bb2] font-bold hover:underline">
                  Ver Todo
                </Link>
              </div>

              <div className="space-y-3">
                <div className="p-3 bg-[#f8fafc] rounded-xl border border-[#e0e3e5] space-y-1">
                  <div className="flex justify-between items-center text-xs font-bold text-[#191c1e]">
                    <span>Reunión de Cierre COPASST</span>
                    <span className="text-[10px] text-[#055bb2] font-semibold">09:00 AM</span>
                  </div>
                  <p className="text-[11px] text-[#545f73]">Cliente: Transportes del Norte S.A.</p>
                  <div className="flex items-center gap-1.5 text-[10px] text-[#727783] pt-1">
                    <Clock className="w-3 h-3 text-[#545f73]" />
                    <span>Duración: 2 Horas • Virtual Teams</span>
                  </div>
                </div>

                <div className="p-3 bg-[#f8fafc] rounded-xl border border-[#e0e3e5] space-y-1">
                  <div className="flex justify-between items-center text-xs font-bold text-[#191c1e]">
                    <span>Auditoría de Campo PESV</span>
                    <span className="text-[10px] text-[#055bb2] font-semibold">02:30 PM</span>
                  </div>
                  <p className="text-[11px] text-[#545f73]">Cliente: Logística Medellín</p>
                  <div className="flex items-center gap-1.5 text-[10px] text-[#727783] pt-1">
                    <Clock className="w-3 h-3 text-[#545f73]" />
                    <span>Presencial • Sede Envigado</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </main>
      </div>
    </div>
  );
};
