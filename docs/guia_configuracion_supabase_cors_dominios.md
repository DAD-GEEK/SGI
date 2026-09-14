# Guía de Configuración de CORS y Dominios en Supabase Auth & Estructura Plesk

Esta guía técnica documenta la configuración obligatoria de orígenes y dominios en **Supabase Auth** para evitar bloqueos por políticas de CORS en desarrollo local y en los despliegues oficiales a producción del ecosistema **Gestión Integral SGI**.

---

## 1. Diagnóstico del Error de CORS en Supabase

### Síntoma
En la consola del navegador al intentar autenticar o consultar el perfil:
```text
Access to fetch at 'https://bmgfqxribkrhbzqvhsjp.supabase.co/auth/v1/user' from origin 'http://localhost:3005' 
has been blocked by CORS policy: No 'Access-Control-Allow-Origin' header is present on the requested resource.
net::ERR_FAILED
```

### Causa Raíz
Supabase Auth restringe las llamadas HTTP y el intercambio de tokens únicamente a los dominios y URLs explícitamente autorizados en la lista blanca de la consola de administración. Cuando el frontend realiza peticiones desde un origen no registrado (ej. `http://localhost:3005`), el API Gateway de Supabase deniega la cabecera `Access-Control-Allow-Origin`.

---

## 2. Configuración en el Dashboard de Supabase

Para registrar los orígenes permitidos:

1. Ingresa a la consola: **[https://supabase.com/dashboard](https://supabase.com/dashboard)**.
2. Selecciona el proyecto institucional de SGI:
   - **Project ID**: `bmgfqxribkrhbzqvhsjp`
3. En el menú lateral izquierdo, dirígete a:
   - **Authentication** -> **URL Configuration**.
4. Configura las siguientes dos secciones:

### A. Site URL (URL Principal)
- **En Desarrollo (DEV)**: `http://localhost:3005`
- **En Producción (PROD)**: `https://crm.gestionintegralsgi.com.co` (o el subdominio canónico asignado para el CRM de SGI).

### B. Redirect URLs (Lista Blanca Multientorno)
Agrega cada una de las siguientes URLs para garantizar compatibilidad total en entornos locales, staging y producción:

#### Entornos de Desarrollo Local (DEV)
```text
http://localhost:3005/**
http://localhost:3005
http://127.0.0.1:3005/**
http://127.0.0.1:3005
http://localhost:5173/**
http://localhost:5173
```

#### Entornos de Producción (PROD)
```text
https://crm.gestionintegralsgi.com.co/**
https://crm.gestionintegralsgi.com.co
https://app.gestionintegralsgi.com.co/**
https://app.gestionintegralsgi.com.co
https://consultor.gestionintegralsgi.com.co/**
https://consultor.gestionintegralsgi.com.co
https://gestionintegralsgi.com.co/**
https://gestionintegralsgi.com.co
https://sgi-crm.web.app/**
https://sgi-crm.firebaseapp.com/**
```

> **Nota**: El patrón con doble asterisco `/**` permite todas las rutas internas de la aplicación (ej. `/login`, `/dashboard`, `/cambiar-password`, `/agenda`, `/consultor`).

5. Haz clic en el botón inferior **Save changes**.

---

## 3. Estructura de Despliegue en Servidor Plesk (IIS / ASP.NET)

### Pregunta: ¿`Global.asax` va al mismo nivel de `Web.config` en Consultor y Agenda?
**SÍ, rotunda y obligatoriamente.**

En la arquitectura de aplicaciones web ASP.NET MVC bajo IIS / Plesk, tanto `Global.asax` como `Web.config` son archivos de arranque raíz. Deben residir en la raíz de la carpeta pública del sitio (`httpdocs/`).

### Estructura Correcta del Directorio en Plesk:

```text
C:\Inetpub\vhosts\gestionintegralsgi.com.co\consultor.gestionintegralsgi.com.co\httpdocs\
│
├── bin/                                <--- CARPETA DE BINARIOS COMPILADOS
│   ├── Web.dll                         <--- (Subir aquí el Web.dll compilado)
│   ├── CoreBusiness.dll
│   ├── DataAccess.dll
│   ├── Models.dll
│   └── ... (librerías de NuGet)
│
├── Content/                            <--- Estilos, imágenes, fonts
├── Scripts/                            <--- Scripts JS del aplicativo
├── Views/                              <--- Vistas Razor (.cshtml)
│
├── Global.asax                         <--- [RAÍZ] Al mismo nivel que Web.config
└── Web.config                          <--- [RAÍZ] Al mismo nivel que Global.asax
```

### Funciones de cada archivo raíz:
1. **`Web.config`**: Define la configuración del servidor web IIS, módulos HTTP, cadenas de conexión (`connectionStrings`), modo de cookies (`<httpCookies requireSSL="true" />`) y configuración de runtime.
2. **`Global.asax`**: Define el ciclo de vida de la aplicación (`Application_Start`, `Application_Error`, `Application_EndRequest`). En nuestro caso, intercepta cada petición saliente para forzar `; SameSite=None; Secure` en todas las cookies emitidas, permitiendo que el aplicativo funcione dentro del iframe del CRM sin bloqueos de sesión.
3. **`bin/Web.dll`**: Contiene la lógica compilada en C# (controladores, endpoints de SSO, hashing de contraseñas y redirecciones).
