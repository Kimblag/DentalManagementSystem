# Test Scenarios — SP-UC-08: Update Treatment Details

---

## Scenario 1 — Update details of an active treatment within an active specialty (Happy Path)

### Given
* A specialty exists.
* The specialty is `Active`.
* A treatment exists within the specialty.
* The treatment is `Active`.
* All provided fields comply with domain rules.

### When
* The UpdateTreatmentDetails use case is executed.

### Then
* The treatment details are updated.
* The aggregate is persisted.

---

## Scenario 2 — Attempt to update treatment of an inactive specialty

### Given
* A specialty exists.
* The specialty is `Inactive`.

### When
* The UpdateTreatmentDetails use case is executed.

### Then
* An `InvalidSpecialtyStateException` is raised.
* No persistence is performed.

---

## Scenario 3 — Attempt to update an inactive treatment

### Given
* A specialty exists.
* The specialty is `Active`.
* A treatment exists within the specialty.
* The treatment is `Inactive`.

### When
* The UpdateTreatmentDetails use case is executed.

### Then
* An `InvalidTreatmentStateException` is raised.
* No persistence is performed.

---

## Scenario 4 — Attempt to update a non-existent treatment

### Given
* A specialty exists.
* The specialty is `Active`.
* The treatment does not exist within the specialty.

### When
* The UpdateTreatmentDetails use case is executed.

### Then
* A `TreatmentNotFoundException` is raised.
* No persistence is performed.

---

## Scenario 5 — Attempt to update treatment with invalid data

### Given
* A specialty exists.
* The specialty is `Active`.
* A treatment exists within the specialty.
* The treatment is `Active`.
* At least one provided field violates domain rules.

### When
* The UpdateTreatmentDetails use case is executed.

### Then
* A domain validation exception is raised.
* No persistence is performed.
