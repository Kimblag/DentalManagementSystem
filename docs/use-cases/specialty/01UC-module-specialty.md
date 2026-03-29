# Casos de uso: Specialty

| Caso de uso | Qué hace | Qué recibe (Input DTO) | Qué retorna | Excepciones de Application | Async |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **`CreateSpecialty`** | Crea una especialidad vacía en estado DRAFT. | `Name` (string), `Description` (string?) | `SpecialtyResponse` | `ApplicationConflictException` (si el nombre ya existe). | Sí |
| **`AddTreatments`** | Agrega uno o varios tratamientos, activando la especialidad si estaba en DRAFT. | `SpecialtyId` (Guid), `List<TreatmentDto>` (code, name, description, amount, currency) | `SpecialtyResponse` | `ApplicationNotFoundException` (si no existe la especialidad), `ApplicationConflictException` (si el código ya existe a nivel global). | Sí |
| **`RenameSpecialty`** | Cambia el nombre de la especialidad. | `SpecialtyId` (Guid), `Name` (string) | `SpecialtyResponse` | `ApplicationNotFoundException`, `ApplicationConflictException` (si el nuevo nombre ya está en uso). | Sí |
| **`UpdateSpecialtyDescription`** | Actualiza o limpia la descripción de la especialidad. | `SpecialtyId` (Guid), `Description` (string?) | `SpecialtyResponse` | `ApplicationNotFoundException` | Sí |
| **`ArchiveSpecialty`** | Archiva la especialidad y todos sus tratamientos en cascada. | `SpecialtyId` (Guid) | `SpecialtyResponse` | `ApplicationNotFoundException` | Sí |
| **`ActivateSpecialty`** | Reactiva la especialidad y sus tratamientos. | `SpecialtyId` (Guid) | `SpecialtyResponse` | `ApplicationNotFoundException` | Sí |
| **`RenameTreatment`** | Cambia el nombre de un tratamiento específico. | `SpecialtyId` (Guid), `TreatmentCode` (string), `Name` (string) | `SpecialtyResponse` | `ApplicationNotFoundException` (si no existe la especialidad). | Sí |
| **`UpdateTreatmentDescription`** | Actualiza o limpia la descripción de un tratamiento. | `SpecialtyId` (Guid), `TreatmentCode` (string), `Description` (string?) | `SpecialtyResponse` | `ApplicationNotFoundException` | Sí |
| **`UpdateTreatmentBaseCost`** | Cambia el precio y moneda de un tratamiento. | `SpecialtyId` (Guid), `TreatmentCode` (string), `Amount` (decimal), `Currency` (string) | `SpecialtyResponse` | `ApplicationNotFoundException` | Sí |
| **`ArchiveTreatment`** | Archiva un tratamiento específico. | `SpecialtyId` (Guid), `TreatmentCode` (string) | `SpecialtyResponse` | `ApplicationNotFoundException` | Sí |
| **`ActivateTreatment`** | Reactiva un tratamiento específico. | `SpecialtyId` (Guid), `TreatmentCode` (string) | `SpecialtyResponse` | `ApplicationNotFoundException` | Sí |

---