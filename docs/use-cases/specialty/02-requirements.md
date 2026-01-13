# SRS: Module – Specialties (IEEE 830 Optimized)

## RF01: Create Specialty
Creates a new specialty with its initial treatment catalog.

---

## RF02: Deactivate Specialty
**Description:** Deactivates an active specialty and propagates the state to its treatments.  
**Constraints:** Cannot deactivate if active appointments exist.

---

## RF03: Reactivate Specialty
**Description:** Reactivates an inactive specialty.  
**Constraints:** Cannot reactivate if domain rules are violated.

---

## RF04: Add Treatment to Specialty
**Description:** Adds a new treatment to an active specialty.  
**Constraints:** Treatment name must be unique within the specialty.

---

## RF05: Deactivate Treatment in Specialty
**Description:** Deactivates a treatment while keeping the specialty consistent.  
**Constraints:** At least one active treatment must remain.

---

## RF06: Update Specialty Description
**Description:** Updates the description of an active specialty.

---

## RF07: Correct Specialty Name
**Description:** Corrects the name of an active specialty.  
**Constraints:** Name must be valid and unique.

---

## RF08: Update Treatment Details
**Description:** Updates name, description, or base cost of a treatment.  
**Constraints:** Specialty and treatment must be active.
