# Test Design — SP-UC-02: Deactivate Specialty

This document describes the test design decisions for the `DeactivateSpecialty` application use case. It defines what is faked, what is observed, and what is explicitly out of scope for testing.

The goal is to validate orchestration behavior at the Application Layer without re-testing domain rules.

---

## 1. Test Scope

The tests for this use case validate:

* Correct orchestration of the Specialty aggregate.
* Proper interaction with the repository port.
* Correct propagation of domain errors.
* Absence of side effects when execution fails.

The tests do NOT validate domain invariants or internal entity behavior.

---

## 2. Test Double Strategy

### Repository Type
A **Fake Repository** is used instead of mocks or stubs.

### Rationale
* The repository is a port, not a collaborator with behavior.
* The test must observe state changes, not call order.
* Using mocks would couple tests to implementation details.

---

## 3. Fake Repository Responsibilities

The fake repository is responsible for:

* Storing a preconfigured `Specialty` aggregate instance.
* Returning the aggregate when queried by identifier.
* Recording whether a persistence operation was requested.

The fake repository does NOT:

* Validate domain rules.
* Modify aggregate state.
* Infer or enforce business conditions.

---

## 4. Required Repository Operations

The fake repository must conceptually support:

* Retrieving a `Specialty` by its identifier.
* Persisting a `Specialty` instance.

These operations mirror the repository interface used by the Application Layer.

---

## 5. Observable Outcomes (Assertions)

The test must be able to observe and assert:

### Aggregate State
* The specialty transitions to `Inactive`.
* All associated treatments transition to `Inactive`.

### Persistence Behavior
* The repository receives the updated aggregate for persistence.

These observations confirm successful orchestration.

---

## 6. Error Scenarios

### Specialty Not Found
* The fake repository returns no aggregate.
* The use case raises `SpecialtyNotFoundException`.
* No persistence operation is performed.

### Specialty Already Inactive
* The fake repository returns an inactive specialty.
* The domain raises `InvalidStatusTransitionException`.
* The application layer does not persist changes.

---

## 7. Explicitly Out of Scope

The following concerns are NOT tested here:

* Regex validation.
* Treatment-level rule enforcement.
* Infrastructure behavior (transactions, database).
* Authorization or authentication.
* DTO mapping or input parsing.

These concerns belong to other layers or have already been validated.

---

## 8. Design Outcome

A test designed according to this document ensures that:

* The Application Layer remains thin and orchestration-focused.
* The Domain Model remains authoritative over business rules.
* The system behavior is validated without duplication or leakage of concerns.
