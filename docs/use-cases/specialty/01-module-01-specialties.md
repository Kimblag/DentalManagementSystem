# Use Cases (Application)

## Module: Specialties

- **Create specialty**: Registers a new specialty with its initial mandatory catalog of treatments.
- **Deactivate specialty**: Changes the specialty status to `Inactive` and propagates the state to its treatments.
- **Reactivate specialty**: Restores a previously inactive specialty to `Active` state.
- **Add treatment to specialty**: Adds a new treatment to an active specialty.
- **Deactivate treatment in specialty**: Deactivates a treatment while preserving specialty invariants.
- **Update specialty description**: Updates the descriptive information of a specialty.
- **Correct specialty name**: Corrects the specialty name without altering its identity.
- **Update treatment details**: Updates name, description, and/or base cost of a treatment through the specialty aggregate.

---

## Rules

### Domain

#### Specialty

- **Create specialty**
    1. Specialty name must be valid.
    2. Specialty description must be valid if provided.
    3. A specialty must be created with at least one treatment.
    4. Initial status is `Active`.

- **Deactivate specialty**
    1. A specialty must be `Active`.
    2. A specialty cannot be deactivated if external blocking conditions exist.
    3. All treatments must transition to a compatible state.

- **Reactivate specialty**
    1. A specialty must be `Inactive`.
    2. A specialty cannot be reactivated if domain constraints are violated.

- **Add treatment to specialty**
    1. The specialty must be `Active`.
    2. Treatment name must be unique within the specialty.
    3. Treatment data must be valid.

- **Deactivate treatment in specialty**
    1. The specialty must be `Active`.
    2. The treatment must be `Active`.
    3. At least one treatment must remain active in the specialty.

- **Update specialty description**
    1. The specialty must be `Active`.
    2. Description must be valid.

- **Correct specialty name**
    1. The specialty must be `Active`.
    2. Name must be valid and different from the current one.

- **Update treatment details**
    1. The specialty must be `Active`.
    2. The treatment must be `Active`.
    3. Name, description, and base cost must be valid.

---

### Application

1. Validate specialty existence.
2. Validate treatment existence within the specialty.
3. Validate external dependencies (e.g. active appointments).
4. Enforce transactional consistency.
5. Persist the aggregate only if all domain rules pass.

---

## Errors

### Domain
1. Invalid state for the requested operation.
2. A specialty cannot exist without at least one active treatment.
3. A treatment already exists within the specialty.
4. A treatment cannot be deactivated if it is the last active one.
5. Invalid treatment data.

### Application
1. Specialty not found.
2. Treatment not found.
3. Duplicate specialty name.
4. External blocking condition detected.
