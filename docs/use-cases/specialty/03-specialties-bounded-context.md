# Specialties Bounded Context

## Purpose
This bounded context models medical specialties and their treatments, enforcing consistency rules over their lifecycle.

It acts as the authoritative source of truth for specialty and treatment definitions.

---

## Aggregate Structure

### Aggregate Root
- Specialty

### Child Entities
- Treatment

All treatment modifications occur exclusively through the Specialty aggregate.

---

## Invariants

### Specialty
1. Must have at least one treatment.
2. Must have a valid and unique name.
3. Has a lifecycle state: `Active`, `Inactive`.
4. Cannot be modified unless `Active`.

### Treatment
1. Belongs to exactly one specialty.
2. Cannot exist independently.
3. Must have a valid name and base cost.
4. Cannot be more permissive than its specialty.

---

## Lifecycle Rules

- Deactivating a specialty deactivates all its treatments.
- Reactivating a specialty restores its treatments to a compatible state.
- A specialty must always retain at least one active treatment.
- A treatment can only be deactivated if the specialty remains valid.

---

## Excluded Concerns

- Appointments
- Billing
- Insurance logic
- Staff scheduling
- Notifications

---

## Design Rationale

This context follows a rich domain model approach where:
- Business rules live in the domain
- Invalid states are unrepresentable
- Use cases coordinate, aggregates decide
