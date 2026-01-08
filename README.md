# DentalSystem 2.0
From Academic Legacy (WebForms) to Clean Architecture (.NET 8)

## Project Context
This project is a redesign and technical evolution of my original academic final project, developed using ASP.NET WebForms (.NET Framework 4.5) and ADO.NET.

The goal of this version is not feature expansion, but system design maturity: applying proper domain modeling, clean architecture, and modern .NET practices.

This repository intentionally prioritizes design decisions, domain modeling, and correctness over rapid implementation.


## Original Version (Legacy)

- Repository: [Sistema Turnos Odontología](https://github.com/Kimblag/SistemaTurnosOdontologia)
- Language: Spanish
- Stack: ASP.NET WebForms, ADO.NET, .NET Framework 4.5
- Architecture: N-layered, procedural logic

This version reflects academic constraints and institutional requirements.

## Focus of This Redesign
This redesign focuses on software engineering principles rather than framework usage:

- Domain-first design
- Clean Architecture
- Rich domain model (invariants, aggregates)
- Separation of domain vs application rules
- Async / Await
- EF Core with Fluent API
- Test-driven domain modeling

## Architectural Overview
```bash
/src
  ├── Domain
  ├── Application
  ├── Infrastructure
  └── API
```
The system is built around domain aggregates rather than CRUD entities.

## Current Status
This project is intentionally being developed incrementally following a domain-first approach.:
- Domain modeling and invariants
- Domain tests
- Application layer
- Persistence and API