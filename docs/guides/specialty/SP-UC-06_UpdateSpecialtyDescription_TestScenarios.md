# Test Scenarios — SP-UC-06: Update Specialty Description

---

## Scenario 1 — Update description of an active specialty

### Given
* A specialty exists.
* The specialty is `Active`.

### When
* The UpdateSpecialtyDescription use case is executed.

### Then
* The description is updated.
* The aggregate is persisted.

---

## Scenario 2 — Attempt to update description of an inactive specialty

### Given
* A specialty exists.
* The specialty is `Inactive`.

### When
* The UpdateSpecialtyDescription use case is executed.

### Then
* An `InvalidSpecialtyStateException` is raised.
* No persistence is performed.

---
## Scenario 3 — Attempt to update description with an invalid value

### Given
* A specialty exists.
* The specialty is `Active`.
* The new description is invalid according to domain rules.

### When
* The UpdateSpecialtyDescription use case is executed.

### Then
* An `InvalidSpecialtyDescriptionException` is raised.
* The specialty description is not changed.
* No persistence is performed.
