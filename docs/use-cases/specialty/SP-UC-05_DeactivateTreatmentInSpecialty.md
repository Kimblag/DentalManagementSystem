# SP-UC-07: Deactivate Treatment from Specialty

## Aggregate Root
Specialty

## Internal Entity
Treatment

---

## 1. Intent

To deactivate (deactivate) an existing treatment from a specialty, ensuring that the aggregate remains consistent and that business rules regarding minimum treatments and state transitions are preserved.

Physical deletion is explicitly forbidden.

---

## 2. Actor

* Authorized Catalog Manager.

---

## 3. Preconditions

* The specialty exists in the system (application responsibility).
* The specialty is in `Active` state (domain responsibility).
* The treatment exists within the specialty (domain responsibility).

---

## 4. Input Data

* `SpecialtyId`: Unique identifier of the specialty aggregate.
* `TreatmentId`: Unique identifier of the treatment to deactivate.

---

## 5. Main Flow (Happy Path)

1. The system receives `SpecialtyId` and `TreatmentId`.
2. The system retrieves the specialty aggregate from the repository.
3. The system invokes `DeactivateTreatment(TreatmentId)` on the specialty aggregate.
4. The domain:
   * Validates that the specialty is active.
   * Locates the treatment within the aggregate.
   * Validates that the specialty will still contain at least one active treatment after removal.
5. The treatment state is changed to `Inactive`.
6. The system persists the updated aggregate.

---

## 6. Alternative Flows / Errors

### Application Errors

* **SpecialtyNotFoundException**
  * Raised when the specialty does not exist.

---

### Domain Errors

These exceptions must be thrown by the domain model and must not be handled by the Application Layer.

* **InvalidSpecialtyStateException**
  * Raised when attempting to deactivate a treatment from an inactive specialty.

* **TreatmentNotFoundException**
  * Raised when the treatment does not exist within the specialty.

* **MinimumActiveTreatmentsViolationException**
  * Raised when removing the treatment would leave the specialty with zero active treatments.

* **InvalidStatusTransitionException**
  * Raised when the treatment is already inactive.

---

## 7. Postconditions

* The treatment remains in the aggregate but with state `Inactive`.
* No other treatments are modified.
* The specialty remains active.
* The aggregate is persisted in a consistent state.

---

## 8. Notes & Design Constraints

* Treatments are never physically deleted.
* Treatment lifecycle is fully controlled by the specialty aggregate.
* The Application Layer:
  * Does not inspect the treatments collection.
  * Does not enforce minimum-count rules.
  * Does not toggle treatment states.
* The domain enforces **minimum viable aggregate rules**.

---

## 9. Explicit Non-Goals

This use case does **not**:

* Delete treatments from the database.
* Reactivate treatments.
* Modify treatment data.
* Deactivate the specialty.
