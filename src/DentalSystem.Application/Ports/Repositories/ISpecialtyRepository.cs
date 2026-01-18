using DentalSystem.Domain.Entities;

namespace DentalSystem.Application.Ports.Repositories
{
    public interface ISpecialtyRepository
    {
        // use Guid instead of int because the identity of a Specialty
        // belongs to the Domain, not to the database.
        // 
        // The application layer should not care if the data is stored
        // in SQL, NoSQL, memory, or anywhere else.
        // Using Guid avoids coupling the domain to database-generated IDs
        // and allows the aggregate to be created independently of persistence.
        //
        // This method is used by application use cases to retrieve
        // the Specialty aggregate so the domain can decide what to do with it.
        // The repository itself does NOT apply business rules.
        Task<Specialty?> GetByIdAsync(Guid specialtyId, CancellationToken cancellationToken = default);

        void Add(Specialty specialty);
    }
}
