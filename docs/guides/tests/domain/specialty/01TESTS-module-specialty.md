# Escenarios de Test — **Dominio / Aggregate Specialty**

## Tests de creación del agregado

### `Specialty`
La especialidad nace en estado DRAFT, ya que sólo se cre la especialidad, pero aún no se le agrega el tratamiento (es un paso después). Se activa cuando se agrega el tratamiento.
No se prueba nombres, baseCost, códigos inválidos porque eso es responsabilidad de los VOs.

| Escenario                                  | Entrada                                    | Estado Inicial | Resultado Esperado         | Verificaciones                             |
| ------------------------------------------ | ------------------------------------------ | -------------- | -------------------------- | ------------------------------------------ |
| **Happy Path - Crear especialidad**        | Name: "Orthodontics", Description: "Test"  | —              | Specialty creada           | Status: Draft. Name, description           |


---

## Tests de estado del agregado

### `Rename`

| Escenario                         | Entrada              | Estado Inicial     | Resultado Esperado      | Verificaciones     |
| --------------------------------- | -------------------- | ------------------ | ----------------------- | ------------------ |
| **Happy Path - Renombrar activa** | Name: "Endodonthics" | Specialty activa   | Éxito                   | - Name actualizado |
| **Error - Renombrar archivada**    | Name "Endodonthics"  | Specialty archivada | `DomainRuleException`   | - Name no cambia   |

---

### `Archive`

| Escenario                                              | Entrada | Estado Inicial                            | Resultado Esperado | Verificaciones                                             |
| ------------------------------------------------------ | ------- | ----------------------------------------- | ------------------ | ---------------------------------------------------------- |
| **Happy Path - Archivar activa**                       | —       | Specialty activa con tratamientos activos | Éxito              | - Specialty.Status = ARCHIVE- Treatments.Status = ARCHIVED |
| **Happy Path - Archivar draft**                        | —       | Specialty draft                           | Éxito              | - Specialty.Status = ARCHIVE                               |
| **Happy Path - Archivar ya archivada (idempotente)**   | —       | Specialty archivada                       | Éxito              | - Estado no cambia                                         |

---

### `Activate`

| Escenario                                        | Entrada | Estado Inicial                                 | Resultado Esperado | Verificaciones                                              |
| ------------------------------------------------ | ------- | ---------------------------------------------- | ------------------ | ----------------------------------------------------------- |
| **Happy Path - Activar ARCHIVED**                | —       | Specialty archivada con tratamientos inactivos | Éxito              | - Specialty.Status = ACTIVE - Treatments.Status = ACTIVE    |
| **Happy Path - Activar ya activa (idempotente)** | —       | Specialty activa                               | Éxito              | - Sin cambios                                               |

---

## Tests de gestión de tratamientos (desde el agregado)

### `AddTreatment`

| Escenario                                    | Entrada                     | Estado Inicial                   | Resultado Esperado                       | Verificaciones                                    |
| -------------------------------------------- | --------------------------- | -------------------------------- | ---------------------------------------- | ------------------------------------------------- |
| **Happy Path - Agregar tratamiento inicial** | "01.01", "Cleaning",  20000 | Specialty en estado DRAFT        | Treatment agregado y Specialty activada  | - Treatments.Count = 1, Specialty.Status = ACTIVE |
| **Happy Path - Agregar tratamiento**         | "01.01", "Cleaning",  20000 | Specialty activa con 1 treatment | Treatment agregado                       | - Treatments.Count +1 - Specialty.Status = Active |
| **Error - Specialty archivada**               | "01.01", "Cleaning",  20000 | Specialty archivada               | `DomainRuleException`                    | - No se agrega                                    |
| **Error - Código duplicado**                 | Code existente              | Specialty activa                 | `DomainConflictException`                | - No se agrega                                    |

---

### `RenameTreatment`

| Escenario                                     | Entrada                          | Estado Inicial      | Resultado Esperado           | Verificaciones     |
| --------------------------------------------- | -------------------------------- | ------------------- | ---------------------------- | ------------------ |
| **Happy Path - Renombrar tratamiento activo** | TreatmentCode + Name             | Specialty activa    | Éxito                        | - Name actualizado |
| **Error - Specialty Archivada**               | TreatmentCode + Name             | Specialty archivada | `DomainRuleException`        | - Name no cambia   |
| **Error - Treatment no encontrado**           | TreatmentCode inexistente        | Specialty activa    | `DomainNotFoundException`    | - Nada cambia      |

---

### `ChangeTreatmentBaseCost`

| Escenario                           | Entrada                      | Estado Inicial      | Resultado Esperado        | Verificaciones         |
| ----------------------------------- | ---------------------------- | ------------------- | ------------------------- | ---------------------- |
| **Happy Path - Cambiar costo**      | TreatmentCode + Money válido | Specialty activa    | Éxito                     | - BaseCost actualizado |
| **Error - Specialty archivada**      | TreatmentCode + Money        | Specialty archivada | `DomainRuleException`     | - No se modifica       |
| **Error - Treatment no encontrado** | TreatmentCode inexistente    | Specialty activa    | `DomainNotFoundException` | - No se modifica       |

---

### `ArchiveTreatment`

| Escenario                                              | Entrada                   | Estado Inicial                             | Resultado Esperado          | Verificaciones                |
| ------------------------------------------------------ | ------------------------- | ------------------------------------------ | --------------------------- | ----------------------------- |
| **Happy Path - Archivar treatment**                    | TreatmentCode             | Specialty con ≥2 activos                   | Éxito                       | - Treatment.Status = ARCHIVED |
| **Happy Path - Desactivar ya archivado (idempotente)** | TreatmentCode             | Treatment ya archivado y hay otros activos | Éxito                       | - Sin cambios                 |
| **Error - Último treatment**                           | TreatmentCode             | Specialty con 1 solo activo                | `DomainRuleException`       | - Sigue activo                |
| **Error - Treatment no encontrado**                    | TreatmentCode inexistente | Specialty válida                           | `DomainNotFoundException`   | - Nada cambia                 |

---

### `ActivateTreatment`

| Escenario                                        | Entrada                   | Estado Inicial                        | Resultado Esperado        | Verificaciones               |
| ------------------------------------------------ | ------------------------- | ------------------------------------- | ------------------------- | ---------------------------- |
| **Happy Path - Activar treatment**               | TreatmentCode             | Specialty activa, treatment archivado | Éxito                     | - Treatment.Status = ACTIVE  |
| **Happy Path - Activar ya activo (idempotente)** | TreatmentCode             | Treatment activo                      | Éxito                     | - Sin cambios                |
| **Error - Specialty archivada**                   | TreatmentCode             | Specialty archivada                    | `DomainRuleException`     | - No se activa               |
| **Error - Treatment no encontrado**              | TreatmentCode inexistente | Specialty activa                      | `DomainNotFoundException` | - Nada cambia                |

---
