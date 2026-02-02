## Tests — `Money` (Value Object)

### Creación

| Escenario                       | Entrada                         | Resultado Esperado | Verificaciones                          |
| ------------------------------- | ------------------------------- | ------------------ | --------------------------------------- |
| **Happy Path - Monto positivo** | Amount: 1000<br>Currency: "ARS" | Money creada       | - Amount == 1000<br>- Currency == "ARS" |
| **Happy Path - Monto cero**     | Amount: 0<br>Currency: "ARS"    | Money creada       | - Amount == 0                           |
| **Error - Monto negativo**      | Amount: -1<br>Currency: "ARS"   | `DomainException`  | - No se crea                            |
| **Error - Currency nula**       | Amount: 1000<br>Currency: null  | `DomainException`  | - No se crea                            |
| **Error - Currency vacía**      | Amount: 1000<br>Currency: ""    | `DomainException`  | - No se crea                            |

---

### Inmutabilidad

| Escenario                | Acción                               | Resultado Esperado | Verificaciones                       |
| ------------------------ | ------------------------------------ | ------------------ | ------------------------------------ |
| **Inmutable por diseño** | Intentar modificar Amount o Currency | No permitido       | - Propiedades readonly / sin setters |

---

### Igualdad por valor

| Escenario             | Instancias                     | Resultado Esperado |
| --------------------- | ------------------------------ | ------------------ |
| **Iguales por valor** | (1000, "ARS") vs (1000, "ARS") | Equals == true     |
| **Distinto monto**    | (1000, "ARS") vs (2000, "ARS") | Equals == false    |
| **Distinta moneda**   | (1000, "ARS") vs (1000, "USD") | Equals == false    |

---