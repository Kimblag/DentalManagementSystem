# SRS: Module – Specialties (IEEE 830 Optimized)

## RF01: List Specialties
**Description:** Allows the retrieval of all specialties registered in the system.  
**Input:** None.  
**Processing:** Query all specialty entities via the domain repository.  
**Output:** Collection of specialties (ID, Name, Description, Status).  
**Error Handling / Constraints:** Returns an empty collection if no records exist.

---

## RF02: List Treatments of a Specialty
**Description:** Retrieves all treatments associated with a specific specialty aggregate.  
**Input:** Specialty ID.  
**Processing:** - Validate existence of Specialty ID.  
- Access the internal collection of the specialty aggregate.  
**Output:** Collection of treatments (ID, Name, Description, BaseCost, Status).  
**Error Handling / Constraints:** Return "Specialty Not Found" error if ID is invalid.

---

## RF03: Create Specialty with Treatments
**Description:** Atomic operation to create a new specialty with its mandatory initial catalog.  
**Input:** Name, Description (optional), List of Treatments (Name, Description, BaseCost).  
**Processing:** 
- Validate Specialty name format (Regex) and global uniqueness.  
- Validate that the treatment list contains at least one item.  
- Validate each treatment's name uniqueness within the new catalog.  
- Initialize status as `Active` for both specialty and treatments.  
**Output:** Created Specialty aggregate with unique ID.  
**Error Handling / Constraints:** 
- Reject if name is invalid or duplicated.  
- Reject if treatment list is empty.  
- Reject if any treatment has a negative base cost.

---

## RF04: Change Specialty Status
**Description:** Updates the operational status of the specialty and propagates changes to its treatments.  
**Input:** Specialty ID, New Status.  
**Processing:** - Validate specialty exists and current status is NOT `Archived` (Archived is a final state).  
- If status is `Inactive` or `Archived`, verify absence of active appointments (Application level validation).  
- Update specialty status and force all child treatments to the same status to maintain hierarchy.  
**Output:** Updated Specialty and Treatment statuses.  
**Error Handling / Constraints:** - **Finality Constraint:** Cannot change status if current status is `Archived`.  
- **Hierarchy Constraint:** Specialty status change overrides all individual treatment statuses.

---

## RF05: Update Specialty Details
**Description:** Allows modification of the specialty's identity attributes.  
**Input:** Specialty ID, New Name, New Description.  
**Processing:** - Validate specialty is in `Active` status.  
- Validate new name uniqueness and format.  
**Output:** Updated Specialty entity.  
**Error Handling / Constraints:** - Reject if specialty is `Inactive` or `Archived`.

---

## RF06: Add Treatment to Specialty
**Description:** Adds a new treatment entity to an existing active specialty.  
**Input:** Specialty ID, Treatment details (Name, Description, BaseCost).  
**Processing:** - Validate specialty exists and is `Active`.  
- Validate name uniqueness within the specialty's collection.  
**Output:** New Treatment ID linked to Specialty.  
**Error Handling / Constraints:** - Reject if specialty is not `Active`.  
- Reject if treatment name already exists in the catalog.

---

## RF07: Logical Removal of Treatment (Deactivation)
**Description:** Disables a treatment while preserving historical integrity.  
**Input:** Specialty ID, Treatment ID.  
**Processing:** - Validate that at least one OTHER treatment in the specialty remains `Active`.  
- Set treatment status to `Inactive` or `Archived`.  
**Output:** Updated Treatment status.  
**Error Handling / Constraints:** - **Minimum Invariant:** Cannot deactivate the last active treatment of a specialty.

---

## RF08: Update Treatment Details
**Description:** Modifies treatment attributes through the aggregate root.  
**Input:** Specialty ID, Treatment ID, New details.  
**Processing:** - Validate specialty is `Active`.  
- Validate new base cost is non-negative.  
**Output:** Updated Treatment.  
**Error Handling / Constraints:** - Reject if specialty is `Inactive` or `Archived`.

---

## RF09: Change Treatment Status
**Description:** Manages individual treatment status transitions.  
**Input:** Specialty ID, Treatment ID, New Status.  
**Processing:** - **Hierarchy Check:** A treatment cannot be set to `Active` if the Specialty is `Inactive` or `Archived`.  
- Validate current treatment status is NOT `Archived`.  
**Output:** Updated Treatment status.  
**Error Handling / Constraints:** - **Hierarchy Violation:** Reject activation if parent specialty is not `Active`.  
- **Finality Constraint:** Cannot modify status if treatment is `Archived`.

---

## RF10: List Inactive Treatments of a Specialty
**Description:** Retrieves all inactive treatments associated with a specific specialty aggregate.  
**Input:** Specialty ID.  
**Processing:** - Validate existence of Specialty ID.  
- Access the internal collection of the specialty aggregate.  
**Output:** Collection of inactive treatments (ID, Name, Description, BaseCost, Status).  
**Error Handling / Constraints:** Return "Specialty Not Found" error if ID is invalid.

---