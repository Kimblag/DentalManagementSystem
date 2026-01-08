# Specialties Bounded Context

## Purpose
This bounded context defines how medical specialties and their treatments are modeled and constrained within the dental clinic system.

Its responsibility is to maintain a consistent and valid catalog of specialties and treatments that can later be used by other contexts (appointments, medical staff, clinical history).

This context does NOT manage scheduling, pricing rules related to insurances, or clinical outcomes.

---

## Core Concepts

### Specialty
Represents a medical specialty offered by the clinic (e.g. Dentistry, Orthodontics).

A specialty is the **aggregate root** of this context.

### Treatment
Represents a concrete treatment that belongs to exactly one specialty.

A treatment **cannot exist outside** a specialty.

---

## Aggregate Design

### Aggregate Root
- Specialty

### Aggregate Members
- Treatment (child entity)

All modifications to treatments must occur **through the Specialty aggregate**.

---

## Invariants (Domain Rules)

These rules must always hold true, regardless of use case or application flow.

### Specialty Invariants
1. A specialty must always have **at least one treatment**.
2. A specialty cannot exist without a valid name.
3. Specialty names must be unique within the system.
4. A specialty has an operational status (`Active`, `Inactive`, `Archived`).
5. A specialty in `Inactive` or `Archived` state **cannot be modified**.
6. A specialty cannot be archived or inactivated if it is referenced by active appointments (external invariant, enforced at application level).

### Treatment Invariants
1. A treatment belongs to **exactly one specialty**.
2. A treatment cannot exist independently.
3. Treatment names must be unique **within the same specialty**.
4. A treatment must have a valid base cost (non-negative).
5. A treatment inherits operational restrictions from its parent specialty.
6. A treatment cannot be activated if its specialty is not active.

---

## State Propagation Rules

- When a specialty changes its status, all its treatments must reflect a compatible state.
- A treatment cannot be in a more permissive state than its specialty.
  - Example: an `Active` treatment under an `Inactive` specialty is invalid.

---

## What This Context Does NOT Handle

This bounded context explicitly excludes:

- Appointment scheduling
- Availability calculation
- Medical staff assignments
- Insurance coverage and discounts
- Clinical history and diagnoses
- Notifications or email sending

These concerns belong to other bounded contexts.

---

## Integration Points

Other contexts may:
- Query specialties and treatments (read-only).
- Validate specialty availability before performing operations (e.g. appointments).

Other contexts may NOT:
- Modify treatments directly.
- Bypass specialty invariants.

---

## Terminology Notes

- "Delete" always means **logical deletion** via state change.
- All rules described here are **domain rules**, not UI or API constraints.
- Persistence concerns are intentionally excluded.

---

## Design Rationale

This context is designed around a **rich domain model**, where consistency is enforced by the domain itself rather than scattered validations.

The goal is to ensure that invalid states are **unrepresentable**.
