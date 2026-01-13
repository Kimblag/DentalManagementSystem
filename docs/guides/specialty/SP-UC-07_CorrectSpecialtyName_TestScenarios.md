# Test Scenarios — SP-UC-07: Correct Specialty Name

---

## Scenario 1 — Correct name of an active specialty (Happy Path)

### Given
* A specialty exists.
* The specialty is `Active`.
* The provided name is valid.
* The provided name differs from the current one.

### When
* The CorrectSpecialtyName use case is executed.

### Then
* The specialty name is updated.
* The aggregate is persisted.

---

## Scenario 2 — Correct name with identical value

### Given
* A specialty exists.
* The specialty is `Active`.
* The provided name matches the current name (case-insensitive).

### When
* The CorrectSpecialtyName use case is executed.

### Then
* No state change occurs.
* The aggregate is persisted.

---

## Scenario 3 — Attempt to correct name of an inactive specialty

### Given
* A specialty exists.
* The specialty is `Inactive`.

### When
* The CorrectSpecialtyName use case is executed.

### Then
* An `InvalidSpecialtyStateException` is raised.
* No persistence is performed.

---

## Scenario 4 — Attempt to correct name with an empty or whitespace value

### Given
* A specialty exists.
* The specialty is `Active`.
* The provided name is empty or contains only whitespace.

### When
* The CorrectSpecialtyName use case is executed.

### Then
* An `InvalidSpecialtyNameException` is raised.
* No persistence is performed.

---

## Scenario 5 — Attempt to correct name with an invalid format

### Given
* A specialty exists.
* The specialty is `Active`.
* The provided name does not match the required format.

### When
* The CorrectSpecialtyName use case is executed.

### Then
* An `InvalidSpecialtyNameException` is raised.
* No persistence is performed.
