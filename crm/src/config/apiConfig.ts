/// <reference types="vite/client" />

// Normalización defensiva de URL de la API de SGI conforme a la regla de gobernanza técnica
const rawApiUrl = (import.meta as any).env?.VITE_SGI_API_URL || 'http://localhost:8084/api';

const normalizeApiUrl = (url: string): string => {
  let cleaned = url.trim().replace(/\/+$/, '');
  if (!cleaned.endsWith('/api')) {
    cleaned += '/api';
  }
  return cleaned;
};

export const API_BASE_URL = normalizeApiUrl(rawApiUrl);
