# Specialty Repository - Integration Test Scenarios (Ultra-Detailed)

## Context
- Integration tests for `SpecialtyRepository` and `UnitOfWork` using the real DbContext.
- Focus on verifying:
  - EF Core mappings
  - Foreign key relationships
  - Check constraints
  - Cascade behaviors
  - Tracking and commit behavior
- Domain rules are **assumed correct** (covered by domain unit tests).

---

## 1. Add Specialty with Treatments
**Scenario:** Insert a new Specialty with multiple treatments

**Arrange**
- Create a Specialty object in memory:
  - Name = "General Dentistry"
  - Description = "Basic dental treatments"
  - Treatments = ["Cleaning" (BaseCost=50), "Check-up" (BaseCost=30)]
- Prepare a fresh DbContext configured in-memory
- Initialize a UnitOfWork with this context
- Prepare the SpecialtyRepository with this context

**Act**
- Add the Specialty to the repository
- Commit via UnitOfWork

**Assert**
- The Specialty is present in the database
- The Specialty has the correct Name and Description
- Each Treatment exists with:
  - Correct Name
  - Correct BaseCost
  - Correct Description
- Treatments are linked via `SpecialtyId` foreign key
- All EF Core constraints are satisfied:
  - Specialty Name length >= 3
  - Description length >= 3
  - BaseCost >= 0

---

## 2. Retrieve Specialty by Id (Including Treatments)
**Scenario:** Fetch Specialty with all related treatments

**Arrange**
- Persist a Specialty with multiple treatments using the DbContext
- Ensure each Treatment has meaningful values
- Initialize the repository and UnitOfWork

**Act**
- Call `GetByIdAsync` with the Specialty's ID

**Assert**
- Specialty is returned, not null
- Specialty Name and Description match persisted values
- All active Treatments are returned
- Each Treatment has correct values (Name, BaseCost, Description)
- EF Core navigation ensures `_treatments` collection matches database

---

## 3. Enforce Check Constraints
**Scenario:** Ensure EF Core and DB constraints are applied

**Arrange**
- Prepare invalid Specialty/Treatment objects:
  - Name shorter than 3 characters
  - Description shorter than 3 characters
  - BaseCost negative
- Use a fresh DbContext

**Act**
- Attempt to add invalid objects and commit

**Assert**
- The DbContext throws a persistence exception (e.g., DbUpdateException)
- Database state remains unchanged (no partial inserts)
- Verify constraints prevent invalid data

---

## 4. Cascade Delete Specialty
**Scenario:** Deleting a Specialty deletes its Treatments

**Arrange**
- Persist a Specialty with multiple Treatments
- Record SpecialtyId and TreatmentIds
- Initialize repository and UnitOfWork

**Act**
- Delete the Specialty
- Commit via UnitOfWork

**Assert**
- Specialty no longer exists in database
- All Treatments linked to SpecialtyId are deleted
- Foreign keys are clean (no orphaned rows)
- Cascade behavior works as configured in EF Core

---

## 5. UnitOfWork Commit Behavior
**Scenario:** Verify commit saves all tracked changes

**Arrange**
- Make multiple changes in DbContext:
  - Add new Specialty
  - Modify an existing Treatment
  - Delete another Treatment
- Initialize UnitOfWork with this context

**Act**
- Check `HasChanges()` returns true
- Commit changes via `CommitAsync`
- Check `HasChanges()` returns false

**Assert**
- All changes are correctly persisted in database
- No uncommitted changes remain
- Confirm all relationships, foreign keys, and constraints are valid after commit

---

## 6. Concurrency and Tracking
**Scenario:** Verify EF Core tracking and concurrency

**Arrange**
- Load the same Specialty in two separate DbContext instances
- Make different changes in each context
- Use UnitOfWork for each context

**Act**
- Commit the first context
- Commit the second context

**Assert**
- First commit persists correctly
- Second commit either:
  - Updates correctly if no concurrency token is configured
  - Throws concurrency exception if versioning/concurrency token is configured
- Verify `Version` property increments after each commit if used
- Database reflects latest valid values
- Previous contexts still have old values until refreshed

---

## Notes
- Each test must run on an isolated in-memory database to avoid cross-test pollution
- Use `DbContextOptionsBuilder` to configure the test database
- Tests focus on **EF Core behavior**, **relationships**, **constraints**, and **UnitOfWork commit behavior**
- No domain logic validation is done here; those are already verified in domain unit tests
