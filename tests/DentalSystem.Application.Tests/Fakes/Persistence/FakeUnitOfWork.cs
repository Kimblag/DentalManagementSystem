using DentalSystem.Application.Ports.Persistence;

namespace DentalSystem.Application.Tests.Fakes.Persistence
{
    public sealed class FakeUnitOfWork : IUnitOfWork
    {
        public Task CommitAsync()
        {
            return Task.CompletedTask;
        }

        public bool HasChanges()
            => true;
    }
}
