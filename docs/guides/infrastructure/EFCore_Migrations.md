# EF Core Migrations Guide

This guide explains how to manage Entity Framework Core migrations for the **DentalSystem.Infrastructure** project. It ensures that your database schema stays in sync with your domain model while following Clean Architecture and DDD principles.

---

## 1. Purpose of Migrations

Migrations are required whenever:

- New entities, value objects, or aggregates are added.
- Existing entities change (e.g., add or remove properties, modify relationships).
- You need to initialize a new database for development or testing.
  
> ⚠️ Note: Only the **Infrastructure** layer interacts directly with EF Core and the database. The domain layer remains database-agnostic.

---

## 2. Project Structure

For our project, migrations are handled **per module**. Each module (e.g., Specialties) lives inside `DentalSystem.Infrastructure`, and its migrations should be placed inside the `Migrations` folder in the same project:

```

src/DentalSystem.Infrastructure
├─ Migrations/
│   └─ Specialty/
├─ Persistence/
│   └─ DentalSystemDbContext.cs
├─ DentalSystem.Infrastructure.csproj
└─ appsettings.json

````

> Keeping migrations inside the module's Infrastructure project ensures module isolation and prevents cross-module coupling.

---

## 3. Requirements

1. **EF Core Tools** installed locally in the project:

```bash
dotnet tool install dotnet-ef --version 8.*
````

Use it via:

```bash
dotnet tool run dotnet-ef <command>
```

2. **DbContextFactory** to create your `DbContext` at design time. EF Core requires this to instantiate the DbContext for migrations. See `DentalSystemDbContextFactory.cs`.

3. `appsettings.json` must exist in the Infrastructure project with a valid connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DentalSystemDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

---

## 4. Creating a Migration

Run the following command **from the `DentalSystem.Infrastructure` project root**:

```bash
dotnet tool run dotnet-ef migrations add InitialCreate_Specialty --project . --startup-project . --output-dir Migrations/Specialty
```

Explanation:

* `migrations add <MigrationName>` → Creates a new migration.
* `--project .` → Indicates the project containing the `DbContext`.
* `--startup-project .` → Specifies the project EF Core will use to run.
* `--output-dir Migrations/Specialty` → Places migration files inside the module's migrations folder.

> Each module should have its **own folder** inside `Migrations` to avoid mixing migrations across modules.

---

## 5. Applying a Migration

To apply migrations to your database:

```bash
dotnet tool run dotnet-ef database update --project . --startup-project .
```

This ensures your database schema matches the current domain model.

---

## 6. Best Practices

* **One module at a time:** Only create migrations for the module you are working on.
* **Do not modify previous migrations** once applied in shared environments; create a new migration instead.
* **Version control:** Always commit migration files to the repository.
* **Use DbContextFactory:** This is the standard EF Core approach to instantiate the DbContext during design-time tasks.

---

## 7. When to create a migration

* After adding or modifying domain entities.
* After adding value objects that require persistence.
* After changing relationships between aggregates (e.g., one-to-many, owned entities).
* Whenever you need a fresh database initialization for development/testing.

---

## 8. References

* [EF Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli)
* [Clean Architecture](https://8thlight.com/blog/uncle-bob/2012/08/13/the-clean-architecture.html)
* [Design-time DbContext creation](https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation)

---

