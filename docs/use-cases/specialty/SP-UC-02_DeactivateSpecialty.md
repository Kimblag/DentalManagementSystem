# SP-UC-02: Deactivate Specialty

## Aggregate Root
Specialty

## 1. Intent
To allow an authorized catalog manager to transition an operational specialty to an inactive state. This ensures immediate consistency within the aggregate boundary by applying a deactivation cascade to all dependent treatments, while preserving historical data integrity.

## 2. Actor
* Authorized Catalog Manager.

## 3. Preconditions
* The specialty exists in the system (application responsibility).
* The specialty is currently in the `Active` state (domain responsibility).

## 4. Input Data
* `SpecialtyId`: Unique identifier of the target specialty aggregate.

## 5. Main Flow (Happy Path)
1. The system receives the `SpecialtyId`.
2. The system retrieves the specialty aggregate from the repository.
3. The system invokes the `Deactivate()` behavior on the aggregate root.
   * *Note:* All validation and cascading state transitions are enforced by the Domain Model.
4. The system persists the updated aggregate state.

## 6. Alternative Flows / Errors
* **Application Error:** `SpecialtyNotFoundException`
    * Triggered when no aggregate exists for the provided identifier.
* **Domain Error:** `InvalidStatusTransitionException`
    * Raised when the domain rejects the operation because the specialty is already inactive.

## 7. Postconditions
* The specialty state is `Inactive`.
* All associated treatments are in the `Inactive` state.
* The aggregate identity and internal structure remain unchanged.
* The updated state is persisted.
