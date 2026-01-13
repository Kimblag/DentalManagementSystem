# SP-UC-08: Update Treatment Details

## Aggregate Root
Specialty

## Internal Entity
Treatment

---

## 1. Intent

To update mutable attributes of an existing treatment within a specialty while enforcing domain invariants related to treatment state, data validity, and aggregate consistency.

The treatment lifecycle remains fully controlled by the specialty aggregate.

---

## 2. Actor

* Authorized Catalog Manager.

---

## 3. Preconditions

* The specialty exists in the system (application responsibility).
* The specialty is in `Active` state (domain responsibility).
* The treatment exists within the specialty (domain responsibility).
* The treatment is in `Active` state (domain responsibility).

---

## 4. Input Data

* `SpecialtyId`: Unique identifier of the specialty aggregate.
* `TreatmentId`: Unique identifier of the treatment to update.
* `TreatmentName` (optional)
* `TreatmentBaseCost` (optional)
* `TreatmentDescription` (optional)

> All fields are optional. Only provided values are evaluated and applied.

---

## 5. Main Flow (Happy Path)

1. The system receives all input data.
2. The system retrieves the specialty aggregate from the repository.
3. The system invokes `UpdateTreatmentDetails(...)` on the specialty aggregate.
4. The domain:
   * Validates that the specialty is active.
   * Locates the treatment within the aggregate.
   * Validates that the treatment is active.
   * Validates all input data (name, cost, description).
5. The treatment state is updated with the new values.
6. The system persists the updated aggregate.

---

## 6. Alternative Flows / Errors

### Application Errors

* **SpecialtyNotFoundException**
  * Raised when the specialty does not exist.

---

### Domain Errors

These exceptions must be raised by the domain and must not be handled by the Application Layer.

* **InvalidSpecialtyStateException**
  * Raised when attempting to update a treatment of an inactive specialty.

* **TreatmentNotFoundException**
  * Raised when the treatment does not exist within the specialty.

* **InvalidTreatmentStateException**
  * Raised when attempting to update an inactive treatment.

* **InvalidTreatmentNameException**
  * Raised when the new name violates domain rules.

* **InvalidTreatmentBaseCostException**
  * Raised when the base cost is invalid.

* **InvalidTreatmentDescriptionException**
  * Raised when the description violates domain rules.

---

## 7. Postconditions

* The treatment remains active.
* Only the specified treatment fields are updated.
* The specialty remains unchanged except for the internal entity update.
* The aggregate is persisted in a consistent state.

---

## 8. Notes & Design Constraints

* The Application Layer:
  * Does not validate treatment fields.
  * Does not access the treatments collection directly.
  * Does not mutate treatment state.
* The Treatment entity:
  * Cannot update itself outside the aggregate.
* All invariants are enforced inside the aggregate boundary.

---

## 9. Explicit Non-Goals

This use case does **not**:

* Activate or deactivate treatments.
* Add or remove treatments.
* Modify specialty-level data.
* Perform partial updates outside domain rules.
