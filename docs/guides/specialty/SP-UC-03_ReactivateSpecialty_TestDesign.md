# Test Design — SP-UC-03: Reactivate Specialty

This document defines the test design for the `ReactivateSpecialty` use case.

---

## 1. Test Scope

The tests validate:

* Correct orchestration of reactivation.
* Proper propagation of domain state transition rules.
* Persistence behavior on success.

---

## 2. Test Double Strategy

Fake Repository.

### Rationale
State verification is required. No interaction mocking is needed.

---

## 3. Fake Repository Responsibilities

* Return an inactive Specialty aggregate.
* Record persistence calls.

---

## 4. Observable Outcomes

* Specialty transitions to `Active`.
* All treatments transition to `Active`.
* Aggregate is persisted.

---

## 5. Error Scenarios

* Specialty not found → application error.
* Specialty already active → domain error.

---

## 6. Out of Scope

* Treatment-level validation.
* Authorization.
* Infrastructure concerns.

---

## 7. Design Outcome

Reactivation logic is fully domain-driven and atomically applied.
