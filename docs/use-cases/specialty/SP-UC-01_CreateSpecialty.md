# SP-UC-01: Create Specialty

## Aggregate Root
Specialty

---

## 1. Intent

To create a new dental specialty aggregate in an operational (`Active`) state, ensuring that all domain invariants are enforced at creation time.  
The specialty must be created with at least one valid treatment, and all provided data must comply with the domain validation rules.

This use case represents the **only valid entry point** for creating a `Specialty` aggregate.

---

## 2. Actor

* Authorized Catalog Manager.

---

## 3. Preconditions

* The actor has permission to manage the specialty catalog.
* The system is able to persist a new specialty aggregate.

> There are **no domain preconditions** related to existing state, since this is a creation use case.

---

## 4. Input Data

* `Name` (string): Proposed name of the specialty.
* `Description` (string, optional): Optional descriptive text for the specialty.
* `Treatments` (collection):
  * Each treatment includes:
    * `Name` (string)
    * `BaseCost` (decimal)
    * `Description` (string, optional)

---

## 5. Main Flow (Happy Path)

1. The system receives the specialty creation request with the provided input data.
2. The system constructs the treatment entities using the supplied treatment data.
3. The system creates a new `Specialty` aggregate by invoking the domain constructor.
4. The domain validates all invariants:
   * The specialty name is valid and well-formed.
   * The treatment list is not empty.
   * Treatment names are unique within the specialty.
   * Each treatment has a valid name, cost, and description.
5. The newly created specialty is initialized with:
   * Status set to `Active`.
   * All treatments set to `Active`.
6. The system persists the newly created specialty aggregate.
7. The system returns the identity of the created specialty.

---

## 6. Alternative Flows / Errors

### Domain Validation Errors

These errors are raised **by the Domain Model during construction** and must not be intercepted or altered by the Application Layer.

* **InvalidSpecialtyNameException**
  * The provided specialty name is null, empty, or does not match the required format.

* **InvalidSpecialtyDescriptionException**
  * The provided description does not match the allowed pattern.

* **EmptyTreatmentListException**
  * The specialty is created without any treatments.

* **DuplicateTreatmentNameException**
  * Two or more treatments share the same name (case-insensitive).

* **InvalidTreatmentNameException**
  * A treatment name is null, empty, or does not match the required format.

* **InvalidTreatmentCostException**
  * A treatment has a negative base cost.

* **InvalidTreatmentDescriptionException**
  * A treatment description does not match the allowed pattern.

---

## 7. Postconditions

* A new `Specialty` aggregate exists.
* The specialty status is `Active`.
* The specialty has at least one associated treatment.
* All associated treatments are in the `Active` state.
* The specialty and its treatments have valid domain identities.
* The aggregate has been persisted.

---

## 8. Notes & Design Constraints

* This use case **does not check for existing specialties with the same name**.
  * Any uniqueness constraint beyond the aggregate boundary is an infrastructure or policy concern, not a domain invariant.
* The Application Layer:
  * Does not validate input formats.
  * Does not enforce business rules.
  * Delegates all invariant enforcement to the Domain Model.
* The creation of treatments is **strictly scoped to the specialty aggregate**.
  * Treatments cannot be created independently.

---

## 9. Explicit Non-Goals

This use case does **not**:

* Modify existing specialties.
* Activate or deactivate existing specialties.
* Handle updates to specialty name or description.
* Manage treatments outside of initial creation.
