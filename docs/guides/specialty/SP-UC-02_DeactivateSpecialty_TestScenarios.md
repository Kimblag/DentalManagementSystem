# Test Scenarios — SP-UC-02: Deactivate Specialty

This document defines the behavioral scenarios that must be satisfied by the
`DeactivateSpecialty` application use case. These scenarios are derived directly
from the approved use case specification and do not introduce new business rules.

---

## Scenario 1 — Deactivate an active specialty (Happy Path)
 
### Context (Given)
* A specialty aggregate exists in the system.
* The specialty is in `Active` state.
* The specialty contains one or more associated treatments, all in `Active` state.

### Action (When)
* The DeactivateSpecialty use case is executed with the specialty identifier.

### Expected Outcome (Then)
* The specialty state transitions to `Inactive`.
* All associated treatments transition to `Inactive`.
* The updated aggregate is persisted.

### Responsibility
* **Domain Model:** Enforces state transition rules and cascading deactivation.
* **Application Layer:** Retrieves the aggregate, invokes the behavior, and persists the result.

---

## Scenario 2 — Attempt to deactivate a non-existing specialty

### Context (Given)
* No specialty aggregate exists for the provided identifier.

### Action (When)
* The DeactivateSpecialty use case is executed with the specialty identifier.

### Expected Outcome (Then)
* A `SpecialtyNotFoundException` is raised.
* No persistence operation is performed.

### Responsibility
* **Application Layer:** Detects the absence of the aggregate and stops execution.

---

## Scenario 3 — Attempt to deactivate an already inactive specialty

### Context (Given)
* A specialty aggregate exists in the system.
* The specialty is already in `Inactive` state.

### Action (When)
* The DeactivateSpecialty use case is executed with the specialty identifier.

### Expected Outcome (Then)
* An `InvalidStatusTransitionException` is raised.
* The aggregate is not persisted.

### Responsibility
* **Domain Model:** Rejects the invalid state transition.
