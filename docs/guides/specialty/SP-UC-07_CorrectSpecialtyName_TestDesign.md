# Test Design — SP-UC-07: Correct Specialty Name

This document defines the test design for correcting a specialty name.

---

## 1. Test Scope

The tests validate:

* Name correction behavior.
* Idempotent name updates.
* Persistence behavior.

---

## 2. Test Double Strategy

Fake Repository.

---

## 3. Observable Outcomes

* Name is updated if different.
* Aggregate is persisted only when a change occurs.

---

## 4. Error Scenarios

* Specialty inactive.
* Invalid name format.

---

## 5. Out of Scope

* Case normalization.
* UI-level validation.

---

## 6. Design Outcome

Name correction is a controlled domain mutation with no side effects.
