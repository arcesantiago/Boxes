# Boxes

## Análisis del proyecto

---

## 🏗️ Arquitectura y patrones implementados

### Clean Architecture
- Separación en capas:
  - **Domain**
  - **Application**
  - **Infrastructure**
  - **API**
- El **Domain** no tiene dependencias externas.

### CQRS con MediatR
- Separación de:
  - **Commands** (ej. `CreateAppointmentCommand`)
  - **Queries** (ej. `GetAllAppointmentsQuery`)
- **Handlers** dedicados por operación.

### DDD (Domain-Driven Design)
- **Entidades** con lógica encapsulada:
  - `Appointment`
  - `Contact`
  - `Vehicle`
- **Value Objects**:
  - `Contact`
  - `Vehicle`
- **Validaciones en el dominio** (constructores, reglas de negocio).

### Repository Pattern
- Abstracción mediante interfaces:
  - `IAppointmentRepository`
  - `IReadRepository`
  - `IWriteRepository`
- Implementación **in-memory** para desarrollo y testing.
- **Unit of Work**: `IAppointmentUnitOfWork`.

---

## 🧠 Decisiones técnicas destacables

### Decorator Pattern para caching
- `CachedWorkshopService` envuelve a `WorkshopService`.
- Permite agregar caching sin modificar la lógica original.
- Ventajas:
  - Separación de responsabilidades
  - Fácil de testear
- Registro manual en DI (sin Scrutor).

### Custom JSON Converter
- `AddressJsonConverter` maneja inconsistencias de la API externa:
  - `Address` como **string** vs **objeto**
- Evita `StackOverflowException` usando opciones de serialización separadas.

### Validación en múltiples capas
- **Frontend**: validadores reactivos en Angular.
- **Application**: FluentValidation  
  (`CreateAppointmentCommandValidator`)
- **Domain**: validaciones en constructores.

### Manejo de errores centralizado
- `ExceptionMiddleware` unifica respuestas de error.
- Tipos de excepción específicos:
  - `ValidationException`
  - `NotFoundException`
- Respuestas estructuradas con `CodeErrorException`.

---

## 🌐 Consumo de API externa

### WorkshopService
- Uso de **HttpClientFactory** para gestión eficiente de conexiones.
- Autenticación **Basic** configurada.
- Manejo de errores HTTP.
- **AutoMapper** para transformación de DTOs.

### Caching estratégico
- `CachedWorkshopService` con `IMemoryCache`.
- **TTL: 5 minutos**
- Reduce llamadas a la API externa.

---

## 🎨 Frontend (Angular)

### Arquitectura
- Componentes **standalone**.
- Servicios inyectables:
  - `AppointmentsService`
  - `WorkshopsService`

### Formularios y estado
- **Reactive Forms** con validación.
- Estado reactivo con **Observables**:
  - `appointments$`
- Control de flujo con `async pipe`.

### Manejo de estados de UI
- `loading`
- `error`
- `empty`

---

## ✅ Calidad de código

### Fortalezas
- Clara separación de responsabilidades.
- Interfaces bien definidas.
- Tests unitarios y de integración.
- Código documentado (comentarios XML en algunos puntos).

### Áreas de mejora sugeridas
- **Documentación XML**:
  - Habilitar generación y completar comentarios.
- **Logging**:
  - Agregar más contexto (Correlation IDs, métricas).
- **Configuración**:
  - Mover credenciales y URLs a `appsettings.json`.
- **Rate limiting**:
  - Considerar para la API externa.

---

## 🗣️ Puntos para la reunión

### Enfoque de resolución
- Análisis inicial:
  - Identificación de patrones necesarios (CQRS, Repository, Decorator).
- Testing:
  - TDD en casos críticos (validaciones, conversión JSON).

### Decisiones técnicas
- **In-memory vs EF Core**:
  - In-memory por simplicidad del desafío.
- **AutoMapper vs mapeo manual**:
  - AutoMapper para reducir boilerplate.
- **Angular standalone**:
  - Aprovechar features modernas.

### Dificultades y soluciones
- **StackOverflowException** en `AddressJsonConverter`
  - Solución: opciones de serialización separadas.
- **Inconsistencia de datos** (`Address` string vs objeto)
  - Solución: converter personalizado.
- **Carga inicial de appointments en Angular**
  - Solución: uso de Observables y control de flujo.

### Oportunidades de mejora
- Agregar paginación para listas grandes.
- Implementar refresh token para la API externa.
- Agregar **health checks**.
- Implementar **retry policies** con Polly.

---

## ▶️ Ejecución

### Ejecutar ambos proyectos simultáneamente

#### Terminal 1 – Backend
```bash
cd Backend/Boxes.API
dotnet run --launch-profile http

#### Terminal 2 – Frontend
```bash
cd Frontend/boxes
npm start
