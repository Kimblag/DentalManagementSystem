# Dominio
Se documentan las excepciones que pueden ser lanzadas dentro del dominio. Es importante recalcar esta será la última barrera de defensa, si algo mal llega hasta este punto es porque la capa de Application no validó correctamente y/o hay un bug.

## Excepción base
- DomainException: Clase de excepción base para excepciones del dominio.

### Excepciones Derivadas
Dependiendo del error tenemos que lanzar el status code correcto en la API para cumplir con la normativa para crear APIs REST. Se usarán excepciones generales que puedan aplicar a todas las entidades del dominio y que se mapeen a un código de estado específico. 


| Excepción de Dominio | Caso de Uso (Ejemplos) | Mapeo futuro (Status Code) |
| :--- | :--- | :--- |
| **`DomainValidationException`** | Un VO falló (Nombre vacío, Dinero negativo, Código mal formado). | `422 Unprocessable Entity` |
| **`DomainConflictException`** | Intentaste agregar un Tratamiento que ya existe en la lista de la Especialidad. | `409 Conflict` |
| **`DomainRuleException`** | Intentaste activar una Especialidad sin tratamientos (Violación de estado). | `422 o 400` |
| **`DomainNotFoundException`** | Buscaste un hijo (Tratamiento) dentro del Agregado y no estaba. | `404 Not Found` |

---
