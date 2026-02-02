# Escenarios de Test — **Dominio / Aggregate Specialty**

## Tests de creación del agregado

### `Specialty.Create`

| Escenario                                              | Entrada                               | Estado Inicial | Resultado Esperado           | Verificaciones                                                                |
| ------------------------------------------------------ | ------------------------------------- | -------------- | ---------------------------- | ----------------------------------------------------------------------------- |
| **Happy Path - Crear con un tratamiento**              | Name válido<br>1 `NewTreatmentData`   | —              | Specialty creada             | - IsActive == true<br>- Treatments.Count == 1<br>- Treatment.IsActive == true |
| **Happy Path - Crear con múltiples tratamientos**      | Name válido<br>2+ `NewTreatmentData`  | —              | Specialty creada             | - Treatments.Count correcto<br>- Todos activos                                |
| **Error - Lista de tratamientos vacía**                | Name válido<br>Treatments: []         | —              | `EmptyTreatmentList`         | - No se crea el agregado                                                      |
| **Error - Código duplicado en tratamientos iniciales** | Dos `NewTreatmentData` con mismo Code | —              | `TreatmentCodeAlreadyExists` | - No se crea el agregado                                                      |
| **Error - Nombre duplicado en tratamientos iniciales** | Dos `NewTreatmentData` con mismo Name | —              | `TreatmentNameAlreadyExists` | - No se crea el agregado                                                      |


---

## Tests de estado del agregado

### `Rename`

| Escenario                         | Entrada     | Estado Inicial     | Resultado Esperado | Verificaciones     |
| --------------------------------- | ----------- | ------------------ | ------------------ | ------------------ |
| **Happy Path - Renombrar activa** | Name válido | Specialty activa   | Éxito              | - Name actualizado |
| **Error - Renombrar inactiva**    | Name válido | Specialty inactiva | `EntityInactive`   | - Name no cambia   |

---

### `Deactivate`

| Escenario                                             | Entrada | Estado Inicial                            | Resultado Esperado | Verificaciones                                                            |
| ----------------------------------------------------- | ------- | ----------------------------------------- | ------------------ | ------------------------------------------------------------------------- |
| **Happy Path - Desactivar activa**                    | —       | Specialty activa con tratamientos activos | Éxito              | - Specialty.IsActive == false<br>- Todos los Treatments.IsActive == false |
| **Happy Path - Desactivar ya inactiva (idempotente)** | —       | Specialty inactiva                        | Éxito              | - Estado no cambia                                                        |

---

### `Activate`

| Escenario                                        | Entrada | Estado Inicial                                | Resultado Esperado | Verificaciones                                                          |
| ------------------------------------------------ | ------- | --------------------------------------------- | ------------------ | ----------------------------------------------------------------------- |
| **Happy Path - Activar inactiva**                | —       | Specialty inactiva con tratamientos inactivos | Éxito              | - Specialty.IsActive == true<br>- Todos los Treatments.IsActive == true |
| **Happy Path - Activar ya activa (idempotente)** | —       | Specialty activa                              | Éxito              | - Sin cambios                                                           |

---

## Tests de gestión de tratamientos (desde el agregado)

### `AddTreatment`

| Escenario                            | Entrada                   | Estado Inicial                   | Resultado Esperado           | Verificaciones                                    |
| ------------------------------------ | ------------------------- | -------------------------------- | ---------------------------- | ------------------------------------------------- |
| **Happy Path - Agregar tratamiento** | `NewTreatmentData` válido | Specialty activa con 1 treatment | Treatment agregado           | - Treatments.Count +1<br>- Nuevo IsActive == true |
| **Error - Specialty inactiva**       | `NewTreatmentData` válido | Specialty inactiva               | `EntityInactive`             | - No se agrega                                    |
| **Error - Código duplicado**         | Code existente            | Specialty activa                 | `TreatmentCodeAlreadyExists` | - No se agrega                                    |
| **Error - Nombre duplicado**         | Name existente            | Specialty activa                 | `TreatmentNameAlreadyExists` | - No se agrega                                    |

---

### `RenameTreatment`

| Escenario                                     | Entrada                          | Estado Inicial     | Resultado Esperado           | Verificaciones     |
| --------------------------------------------- | -------------------------------- | ------------------ | ---------------------------- | ------------------ |
| **Happy Path - Renombrar tratamiento activo** | TreatmentId + Name válido        | Specialty activa   | Éxito                        | - Name actualizado |
| **Error - Specialty inactiva**                | TreatmentId + Name               | Specialty inactiva | `EntityInactive`             | - Name no cambia   |
| **Error - Treatment no encontrado**           | TreatmentId inexistente          | Specialty activa   | `TreatmentNotFound`          | - Nada cambia      |
| **Error - Nombre duplicado**                  | Name ya usado por otro treatment | Specialty activa   | `TreatmentNameAlreadyExists` | - No se renombra   |

---

### `ChangeTreatmentBaseCost`

| Escenario                           | Entrada                    | Estado Inicial     | Resultado Esperado  | Verificaciones         |
| ----------------------------------- | -------------------------- | ------------------ | ------------------- | ---------------------- |
| **Happy Path - Cambiar costo**      | TreatmentId + Money válido | Specialty activa   | Éxito               | - BaseCost actualizado |
| **Error - Specialty inactiva**      | TreatmentId + Money        | Specialty inactiva | `EntityInactive`    | - No se modifica       |
| **Error - Treatment no encontrado** | TreatmentId inexistente    | Specialty activa   | `TreatmentNotFound` | - No se modifica       |

---

### `DeactivateTreatment`

| Escenario                                             | Entrada                 | Estado Inicial                            | Resultado Esperado          | Verificaciones                |
| ----------------------------------------------------- | ----------------------- | ----------------------------------------- | --------------------------- | ----------------------------- |
| **Happy Path - Desactivar treatment**                 | TreatmentId             | Specialty con ≥2 activos                  | Éxito                       | - Treatment.IsActive == false |
| **Happy Path - Desactivar ya inactivo (idempotente)** | TreatmentId             | Treatment ya inactivo y hay otros activos | Éxito                       | - Sin cambios                 |
| **Error - Último treatment**                          | TreatmentId             | Specialty con 1 solo activo               | `CannotRemoveLastTreatment` | - Sigue activo                |
| **Error - Treatment no encontrado**                   | TreatmentId inexistente | Specialty válida                          | `TreatmentNotFound`         | - Nada cambia                 |

---

### `ActivateTreatment`

| Escenario                                        | Entrada                 | Estado Inicial                       | Resultado Esperado  | Verificaciones               |
| ------------------------------------------------ | ----------------------- | ------------------------------------ | ------------------- | ---------------------------- |
| **Happy Path - Activar treatment**               | TreatmentId             | Specialty activa, treatment inactivo | Éxito               | - Treatment.IsActive == true |
| **Happy Path - Activar ya activo (idempotente)** | TreatmentId             | Treatment activo                     | Éxito               | - Sin cambios                |
| **Error - Specialty inactiva**                   | TreatmentId             | Specialty inactiva                   | `EntityInactive`    | - No se activa               |
| **Error - Treatment no encontrado**              | TreatmentId inexistente | Specialty activa                     | `TreatmentNotFound` | - Nada cambia                |

---

## Tests directos de `Treatment` (entidad interna)


### `Rename`

| Escenario  | Entrada     | Estado Inicial     | Resultado Esperado |
| ---------- | ----------- | ------------------ | ------------------ |
| Happy Path | Name válido | Treatment activo   | Name actualizado   |
| Error      | Name válido | Treatment inactivo | `EntityInactive`   |

---

### `ChangeBaseCost`

| Escenario  | Entrada      | Estado Inicial     | Resultado Esperado   |
| ---------- | ------------ | ------------------ | -------------------- |
| Happy Path | Money válido | Treatment activo   | BaseCost actualizado |
| Error      | Money válido | Treatment inactivo | `EntityInactive`     |

---
