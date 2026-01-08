# Use Cases (Application)

## Module: Specialties

- **Create specialty**: The system registers a new specialty with its initial catalog of treatments.
- **Add treatment to specialty**: The system extends the treatment catalog of an existing specialty.
- **Remove treatment from specialty**: The system removes a treatment from a specialty catalog.
- **Update specialty details**: The system updates the name and/or description of a specialty.
- **Change specialty status**: The system changes the operational status of a specialty and manages its impact on the catalog.
- **Update treatment details**: The system updates the name, description, and/or base cost of a treatment.

---

## Rules

### Domain

#### Specialty

- **Create specialty**
    1. Specialty name must be valid.
    2. Specialty description must be valid if provided.
    3. A specialty must be created with at least one treatment.

- **Add treatment to specialty**
    1. A treatment cannot be duplicated within the specialty.
    2. The treatment must be valid as a domain entity.
    3. An archived or inactive specialty cannot be modified.

- **Remove treatment from specialty**
    1. A specialty must always have at least one treatment.
    2. An archived or inactive specialty cannot be modified.
    3. The treatment must belong to the specialty.

- **Update specialty details**
    1. Name must be valid.
    2. Description must be valid.
    3. An archived or inactive specialty cannot be modified.

- **Change specialty status**
    1. The specialty cannot be changed to the same status.
    2. An archived or inactive specialty cannot be modified.
    3. A specialty cannot be reactivated if required conditions are not met.
    4. When the specialty status changes, its treatments must follow the same state transition.

- **Update treatment details**
    1. Name must be valid.
    2. Description must be valid.
    3. The specialty must not be archived or inactive.
    4. Base cost must be valid.

---

### Application

1. Validate specialty existence.
2. Validate specialty uniqueness.
3. Validate external dependencies: a specialty cannot be archived or inactivated if it has active appointments associated.
4. Apply treatment state changes according to the new specialty status.
5. Orchestrate transactions (creating a specialty and its initial treatments is an atomic operation).

---

## Errors

### Domain
1. A specialty cannot exist without at least one treatment.
2. The treatment already exists within the specialty.
3. Invalid state for the requested operation.
4. The specialty cannot be modified when archived.
5. The treatment does not belong to the specialty.

### Application
1. Specialty not found.
2. Duplicate specialty name in persistence.
3. Active appointments exist for the specialty.
