## Tests — `TreatmentCode` (Value Object)

### Creación

| Escenario                            | Entrada               | Resultado Esperado               | Verificaciones          |
| ------------------------------------ | --------------------- | -------------------------------- | ----------------------- |
| **Happy Path - Código válido**       | "01.01"               | TreatmentCode creado             | - Value == "01.01"      |
| **Error - String vacío**             | ""                    | `DomainValidationException`      | - No se crea            |
| **Error - Solo espacios**            | "   "                 | `DomainValidationException`      | - No se crea            |
| **Error - Caracteres inválidos**     | "@@@@"                | `DomainValidationException`      | - No se crea            |
| **Happy Path - Trim aplicado**       | "  01.01  "           | TreatmentCode creado             | - Value == "01.01"      |

---

### Inmutabilidad

| Escenario                | Acción                   | Resultado Esperado | Verificaciones       |
| ------------------------ | ------------------------ | ------------------ | -------------------- |
| **Inmutable por diseño** | Intentar modificar Value | No permitido       | - Propiedad readonly |

---

### Igualdad por valor

| Escenario             | Instancias                   | Resultado Esperado |
| --------------------- | ---------------------------- | ------------------ |
| **Iguales por valor** | "01.01" vs "01.01"           | Equals == true     |
| **Distinto valor**    | "01.01" vs "01.02"           | Equals == false    |

---