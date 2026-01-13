# Test Scenarios — SP-UC-03: Reactivate Specialty

---

## Scenario 1 — Reactivate an inactive specialty (Happy Path)

### Given
* A specialty exists.
* The specialty is `Inactive`.
* All treatments are `Inactive`.

### When
* The ReactivateSpecialty use case is executed.

### Then
* The specialty becomes `Active`.
* All treatments become `Active`.
* The aggregate is persisted.

---

## Scenario 2 — Attempt to reactivate an active specialty

### Given
* A specialty exists.
* The specialty is already `Active`.

### When
* The ReactivateSpecialty use case is executed.

### Then
* An `InvalidStatusTransitionException` is raised.
* No persistence is performed.
