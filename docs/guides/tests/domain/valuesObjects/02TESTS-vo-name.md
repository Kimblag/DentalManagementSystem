## Tests — `Name` (Value Object)

### Creación

| Escenario                            | Entrada               | Resultado Esperado | Verificaciones          |
| ------------------------------------ | --------------------- | ------------------ | ----------------------- |
| **Happy Path - Nombre válido**       | "Ortodoncia"          | Name creada        | - Value == "Ortodoncia" |
| **Error - String vacío**             | ""                    | `DomainException`  | - No se crea            |
| **Error - Solo espacios**            | "   "                 | `DomainException`  | - No se crea            |
| **Error - Longitud menor al mínimo** | "A"                   | `DomainException`  | - No se crea            |
| **Error - Longitud mayor al máximo** | String largo inválido | `DomainException`  | - No se crea            |
| **Error - Caracteres inválidos**     | "@@@@"                | `DomainException`  | - No se crea            |
| **Happy Path - Trim aplicado**       | "  Ortodoncia  "      | Name creada        | - Value == "Ortodoncia" |

---

### Inmutabilidad

| Escenario                | Acción                   | Resultado Esperado | Verificaciones       |
| ------------------------ | ------------------------ | ------------------ | -------------------- |
| **Inmutable por diseño** | Intentar modificar Value | No permitido       | - Propiedad readonly |

---

### Igualdad por valor

| Escenario             | Instancias                   | Resultado Esperado |
| --------------------- | ---------------------------- | ------------------ |
| **Iguales por valor** | "Ortodoncia" vs "Ortodoncia" | Equals == true     |
| **Distinto valor**    | "Ortodoncia" vs "Endodoncia" | Equals == false    |

---