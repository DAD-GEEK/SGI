import { describe, it, expect, vi } from 'vitest';
import { render, screen } from '@testing-library/react';
import React from 'react';
import ClientesView from '../ClientesView';

// Mock de react-router-dom o componentes hijos si aplica
vi.mock('../../components/CrmSidebar', () => ({
  CrmSidebar: () => <div data-testid="crm-sidebar">Sidebar Mock</div>
}));

describe('ClientesView Component Test Suite', () => {

  it('debe renderizar el título del módulo de Clientes y la etiqueta de SSE Reactivo', () => {
    render(<ClientesView />);

    expect(screen.getByText('Clientes')).toBeInTheDocument();
    expect(screen.getByText(/Directorio B2B/i)).toBeInTheDocument();
    expect(screen.getByTestId('crm-sidebar')).toBeInTheDocument();
  });

  it('debe mostrar el botón para Agregar Cliente', () => {
    render(<ClientesView />);

    const addButton = screen.getByRole('button', { name: /Agregar Cliente/i });
    expect(addButton).toBeInTheDocument();
  });

  it('debe contener las opciones de paginación comenzando desde 5 registros', () => {
    render(<ClientesView />);

    const selectElement = screen.getByDisplayValue(/5 registros por página/i);
    expect(selectElement).toBeInTheDocument();
  });
});
