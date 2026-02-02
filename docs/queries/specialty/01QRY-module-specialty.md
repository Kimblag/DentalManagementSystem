# Queries – Módulo Especialidades / Tratamientos (Read Model)

| Query                    | Qué responde                         | Qué recibe                       | Qué retorna                  | Excepciones de Application           | Async | Por qué                          |
| ------------------------ | ------------------------------------ | -------------------------------- | ---------------------------- | ------------------------------------ | ----- | -------------------------------- |
| GetSpecialties           | Lista de especialidades              | Filtros opcionales (soloActivas) | Lista de SpecialtySummaryDto | Ninguna                              | Sí    | Acceso a datos                   |
| GetSpecialtyById         | Detalle completo de una especialidad | SpecialtyId                      | SpecialtyDetailDto           | SpecialtyNotFound                    | Sí    | IO + proyección                  |
| GetActiveSpecialties     | Especialidades activas               | —                                | Lista de SpecialtySummaryDto | Ninguna                              | Sí    | Query directa                    |
| GetTreatmentsBySpecialty | Tratamientos de una especialidad     | SpecialtyId, soloActivos         | Lista de TreatmentDto        | SpecialtyNotFound                    | Sí    | Join / filtro                    |
| GetTreatmentById         | Detalle de un tratamiento            | SpecialtyId, TreatmentId         | TreatmentDto                 | SpecialtyNotFound, TreatmentNotFound | Sí    | Tratamiento depende del agregado |


## DTOs – Read Model (Especialidades / Tratamientos)
| DTO                 | Propósito                          | Campos                                                | Notas                        |
| ------------------- | ---------------------------------- | ----------------------------------------------------- | ---------------------------- |
| SpecialtySummaryDto | Mostrar especialidades en listados | SpecialtyId, Name, IsActive                           | Sin tratamientos             |
| SpecialtyDetailDto  | Detalle completo de especialidad   | SpecialtyId, Name, IsActive, Treatments[]             | Usado en pantalla de detalle |
| TreatmentDto        | Representar un tratamiento         | TreatmentId, Code, Name, BaseCost, Currency, IsActive | DTO plano                    |
| TreatmentSummaryDto | Listados simples de tratamientos   | TreatmentId, Code, Name, IsActive                     | Sin costos                   |


## Puertos de lectura (Application → Outbound)
| Puerto                   | Responsabilidad           | Métodos expuestos          | Retorna              | Notas                         |
| ------------------------ | ------------------------- | -------------------------- | -------------------- | ----------------------------- |
| ISpecialtyReadRepository | Queries de especialidades | GetAll, GetById, GetActive | DTOs de especialidad | No devuelve agregados         |
| ITreatmentReadRepository | Queries de tratamientos   | GetBySpecialty, GetById    | DTOs de tratamiento  | Siempre ligado a especialidad |
