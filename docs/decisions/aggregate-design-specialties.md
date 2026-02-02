# Specialties (Aggregate Root)
Representa la gestión del módulo de especialidades y su tratamiento.

## Specialty
### Atributos
- Id: GUID (Identidad interna de la clínica)
- Name: VO (único en el sistema)
- isActive: boolean (estado lógico)
- Treatments: Collection<Treatment>

### Comportamientos
- Create(name: Name, treatments: Collection<NewTreatmentData>)
    * Crea una especialidad con uno o más tratamientos iniciales.
    * Los tratamientos se instancian internamente.
    * Valida que exista al menos un tratamiento activo.

- Rename(name: Name)
    * Corrige el nombre de la especialidad.
    * No permitido si está inactiva.

- AddTreatment(data: NewTreatmentData)
    * Agrega un tratamiento a la especialidad.
    * No permitido si la especialidad está inactiva.
    * Valida unicidad de nombre y código dentro de la especialidad.

- RenameTreatment(treatmentId: UUID, name: Name)
    * Coordina el renombrado de un tratamiento.
    * No permitido si la especialidad está inactiva.

- ChangeTreatmentBaseCost(treatmentId: UUID, baseCost: Money)
    * Coordina el cambio de costo base de un tratamiento.
    * No permitido si la especialidad está inactiva.

- DeactivateTreatment(treatmentId: UUID)
    * Coordina la desactivación de un tratamiento.
    * No permitido si es el último tratamiento activo.

- ActivateTreatment(treatmentId: UUID)
    * Coordina la activación de un tratamiento.
    * No permitido si la especialidad está inactiva.

- Deactivate()
    * Desactiva la especialidad.
    * Desactiva todos sus tratamientos.

- Activate()
    * Reactiva la especialidad.
    * Reactiva todos sus tratamientos.

## Treatment
- Id: UUID - Identidad interna dentro del agregado.
- Code: string - Código del nomenclador odontológico. - Inmutable y único dentro de la especialidad.
Name: Name (VO) Nombre del tratamiento. Único dentro de la especialidad.
- BaseCost: Money (VO) - Costo base del tratamiento. Puede ser 0, no negativo, con moneda.
- IsActive: boolean - Estado lógico.

### Comportamientos
- Rename(name: Name)
    * Corrige el nombre del tratamiento.
    * No permitido si el tratamiento está inactivo.

- ChangeBaseCost(baseCost: Money)
    * Actualiza el costo base.
    * No permitido si el tratamiento está inactivo.

- Deactivate()
    * Desactiva el tratamiento.
    * Solo puede ejecutarse si el agregado lo permite.

- Activate()
    * Reactiva el tratamiento.



## Invariantes del agregado
- No existe especialidad sin al menos un tratamiento activo
- No se puede desactivar el último tratamiento activo
- Nombre de tratamiento único dentro de la especialidad
- Código de tratamiento único dentro de la especialidad
- No se pueden modificar entidades inactivas
- El código del tratamiento no se modifica
- Tratamiento no existe fuera de Especialidad
- Todo cambio en Tratamiento pasa por Especialidad


## Tabla de comportamiento
### Aggregate Root: Specialty
| Comportamiento              | Entradas                                                   | Salida        | Excepciones                                                                      |
| --------------------------- | ---------------------------------------------------------- | ------------- | -------------------------------------------------------------------------------- |
| **Create**                  | `name: Name`<br>`treatments: Collection<NewTreatmentData>` | `Specialty`   | `EmptyTreatmentList`                                                             |
| **Rename**                  | `name: Name`                                               | `void`        | `EntityInactive`                                                                 |
| **AddTreatment**            | `data: NewTreatmentData`                                   | `TreatmentId` | `EntityInactive`<br>`TreatmentCodeAlreadyExists`<br>`TreatmentNameAlreadyExists` |
| **RenameTreatment**         | `treatmentId: UUID`<br>`name: Name`                        | `void`        | `EntityInactive`<br>`TreatmentNotFound`                                          |
| **ChangeTreatmentBaseCost** | `treatmentId: UUID`<br>`baseCost: Money`                   | `void`        | `EntityInactive`<br>`TreatmentNotFound`                                          |
| **DeactivateTreatment**     | `treatmentId: UUID`                                        | `void`        | `TreatmentNotFound`<br>`CannotRemoveLastTreatment`                               |
| **ActivateTreatment**       | `treatmentId: UUID`                                        | `void`        | `EntityInactive`<br>`TreatmentNotFound`                                          |
| **Deactivate**              | —                                                          | `void`        | —                                                                                |
| **Activate**                | —                                                          | `void`        | —                                                                                |

### Treatment
| Comportamiento     | Entradas          | Salida | Excepciones      |
| ------------------ | ----------------- | ------ | ---------------- |
| **Rename**         | `name: Name`      | `void` | `EntityInactive` |
| **ChangeBaseCost** | `baseCost: Money` | `void` | `EntityInactive` |
| **Deactivate**     | —                 | `void` | —                |
| **Activate**       | —                 | `void` | —                |
