# Application Specialty

Esta capa será el director de orquesta del sistema. No contiene reglas de negocio, ya que pertenecen al dominio y es agnóstica de la base de datos.

Se seguirá el principio KISS para mantener la capa simple, por lo que su único trabajo es recibir el pedido de un usuario, buscar las herramientas necesarias para ejecutar la acción solicitada y hacer que el Dominio trabaje y la capa de infraestructura guarde el resultado en la BD.

## Estructura de Application
### Casos de Uso (Use Cases / Interactors)
Representa e corazón de la capa ya que es la acción específica que un usuario puede hacer en el sistema.
    - **Regla**: Una clase = una acción.
    - **Cómo se vería**: Clases como `CreateSpecialtyUseCase`, `AddTreatmentUseCase`.
    - **Estructura**: Tendrán un único método público llamado `ExecuteAsync` o `HandleAsync`. Se inyectarán las interfaces necesarias a través de su constructor. 

### DTOs (Data Transfer Objects)
Son los "mensajeros" tontos. Son records sin nada de lógica ni comportamiento.

Input DTOs (Request): Lo que envía el controlador (ej. AddTreatmentRequest). Contienen tipos primitivos (string, decimal, Guid).

Output DTOs (Response): Lo devuelto al controlador (ej. SpecialtyResponse).

NUNCA devolver Entidades de Dominio (Specialty, Treatment) directamente hacia afuera. El controlador no conoce de dominio.

### Interfaces (Los Contratos / Puertos)
Aquí se define qué necesita la capa del mundo exterior para poder trabajar.

Repositorios (ISpecialtyRepository): NeSe necesita una interfaz que prometa métodos como GetByIdAsync, AddAsync, UpdateAsync. En este caso, se agregará a Application en lugar de crearlos en el dominio, donde suele agregarse a veces.

Unidad de Trabajo (IUnitOfWork): Un contrato opcional pero recomendado para manejar transacciones y hacer el SaveChangesAsync() centralizado. Esto se hace para evitar acoplar Application a la capa de base de datos. 

Servicios Externos (IEmailService): Interfaz para enviar emails sina coplar a un proveedor específico.

### Excepciones de Aplicación
El Dominio ya lanza excepciones de negocio (DomainRuleException), pero la Aplicación necesita sus propias excepciones para errores de "flujo".

Ejemplo clásico: SpecialtyNotFoundException. Si piden agregar un tratamiento al ID "123" y la base de datos me devuelve nulo, el Caso de Uso lanzará esta excepción (que luego el controlador transformará en un HTTP 404).

### Mapeadores Manuales (KISS Mappers)
No se utilizará la librería AutoMapper, por lo que se usarán métodos de extensión sencillos. Por ejemplo, un archivo SpecialtyMapper.cs con un método estático public static SpecialtyResponse ToDto(this Specialty specialty). Esto brinda control total, es rapidísimo y si algo falla, el compilador avisa de inmediato.