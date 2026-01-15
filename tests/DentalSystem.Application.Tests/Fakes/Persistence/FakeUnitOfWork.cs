using DentalSystem.Application.Ports.Persistence;
using DentalSystem.Domain.Common;

namespace DentalSystem.Application.Tests.Fakes.Persistence
{
    public sealed class FakeUnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<AggregateRoot, int> _tracked = [];
        private bool _committed = false;

        public void Track(AggregateRoot aggregate)
        {
            if (!_tracked.ContainsKey(aggregate))
                _tracked[aggregate] = aggregate.Version;
        }

        public bool HasChanges()
        {
            return _tracked.Any(kv => kv.Key.Version != kv.Value);
        }

        public void Commit()
        {
            _committed = true;
        }

        // CAREFUL!!! ONLY use this for tests
        public bool WasCommitted => _committed;
    }
}
