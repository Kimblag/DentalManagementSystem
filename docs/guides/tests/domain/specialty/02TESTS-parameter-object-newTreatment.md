# Escenarios de Test — `NewTreatmentData`


### Creación de `NewTreatmentData`

| Escenario                        | Entrada                                    | Resultado Esperado                          | Verificaciones                                            |
| -------------------------------- | ------------------------------------------ | ------------------------------------------- | --------------------------------------------------------- |
| **Happy Path - Datos completos** | Code válido<br>Name válido<br>Money válido | Instancia creada                            | - Code asignado<br>- Name asignado<br>- BaseCost asignado |
| **Error - Code nulo**            | Code: null<br>Name válido<br>Money válido  | `ArgumentNullException` / `DomainException` | - No se crea                                              |
| **Error - Code vacío**           | Code: ""<br>Name válido<br>Money válido    | `DomainException`                           | - No se crea                                              |
| **Error - Name nulo**            | Code válido<br>Name: null<br>Money válido  | `ArgumentNullException`                     | - No se crea                                              |
| **Error - BaseCost nulo**        | Code válido<br>Name válido<br>Money: null  | `ArgumentNullException`                     | - No se crea                                              |

