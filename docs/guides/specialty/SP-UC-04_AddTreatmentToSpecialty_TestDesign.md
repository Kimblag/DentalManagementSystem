# Test Design — SP-UC-04: Add Treatment to Specialty

This document defines the test design for the `AddTreatmentToSpecialty` application use case.

The objective is to validate that the Application Layer correctly orchestrates
the addition of a new Treatment to an existing Specialty aggregate while
delegating all business validation to the Domain Model.

---

## 1. Test Scope

The tests validate:

* Retrieval of the Specialty aggregate.
* Invocation of the domain behavior to add a treatment.
* Persistence of the modified aggregate.

The tests do NOT validate treatment validation rules.

---

## 2. Test Double Strategy

### Repository Type
Fake Repository.

### Rationale
* Aggregate mutation must be observed.
* State-based verification is required.
* Mocks would leak implementation details.

---

## 3. Fake Repository Responsibilities

The fake repository must:

* Return an existing Specialty aggregate.
* Store the aggregate after mutation.
* Record whether persistence was requested.

---

## 4. Required Repository Operations

* GetById(SpecialtyId)
* Save(Specialty)

---

## 5. Observable Outcomes

### Aggregate State
* A new treatment is added to the specialty.
* The treatment is created in `Active` state.

### Persistence Behavior
* The updated aggregate is persisted.

---

## 6. Error Scenarios

* Specialty not found.
* Specialty inactive.
* Duplicate treatment name.
* Invalid treatment data.

Errors must originate from the domain and propagate unchanged.

---

## 7. Explicitly Out of Scope

* Regex correctness.
* Treatment constructor behavior.
* Authorization.
* DTO mapping.

---

## 8. Design Outcome

This test ensures that treatment creation is an aggregate responsibility
and that the Application Layer performs no business logic.
