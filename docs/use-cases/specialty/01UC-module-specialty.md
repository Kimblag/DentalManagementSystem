# Casos de uso: Specialty

| Caso de uso             | Qué hace                                         | Qué recibe                                                                          | Qué retorna           | Excepciones de Application                                | Async | Por qué                                  |
| ----------------------- | ------------------------------------------------ | ----------------------------------------------------------------------------------- | --------------------- | --------------------------------------------------------- | ----- | ---------------------------------------- |
| CreateSpecialty         | Crea una especialidad con tratamientos iniciales | Name (string), colección de tratamientos iniciales (code, name, baseCost, currency) | Id de la especialidad | SpecialtyAlreadyExists                                    | Sí    | Persistencia + verificación de unicidad  |
| RenameSpecialty         | Cambia el nombre de la especialidad              | SpecialtyId, Name                                                                   | Nada                  | SpecialtyNotFound, SpecialtyInactive                      | Sí    | Carga y guarda agregado                  |
| ActivateSpecialty       | Reactiva especialidad y tratamientos             | SpecialtyId                                                                         | Nada                  | SpecialtyNotFound                                         | Sí    | Persistencia                             |
| DeactivateSpecialty     | Desactiva especialidad y todos los tratamientos  | SpecialtyId                                                                         | Nada                  | SpecialtyNotFound                                         | Sí    | Persistencia                             |
| AddTreatmentToSpecialty | Agrega un tratamiento a la especialidad          | SpecialtyId, data del tratamiento                                                   | TreatmentId           | SpecialtyNotFound, SpecialtyInactive                      | Sí    | Persistencia + validación de unicidad    |
| RenameTreatment         | Cambia nombre de un tratamiento                  | SpecialtyId, TreatmentId, Name                                                      | Nada                  | SpecialtyNotFound, TreatmentNotFound, SpecialtyInactive   | Sí    | Tratamiento no existe fuera del agregado |
| ChangeTreatmentBaseCost | Cambia costo base de un tratamiento              | SpecialtyId, TreatmentId, Money                                                     | Nada                  | SpecialtyNotFound, TreatmentNotFound, SpecialtyInactive   | Sí    | Reglas viven en dominio                  |
| DeactivateTreatment     | Desactiva un tratamiento                         | SpecialtyId, TreatmentId                                                            | Nada                  | SpecialtyNotFound, TreatmentNotFound, LastActiveTreatment | Sí    | Invariante del agregado                  |
| ActivateTreatment       | Reactiva un tratamiento                          | SpecialtyId, TreatmentId                                                            | Nada                  | SpecialtyNotFound, TreatmentNotFound, SpecialtyInactive   | Sí    | Persistencia                             |


Correcto: **sí**, los casos de uso **tienen DTOs**.
Y no es opcional según la bibliografía (Clean Architecture / DDD táctico):
el **Use Case Input Model** existe para **aislar el dominio de los mecanismos externos**.

Fallo mío antes no explicitarlo. Vamos a corregirlo **bien y completo**, sin ruido.

---

# DTOs de entrada – Casos de uso (Application / Commands)

---

## CreateSpecialty

| DTO                    | Campo      | Tipo                        | Obligatorio | Observaciones                            |
| ---------------------- | ---------- | --------------------------- | ----------- | ---------------------------------------- |
| CreateSpecialtyCommand | Name       | string                      | Sí          | Sin validar reglas acá                   |
|                        | Treatments | Collection<NewTreatmentDto> | Sí          | Debe venir al menos uno (dominio valida) |

**NewTreatmentDto**

| Campo    | Tipo    | Obligatorio | Observaciones                 |
| -------- | ------- | ----------- | ----------------------------- |
| Code     | string  | Sí          | Inmutable, dominio lo protege |
| Name     | string  | Sí          | Unicidad se valida en dominio |
| BaseCost | decimal | Sí          | ≥ 0, dominio valida           |
| Currency | string  | Sí          | ISO / string plano            |

---

## RenameSpecialty

| DTO                    | Campo       | Tipo   | Obligatorio | Observaciones               |
| ---------------------- | ----------- | ------ | ----------- | --------------------------- |
| RenameSpecialtyCommand | SpecialtyId | Guid   | Sí          | Identidad                   |
|                        | Name        | string | Sí          | Dominio decide si es válido |

---

## ActivateSpecialty

| DTO                      | Campo       | Tipo | Obligatorio | Observaciones |
| ------------------------ | ----------- | ---- | ----------- | ------------- |
| ActivateSpecialtyCommand | SpecialtyId | Guid | Sí          | Sin más datos |

---

## DeactivateSpecialty

| DTO                        | Campo       | Tipo | Obligatorio | Observaciones                |
| -------------------------- | ----------- | ---- | ----------- | ---------------------------- |
| DeactivateSpecialtyCommand | SpecialtyId | Guid | Sí          | Cascada la maneja el dominio |

---

## AddTreatmentToSpecialty

| DTO                 | Campo       | Tipo    | Obligatorio | Observaciones          |
| ------------------- | ----------- | ------- | ----------- | ---------------------- |
| AddTreatmentCommand | SpecialtyId | Guid    | Sí          | Aggregate root         |
|                     | Code        | string  | Sí          | Único por especialidad |
|                     | Name        | string  | Sí          |                        |
|                     | BaseCost    | decimal | Sí          |                        |
|                     | Currency    | string  | Sí          |                        |

---

## RenameTreatment

| DTO                    | Campo       | Tipo   | Obligatorio | Observaciones                     |
| ---------------------- | ----------- | ------ | ----------- | --------------------------------- |
| RenameTreatmentCommand | SpecialtyId | Guid   | Sí          | Siempre se navega por el agregado |
|                        | TreatmentId | Guid   | Sí          |                                   |
|                        | Name        | string | Sí          |                                   |

---

## ChangeTreatmentBaseCost

| DTO                            | Campo       | Tipo    | Obligatorio | Observaciones |
| ------------------------------ | ----------- | ------- | ----------- | ------------- |
| ChangeTreatmentBaseCostCommand | SpecialtyId | Guid    | Sí          |               |
|                                | TreatmentId | Guid    | Sí          |               |
|                                | BaseCost    | decimal | Sí          |               |
|                                | Currency    | string  | Sí          |               |

---

## DeactivateTreatment

| DTO                        | Campo       | Tipo | Obligatorio | Observaciones                  |
| -------------------------- | ----------- | ---- | ----------- | ------------------------------ |
| DeactivateTreatmentCommand | SpecialtyId | Guid | Sí          |                                |
|                            | TreatmentId | Guid | Sí          | Dominio valida “último activo” |

---

## ActivateTreatment

| DTO                      | Campo       | Tipo | Obligatorio | Observaciones |
| ------------------------ | ----------- | ---- | ----------- | ------------- |
| ActivateTreatmentCommand | SpecialtyId | Guid | Sí          |               |
|                          | TreatmentId | Guid | Sí          |               |

