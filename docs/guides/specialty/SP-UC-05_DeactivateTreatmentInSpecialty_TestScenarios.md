# Test Scenarios — SP-UC-05: Deactivate Treatment in Specialty

---

## Scenario 1 — Deactivate a treatment in an active specialty (Happy Path)

### Given
* A specialty exists.
* The specialty is `Active`.
* The specialty has more than one **active** treatment.

### When
* The DeactivateTreatment use case is executed.

### Then
* The treatment status is changed to `Inactive`.
* The specialty remains `Active`.
* The aggregate is persisted.

---

## Scenario 2 — Attempt to deactivate the last active treatment

### Given
* A specialty exists.
* The specialty has only one **active** treatment remaining.

### When
* The DeactivateTreatment use case is executed.

### Then
* A `MinimumSpecialtyTreatmentsException` is raised.
* No persistence is performed.

---

## Scenario 3 — Attempt to deactivate a treatment in an inactive specialty

### Given
* A specialty exists.
* The specialty is `Inactive`.

### When
* The DeactivateTreatment use case is executed.

### Then
* An `InvalidSpecialtyStateException` is raised.
* No persistence is performed.
