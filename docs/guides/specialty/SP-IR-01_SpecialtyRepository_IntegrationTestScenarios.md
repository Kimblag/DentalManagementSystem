# Specialty Repository – Integration Test Scenarios (Corrected & Aligned)

## Context

Integration tests for `SpecialtyRepository` and `UnitOfWork` using the real `DentalSystemDbContext`.

These tests verify **infrastructure behavior only**, specifically:

- EF Core mappings
- Owned Value Object persistence
- Backing field configuration
- Foreign key relationships
- Cascade delete behavior
- Change tracking and commit behavior

### Explicit Non-Goals

The following are **NOT** tested here:

- Domain invariants (lengths, costs, uniqueness, business rules)
- Validation logic
- Aggregate consistency rules

All domain rules are assumed correct and covered by **domain unit tests**.

Each test runs against an **isolated in-memory SQLite database**.

---

## 1. Add Specialty with Treatments

**Scenario:** Persist a new Specialty aggregate with multiple Treatments.

### Arrange
- Create a `Specialty` aggregate in memory:
  - Name = "General Dentistry"
  - Description = "Basic dental treatments"
  - Treatments:
    - "Cleaning" (BaseCost = 50)
    - "Check-up" (BaseCost = 30)
- Create a fresh in-memory SQLite connection
- Create a DbContext and ensure database creation
- Initialize `SpecialtyRepository` and `UnitOfWork`

### Act
- Add the Specialty via the repository
- Commit via `UnitOfWork`

### Assert
- The Specialty row exists in the database
- Name and Description are correctly persisted
- Treatments are persisted as separate rows
- Each Treatment has:
  - Correct Name value
  - Correct BaseCost
  - Correct Description value (if present)
- Treatments are linked via `SpecialtyId` foreign key
- No exceptions are thrown during persistence

---

## 2. Retrieve Specialty by Id (Including Treatments)

**Scenario:** Retrieve a Specialty aggregate with its Treatments.

### Arrange
- Persist a Specialty with multiple Treatments using the DbContext
- Create a new DbContext instance using the same connection
- Initialize the repository

### Act
- Call `GetByIdAsync` with the Specialty ID

### Assert
- A Specialty is returned (not null)
- Name and Description match persisted values
- Treatments collection is populated
- All persisted Treatments are loaded
- Owned Value Objects are correctly materialized
- Backing field (`_treatments`) is used, not the public property
- No lazy loading is required

---

## 3. Cascade Delete Specialty

**Scenario:** Deleting a Specialty deletes all associated Treatments.

### Arrange
- Persist a Specialty with multiple Treatments
- Store the SpecialtyId
- Create a new DbContext instance
- Initialize repository and UnitOfWork

### Act
- Delete the Specialty
- Commit via UnitOfWork

### Assert
- The Specialty no longer exists in the database
- No Treatment rows exist with the deleted `SpecialtyId`
- Cascade delete behavior works as configured in EF Core
- No orphaned rows remain


---

## Notes

- Each test uses a **shared SQLite in-memory connection** but **separate DbContext instances**
- `EnsureCreated()` is executed once per connection
- No database-level validation or business rules are enforced
- EF Core is treated as:
  - A persistence mechanism
  - A relationship mapper
  - A transaction boundary

Domain correctness is **explicitly outside the scope** of these tests.
