# Test Design — SP-UC-01: Create Specialty

This document describes the test design decisions for the `CreateSpecialty`
application use case.

The goal is to validate correct orchestration between the Application Layer and
the Domain Model during aggregate creation, without re-testing domain validation
rules.

---

## 1. Test Scope

The tests validate:

* Proper construction of the Specialty aggregate.
* Correct invocation of repository persistence.
* Correct propagation of domain validation errors.

The tests do NOT validate internal domain rules already enforced by constructors.

---

## 2. Test Double Strategy

### Repository Type
Fake Repository.

### Rationale
* Aggregate creation is state-based, not interaction-based.
* The test must observe persisted aggregates.
* Mocks would add unnecessary coupling.

---

## 3. Fake Repository Responsibilities

The fake repository must:

* Accept a newly created Specialty aggregate.
* Store it for later inspection.
* Expose whether persistence occurred.

The fake repository does NOT:

* Validate input.
* Generate identifiers.
* Enforce domain rules.

---

## 4. Required Repository Operations

* Add(Specialty specialty)
* Optional retrieval for verification

---

## 5. Observable Outcomes

### Aggregate State
* Specialty is created with `Active` status.
* Treatments are created and active.
* Data matches the provided input.

### Persistence Behavior
* Repository persistence is executed exactly once.

---

## 6. Error Scenarios

* Invalid specialty name.
* Empty treatment list.
* Duplicate treatment names.
* Invalid description.

All errors must be raised by the domain and propagated unchanged.

---

## 7. Explicitly Out of Scope

* Regex correctness.
* EF mapping.
* Authorization.
* DTO mapping.
* Transaction handling.

---

## 8. Design Outcome

This test ensures that:

* Creation logic resides in the domain.
* The Application Layer remains a thin orchestrator.
* Invalid aggregates are never persisted.
