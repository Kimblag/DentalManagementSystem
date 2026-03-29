# Application
Se documentan las excepciones que pueden ser lanzadas dentro de Application.

## Excepción base
- `ApplicationException`: Clase de excepción base para las excepciones de la capa de aplicación.

### Excepciones derivadas
Dependiendo del error tenemos que lanzar el status code correcto en la API para cumplir con la normativa para crear APIs REST. Se usarán excepciones generales que puedan aplicar a todas las entidades manejadas por los Casos de Uso y que se mapeen a un código de estado específico. No se atrapan excepciones nativas de base de datos (ej. `SqlException`); esas fluirán hacia un manejador global para retornar `500 Internal Server Error`.

| Excepción de Aplicación | Caso de Uso (Ejemplos) | Mapeo futuro (Status Code) |
| :--- | :--- | :--- |
| **`ApplicationNotFoundException`** | El repositorio buscó el Agregado Raíz (`Specialty`) por su ID y devolvió `null`. | `404 Not Found` |
| **`ApplicationConflictException`** | Intentar crear una `Specialty` con un nombre que **ya existe en la base de datos** (Regla de unicidad que el Dominio no puede validar por sí solo). | `409 Conflict` |
| **`ApplicationValidationException`** | El DTO de entrada (`Request`) no pasó validaciones estructurales básicas (ej. el cliente envió un JSON mal formado o faltó un ID obligatorio) antes de llegar al Dominio. | `400 Bad Request` |