using DentalSystem.Domain.Common;

namespace DentalSystem.Application.Ports.Persistence
{
    public interface IUnitOfWork
    {
        void Track(AggregateRoot aggregate);
        void Commit();
        bool HasChanges();
    }
}
