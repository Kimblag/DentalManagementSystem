# Test Scenarios — SP-UC-01: Create Specialty

---

## Scenario 1 — Create a valid specialty (Happy Path)

### Given
* A valid specialty name.
* A valid description (optional).
* A non-empty list of valid treatments.

### When
* The CreateSpecialty use case is executed.

### Then
* A new Specialty aggregate is created.
* The specialty status is `Active`.
* All treatments are `Active`.
* The aggregate is persisted.

---

## Scenario 2 — Attempt to create a specialty with no treatments

### Given
* A valid specialty name.
* An empty treatment list.

### When
* The CreateSpecialty use case is executed.

### Then
* An `EmptyTreatmentListException` is raised.
* No persistence is performed.

---

## Scenario 3 — Attempt to create a specialty with duplicate treatments

### Given
* A treatment list containing duplicate names.

### When
* The CreateSpecialty use case is executed.

### Then
* A `DuplicateTreatmentNameException` is raised.
* No persistence is performed.
