# UML Class Diagram – Module 01: Specialties

## Scope
Este diagrama representa el Modelo de Dominio para el módulo de Especialidades. Refleja el diseño de agregados y las invariantes de dominio definidas.

## UML Diagram mermaid
```mermaid
classDiagram
    direction TB

    class Specialty {
        +Guid Id
        +Name Name
        +String Description
        +SpecialtyStatus Status
        +IReadOnlyCollection~Treatment~ Treatments
        -List~Treatment~ _treatments
        +Specialty(Name, String?)
        +Rename(Name)
        +UpdateDescription(String?)
        +Activate()
        +Archive()
        +AddTreatment(TreatmentCode, Name, Money, String?)
        +RenameTreatment(TreatmentCode, Name)
        +UpdateTreatmentBaseCost(TreatmentCode, Money)
        +ActivateTreatment(TreatmentCode)
        +ArchiveTreatment(TreatmentCode)
        +UpdateTreatmentDescription(TreatmentCode, String?)
        -EnsureActive()
    }

    class Treatment {
        +TreatmentCode Code
        +Name Name
        +Money BaseCost
        +TreatmentStatus Status
        +String? Description
        internal Treatment(TreatmentCode, Name, Money, String?)
        internal Rename(Name)
        internal UpdateBaseCost(Money)
        internal Activate()
        internal Archive()
        internal UpdateDescription(String?)
    }

    class Name {
        <<valueObject>>
        +String Value
    }

    class Money {
        <<valueObject>>
        +Decimal Amount
        +String Currency
    }

    class TreatmentCode {
        <<valueObject>>
        +String Value
    }

    %% Relaciones de Agregado
    Specialty "1" *-- "0..*" Treatment : Aggregate Root

    %% Relaciones con Value Objects
    Specialty ..> Name
    Treatment ..> Name
    Treatment ..> Money
    Treatment ..> TreatmentCode
```