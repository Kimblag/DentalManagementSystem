# ADR 01: Estrategia de Persistencia y Portabilidad de Base de Datos

## Contexto
El sistema requiere persistir datos de especialidades y tratamientos. Se evalúa la necesidad de soportar múltiples motores de base de datos (SQL Server y MySQL) para evitar el acoplamiento a un proveedor específico (*Vendor Lock-in*).

## Decisión
Se utilizará **Entity Framework Core 8** como ORM principal, actuando como la abstracción de persistencia (Repository Pattern). No se creará una capa de abstracción adicional sobre EF Core para evitar la sobreingeniería.

## Justificación
* **Abstracción Nativa:** EF Core permite cambiar de motor mediante el cambio de *Providers* en la capa de infraestructura sin afectar la lógica del dominio o aplicación.
* **Mapeo de Tipos Complejos:** EF Core 8 soporta `Complex Types`, permitiendo que nuestros `records` de dominio se mapeen directamente a columnas de tabla de forma eficiente.
* **Mantenibilidad:** Se evita el patrón "Repository sobre Repository" que añade complejidad innecesaria y dificulta el uso de características avanzadas del ORM como *Change Tracking*.

## Consecuencias
* Si se requiere un cambio de motor en producción, se deberán generar migraciones específicas para el nuevo proveedor debido a las diferencias en los dialectos SQL.
* La lógica de auditoría se centralizará en el `DbContext` mediante el uso de *Shadow Properties*.
