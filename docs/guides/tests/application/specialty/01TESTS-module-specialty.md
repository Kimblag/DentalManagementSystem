# Escenarios de Test - Módulo Especialidades

## Tests de Casos de Uso (Commands)

### CreateSpecialty

| Escenario | Entrada (Command) | Estado Inicial | Resultado Esperado | Verificaciones |
|-----------|-------------------|----------------|-------------------|----------------|
| **Happy Path - Crear especialidad con un tratamiento** | Name: "Ortodoncia"<br>Treatments: [{Code: "ORT001", Name: "Brackets metálicos", BaseCost: 5000, Currency: "ARS"}] | Repositorio vacío | SpecialtyId retornado<br>Specialty guardada | - Specialty.Name == "Ortodoncia"<br>- Specialty.IsActive == true<br>- Treatments.Count == 1<br>- Repository.Save llamado 1 vez |
| **Happy Path - Crear con múltiples tratamientos** | Name: "Endodoncia"<br>Treatments: [{Code: "END001", Name: "Conducto unirradicular", BaseCost: 3000, Currency: "ARS"}, {Code: "END002", Name: "Conducto birradicular", BaseCost: 4500, Currency: "ARS"}] | Repositorio vacío | SpecialtyId retornado | - Treatments.Count == 2<br>- Todos activos<br>- Códigos únicos |
| **Error - Especialidad ya existe** | Name: "Ortodoncia"<br>Treatments: [válido] | Existe "Ortodoncia" activa | SpecialtyAlreadyExistsException | - Repository.Save NO llamado<br>- Verificación de unicidad ejecutada |
| **Error - Sin tratamientos** | Name: "Periodoncia"<br>Treatments: [] | Repositorio vacío | DomainException<br>("Al menos un tratamiento requerido") | - Validación en dominio<br>- No se guarda nada |
| **Error - Tratamientos con código duplicado** | Name: "Cirugía"<br>Treatments: [{Code: "CIR001", ...}, {Code: "CIR001", ...}] | Repositorio vacío | DomainException<br>("Código duplicado") | - Validación de unicidad interna |
| **Error - Tratamientos con nombre duplicado** | Name: "Estética"<br>Treatments: [{Name: "Blanqueamiento", ...}, {Name: "Blanqueamiento", ...}] | Repositorio vacío | DomainException<br>("Nombre duplicado") | - Validación de unicidad interna |
| **Error - BaseCost negativo** | Name: "Implantes"<br>Treatments: [{..., BaseCost: -100, ...}] | Repositorio vacío | DomainException<br>("Monto no puede ser negativo") | - Validación de Money VO |

---

### RenameSpecialty

| Escenario | Entrada (Command) | Estado Inicial | Resultado Esperado | Verificaciones |
|-----------|-------------------|----------------|-------------------|----------------|
| **Happy Path - Renombrar especialidad activa** | SpecialtyId: {guid}<br>Name: "Ortodoncia Avanzada" | Specialty "Ortodoncia" activa | Void (éxito) | - Specialty.Name == "Ortodoncia Avanzada"<br>- Repository.Update llamado<br>- IsActive sigue true |
| **Error - Especialidad no encontrada** | SpecialtyId: {guid-inexistente}<br>Name: "Nuevo nombre" | Repositorio vacío | SpecialtyNotFoundException | - Repository.Update NO llamado |
| **Error - Especialidad inactiva** | SpecialtyId: {guid}<br>Name: "Nuevo nombre" | Specialty inactiva | SpecialtyInactiveException | - No se permite modificación<br>- Estado no cambia |
| **Error - Nombre vacío** | SpecialtyId: {guid}<br>Name: "" | Specialty activa | DomainException<br>("Nombre no válido") | - Validación de Name VO |
| **Error - Nombre solo espacios** | SpecialtyId: {guid}<br>Name: "   " | Specialty activa | DomainException | - Validación de Name VO |

---

### ActivateSpecialty

| Escenario | Entrada (Command) | Estado Inicial | Resultado Esperado | Verificaciones |
|-----------|-------------------|----------------|-------------------|----------------|
| **Happy Path - Activar especialidad con tratamientos** | SpecialtyId: {guid} | Specialty inactiva con 2 tratamientos inactivos | Void (éxito) | - Specialty.IsActive == true<br>- Todos los Treatments.IsActive == true<br>- Repository.Update llamado |
| **Happy Path - Activar ya activa (idempotente)** | SpecialtyId: {guid} | Specialty ya activa | Void (éxito) | - Sin cambios<br>- Sin errores |
| **Error - Especialidad no encontrada** | SpecialtyId: {guid-inexistente} | Repositorio vacío | SpecialtyNotFoundException | - Repository.Update NO llamado |

---

### DeactivateSpecialty

| Escenario | Entrada (Command) | Estado Inicial | Resultado Esperado | Verificaciones |
|-----------|-------------------|----------------|-------------------|----------------|
| **Happy Path - Desactivar especialidad** | SpecialtyId: {guid} | Specialty activa con 3 tratamientos activos | Void (éxito) | - Specialty.IsActive == false<br>- Todos los Treatments.IsActive == false<br>- Cascade ejecutado |
| **Happy Path - Desactivar ya inactiva (idempotente)** | SpecialtyId: {guid} | Specialty ya inactiva | Void (éxito) | - Sin cambios<br>- Sin errores |
| **Error - Especialidad no encontrada** | SpecialtyId: {guid-inexistente} | Repositorio vacío | SpecialtyNotFoundException | - Repository.Update NO llamado |

---

### AddTreatmentToSpecialty

| Escenario | Entrada (Command) | Estado Inicial | Resultado Esperado | Verificaciones |
|-----------|-------------------|----------------|-------------------|----------------|
| **Happy Path - Agregar tratamiento** | SpecialtyId: {guid}<br>Code: "ORT002"<br>Name: "Brackets cerámicos"<br>BaseCost: 7000<br>Currency: "ARS" | Specialty activa con 1 tratamiento | TreatmentId retornado | - Treatments.Count == 2<br>- Nuevo tratamiento IsActive == true<br>- Repository.Update llamado |
| **Error - Especialidad no encontrada** | SpecialtyId: {guid-inexistente}<br>[datos válidos] | Repositorio vacío | SpecialtyNotFoundException | - No se agrega nada |
| **Error - Especialidad inactiva** | SpecialtyId: {guid}<br>[datos válidos] | Specialty inactiva | SpecialtyInactiveException | - No se permite agregar |
| **Error - Código duplicado** | SpecialtyId: {guid}<br>Code: "ORT001" (ya existe)<br>[otros datos válidos] | Specialty con Code "ORT001" | DomainException<br>("Código ya existe") | - Validación de unicidad |
| **Error - Nombre duplicado** | SpecialtyId: {guid}<br>Name: "Brackets metálicos" (ya existe)<br>[otros datos válidos] | Specialty con ese nombre | DomainException<br>("Nombre ya existe") | - Validación de unicidad |

---

### RenameTreatment

| Escenario | Entrada (Command) | Estado Inicial | Resultado Esperado | Verificaciones |
|-----------|-------------------|----------------|-------------------|----------------|
| **Happy Path - Renombrar tratamiento** | SpecialtyId: {guid}<br>TreatmentId: {guid}<br>Name: "Brackets autoligables" | Specialty activa, Treatment "Brackets metálicos" activo | Void (éxito) | - Treatment.Name == "Brackets autoligables"<br>- Repository.Update llamado |
| **Error - Especialidad no encontrada** | SpecialtyId: {guid-inexistente}<br>[otros datos] | Repositorio vacío | SpecialtyNotFoundException | - No se modifica nada |
| **Error - Tratamiento no encontrado** | SpecialtyId: {guid}<br>TreatmentId: {guid-inexistente}<br>Name: "Nuevo" | Specialty existe | TreatmentNotFoundException | - No se modifica nada |
| **Error - Especialidad inactiva** | SpecialtyId: {guid}<br>TreatmentId: {guid}<br>Name: "Nuevo" | Specialty inactiva | SpecialtyInactiveException | - No se permite modificar |
| **Error - Nombre duplicado en specialty** | SpecialtyId: {guid}<br>TreatmentId: {guid1}<br>Name: "Brackets cerámicos" | Specialty con otro treatment llamado "Brackets cerámicos" | DomainException<br>("Nombre ya existe") | - Validación de unicidad |

---

### ChangeTreatmentBaseCost

| Escenario | Entrada (Command) | Estado Inicial | Resultado Esperado | Verificaciones |
|-----------|-------------------|----------------|-------------------|----------------|
| **Happy Path - Cambiar costo** | SpecialtyId: {guid}<br>TreatmentId: {guid}<br>BaseCost: 8000<br>Currency: "ARS" | Specialty activa, Treatment activo con costo 5000 | Void (éxito) | - Treatment.BaseCost.Amount == 8000<br>- Repository.Update llamado |
| **Happy Path - Costo a cero** | SpecialtyId: {guid}<br>TreatmentId: {guid}<br>BaseCost: 0<br>Currency: "ARS" | Treatment con costo 3000 | Void (éxito) | - Treatment.BaseCost.Amount == 0<br>- Permitido por regla de negocio |
| **Error - Especialidad no encontrada** | SpecialtyId: {guid-inexistente}<br>[otros datos] | Repositorio vacío | SpecialtyNotFoundException | - No se modifica |
| **Error - Tratamiento no encontrado** | SpecialtyId: {guid}<br>TreatmentId: {guid-inexistente}<br>[otros datos] | Specialty existe | TreatmentNotFoundException | - No se modifica |
| **Error - Especialidad inactiva** | SpecialtyId: {guid}<br>TreatmentId: {guid}<br>BaseCost: 6000<br>Currency: "ARS" | Specialty inactiva | SpecialtyInactiveException | - No se permite modificar |
| **Error - Costo negativo** | SpecialtyId: {guid}<br>TreatmentId: {guid}<br>BaseCost: -500<br>Currency: "ARS" | Specialty y Treatment activos | DomainException<br>("Monto no válido") | - Validación de Money VO |

---

### DeactivateTreatment

| Escenario | Entrada (Command) | Estado Inicial | Resultado Esperado | Verificaciones |
|-----------|-------------------|----------------|-------------------|----------------|
| **Happy Path - Desactivar tratamiento** | SpecialtyId: {guid}<br>TreatmentId: {guid} | Specialty con 3 treatments activos | Void (éxito) | - Treatment.IsActive == false<br>- Otros 2 siguen activos<br>- Repository.Update llamado |
| **Error - Especialidad no encontrada** | SpecialtyId: {guid-inexistente}<br>TreatmentId: {guid} | Repositorio vacío | SpecialtyNotFoundException | - No se modifica |
| **Error - Tratamiento no encontrado** | SpecialtyId: {guid}<br>TreatmentId: {guid-inexistente} | Specialty existe | TreatmentNotFoundException | - No se modifica |
| **Error - Último tratamiento activo** | SpecialtyId: {guid}<br>TreatmentId: {guid} | Specialty con 1 solo treatment activo | LastActiveTreatmentException | - Invariante protegido<br>- IsActive sigue true |
| **Happy Path - Desactivar ya inactivo (idempotente)** | SpecialtyId: {guid}<br>TreatmentId: {guid} | Treatment ya inactivo (pero hay otros activos) | Void (éxito) | - Sin cambios<br>- Sin error |

---

### ActivateTreatment

| Escenario | Entrada (Command) | Estado Inicial | Resultado Esperado | Verificaciones |
|-----------|-------------------|----------------|-------------------|----------------|
| **Happy Path - Activar tratamiento** | SpecialtyId: {guid}<br>TreatmentId: {guid} | Specialty activa, Treatment inactivo | Void (éxito) | - Treatment.IsActive == true<br>- Repository.Update llamado |
| **Error - Especialidad no encontrada** | SpecialtyId: {guid-inexistente}<br>TreatmentId: {guid} | Repositorio vacío | SpecialtyNotFoundException | - No se modifica |
| **Error - Tratamiento no encontrado** | SpecialtyId: {guid}<br>TreatmentId: {guid-inexistente} | Specialty existe | TreatmentNotFoundException | - No se modifica |
| **Error - Especialidad inactiva** | SpecialtyId: {guid}<br>TreatmentId: {guid} | Specialty inactiva, Treatment inactivo | SpecialtyInactiveException | - No se puede activar treatment de specialty inactiva |
| **Happy Path - Activar ya activo (idempotente)** | SpecialtyId: {guid}<br>TreatmentId: {guid} | Treatment ya activo | Void (éxito) | - Sin cambios<br>- Sin error |

---

## Tests de Queries (Read Model)

### GetSpecialties

| Escenario | Entrada (Query) | Estado Inicial (DB) | Resultado Esperado | Verificaciones |
|-----------|-----------------|---------------------|-------------------|----------------|
| **Happy Path - Todas las especialidades** | SoloActivas: false | 3 specialties (2 activas, 1 inactiva) | Lista con 3 SpecialtySummaryDto | - Count == 3<br>- Contiene activas e inactivas<br>- DTO no incluye treatments |
| **Happy Path - Solo activas** | SoloActivas: true | 3 specialties (2 activas, 1 inactiva) | Lista con 2 SpecialtySummaryDto | - Count == 2<br>- Todas IsActive == true |
| **Happy Path - Sin datos** | SoloActivas: false | DB vacía | Lista vacía | - Count == 0<br>- Sin excepciones |

---

### GetSpecialtyById

| Escenario | Entrada (Query) | Estado Inicial (DB) | Resultado Esperado | Verificaciones |
|-----------|-----------------|---------------------|-------------------|----------------|
| **Happy Path - Specialty con treatments** | SpecialtyId: {guid} | Specialty "Ortodoncia" con 2 treatments | SpecialtyDetailDto con treatments | - SpecialtyId correcto<br>- Treatments.Count == 2<br>- Incluye TreatmentDto completo |
| **Error - No encontrada** | SpecialtyId: {guid-inexistente} | DB vacía | SpecialtyNotFoundException | - Null/excepción según implementación |

---

### GetActiveSpecialties

| Escenario | Entrada (Query) | Estado Inicial (DB) | Resultado Esperado | Verificaciones |
|-----------|-----------------|---------------------|-------------------|----------------|
| **Happy Path - Múltiples activas** | — | 4 specialties (3 activas, 1 inactiva) | Lista con 3 SpecialtySummaryDto | - Count == 3<br>- Todas IsActive == true |
| **Happy Path - Ninguna activa** | — | Solo specialties inactivas | Lista vacía | - Count == 0 |
| **Happy Path - Sin datos** | — | DB vacía | Lista vacía | - Count == 0 |

---

### GetTreatmentsBySpecialty

| Escenario | Entrada (Query) | Estado Inicial (DB) | Resultado Esperado | Verificaciones |
|-----------|-----------------|---------------------|-------------------|----------------|
| **Happy Path - Todos los treatments** | SpecialtyId: {guid}<br>SoloActivos: false | Specialty con 4 treatments (3 activos, 1 inactivo) | Lista con 4 TreatmentDto | - Count == 4<br>- Incluye activos e inactivos |
| **Happy Path - Solo activos** | SpecialtyId: {guid}<br>SoloActivos: true | Specialty con 4 treatments (3 activos, 1 inactivo) | Lista con 3 TreatmentDto | - Count == 3<br>- Todas IsActive == true |
| **Error - Specialty no encontrada** | SpecialtyId: {guid-inexistente}<br>SoloActivos: false | DB vacía | SpecialtyNotFoundException | - Null/excepción |
| **Happy Path - Specialty sin treatments** | SpecialtyId: {guid}<br>SoloActivos: false | Specialty válida pero sin treatments (caso edge) | Lista vacía | - Count == 0<br>- Sin error |

---

### GetTreatmentById

| Escenario | Entrada (Query) | Estado Inicial (DB) | Resultado Esperado | Verificaciones |
|-----------|-----------------|---------------------|-------------------|----------------|
| **Happy Path - Treatment existe** | SpecialtyId: {guid}<br>TreatmentId: {guid} | Treatment "Brackets metálicos" en Specialty | TreatmentDto completo | - TreatmentId correcto<br>- Code, Name, BaseCost, Currency presentes |
| **Error - Specialty no encontrada** | SpecialtyId: {guid-inexistente}<br>TreatmentId: {guid} | DB vacía | SpecialtyNotFoundException | - Null/excepción |
| **Error - Treatment no encontrado** | SpecialtyId: {guid}<br>TreatmentId: {guid-inexistente} | Specialty existe sin ese treatment | TreatmentNotFoundException | - Null/excepción |

---

## Implementación

### Estructura

```
Tests/
├── Application/
│   ├── Commands/
│   │   ├── CreateSpecialtyCommandHandlerTests.cs
│   │   ├── RenameSpecialtyCommandHandlerTests.cs
│   │   ├── AddTreatmentCommandHandlerTests.cs
│   │   └── ...
│   └── Queries/
│       ├── GetSpecialtiesQueryHandlerTests.cs
│       ├── GetSpecialtyByIdQueryHandlerTests.cs
│       └── ...
├── Domain/
│   ├── SpecialtyTests.cs
│   ├── TreatmentTests.cs
│   └── ValueObjects/
│       ├── NameTests.cs
│       └── MoneyTests.cs
└── TestHelpers/
    ├── SpecialtyBuilder.cs (Test Data Builder)
    └── TreatmentBuilder.cs
```

### Herramientas recomendadas
- **xUnit** o **NUnit** para tests
- **FluentAssertions** para aserciones legibles
- **Moq** o **NSubstitute** para mocks de repositorios
- **AutoFixture** (opcional) para generación de datos

### Patrón AAA (Arrange-Act-Assert)
Todos los tests deben seguir:
1. **Arrange**: Preparar estado inicial y dependencias
2. **Act**: Ejecutar el caso de uso
3. **Assert**: Verificar resultado y efectos secundarios