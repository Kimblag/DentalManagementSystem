# Aggregate Design – Specialties

## Aggregate Root
Specialty is the Aggregate Root of the Specialties module.

## Aggregate Composition
The Specialty aggregate is composed of:
- Specialty (root)
- Treatment (entity)

A Treatment cannot exist outside a Specialty.

## Aggregate Invariants
The following invariants are enforced by the aggregate:
1. A Specialty must always have at least one Treatment.
2. A Treatment cannot belong to more than one Specialty.
3. A Specialty cannot be modified when archived.
4. A Treatment cannot be modified if its parent Specialty is archived or inactive.
5. Specialty status transitions propagate to Treatments.

## Allowed Operations (Through Aggregate Root)
The following operations can only be executed through the Specialty aggregate:
- Add treatment
- Remove treatment
- Change specialty status
- Update specialty details
- Update treatment details

Direct modification of Treatment entities is not allowed.

## Responsibilities
The Specialty aggregate is responsible for:
- Maintaining consistency between Specialty and Treatments
- Enforcing business invariants
- Coordinating state transitions within the aggregate

## Out of Scope
The following concerns are explicitly outside this aggregate:
- Persistence
- Appointment validation
- Scheduling rules
- Authorization and roles

## Encapsulation via Internal Visibility
The methods ChangeStatus and UpdateDetails within the Treatment entity use internal visibility (represented by ~ in UML). This design choice ensures that the Specialty Aggregate Root is the only authorized orchestrator for these operations, preventing external layers from bypassing business invariants and enforcing the rule that all modifications must occur through the aggregate root.