/// <reference types="vite/client" />

// La URL del backend debe venir del secreto del pipeline. Solo en local se usa localhost.
const getLocalApiUrl = (): string => {
  if (typeof window !== 'undefined') {
    const host = window.location.hostname;
    if (host === 'localhost' || host === '127.0.0.1') {
      return 'http://localhost:8084/api';
    }
  }

  return '';
};

const rawApiUrl = (import.meta as any).env?.VITE_SGI_API_URL ?? getLocalApiUrl();

const normalizeApiUrl = (url: string): string => {
  const trimmed = url.trim();
  if (!trimmed) {
    return '/api';
  }

  let cleaned = trimmed.replace(/\/+$/, '');
  if (!cleaned.endsWith('/api')) {
    cleaned += '/api';
  }
  return cleaned;
};

export const API_BASE_URL = normalizeApiUrl(rawApiUrl);
