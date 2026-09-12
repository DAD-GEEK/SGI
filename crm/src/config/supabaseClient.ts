/// <reference types="vite/client" />
import { createClient } from '@supabase/supabase-js';

const supabaseUrl = import.meta.env.VITE_SUPABASE_URL || 'https://bmgfqxribkrhbzqvhsjp.supabase.co';
const supabaseAnonKey = import.meta.env.VITE_SUPABASE_ANON_KEY || 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImJtZ2ZxeHJpYmtyaGJ6cXZoc2pwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3ODQ5MjAwODAsImV4cCI6MjEwMDQ5NjA4MH0.4rT15aVo56yHzfrtjQfxrMZS9k3uItWyJ9HMMUSycaw';

export const supabase = createClient(supabaseUrl, supabaseAnonKey, {
  auth: {
    persistSession: true,
    autoRefreshToken: true,
    detectSessionInUrl: true
  }
});

