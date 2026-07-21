# Changelog — SGI Core Service (Spring Boot 3 Microservice)

Todos los cambios del microservicio backend Java Spring Boot 3 de Gestión Integral SGI se registran en este archivo.

## [Sin Versionar] - 2026-07-21

### Added
- **`ClienteController.java`**: Endpoints REST `/api/clientes` (GET, POST, PUT, DELETE) y transmisión reactiva SSE en `/api/clientes/stream`.
- **`ContratoController.java`**: Endpoints REST `/api/contratos` y `/api/contratos/cliente/{clienteId}` para contratos migrados con valores en COP y notificaciones.
- **`ContactoController.java`**: Endpoints REST `/api/contactos` y `/api/contactos/cliente/{clienteId}` para personas de contacto asociadas a empresas clientes.
- **Entidades JPA & Repositorios**: `ClienteEntity`, `ContratoEntity`, `ContactoEntity`, `UsuarioEntity`, `AgendaEventoEntity`.
- **Suite de Pruebas JUnit 5 (`ClienteControllerTest.java`)**: Pruebas de integración de listado, creación con NIT único y desactivación lógica de clientes.
