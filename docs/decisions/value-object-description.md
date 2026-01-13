# Value Object: Description

## Context
* Represents an optional description
* Does not define identity
* Only encapsulates validation and normalization

## Business Rules
* Can be null
* If it exists, it must match the regex pattern
* If it is whitespace, it is considered absent
* Does not participate in entity equality

## Valid States
* null
* "Basic preventive treatment"

## Invalid Scenarios
* Text that does not match the regex
* Text shorter than the minimum length

## Mutability
* Immutable
* It is replaced or removed (set to null)

## Usage Notes
* Entities decide when to clear the description
* The VO does not decide policies, it only validates