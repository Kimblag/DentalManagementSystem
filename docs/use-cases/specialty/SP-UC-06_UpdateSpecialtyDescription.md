# SP-UC-05: Update Specialty Description

## Aggregate Root
Specialty

---

## 1. Intent

To allow an authorized catalog manager to update or clear the description of an existing specialty, while preserving all other aggregate invariants and state.

This use case enables **controlled modification of optional descriptive data** without affecting the specialty identity, status, or its associated treatments.

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
* `Description` (string, optional): The new description value.
  * May be `null` or empty to explicitly clear the description.

---

## 5. Main Flow (Happy Path)

1. The system receives the `SpecialtyId` and the optional `Description`.
2. The system retrieves the specialty aggregate from the repository.
3. The system invokes the `UpdateDescription(description)` behavior on the aggregate root.
4. The domain:
   * Validates that the specialty is active.
   * If the description is not null or empty:
     * Validates that it matches the required format.
   * Applies one of the following outcomes:
     * Sets the description to the validated value.
     * Clears the description when the input is null or empty.
5. The system persists the updated aggregate state.

---

## 6. Alternative Flows / Errors

### Application Errors

* **SpecialtyNotFoundException**
  * Triggered when no specialty exists for the provided identifier.

---

### Domain Errors

These errors are raised by the domain model and must not be handled or transformed by the Application Layer.

* **InvalidSpecialtyStateException**
  * Occurs when attempting to update the description of an inactive specialty.

* **InvalidSpecialtyDescriptionException**
  * Occurs when the provided description does not match the required pattern.

---

## 7. Postconditions

* If a valid description is provided:
  * The specialty description is updated.
* If the description is null or empty:
  * The specialty description is cleared (set to an empty string).
* In all cases:
  * The specialty name is unchanged.
  * The specialty status is unchanged.
  * All associated treatments are unchanged.
  * The aggregate identity remains intact.
  * Any applied change is persisted.

---

## 8. Notes & Design Constraints

* The description is treated as **optional, non-critical data**.
* Clearing the description is an explicit and valid operation.
* The Application Layer:
  * Does not validate description formats.
  * Does not decide whether to clear or update.
  * Delegates all logic to the domain model.
* This use case is intentionally separate from name correction to avoid generic “update” operations.

---

## 9. Explicit Non-Goals

This use case does **not**:

* Activate or deactivate the specialty.
* Modify the specialty name.
* Add, remove, or update treatments.
* Perform partial updates beyond the description field.
