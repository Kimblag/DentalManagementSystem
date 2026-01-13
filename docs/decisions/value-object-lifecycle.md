# Value Object: LifecycleStatus

## Context
* Models the lifecycle state of a domain entity
* Replaces thin enums lacking behavior
* Centralizes activation/deactivation rules

## Business Rules
* An entity is born in Active state
* Can only be deactivated if it is active
* Can only be reactivated if it is inactive
* No implicit transitions exist
* Invalid states do not exist

## Valid States
* Active
* Inactive

## Invalid Scenarios
* Reactivating when already active
* Deactivating when already inactive

## Mutability
* Controlled
* The VO encapsulates the transition and returns a new instance

## Usage Notes
* Entities delegate transitions to the VO
* Handlers do not manipulate states directly
* Never expose public setters