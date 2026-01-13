# SP-UC-03: Reactivate Specialty

## Aggregate Root
Specialty

---

## 1. Intent

To restore an inactive specialty to an operational (`Active`) state.  
This use case re-enables the specialty and **reactivates all its associated treatments**, enforcing aggregate-wide consistency regardless of the treatments’ previous states.

This operation represents a **controlled state transition** governed exclusively by the domain model.

---

## 2. Actor

* Authorized Catalog Manager.

---

## 3. Preconditions

* The specialty exists in the system (application responsibility).
* The specialty is currently in the `Inactive` state (domain responsibility).

---

## 4. Input Data

* `SpecialtyId`: Unique identifier of the specialty aggregate to be reactivated.

---

## 5. Main Flow (Happy Path)

1. The system receives the `SpecialtyId`.
2. The system retrieves the specialty aggregate from the repository.
3. The system invokes the `Reactivate()` behavior on the aggregate root.
4. The domain:
   * Validates that the specialty is not already active.
   * Transitions the specialty state to `Active`.
   * Reactivates **all associated treatments**, ignoring their previous states.
5. The system persists the updated aggregate state.

---

## 6. Alternative Flows / Errors

### Application Errors

* **SpecialtyNotFoundException**
  * Triggered when no specialty exists for the provided identifier.

---

### Domain Errors

These errors are raised directly by the domain model and must not be intercepted by the Application Layer.

* **InvalidStatusTransitionException**
  * Occurs when the specialty is already in the `Active` state.

* **TreatmentAlreadyActiveException**
  * May be raised if a treatment is already active and the domain explicitly prevents redundant activation.

> Note: The aggregate root intentionally controls treatment reactivation.  
> The Application Layer does not evaluate or manipulate treatment states directly.

---

## 7. Postconditions

* The specialty status is `Active`.
* All associated treatments have status `Active`.
* The aggregate identity and internal structure remain unchanged.
* The updated state is persisted.

---

## 8. Notes & Design Constraints

* Reactivation is **aggregate-scoped**:
  * Individual treatments cannot be selectively reactivated.
* Previous treatment states are irrelevant:
  * The reactivation cascade enforces a uniform active state.
* The Application Layer:
  * Does not validate current status.
  * Does not catch domain exceptions.
  * Only orchestrates retrieval, execution, and persistence.

---

## 9. Explicit Non-Goals

This use case does **not**:

* Create new specialties.
* Modify specialty name or description.
* Add or remove treatments.
* Reactivate treatments independently of the specialty.
