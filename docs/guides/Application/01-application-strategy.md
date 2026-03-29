# Data Transfer Objects (DTOs) - Specialty

**Contexto:** Estos son los objetos de transporte de datos que Aplicación usará para comunicarse con el exterior (Controladores/API). Se dividen en DTOs de Entrada (Requests) que incluyen las validaciones estructurales básicas, y DTOs de Salida (Responses) para lectura.
# Data Transfer Objects (DTOs) - Specialty

**Contexto:** Estos son los objetos de transporte de datos utilizados por la capa de Aplicación para comunicarse con el exterior (Controladores/API). Se dividen en DTOs de Entrada (Requests) que incluyen las validaciones estructurales básicas, y DTOs de Salida (Responses) para lectura. Todos se implementan como `records` posicionales inmutables.

## 1. DTOs de Entrada (Input / Requests)
Se utilizan como parámetros en los Casos de Uso. Las validaciones marcadas deben cumplirse antes de invocar al Dominio o a la base de datos.

| Nombre del DTO | Propiedades y Tipos Exactos | Restricciones (Constraints) |
| :--- | :--- | :--- |
| **`CreateSpecialtyRequest`** | `string Name`<br>`string? Description` | **Name:** Requerido, máx. 100 caracteres.<br>**Description:** Opcional, máx. 250 caracteres. |
| **`AddTreatmentsRequest`** | `Guid SpecialtyId`<br>`List<TreatmentItemDto> Treatments` | **SpecialtyId:** Requerido.<br>**Treatments:** Requerido, mínimo 1 elemento. |
| **`TreatmentItemDto`** | `string Code`<br>`string Name`<br>`string? Description`<br>`decimal Amount`<br>`string Currency` | **Code:** Requerido, máx. 7 caracteres.<br>**Name:** Requerido, máx. 100 caracteres.<br>**Description:** Opcional, máx. 250 caracteres.<br>**Amount:** Requerido, mayor a 0.<br>**Currency:** Requerido, exactos 3 caracteres. |
| **`RenameSpecialtyRequest`** | `Guid SpecialtyId`<br>`string Name` | **SpecialtyId:** Requerido.<br>**Name:** Requerido, máx. 100 caracteres. |
| **`UpdateSpecialtyDescRequest`**| `Guid SpecialtyId`<br>`string? Description` | **SpecialtyId:** Requerido.<br>**Description:** Opcional (permite nulos), máx. 250 caracteres. |
| **`ArchiveSpecialtyRequest`** | `Guid SpecialtyId` | **SpecialtyId:** Requerido. |
| **`ActivateSpecialtyRequest`** | `Guid SpecialtyId` | **SpecialtyId:** Requerido. |
| **`RenameTreatmentRequest`** | `Guid SpecialtyId`<br>`string TreatmentCode`<br>`string Name` | **SpecialtyId:** Requerido.<br>**TreatmentCode:** Requerido, máx. 7 caracteres.<br>**Name:** Requerido, máx. 100 caracteres. |
| **`UpdateTreatmentDescRequest`**| `Guid SpecialtyId`<br>`string TreatmentCode`<br>`string? Description` | **SpecialtyId:** Requerido.<br>**TreatmentCode:** Requerido, máx. 7 caracteres.<br>**Description:** Opcional (permite nulos), máx. 250 caracteres. |
| **`UpdateTreatmentCostRequest`**| `Guid SpecialtyId`<br>`string TreatmentCode`<br>`decimal Amount`<br>`string Currency` | **SpecialtyId:** Requerido.<br>**TreatmentCode:** Requerido, máx. 7 caracteres.<br>**Amount:** Requerido, mayor a 0.<br>**Currency:** Requerido, exactos 3 caracteres. |
| **`ArchiveTreatmentRequest`** | `Guid SpecialtyId`<br>`string TreatmentCode` | **SpecialtyId:** Requerido.<br>**TreatmentCode:** Requerido, máx. 7 caracteres. |
| **`ActivateTreatmentRequest`** | `Guid SpecialtyId`<br>`string TreatmentCode` | **SpecialtyId:** Requerido.<br>**TreatmentCode:** Requerido, máx. 7 caracteres. |

---

## 2. DTOs de Salida (Output / Responses)
Representan la vista de lectura estandarizada. Se omiten por completo los Value Objects y las Entidades de Dominio; los datos se exponen utilizando exclusivamente tipos primitivos de C#.

| Nombre del DTO | Propiedades y Tipos Exactos | Descripción del Dato |
| :--- | :--- | :--- |
| **`SpecialtyResponse`** | `Guid Id`<br>`string Name`<br>`string? Description`<br>`string Status`<br>`List<TreatmentResponse> Treatments` | Identificador único.<br>Nombre de la especialidad.<br>Descripción de la especialidad o nulo.<br>Enum convertido a texto (ej: "ACTIVE", "DRAFT").<br>Colección de tratamientos asociados. |
| **`TreatmentResponse`** | `string Code`<br>`string Name`<br>`string? Description`<br>`decimal BaseCost_Amount`<br>`string BaseCost_Currency`<br>`string Status` | Código identificador (ej: "01.01").<br>Nombre del tratamiento.<br>Descripción del tratamiento o nulo.<br>Valor numérico del costo base.<br>Moneda del costo base (ej: "USD").<br>Enum convertido a texto (ej: "ACTIVE", "ARCHIVED"). |

# Interfaces (Puertos) - Capa de Aplicación

**Contexto:** Se definen los contratos (interfaces) que la capa de Aplicación requiere para interactuar con la persistencia de datos. Estas interfaces garantizan el principio de Inversión de Dependencias, evitando el acoplamiento estricto con frameworks externos (como Entity Framework Core). La capa de Infraestructura será la responsable de implementar estos contratos.

## 1. Manejo de Transacciones (Unit of Work)
Contrato genérico para confirmar las operaciones en la base de datos de forma atómica.

| Interfaz | Método | Parámetros | Retorno | Propósito |
| :--- | :--- | :--- | :--- | :--- |
| **`IUnitOfWork`** | `SaveChangesAsync` | `CancellationToken` (opcional) | `Task<int>` | Confirma todos los cambios acumulados (Add, Update) en una sola transacción hacia la base de datos. Actúa como escudo protector para no depender de `DbContext`. |

---

## 2. Repositorios (Acceso a Datos)
Contratos para acceder y modificar las Entidades Raíz (Agregados). Operan exclusivamente con entidades de Dominio (`Specialty`), nunca con DTOs.

| Interfaz | Método | Parámetros | Retorno | Propósito |
| :--- | :--- | :--- | :--- | :--- |
| **`ISpecialtyRepository`** | `GetByIdAsync` | `Guid id` | `Task<Specialty?>` | Recupera la especialidad completa, **incluyendo obligatoriamente su lista de tratamientos**, para poder aplicar lógica de negocio. Retorna `null` si no se encuentra. |
| | `ExistsByNameAsync` | `string name` | `Task<bool>` | Verifica en la base de datos si el nombre especificado ya está en uso. Esencial para validar conflictos (`409 Conflict`) antes de la creación o el renombramiento. |
| | `ExistsTreatmentCodeAsync` | `string code` | `Task<bool>` | Verifica de forma global si el código de un tratamiento ya existe en la base de datos, asegurando la unicidad del identificador. |
| | `AddAsync` | `Specialty specialty` | `Task` | Agrega una nueva entidad al contexto de seguimiento en memoria para su posterior inserción. |
| | `UpdateAsync` | `Specialty specialty` | `Task` | Marca la entidad existente (o sus colecciones internas modificadas) para ser actualizada en la base de datos. |



---


# Mapeadores (Mappers) - Capa de Aplicación

**Contexto:** Para mantener la arquitectura simple (KISS) y evitar dependencias de librerías externas (como AutoMapper), la transformación de Entidades de Dominio a DTOs de salida se realiza de forma manual. Se utilizan **métodos de extensión estáticos** para mantener el código limpio, fuertemente tipado y con un rendimiento óptimo (cero reflection).

## Clases de Mapeo
Estas clases estáticas centralizan la lógica de traducción, asegurando que los Casos de Uso no se ensucien con asignaciones manuales propiedad por propiedad.

| Clase (Estática) | Método de Extensión | Entidad Origen | DTO Destino | Detalles de Transformación |
| :--- | :--- | :--- | :--- | :--- |
| **`SpecialtyMapper`** | `ToDto(this Specialty specialty)` | `Specialty` | `SpecialtyResponse` | Extrae el texto nativo del Value Object `Name` (`Name.Value`). Convierte el Enum `Status` a cadena de texto (`.ToString()`). Ejecuta un `.Select(t => t.ToDto())` sobre la colección de tratamientos. |
| **`TreatmentMapper`** | `ToDto(this Treatment treatment)` | `Treatment` | `TreatmentResponse` | Extrae los valores nativos de `Code` y `Name`. Descompone el Value Object `Money` separando sus propiedades en `BaseCost_Amount` y `BaseCost_Currency`. Convierte el Enum `Status` a texto. |

---

### Ejemplo de Implementación (Referencia Técnica)
La estructura estándar a seguir en el código para estos mapeadores es la siguiente:

```csharp
public static class SpecialtyMapper
{
    public static SpecialtyResponse ToDto(this Specialty specialty)
    {
        return new SpecialtyResponse
        {
            Id = specialty.Id,
            Name = specialty.Name.Value, // Extracción del VO
            Description = specialty.Description,
            Status = specialty.Status.ToString(), // Enum a String
            Treatments = specialty.Treatments.Select(t => t.ToDto()).ToList() // Mapeo en cascada
        };
    }
}