# Test Design — SP-UC-06: Update Specialty Description

This document defines the test design for updating a specialty description.

---

## 1. Test Scope

The tests validate:

* Invocation of description update behavior.
* Enforcement of specialty state rules.
* Persistence behavior.

---

## 2. Test Double Strategy

Fake Repository.

---

## 3. Observable Outcomes

* Description is updated or cleared.
* Aggregate is persisted.

---

## 4. Error Scenarios

* Specialty inactive.
* Invalid description format.

---

## 5. Out of Scope

* Regex behavior.
* Localization.
* Auditing.

---

## 6. Design Outcome

Description updates are controlled exclusively by the domain.
