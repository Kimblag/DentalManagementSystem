# Test Design — SP-UC-05: Deactivate Treatment from Specialty

This document defines the test design for deactivate a treatment from a specialty.

---

## 1. Test Scope

The tests validate:

* Correct retrieval of the aggregate.
* Invocation of treatment deactivation.
* Enforcement of minimum treatment constraints via the domain.
* Persistence behavior.

---

## 2. Test Double Strategy

Fake Repository.

---

## 3. Fake Repository Responsibilities

* Return a specialty with treatments.
* Persist changes when allowed.
* Record persistence attempts.

---

## 4. Observable Outcomes

* Treatment transitions to `Inactive`.
* Specialty remains `Active`.
* Aggregate is persisted.

---

## 5. Error Scenarios

* Specialty inactive.
* Treatment not found.
* Treatment already inactive.
* Attempt to Deactivate last active treatment.

---

## 6. Out of Scope

* Treatment internal state logic.
* Database constraints.
* Authorization.

---

## 7. Design Outcome

Treatment deactivate is validated as a domain-governed operation with strict invariants.
