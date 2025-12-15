# Boxes

Análisis del proyecto

1. Arquitectura y patrones implementados
Clean Architecture
Separación en capas: Domain, Application, Infrastructure, API
Domain sin dependencias externas
CQRS con MediatR
Comandos (CreateAppointmentCommand) y Queries (GetAllAppointmentsQuery) separados
Handlers dedicados por operación
DDD (Domain-Driven Design)
Entidades con lógica encapsulada (Appointment, Contact, Vehicle)
Value Objects (Contact, Vehicle)
Validaciones en el dominio (constructor de Appointment)
Repository Pattern
Abstracción con IAppointmentRepository, IReadRepository<T>, IWriteRepository<T>
Implementación in-memory para desarrollo/testing
Unit of Work (IAppointmentUnitOfWork)

2. Decisiones técnicas destacables
Decorator Pattern para caching
// CachedWorkshopService envuelve WorkshopService// Permite agregar caching sin modificar la lógica original
Ventaja: separación de responsabilidades, fácil de testear
Registro manual en DI (sin Scrutor)
Custom JSON Converter
AddressJsonConverter maneja inconsistencias de la API externa (string vs objeto)
Evita StackOverflowException usando opciones de serialización separadas
Validación en múltiples capas
Frontend: validadores reactivos en Angular
Application: FluentValidation (CreateAppointmentCommandValidator)
Domain: validaciones en constructores
Manejo de errores centralizado
ExceptionMiddleware unifica respuestas de error
Tipos de excepción específicos (ValidationException, NotFoundException)
Respuestas estructuradas con CodeErrorException

3. Consumo de API externa
WorkshopService
HttpClientFactory para gestión de conexiones
Autenticación Basic configurada
Manejo de errores HTTP
AutoMapper para transformación de DTOs
Caching estratégico
CachedWorkshopService con IMemoryCache
TTL de 5 minutos
Reduce llamadas a la API externa

4. Frontend (Angular)
Arquitectura
Componentes standalone
Servicios inyectables (AppointmentsService, WorkshopsService)
Reactive Forms con validación
Estado reactivo
Uso de Observables (appointments$)
Control de flujo con async pipe
Manejo de estados: loading, error, empty

5. Calidad de código
Fortalezas
Separación de responsabilidades
Interfaces bien definidas
Tests unitarios e integración
Código documentado (comentarios XML en algunos puntos)
Áreas de mejora sugeridas
Documentación XML: habilitar generación y completar comentarios
Logging: más contexto en logs (correlación IDs, métricas)
Configuración: mover credenciales y URLs a appsettings.json
Rate limiting: considerar para la API externa

6. Puntos para la reunión
Enfoque de resolución
Análisis inicial: identificar patrones necesarios (CQRS, Repository, Decorator)
Testing: TDD en casos críticos (validaciones, conversión JSON)
Decisiones técnicas
In-memory vs EF Core: in-memory para simplicidad del desafío
AutoMapper vs mapeo manual: AutoMapper para reducir boilerplate
Angular standalone: aprovechar features modernas
Dificultades y soluciones
StackOverflowException en AddressJsonConverter
Solución: opciones de serialización separadas
Inconsistencia de datos (Address como string vs objeto)
Solución: converter personalizado
Carga inicial de appointments en Angular
Solución: uso de Observables y control de flujo
Oportunidades de mejora
Agregar paginación para listas grandes
Implementar refresh token para la API externa
Agregar health checks
Implementar retry policies con Polly

Ejecución

Ejecutar ambos simultáneamente
Terminal 1 (Backend):
cd Backend/Boxes.APIdotnet run --launch-profile http
Terminal 2 (Frontend):
cd Frontend/boxesnpm start
