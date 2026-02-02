# UML Class Diagram – Module 01: Specialties

## Scope
Este diagrama representa el Modelo de Dominio para el módulo de Especialidades. Refleja el diseño de agregados y las invariantes de dominio definidas previamente.

## UML Diagram mermaid
```mermaid
classDiagram
    direction TB

    class Especialidad {
        +UUID Id
        +Name Name
        +bool IsActive
        -Collection<Tratamiento> Treatments

        +Create(Name, Collection<NewTreatmentData>)
        +Rename(Name)
        +AddTreatment(NewTreatmentData)
        +RenameTreatment(UUID, Name)
        +ChangeTreatmentBaseCost(UUID, Money)
        +DeactivateTreatment(UUID)
        +ActivateTreatment(UUID)
        +Deactivate()
        +Activate()
    }

    class Tratamiento {
        +UUID Id
        +string Code
        +Name Name
        +Money BaseCost
        +bool IsActive

        -Rename(Name)
        -ChangeBaseCost(Money)
        -Deactivate()
        -Activate()
    }

    class Name {
        +string Value
    }

    class Money {
        +decimal Amount
        +string Currency
    }

    class NewTreatmentData {
        +string Code
        +Name Name
        +Money BaseCost
    }

    %% Relaciones
    Especialidad "1" *-- "1..*" Tratamiento : contiene
    Especialidad --> Name
    Especialidad --> NewTreatmentData
    Tratamiento --> Name
    Tratamiento --> Money
    NewTreatmentData --> Name
    NewTreatmentData --> Money
```