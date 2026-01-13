# Value Object: Name

## Context
* Represents a user-visible name
* Shared by multiple domain concepts
* Normalizes input and prevents unnecessary mutations

## Business Rules
* Cannot be null
* Cannot be empty or whitespace
* Must match the defined regex pattern
* It is normalized (trimmed)
* Comparison is case-insensitive
* Two semantically identical names are the same value

## Valid States
* "Orthodontics"
* "Orthodontics " → "Orthodontics"

## Invalid Scenarios
* null
* ""
* "or"

## Mutability
* Immutable
* Any change implies creating a new instance

## Usage Notes
* Entities decide if the change is significant
* Handlers do not validate rules