# SP-UC-06: Add Treatment to Specialty

## Aggregate Root
Specialty

## Internal Entity
Treatment

---

## 1. Intent

To add a new treatment to an existing specialty, ensuring that the specialty remains consistent and that all domain invariants related to treatment creation and specialty state are enforced.

This use case reinforces the rule that **a Treatment cannot exist independently of a Specialty** and can only be created within the aggregate boundary.

---

## 2. Actor

* Authorized Catalog Manager.

---

## 3. Preconditions

* The specialty exists in the system (application responsibility).
* The specialty is currently in the `Active` state (domain responsibility).

---

## 4. Input Data

* `SpecialtyId`: Unique identifier of the target specialty aggregate.
* `TreatmentName` (string): Name of the new treatment.
* `BaseCost` (decimal): Base cost of the treatment.
* `Description` (string, optional): Optional description of the treatment.

---

## 5. Main Flow (Happy Path)

1. The system receives the `SpecialtyId` and the treatment data.
2. The system retrieves the specialty aggregate from the repository.
3. The system creates a new `Treatment` entity using the provided data.
4. The system invokes the `AddTreatment(treatment)` behavior on the specialty aggregate.
5. The domain:
   * Validates that the specialty is active.
   * Validates that the treatment is not null.
   * Validates that no other treatment in the specialty shares the same name (case-insensitive).
6. The new treatment is added to the specialty with status `Active`.
7. The system persists the updated aggregate state.

---

## 6. Alternative Flows / Errors

### Application Errors

* **SpecialtyNotFoundException**
  * Triggered when no specialty exists for the provided identifier.

---

### Domain Errors

These errors are raised by the domain model and must not be intercepted or transformed by the Application Layer.

* **InvalidSpecialtyStateException**
  * Occurs when attempting to add a treatment to an inactive specialty.

* **DuplicateTreatmentNameException**
  * Occurs when another treatment in the specialty already has the same name (case-insensitive).

* **InvalidTreatmentNameException**
  * Occurs when the treatment name is null, empty, or does not match the required pattern.

* **InvalidTreatmentCostException**
  * Occurs when the treatment base cost is negative.

* **InvalidTreatmentDescriptionException**
  * Occurs when the treatment description does not match the allowed pattern.

---

## 7. Postconditions

* A new treatment exists within the specialty aggregate.
* The treatment status is `Active`.
* The specialty status remains unchanged.
* Existing treatments remain unchanged.
* The aggregate identity and internal structure are preserved.
* The updated aggregate state is persisted.

---

## 8. Notes & Design Constraints

* Treatments are **created inside the aggregate**, not retrieved or reused from elsewhere.
* The Application Layer:
  * Does not check for duplicate names.
  * Does not validate treatment data.
  * Does not manipulate the treatments collection directly.
* The specialty is the sole authority responsible for managing its treatments.

---

## 9. Explicit Non-Goals

This use case does **not**:

* Reactivate or deactivate treatments.
* Update existing treatments.
* Remove treatments from the specialty.
* Create treatments independently of a specialty.
