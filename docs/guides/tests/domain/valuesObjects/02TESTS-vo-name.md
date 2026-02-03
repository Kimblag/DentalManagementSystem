## Tests — `Name` (Value Object)

### Creación

| Escenario                            | Entrada               | Resultado Esperado | Verificaciones          |
| ------------------------------------ | --------------------- | ------------------ | ----------------------- |
| **Happy Path - Nombre válido**       | "Ortodoncia"          | Name creada        | - Value == "Ortodoncia" |
| **Error - String vacío**             | ""                    | `InvalidNameException`  | - No se crea            |
| **Error - Solo espacios**            | "   "                 | `InvalidNameException`  | - No se crea            |
| **Error - Longitud menor al mínimo** | "A"                   | `InvalidNameException`  | - No se crea            |
| **Error - Longitud mayor al máximo** | String largo inválido | `InvalidNameException`  | - No se crea            |
| **Error - Caracteres inválidos**     | "@@@@"                | `InvalidNameException`  | - No se crea            |
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