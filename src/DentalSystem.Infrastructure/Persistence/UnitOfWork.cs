using DentalSystem.Application.Ports.Persistence;
using DentalSystem.Domain.Common;

namespace DentalSystem.Infrastructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly DentalSystemDbContext _context;

        public UnitOfWork(DentalSystemDbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public void Track(AggregateRoot aggregate)
        {
            // method to keep track of the given entity
            _context.Update(aggregate);
        }
    }
}
