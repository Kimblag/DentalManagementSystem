# SP-UC-04: Correct Specialty Name

## Aggregate Root
Specialty

---

## 1. Intent

To allow an authorized catalog manager to correct the name of an existing specialty while preserving the aggregate’s identity and integrity.

This use case supports **controlled, idempotent correction** of the specialty name, ensuring that no unintended mutations occur within the aggregate.

---

## 2. Actor

* Authorized Catalog Manager.

---

## 3. Preconditions

* The specialty exists in the system (application responsibility).
* The specialty is currently in the `Active` state (domain responsibility).

---

## 4. Input Data

* `SpecialtyId`: Unique identifier of the specialty aggregate.
* `CorrectedName` (string): The proposed corrected name.

---

## 5. Main Flow (Happy Path)

1. The system receives the `SpecialtyId` and the `CorrectedName`.
2. The system retrieves the specialty aggregate from the repository.
3. The system invokes the `CorrectName(correctedName)` behavior on the aggregate root.
4. The domain:
   * Validates that the specialty is active.
   * Validates that the corrected name is non-null, non-empty, and matches the required format.
   * Trims the corrected name.
   * Compares the corrected name against the current name (case-insensitive).
5. If the corrected name differs from the current name:
   * The domain updates the specialty name.
6. The system persists the aggregate state.

---

## 6. Alternative Flows / Errors

### Application Errors

* **SpecialtyNotFoundException**
  * Triggered when no specialty exists for the provided identifier.

---

### Domain Errors

These errors are raised by the domain model and must not be handled or transformed by the Application Layer.

* **InvalidSpecialtyStateException**
  * Occurs when attempting to correct the name of an inactive specialty.

* **InvalidSpecialtyNameException**
  * Occurs when the corrected name is null, empty, or does not match the required pattern.

---

## 7. Idempotent Behavior

* If the corrected name is identical to the current name (case-insensitive):
  * No mutation occurs.
  * No exception is thrown.
  * The aggregate remains unchanged.
  * Persistence may be skipped or executed without side effects, depending on infrastructure behavior.

---

## 8. Postconditions

* If the corrected name is different and valid:
  * The specialty name is updated.
* If the corrected name is identical:
  * The specialty remains unchanged.
* In all cases:
  * The specialty status is unchanged.
  * The specialty description is unchanged.
  * Associated treatments are unaffected.
  * The aggregate identity remains intact.
  * Any applied change is persisted.

---

## 9. Notes & Design Constraints

* This use case is **not a general update operation**.
  * It exists specifically to express the intent of correcting a name.
* The Application Layer:
  * Does not compare strings.
  * Does not trim values.
  * Does not validate formats.
* All validation and idempotency checks are enforced by the domain.

---

## 10. Explicit Non-Goals

This use case does **not**:

* Activate or deactivate the specialty.
* Modify the specialty description.
* Add, remove, or modify treatments.
* Merge or replace specialties.
 