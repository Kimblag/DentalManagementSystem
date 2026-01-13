# Test Scenarios — SP-UC-04: Add Treatment to Specialty

---

## Scenario 1 — Add a treatment to an active specialty (Happy Path)

### Given
* A specialty exists.
* The specialty is `Active`.
* The treatment name does not already exist.

### When
* The AddTreatmentToSpecialty use case is executed.

### Then
* The treatment is added to the specialty.
* The treatment status is `Active`.
* The aggregate is persisted.

---

## Scenario 2 — Attempt to add a treatment to an inactive specialty

### Given
* A specialty exists.
* The specialty is `Inactive`.

### When
* The AddTreatmentToSpecialty use case is executed.

### Then
* An `InvalidSpecialtyStateException` is raised.
* No persistence is performed.

---

## Scenario 3 — Attempt to add a duplicate treatment

### Given
* A specialty exists.
* A treatment with the same name already exists.

### When
* The AddTreatmentToSpecialty use case is executed.

### Then
* A `DuplicateTreatmentNameException` is raised.
* No persistence is performed.
