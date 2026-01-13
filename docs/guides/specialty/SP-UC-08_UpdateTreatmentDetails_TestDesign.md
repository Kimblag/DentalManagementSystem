# Test Design - SP-UC-08: Update Treatment Details

This document defines the test design for updating the details of a treatment within a specialty.

---

## 1. Test Scope

The tests verify:
* Aggregate retrieval.
* Delegation of behavior to the domain.
* Persistence only when the operation is valid.
* Correct propagation of domain exceptions.

The tests do NOT validate:
* Internal validation rules.
* Regular expressions.
* Internal entity logic.

---

## 2. Test Double Strategy

### Repository Type
Fake Repository

---

## 3. Observable Outcomes

* Treatment details must be updated.
* The aggregate is persisted.

---

## 4. Error Scenarios

* Specialty inactive.
* Treatment not found.
* Treatment inactive.
* Invalid name format.
* Negative base cost.
* Invalid description format.

---

## 5. Out of Scope

* Regex behavior.
* Localization.
* Auditing.

---

## 6. Design Outcome

Name, Base Cost and Description updates are controlled exclusively by the domain.